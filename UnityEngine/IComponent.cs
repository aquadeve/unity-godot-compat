namespace UnityEngine
{
	/// <summary>
	/// Interface implemented by all Unity components.
	/// </summary>
	public interface IComponent
	{
		bool enabled { get; set; }
		GameObject gameObject { get; }
		Transform transform { get; }
		void InternalSetGameObject(GameObject go);
		void Init();
		void OnComponentEnable();
		void OnComponentDisable();
	}
}
