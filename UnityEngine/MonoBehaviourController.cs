using Godot;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
	/// <summary>
	/// Legacy controller kept for backward compatibility.
	/// New code should use MonoBehaviour directly (which implements Node lifecycle).
	/// </summary>
	public class MonoBehaviourController
	{
		Node referencedNode;

		public List<IEnumerator> coroutines = new List<IEnumerator>();
		bool startCalled;

		public GameObject? gameObject { get; private set; }
		public Transform? transform { get; private set; }

		public string name
		{
			get => referencedNode.Name;
			set => referencedNode.Name = value;
		}

		MethodInfo? awakeMethod;
		MethodInfo? startMethod;
		MethodInfo? startMethodCR;
		MethodInfo? updateMethod;
		MethodInfo? fixedUpdateMethod;
		MethodInfo? onEnabledMethod;
		MethodInfo? onDisabledMethod;

		const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		VisibilityHandler visibilityHandler;
		bool prevVisibility;


		public MonoBehaviourController(Node node)
		{
			referencedNode = node;
			visibilityHandler = new VisibilityHandler();
		}


		MethodInfo? FindMethod(string methodName, System.Type returnType, System.Type? type = null)
		{
			type = type == null ? referencedNode.GetType() : type;
			MethodInfo? method = type.GetMethod(methodName, bindingFlags);

			if (method != null)
			{
				if (method.GetParameters().Length == 0 && method.ReturnType == returnType)
					return method;
			}

			return type == typeof(MonoBehaviour) || type.BaseType == null
				? null
				: FindMethod(methodName, returnType, type.BaseType);
		}


		public void Awake()
		{
			_SetupMonoBehaviour();
		}


		void _SetupMonoBehaviour()
		{
			InitMonoBehaviour();
			awakeMethod?.Invoke(referencedNode, null);
		}


		void InitMonoBehaviour()
		{
			if (referencedNode is Node3D node3D)
			{
				transform = new Transform();
				transform.InternalSetGameObject(new GameObject(null, node3D));
				gameObject = transform.gameObject;
				visibilityHandler = new SpatialVisibilityHandler(node3D);
			}
			else if (referencedNode is CanvasItem canvasItem)
			{
				visibilityHandler = new CanvasItemVisibilityHandler(canvasItem);
				gameObject = new GameObject();
			}
			else
			{
				visibilityHandler = new VisibilityHandler();
				gameObject = new GameObject();
			}

			awakeMethod      = FindMethod("Awake",    typeof(void));
			startMethod      = FindMethod("Start",    typeof(void));
			startMethodCR    = FindMethod("Start",    typeof(IEnumerator));
			updateMethod     = FindMethod("Update",   typeof(void));
			fixedUpdateMethod= FindMethod("FixedUpdate", typeof(void));
			onEnabledMethod  = FindMethod("OnEnable", typeof(void));
			onDisabledMethod = FindMethod("OnDisable", typeof(void));
		}


		public void Update()
		{
			try
			{
				if (!visibilityHandler.IsVisible) return;

				if (!startCalled)
				{
					startCalled = true;
					if (startMethod != null)
						startMethod.Invoke(referencedNode, null);
					else if (startMethodCR != null)
						StartCoroutine((IEnumerator)startMethodCR.Invoke(referencedNode, null)!);
				}

				updateMethod?.Invoke(referencedNode, null);

				for (int i = 0; i < coroutines.Count; i++)
				{
					CustomYieldInstruction? yielder = coroutines[i].Current as CustomYieldInstruction;
					bool yielded = yielder != null && yielder.MoveNext();
					if (!yielded && !coroutines[i].MoveNext())
					{
						coroutines.RemoveAt(i);
						i--;
					}
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message + "\n\n" + e.StackTrace);
			}
		}


		public void FixedUpdate()
		{
			fixedUpdateMethod?.Invoke(referencedNode, null);
		}


		public Coroutine StartCoroutine(IEnumerator routine)
		{
			coroutines.Add(routine);
			return new Coroutine(routine);
		}


		public static void Destroy(MonoBehaviour node)
		{
			node.QueueFree();
		}
	}
}
