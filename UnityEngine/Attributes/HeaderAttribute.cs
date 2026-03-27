using System;

namespace UnityEngine
{
	/// <summary>
	/// Add a header above some fields in the Inspector.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HeaderAttribute : Attribute
	{
		public readonly string header;
		public HeaderAttribute(string header) { this.header = header; }
	}
}
