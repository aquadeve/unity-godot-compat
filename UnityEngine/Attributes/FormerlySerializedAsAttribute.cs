using System;

namespace UnityEngine.Serialization
{
	/// <summary>
	/// Use this attribute to rename a field without losing its serialized value.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class FormerlySerializedAsAttribute : Attribute
	{
		public readonly string oldName;
		public FormerlySerializedAsAttribute(string oldName)
		{
			this.oldName = oldName;
		}
	}
}
