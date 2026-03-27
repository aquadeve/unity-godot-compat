namespace UnityEngine
{
	/// <summary>Axis-aligned bounding box.</summary>
	public struct Bounds
	{
		public Vector3 center;
		public Vector3 size;
		public Vector3 extents => size * 0.5f;
		public Vector3 min => center - extents;
		public Vector3 max => center + extents;

		public Bounds(Vector3 center, Vector3 size)
		{
			this.center = center;
			this.size = size;
		}

		public bool Contains(Vector3 point)
		{
			return point.x >= min.x && point.x <= max.x &&
			       point.y >= min.y && point.y <= max.y &&
			       point.z >= min.z && point.z <= max.z;
		}

		public bool Intersects(Bounds other)
		{
			return min.x <= other.max.x && max.x >= other.min.x &&
			       min.y <= other.max.y && max.y >= other.min.y &&
			       min.z <= other.max.z && max.z >= other.min.z;
		}

		public float SqrDistance(Vector3 point) => Vector3.SqrMagnitude(Vector3.Max(min - point, Vector3.Max(Vector3.zero, point - max)));

		public override string ToString() => $"Center: {center}, Extents: {extents}";
	}
}
