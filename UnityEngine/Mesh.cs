using Godot;

namespace UnityEngine
{
	/// <summary>
	/// Represents a mesh asset for procedural or loaded geometry.
	/// Wraps Godot's ArrayMesh.
	/// </summary>
	public class Mesh : Object
	{
		private ArrayMesh _godot = new();
		private Godot.Collections.Array _meshArrays = new();
		private Godot.Mesh? _godotMesh;
		private bool _valid = false;

		public int[] triangles
		{
			set => _meshArrays[(int)Godot.Mesh.ArrayType.Index] = value;
			get => (int[])_meshArrays[(int)Godot.Mesh.ArrayType.Index];
		}

		public Vector3[] vertices
		{
			set => _meshArrays[(int)Godot.Mesh.ArrayType.Vertex] = ToGodotVector3Array(value);
			get => FromGodotVector3Array((Godot.Vector3[])_meshArrays[(int)Godot.Mesh.ArrayType.Vertex]);
		}

		public Vector2[] uv
		{
			set => _meshArrays[(int)Godot.Mesh.ArrayType.TexUV] = ToGodotVector2Array(value);
			get => FromGodotVector2Array((Godot.Vector2[])_meshArrays[(int)Godot.Mesh.ArrayType.TexUV]);
		}

		public Vector3[] normals
		{
			set => _meshArrays[(int)Godot.Mesh.ArrayType.Normal] = ToGodotVector3Array(value);
			get => FromGodotVector3Array((Godot.Vector3[])_meshArrays[(int)Godot.Mesh.ArrayType.Normal]);
		}

		public Color[] colors
		{
			set
			{
				var colors = new Godot.Color[value.Length];
				for (int i = 0; i < value.Length; i++) colors[i] = value[i];
				_meshArrays[(int)Godot.Mesh.ArrayType.Color] = colors;
			}
			get
			{
				var raw = (Godot.Color[])_meshArrays[(int)Godot.Mesh.ArrayType.Color];
				if (raw == null) return System.Array.Empty<Color>();
				var result = new Color[raw.Length];
				for (int i = 0; i < raw.Length; i++) result[i] = raw[i];
				return result;
			}
		}

		public Mesh()
		{
			_godot = new ArrayMesh();
			_meshArrays = new Godot.Collections.Array();
			_meshArrays.Resize((int)Godot.Mesh.ArrayType.Max);
		}

		/// <summary>Called to set an externally-loaded Godot mesh (e.g. from Resources.Load).</summary>
		public void SetGodotMesh(Godot.Mesh? m)
		{
			if (m == null)
			{
				Debug.LogWarning("Mesh.SetGodotMesh: mesh was null, ignoring.");
				return;
			}
			_godotMesh = m;
			_valid = true;
		}

		public Godot.Mesh GetGodotMesh()
		{
			if (!_valid)
			{
				_godot.AddSurfaceFromArrays(Godot.Mesh.PrimitiveType.Triangles, _meshArrays);
				_godotMesh = _godot;
				_valid = true;
			}
			return _godotMesh!;
		}

		public void RecalculateNormals()
		{
			// Godot 4 can generate normals via SurfaceTool
			Debug.LogWarning("Mesh.RecalculateNormals: not fully implemented.");
		}

		public void RecalculateBounds()
		{
			// Godot recomputes bounds automatically
		}

		public void Clear()
		{
			_godot = new ArrayMesh();
			_meshArrays = new Godot.Collections.Array();
			_meshArrays.Resize((int)Godot.Mesh.ArrayType.Max);
			_godotMesh = null;
			_valid = false;
		}

		private static Godot.Vector3[] ToGodotVector3Array(Vector3[] data)
		{
			var result = new Godot.Vector3[data.Length];
			for (int i = 0; i < data.Length; i++) result[i] = data[i];
			return result;
		}

		private static Vector3[] FromGodotVector3Array(Godot.Vector3[]? data)
		{
			if (data == null) return System.Array.Empty<Vector3>();
			var result = new Vector3[data.Length];
			for (int i = 0; i < data.Length; i++) result[i] = data[i];
			return result;
		}

		private static Godot.Vector2[] ToGodotVector2Array(Vector2[] data)
		{
			var result = new Godot.Vector2[data.Length];
			for (int i = 0; i < data.Length; i++) result[i] = data[i];
			return result;
		}

		private static Vector2[] FromGodotVector2Array(Godot.Vector2[]? data)
		{
			if (data == null) return System.Array.Empty<Vector2>();
			var result = new Vector2[data.Length];
			for (int i = 0; i < data.Length; i++) result[i] = data[i];
			return result;
		}
	}
}
