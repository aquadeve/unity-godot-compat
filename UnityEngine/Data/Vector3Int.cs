using System;

namespace UnityEngine
{
	/// <summary>
	/// Representation of 3D vectors and points using integers.
	/// </summary>
	public struct Vector3Int : IEquatable<Vector3Int>
	{
		public int x;
		public int y;
		public int z;

		public static readonly Vector3Int zero    = new Vector3Int(0, 0, 0);
		public static readonly Vector3Int one     = new Vector3Int(1, 1, 1);
		public static readonly Vector3Int up      = new Vector3Int(0, 1, 0);
		public static readonly Vector3Int down    = new Vector3Int(0, -1, 0);
		public static readonly Vector3Int left    = new Vector3Int(-1, 0, 0);
		public static readonly Vector3Int right   = new Vector3Int(1, 0, 0);
		public static readonly Vector3Int forward = new Vector3Int(0, 0, 1);
		public static readonly Vector3Int back    = new Vector3Int(0, 0, -1);

		public Vector3Int(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }

		public float magnitude => Mathf.Sqrt(x * x + y * y + z * z);
		public int sqrMagnitude => x * x + y * y + z * z;

		public int this[int index]
		{
			get => index switch { 0 => x, 1 => y, 2 => z, _ => throw new IndexOutOfRangeException() };
			set { switch (index) { case 0: x = value; break; case 1: y = value; break; case 2: z = value; break; default: throw new IndexOutOfRangeException(); } }
		}

		public void Set(int newX, int newY, int newZ) { x = newX; y = newY; z = newZ; }
		public void Clamp(Vector3Int min, Vector3Int max)
		{
			x = Math.Max(min.x, Math.Min(max.x, x));
			y = Math.Max(min.y, Math.Min(max.y, y));
			z = Math.Max(min.z, Math.Min(max.z, z));
		}

		public static float Distance(Vector3Int a, Vector3Int b) => (a - b).magnitude;
		public static Vector3Int Min(Vector3Int a, Vector3Int b) => new Vector3Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
		public static Vector3Int Max(Vector3Int a, Vector3Int b) => new Vector3Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
		public static Vector3Int Scale(Vector3Int a, Vector3Int b) => new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);

		public static Vector3Int FloorToInt(Vector3 v) => new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
		public static Vector3Int CeilToInt(Vector3 v)  => new Vector3Int(Mathf.CeilToInt(v.x),  Mathf.CeilToInt(v.y),  Mathf.CeilToInt(v.z));
		public static Vector3Int RoundToInt(Vector3 v) => new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));

		public static Vector3Int operator +(Vector3Int a, Vector3Int b) => new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		public static Vector3Int operator -(Vector3Int a, Vector3Int b) => new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
		public static Vector3Int operator *(Vector3Int a, Vector3Int b) => new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
		public static Vector3Int operator *(Vector3Int a, int b) => new Vector3Int(a.x * b, a.y * b, a.z * b);
		public static Vector3Int operator *(int a, Vector3Int b) => new Vector3Int(a * b.x, a * b.y, a * b.z);
		public static Vector3Int operator /(Vector3Int a, int b) => new Vector3Int(a.x / b, a.y / b, a.z / b);
		public static Vector3Int operator -(Vector3Int a) => new Vector3Int(-a.x, -a.y, -a.z);

		public static bool operator ==(Vector3Int a, Vector3Int b) => a.x == b.x && a.y == b.y && a.z == b.z;
		public static bool operator !=(Vector3Int a, Vector3Int b) => !(a == b);

		public static implicit operator Vector3(Vector3Int v) => new Vector3(v.x, v.y, v.z);
		public static explicit operator Vector3Int(Vector3 v) => new Vector3Int((int)v.x, (int)v.y, (int)v.z);

		public bool Equals(Vector3Int other) => x == other.x && y == other.y && z == other.z;
		public override bool Equals(object? obj) => obj is Vector3Int v && Equals(v);
		public override int GetHashCode() => x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
		public override string ToString() => $"({x}, {y}, {z})";
	}
}
