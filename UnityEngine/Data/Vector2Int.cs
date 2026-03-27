using System;

namespace UnityEngine
{
	/// <summary>
	/// Representation of 2D vectors and points using integers.
	/// </summary>
	public struct Vector2Int : IEquatable<Vector2Int>
	{
		public int x;
		public int y;

		public static readonly Vector2Int zero  = new Vector2Int(0, 0);
		public static readonly Vector2Int one   = new Vector2Int(1, 1);
		public static readonly Vector2Int up    = new Vector2Int(0, 1);
		public static readonly Vector2Int down  = new Vector2Int(0, -1);
		public static readonly Vector2Int left  = new Vector2Int(-1, 0);
		public static readonly Vector2Int right = new Vector2Int(1, 0);

		public Vector2Int(int x, int y) { this.x = x; this.y = y; }

		public float magnitude => Mathf.Sqrt(x * x + y * y);
		public int sqrMagnitude => x * x + y * y;

		public int this[int index]
		{
			get => index switch { 0 => x, 1 => y, _ => throw new IndexOutOfRangeException() };
			set { switch (index) { case 0: x = value; break; case 1: y = value; break; default: throw new IndexOutOfRangeException(); } }
		}

		public void Set(int newX, int newY) { x = newX; y = newY; }
		public void Clamp(Vector2Int min, Vector2Int max)
		{
			x = Math.Max(min.x, Math.Min(max.x, x));
			y = Math.Max(min.y, Math.Min(max.y, y));
		}

		public static float Distance(Vector2Int a, Vector2Int b) => (a - b).magnitude;
		public static Vector2Int Min(Vector2Int a, Vector2Int b) => new Vector2Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
		public static Vector2Int Max(Vector2Int a, Vector2Int b) => new Vector2Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
		public static Vector2Int Scale(Vector2Int a, Vector2Int b) => new Vector2Int(a.x * b.x, a.y * b.y);

		public static Vector2Int FloorToInt(Vector2 v) => new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
		public static Vector2Int CeilToInt(Vector2 v)  => new Vector2Int(Mathf.CeilToInt(v.x),  Mathf.CeilToInt(v.y));
		public static Vector2Int RoundToInt(Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));

		public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.x + b.x, a.y + b.y);
		public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
		public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new Vector2Int(a.x * b.x, a.y * b.y);
		public static Vector2Int operator *(Vector2Int a, int b) => new Vector2Int(a.x * b, a.y * b);
		public static Vector2Int operator *(int a, Vector2Int b) => new Vector2Int(a * b.x, a * b.y);
		public static Vector2Int operator /(Vector2Int a, int b) => new Vector2Int(a.x / b, a.y / b);
		public static Vector2Int operator -(Vector2Int a) => new Vector2Int(-a.x, -a.y);

		public static bool operator ==(Vector2Int a, Vector2Int b) => a.x == b.x && a.y == b.y;
		public static bool operator !=(Vector2Int a, Vector2Int b) => !(a == b);

		public static implicit operator Vector2(Vector2Int v) => new Vector2(v.x, v.y);
		public static explicit operator Vector2Int(Vector2 v) => new Vector2Int((int)v.x, (int)v.y);

		public bool Equals(Vector2Int other) => x == other.x && y == other.y;
		public override bool Equals(object? obj) => obj is Vector2Int v && Equals(v);
		public override int GetHashCode() => x.GetHashCode() ^ (y.GetHashCode() << 2);
		public override string ToString() => $"({x}, {y})";
	}
}
