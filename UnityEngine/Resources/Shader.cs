namespace UnityEngine
{
	/// <summary>
	/// Wraps a Godot shader resource.
	/// </summary>
	public class Shader : Object
	{
		public Godot.Shader? godot;

		public static Shader? Find(string name)
		{
			// In Godot, shaders are loaded from files. This method is provided for API compat.
			Debug.LogWarning($"Shader.Find(\"{name}\"): use Resources.Load<Shader>(path) instead.");
			return null;
		}
	}
}
