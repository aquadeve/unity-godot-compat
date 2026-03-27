using System;

namespace UnityEngine
{
	/// <summary>
	/// Attribute that specifies a required component on the same GameObject.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class RequireComponent : Attribute
	{
		public readonly Type m_Type0;
		public readonly Type? m_Type1;
		public readonly Type? m_Type2;

		public RequireComponent(Type requiredComponent)
		{
			m_Type0 = requiredComponent;
		}

		public RequireComponent(Type requiredComponent, Type requiredComponent2)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
		}

		public RequireComponent(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
			m_Type2 = requiredComponent3;
		}
	}
}
