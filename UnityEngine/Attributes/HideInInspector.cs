using System;

namespace UnityEngine
{
	/// <summary>
	/// Makes a variable not show up in the inspector but be serialized.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class HideInInspector : Attribute { }
}
