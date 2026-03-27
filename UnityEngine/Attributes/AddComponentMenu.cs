using System;

namespace UnityEngine
{
	/// <summary>
	/// Add this component to the Component menu at a given path.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class AddComponentMenu : Attribute
	{
		public readonly string componentMenu;
		public readonly int componentOrder;

		public AddComponentMenu(string menuName)
		{
			componentMenu = menuName;
			componentOrder = 0;
		}

		public AddComponentMenu(string menuName, int order)
		{
			componentMenu = menuName;
			componentOrder = order;
		}
	}
}
