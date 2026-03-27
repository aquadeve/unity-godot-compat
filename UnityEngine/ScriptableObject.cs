namespace UnityEngine
{
	/// <summary>
	/// A class you can derive from for creating objects that live outside of scenes.
	/// In Godot, this is a simple base class stub for API compatibility.
	/// </summary>
	public class ScriptableObject : Object
	{
		public static T CreateInstance<T>() where T : ScriptableObject, new()
		{
			return new T();
		}

		public static ScriptableObject CreateInstance(System.Type type)
		{
			return (ScriptableObject)System.Activator.CreateInstance(type)!;
		}

		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }
		protected virtual void OnDestroy() { }
		protected virtual void OnValidate() { }
	}
}
