using System;

namespace UnityEngine
{
	/// <summary>
	/// Attribute for editor sliders. Compatible with [Godot.Export(PropertyHint.Range, "min,max")].
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class RangeAttribute : Attribute
	{
		public readonly float min;
		public readonly float max;

		public RangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}
