using Godot;

namespace UnityEngine
{
	/// <summary>
	/// Renders a mesh using a material. Works in conjunction with MeshFilter.
	/// </summary>
	public class MeshRenderer : Component
	{
		private Material? _sharedMaterial;
		public MeshInstance3D godotMeshInstance3D = new();

		/// <summary>The shared material (no copy made).</summary>
		public Material? sharedMaterial
		{
			get => _sharedMaterial;
			set { _sharedMaterial = value; gameObject?.ValidateMesh(); }
		}

		/// <summary>The material (same as sharedMaterial here).</summary>
		public Material? material
		{
			get => _sharedMaterial;
			set { _sharedMaterial = value; gameObject?.ValidateMesh(); }
		}

		public Material[]? materials
		{
			get => _sharedMaterial != null ? new[] { _sharedMaterial } : null;
			set { if (value != null && value.Length > 0) sharedMaterial = value[0]; }
		}

		public Material[]? sharedMaterials
		{
			get => materials;
			set => materials = value;
		}

		public bool enabled
		{
			get => godotMeshInstance3D.Visible;
			set => godotMeshInstance3D.Visible = value;
		}

		public bool shadowCastingMode { get; set; } = true;
		public bool receiveShadows { get; set; } = true;

		public override void Init()
		{
			godotMeshInstance3D = new MeshInstance3D();
			gameObject?.ValidateMesh();
			gameObject?.godot.CallDeferred(Node.MethodName.AddChild, godotMeshInstance3D);
		}

		public override void OnComponentDisable() => godotMeshInstance3D.Visible = false;
		public override void OnComponentEnable()  => godotMeshInstance3D.Visible = true;

		public Bounds bounds => new Bounds(gameObject.transform.position, Vector3.one);
	}
}
