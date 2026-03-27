namespace UnityEngine
{
	/// <summary>
	/// Base class for everything attached to a GameObject.
	/// </summary>
	public class Component : Object, IComponent
	{
		private bool _enabled = true;
		private GameObject? _gameObject;

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

		public override string name
		{
			get => _gameObject?.name ?? "";
			set { if (_gameObject != null) _gameObject.name = value; }
		}

		public void InternalSetGameObject(GameObject go) => _gameObject = go;

		public virtual void Init() {}

		public virtual void OnComponentEnable() {}

		public virtual void OnComponentDisable() {}

		public T? GetComponent<T>() where T : class, IComponent, new()
			=> gameObject.GetComponent<T>();

		public T[] GetComponents<T>() where T : class, IComponent, new()
			=> gameObject.GetComponents<T>();

		public T? GetComponentInChildren<T>() where T : class, IComponent, new()
			=> gameObject.GetComponentInChildren<T>();

		public T[] GetComponentsInChildren<T>() where T : class, IComponent, new()
			=> gameObject.GetComponentsInChildren<T>();
	}
}
