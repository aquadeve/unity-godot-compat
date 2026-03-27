using Godot;

namespace UnityEngine
{
	/// <summary>
	/// Holds a mesh reference for rendering. Attach to a GameObject alongside MeshRenderer.
	/// </summary>
	public class MeshFilter : Component
	{
		private Mesh? _sharedMesh;

		/// <summary>
		/// The mesh used for rendering (shared reference, not a copy).
		/// Assigning triggers mesh update on the owning GameObject.
		/// </summary>
		public Mesh? sharedMesh
		{
			get => _sharedMesh;
			set { _sharedMesh = value; gameObject?.ValidateMesh(); }
		}

		/// <summary>
		/// Returns the mesh (same as sharedMesh in Godot context).
		/// Setting this creates a copy in Unity but not here.
		/// </summary>
		public Mesh? mesh
		{
			get => _sharedMesh;
			set { _sharedMesh = value; gameObject?.ValidateMesh(); }
		}
	}
}
