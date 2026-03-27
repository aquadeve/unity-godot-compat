using System;

namespace UnityEngine
{
	/// <summary>
	/// Prevents MonoBehaviour of the same type (or subtype) to be added more than once.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DisallowMultipleComponent : Attribute { }
}
