using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace UnityEngine
{
	/// <summary>
	/// Base class for all Unity scripts, wrapping Godot's Node lifecycle.
	/// </summary>
	public partial class MonoBehaviour : Node, IComponent
	{
		// ---- IComponent ----
		private bool _enabled = true;
		private GameObject? _gameObject;
		private readonly Dictionary<string, SceneTreeTimer> _invokeTimers = new();

		public bool enabled
		{
			get => _enabled;
			set
			{
				if (value != _enabled)
				{
					_enabled = value;
					if (_enabled) OnComponentEnable();
					else OnComponentDisable();
				}
			}
		}

		public GameObject gameObject => _gameObject!;

		public Transform transform => _gameObject!.transform;

		public string name
		{
			get => Name;
			set => Name = value;
		}

		public void InternalSetGameObject(GameObject go) => _gameObject = go;

		public virtual void OnComponentEnable() { _onEnable?.Invoke(); }
		public virtual void OnComponentDisable() { _onDisable?.Invoke(); }
		public virtual void Init()
		{
			if (GetParent() == null)
				gameObject.godot.CallDeferred(Node.MethodName.AddChild, this);
		}

		// ---- Lifecycle actions resolved via reflection ----
		private Action? _awake;
		private Action? _start;
		private Action? _onEnable;
		private Action? _update;
		private Action? _lateUpdate;
		private Action? _fixedUpdate;
		private Action? _onDisable;
		private Action? _onDestroy;

		// ---- Coroutine support ----
		private readonly List<IEnumerator> _coroutines = new();
		private double _localTime = 0.0;

		// ---- Godot lifecycle ----
		public override void _Ready()
		{
			base._Ready();

			// Resolve the parent as a GameObject
			var parentNode3D = GetParent() as Node3D;
			if (parentNode3D == null)
			{
				Debug.LogError($"MonoBehaviour {GetType().FullName} parent is not a Node3D!");
				return;
			}
			InternalSetGameObject(UGGameObjectHelper.GetOrCreate(parentNode3D));

			// Bind lifecycle methods via reflection
			var type = GetType();
			var methods = type.GetMethods(
				BindingFlags.DeclaredOnly |
				BindingFlags.NonPublic |
				BindingFlags.Public |
				BindingFlags.Instance);

			foreach (var method in methods)
			{
				if (method.GetParameters().Length != 0) continue;
				if (method.ReturnType == typeof(IEnumerator)) continue;
				switch (method.Name)
				{
					case "Awake":     _awake     = (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "Start":     _start     = (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "OnEnable":  _onEnable  = (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "Update":    _update    = (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "LateUpdate":_lateUpdate= (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "FixedUpdate":_fixedUpdate=(Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "OnDisable": _onDisable = (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
					case "OnDestroy": _onDestroy = (Action)Delegate.CreateDelegate(typeof(Action), this, method); break;
				}
			}

			_awake?.Invoke();
			_start?.Invoke();
			_onEnable?.Invoke();
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
			if (!_enabled) return;

			Time.deltaTime = (float)delta;
			_localTime += delta;
			Time.time = (float)_localTime;

			_update?.Invoke();
			_lateUpdate?.Invoke();

			// Advance coroutines
			for (int i = _coroutines.Count - 1; i >= 0; i--)
			{
				try
				{
					if (!_coroutines[i].MoveNext())
						_coroutines.RemoveAt(i);
				}
				catch (Exception e)
				{
					Debug.LogError($"Coroutine error: {e.Message}\n{e.StackTrace}");
					_coroutines.RemoveAt(i);
				}
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			if (!_enabled) return;
			Time.fixedDeltaTime = (float)delta;
			_fixedUpdate?.Invoke();
		}

		public override void _ExitTree()
		{
			_onDisable?.Invoke();
			_onDestroy?.Invoke();
			base._ExitTree();
		}

		// ---- Unity API ----
		public Coroutine StartCoroutine(IEnumerator routine)
		{
			_coroutines.Add(routine);
			return new Coroutine(routine);
		}

		public void StopCoroutine(Coroutine coroutine)
		{
			if (coroutine?.enumerator != null)
				_coroutines.Remove(coroutine.enumerator);
		}

		public void StopAllCoroutines() => _coroutines.Clear();

		public static new void Destroy(Object obj, float t = 0f)
		{
			Object.Destroy(obj, t);
		}

		public static new void DestroyImmediate(Object obj)
		{
			Object.DestroyImmediate(obj);
		}

		public static void DontDestroyOnLoad(Object obj) { /* no-op */ }

		public T? GetComponent<T>() where T : class, IComponent, new()
			=> gameObject?.GetComponent<T>();

		public T[] GetComponents<T>() where T : class, IComponent, new()
			=> gameObject?.GetComponents<T>() ?? Array.Empty<T>();

		public T? GetComponentInChildren<T>() where T : class, IComponent, new()
			=> gameObject?.GetComponentInChildren<T>();

		public T[] GetComponentsInChildren<T>() where T : class, IComponent, new()
			=> gameObject?.GetComponentsInChildren<T>() ?? Array.Empty<T>();

		public T? GetComponentInParent<T>() where T : class, IComponent, new()
			=> gameObject?.GetComponentInParent<T>();

		public T[] GetComponentsInParent<T>() where T : class, IComponent, new()
			=> gameObject?.GetComponentsInParent<T>() ?? Array.Empty<T>();

		public bool CompareTag(string tag) => this.IsInGroup(tag);

		public void Invoke(string methodName, float time)
		{
			var method = GetType().GetMethod(methodName,
				BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (method == null)
			{
				Debug.LogError($"MonoBehaviour.Invoke: method '{methodName}' not found on {GetType().FullName}");
				return;
			}
			var tree = GetTree();
			if (tree == null) return;
			var timer = tree.CreateTimer(time);
			_invokeTimers[methodName] = timer;
			timer.Timeout += () =>
			{
				_invokeTimers.Remove(methodName);
				method.Invoke(this, null);
			};
		}

		public void InvokeRepeating(string methodName, float time, float repeatRate)
		{
			var method = GetType().GetMethod(methodName,
				BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (method == null)
			{
				Debug.LogError($"MonoBehaviour.InvokeRepeating: method '{methodName}' not found on {GetType().FullName}");
				return;
			}
			InvokeRepeatingInternal(method, methodName, time, repeatRate);
		}

		private async void InvokeRepeatingInternal(MethodInfo method, string methodName, float initialDelay, float repeatRate)
		{
			var tree = GetTree();
			if (tree == null) return;
			var timer = tree.CreateTimer(initialDelay);
			_invokeTimers[methodName] = timer;
			await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

			while (IsInsideTree() && _invokeTimers.ContainsKey(methodName))
			{
				method.Invoke(this, null);
				timer = GetTree()?.CreateTimer(repeatRate)!;
				if (timer == null) break;
				_invokeTimers[methodName] = timer;
				await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
			}
		}

		public bool IsInvoking(string methodName) => _invokeTimers.ContainsKey(methodName);
		public bool IsInvoking() => _invokeTimers.Count > 0;

		public void CancelInvoke(string methodName)
		{
			_invokeTimers.Remove(methodName);
		}

		public void CancelInvoke()
		{
			_invokeTimers.Clear();
		}

		public void SendMessage(string methodName, object? value = null) { }
		public void SendMessageUpwards(string methodName, object? value = null) { }
		public void BroadcastMessage(string methodName, object? value = null) { }

		public static void print(object? message) => Debug.Log(message);
	}

	/// <summary>
	/// Helper to map Node3D instances to UnityEngine GameObjects.
	/// </summary>
	internal static class UGGameObjectHelper
	{
		private static readonly System.Collections.Generic.Dictionary<Node3D, GameObject> _map = new();

		public static GameObject GetOrCreate(Node3D node)
		{
			if (_map.TryGetValue(node, out var go))
				return go;
			go = new GameObject(null, node);
			_map[node] = go;
			return go;
		}
	}
}
