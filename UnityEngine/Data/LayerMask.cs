namespace UnityEngine
{
	/// <summary>
	/// Specifies layers to use in Physics.Raycast and similar methods.
	/// </summary>
	public struct LayerMask
	{
		public int value;

		public static implicit operator int(LayerMask mask) => mask.value;
		public static implicit operator LayerMask(int val) => new LayerMask { value = val };

		public static int NameToLayer(string layerName)
		{
			// Godot doesn't use Unity's named layer system; return 0 as default
			Debug.LogWarning($"LayerMask.NameToLayer(\"{layerName}\"): not fully supported in Godot.");
			return 0;
		}

		public static string LayerToName(int layer)
		{
			Debug.LogWarning($"LayerMask.LayerToName({layer}): not fully supported in Godot.");
			return "";
		}

		public static int GetMask(params string[] layerNames)
		{
			int mask = 0;
			foreach (var name in layerNames)
				mask |= 1 << NameToLayer(name);
			return mask;
		}
	}
}
