using Godot;
using System;

namespace UnityEngine
{
	public static partial class Resources
	{
		/// <summary>
		/// Root path for resources in the Godot project. Override before loading assets.
		/// </summary>
		public static string ResourceRoot = "res://Resources/";

		/// <summary>
		/// Default texture extension for Load&lt;Texture2D&gt;.
		/// </summary>
		public static string TextureExtension = ".jpg";

		/// <summary>
		/// Default mesh extension for Load&lt;Mesh&gt;.
		/// </summary>
		public static string MeshExtension = ".obj";

		/// <summary>
		/// Default shader extension for Load&lt;Shader&gt;.
		/// </summary>
		public static string ShaderExtension = ".gdshader";

		public static T? Load<T>(string path) where T : Object
		{
			if (typeof(T) == typeof(Texture2D))
			{
				var godotTex = GD.Load<Godot.Texture2D>(ResourceRoot + path + TextureExtension);
				if (godotTex == null) godotTex = GD.Load<Godot.Texture2D>(path); // try absolute
				return godotTex != null ? (T)(Object)new Texture2D(godotTex) : null;
			}

			if (typeof(T) == typeof(Mesh))
			{
				var godotMesh = GD.Load<Godot.Mesh>(ResourceRoot + path + MeshExtension);
				if (godotMesh == null) godotMesh = GD.Load<Godot.Mesh>(path);
				if (godotMesh == null) return null;
				var m = new Mesh();
				m.SetGodotMesh(godotMesh);
				return (T)(Object)m;
			}

			if (typeof(T) == typeof(Shader))
			{
				var godotShader = GD.Load<Godot.Shader>(ResourceRoot + path + ShaderExtension);
				if (godotShader == null) godotShader = GD.Load<Godot.Shader>(path);
				return godotShader != null ? (T)(Object)new Shader { godot = godotShader } : null;
			}

			if (typeof(T) == typeof(Material))
			{
				var godotMat = GD.Load<Godot.ShaderMaterial>(ResourceRoot + path + ".tres");
				if (godotMat == null) godotMat = GD.Load<Godot.ShaderMaterial>(path);
				if (godotMat == null) return null;
				var shader = new Shader { godot = godotMat.Shader };
				var mat = new Material(shader);
				mat.godot = godotMat;
				return (T)(Object)mat;
			}

			Debug.LogError($"Resources.Load<{typeof(T).Name}>(\"{path}\"): type not supported.");
			return null;
		}

		public static AsyncOperation UnloadUnusedAssets()
		{
			var asyncOp = new AsyncOperation { isDone = true };
			return asyncOp;
		}

		public static void UnloadAsset(Object asset) { }
	}
}
