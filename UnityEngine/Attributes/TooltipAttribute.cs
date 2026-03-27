using System;

namespace UnityEngine
{
	/// <summary>
	/// Specify a tooltip for a field in the Inspector window.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class TooltipAttribute : Attribute
	{
		public readonly string tooltip;
		public TooltipAttribute(string tooltip) { this.tooltip = tooltip; }
	}
}
