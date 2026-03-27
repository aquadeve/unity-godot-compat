using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace UnityEngine
{
	public class GameObject : Object
	{
		/// <summary>The underlying Godot Node3D for this GameObject.</summary>
		public Node3D godot;

		private Transform? _transform;
		private MeshRenderer? _meshRenderer;
		private MeshFilter? _meshFilter;
		private readonly Dictionary<Type, List<IComponent>> _components = new();

		public override string name
		{
			get => godot.Name;
			set => godot.Name = value;
		}

		public Transform transform
		{
			get
			{
				if (_transform == null)
				{
					_transform = new Transform();
					_transform.InternalSetGameObject(this);
					_transform.Init();
				}
				return _transform;
			}
		}

		public bool activeSelf => godot.Visible;
		public bool activeInHierarchy => godot.IsVisibleInTree();

		public int layer { get; set; } = 0;
		public string tag { get; set; } = "Untagged";

		// ---- Constructors ----
		public GameObject(string? name = null, Node3D? existingNode = null)
		{
			if (existingNode != null)
			{
				godot = existingNode;
			}
			else
			{
				godot = new Node3D();
				UnityEngineAutoLoad.Instance?.AddChild(godot);
			}

			if (name != null)
				this.name = name;
		}

		internal GameObject(Node legacyNode)
		{
			godot = (legacyNode as Node3D) ?? new Node3D();
		}

		// ---- Mesh validation ----
		public void ValidateMesh()
		{
			if (_meshRenderer != null && _meshFilter != null)
			{
				var mat = _meshRenderer.sharedMaterial;
				if (mat != null)
					_meshRenderer.godotMeshInstance3D.MaterialOverride = mat.godot;
				_meshRenderer.godotMeshInstance3D.Mesh = _meshFilter.sharedMesh?.GetGodotMesh();
			}
		}

		// ---- Component API ----
		public T AddComponent<T>() where T : class, IComponent, new()
		{
			if (typeof(T) == typeof(MeshRenderer))
				return (T)(IComponent)CreateSpecial(ref _meshRenderer);
			if (typeof(T) == typeof(MeshFilter))
				return (T)(IComponent)CreateSpecial(ref _meshFilter);
			if (typeof(T) == typeof(Transform))
			{
				Debug.LogWarning("Transform is always present on GameObjects.");
				return (T)(IComponent)transform;
			}

			var c = new T();
			c.InternalSetGameObject(this);
			c.Init();

			if (_components.TryGetValue(typeof(T), out var list))
				list.Add(c);
			else
				_components[typeof(T)] = new List<IComponent> { c };

			return c;
		}

		private T CreateSpecial<T>(ref T? field) where T : class, IComponent, new()
		{
			if (field != null)
			{
				Debug.LogWarning($"Component {typeof(T).Name} already added to {name}.");
				return field;
			}
			field = new T();
			field.InternalSetGameObject(this);
			field.Init();
			return field;
		}

		public T? GetComponent<T>() where T : class, IComponent, new()
		{
			if (typeof(T) == typeof(MeshRenderer)) return _meshRenderer as T;
			if (typeof(T) == typeof(MeshFilter))   return _meshFilter as T;
			if (typeof(T) == typeof(Transform))     return transform as T;

			if (_components.TryGetValue(typeof(T), out var list) && list.Count > 0)
				return list[0] as T;
			return null;
		}

		public T[] GetComponents<T>() where T : class, IComponent, new()
		{
			if (typeof(T) == typeof(MeshRenderer)) return _meshRenderer != null ? new T[] { (T)(IComponent)_meshRenderer } : Array.Empty<T>();
			if (typeof(T) == typeof(MeshFilter))   return _meshFilter   != null ? new T[] { (T)(IComponent)_meshFilter   } : Array.Empty<T>();
			if (typeof(T) == typeof(Transform))     return new T[] { (T)(IComponent)transform };

			if (_components.TryGetValue(typeof(T), out var list))
				return list.Select(x => (T)x).ToArray();
			return Array.Empty<T>();
		}

		public T? GetComponentInChildren<T>() where T : class, IComponent, new()
		{
			var result = GetComponent<T>();
			if (result != null) return result;

			foreach (Node child in godot.GetChildren())
			{
				if (child is Node3D child3D)
				{
					var childGO = UGGameObjectHelper.GetOrCreate(child3D);
					result = childGO.GetComponentInChildren<T>();
					if (result != null) return result;
				}
			}
			return null;
		}

		public T[] GetComponentsInChildren<T>() where T : class, IComponent, new()
		{
			var results = new List<T>();
			results.AddRange(GetComponents<T>());
			foreach (Node child in godot.GetChildren())
			{
				if (child is Node3D child3D)
				{
					var childGO = UGGameObjectHelper.GetOrCreate(child3D);
					results.AddRange(childGO.GetComponentsInChildren<T>());
				}
			}
			return results.ToArray();
		}

		public T? GetComponentInParent<T>() where T : class, IComponent, new()
		{
			var result = GetComponent<T>();
			if (result != null) return result;

			var parentNode = godot.GetParent() as Node3D;
			if (parentNode != null)
			{
				var parentGO = UGGameObjectHelper.GetOrCreate(parentNode);
				return parentGO.GetComponentInParent<T>();
			}
			return null;
		}

		public T[] GetComponentsInParent<T>() where T : class, IComponent, new()
		{
			var results = new List<T>();
			results.AddRange(GetComponents<T>());

			var parentNode = godot.GetParent() as Node3D;
			if (parentNode != null)
			{
				var parentGO = UGGameObjectHelper.GetOrCreate(parentNode);
				results.AddRange(parentGO.GetComponentsInParent<T>());
			}
			return results.ToArray();
		}

		// ---- Scene API ----
		public void SetActive(bool value)
		{
			godot.Visible = value;
		}

		public static GameObject? Find(string name)
		{
			if (UnityEngineAutoLoad.Instance == null) return null;
			var root = UnityEngineAutoLoad.Instance.GetTree().Root;
			var node = FindNodeByName(root, name);
			if (node is Node3D n3d) return UGGameObjectHelper.GetOrCreate(n3d);
			return null;
		}

		public static GameObject[] FindGameObjectsWithTag(string tag)
		{
			// No direct equivalent; return empty for now
			return Array.Empty<GameObject>();
		}

		public static GameObject? FindWithTag(string tag) => null;

		private static Node? FindNodeByName(Node parent, string name)
		{
			if (parent.Name == name) return parent;
			foreach (Node child in parent.GetChildren())
			{
				var result = FindNodeByName(child, name);
				if (result != null) return result;
			}
			return null;
		}

		// ---- Implicit conversions (for backward compat) ----
		public static implicit operator Node(GameObject go) => go.godot;
		public static implicit operator Node3D(GameObject go) => go.godot;
	}
}
