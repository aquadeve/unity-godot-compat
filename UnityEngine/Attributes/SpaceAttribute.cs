using System;

namespace UnityEngine
{
	/// <summary>
	/// Use this attribute to add some spacing in the Inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class SpaceAttribute : Attribute
	{
		public readonly float height;
		public SpaceAttribute() { this.height = 8f; }
		public SpaceAttribute(float height) { this.height = height; }
	}
}
