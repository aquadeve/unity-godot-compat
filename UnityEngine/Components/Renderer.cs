namespace UnityEngine
{
	public class Renderer : Component
	{
		public Material? material { get; set; }
		public Material? sharedMaterial { get; set; }
		public Material[]? materials { get; set; }
		public Material[]? sharedMaterials { get; set; }
		public bool enabled { get; set; } = true;
		public bool receiveShadows { get; set; } = true;
		public Bounds bounds => new Bounds(gameObject.transform.position, Vector3.one);
	}
}