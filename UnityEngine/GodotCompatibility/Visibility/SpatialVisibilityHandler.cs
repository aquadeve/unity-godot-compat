using Godot;

namespace UnityEngine
{
	class SpatialVisibilityHandler : VisibilityHandler
	{
		public override bool IsVisible => node3D.Visible;

		Node3D node3D;

		public SpatialVisibilityHandler(Node3D node)
		{
			this.node3D = node;
		}
	}
}