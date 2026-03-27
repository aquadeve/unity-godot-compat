using System;
using Godot;

namespace UnityEngine
{
	public class Object
	{
		public virtual string name { get; set; } = "";

		public static void Destroy(Object obj, float t = 0f)
		{
			if (obj is Node node)
				node.QueueFree();
		}

		public static void DestroyImmediate(Object obj)
		{
			if (obj is Node node)
				node.Free();
		}

		public static void DontDestroyOnLoad(Object obj)
		{
			// Not fully supported in Godot; no-op as closest equivalent
		}

		public static T Instantiate<T>(T original) where T : Object, new()
		{
			// Basic clone - only works for simple objects
			throw new NotImplementedException("Instantiate not fully implemented.");
		}

		public static bool operator ==(Object? a, Object? b)
		{
			if (a is null && b is null) return true;
			if (a is null || b is null) return false;
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(Object? a, Object? b) => !(a == b);

		public override bool Equals(object? other)
		{
			if (other is Object o) return this == o;
			return false;
		}

		public override int GetHashCode() => base.GetHashCode();

		public override string ToString() => name;

		// Implicit bool conversion: null Object == false
		public static implicit operator bool(Object? obj) => obj is not null;
	}
}