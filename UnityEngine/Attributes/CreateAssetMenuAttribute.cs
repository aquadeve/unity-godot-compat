using System;

namespace UnityEngine
{
	/// <summary>
	/// Mark a ScriptableObject-derived type to be automatically listed in the Assets/Create submenu.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CreateAssetMenuAttribute : Attribute
	{
		public string menuName { get; set; } = "";
		public string fileName { get; set; } = "";
		public int order { get; set; } = 0;
	}
}
