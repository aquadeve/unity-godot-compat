using System;

namespace UnityEngine
{
	/// <summary>
	/// Use this attribute to specify a non-serialized field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class NonSerializedAttribute : Attribute { }

	/// <summary>
	/// Use this attribute to specify a multiline text area for a string.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MultilineAttribute : Attribute
	{
		public readonly int lines;
		public MultilineAttribute() { lines = 3; }
		public MultilineAttribute(int lines) { this.lines = lines; }
	}

	/// <summary>
	/// Attribute to make a string be edited with a height-flexible and scrollable text area.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class TextAreaAttribute : Attribute
	{
		public readonly int minLines;
		public readonly int maxLines;
		public TextAreaAttribute() { minLines = 3; maxLines = 3; }
		public TextAreaAttribute(int minLines, int maxLines) { this.minLines = minLines; this.maxLines = maxLines; }
	}

	/// <summary>
	/// Attribute for context menu items in the Inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ContextMenu : Attribute
	{
		public readonly string menuItem;
		public ContextMenu(string itemName) { menuItem = itemName; }
	}

	/// <summary>
	/// Use this attribute to add a context menu to a field in the Inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ContextMenuItemAttribute : Attribute
	{
		public readonly string name;
		public readonly string function;
		public ContextMenuItemAttribute(string name, string function) { this.name = name; this.function = function; }
	}

	/// <summary>
	/// Attribute to specify minimum value for a float or int property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MinAttribute : Attribute
	{
		public readonly float min;
		public MinAttribute(float min) { this.min = min; }
	}

	/// <summary>
	/// Attribute to define a field's color usage in the inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ColorUsageAttribute : Attribute
	{
		public readonly bool showAlpha;
		public readonly bool hdr;
		public ColorUsageAttribute(bool showAlpha) { this.showAlpha = showAlpha; this.hdr = false; }
		public ColorUsageAttribute(bool showAlpha, bool hdr) { this.showAlpha = showAlpha; this.hdr = hdr; }
	}

	/// <summary>
	/// Attribute to specify a gradient usage for a field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class GradientUsageAttribute : Attribute
	{
		public readonly bool hdr;
		public GradientUsageAttribute(bool hdr = false) { this.hdr = hdr; }
	}
}
