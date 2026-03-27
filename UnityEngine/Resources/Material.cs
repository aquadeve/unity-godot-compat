using Godot;

namespace UnityEngine
{
	public class Material : Object
	{
		public Godot.ShaderMaterial? godot;
		private Shader? _shader;
		private Color _color = Color.white;
		private Texture2D? _mainTexture;

		// ---- Constructors ----
		public Material(Shader shader)
		{
			_shader = shader;
			godot = new Godot.ShaderMaterial();
			if (shader.godot != null)
				godot.Shader = shader.godot;
		}

		public Material(Material source)
		{
			_shader = source._shader;
			_color = source._color;
			_mainTexture = source._mainTexture;
			godot = (Godot.ShaderMaterial?)source.godot?.Duplicate();
		}

		// ---- Shader ----
		public Shader? shader
		{
			get => _shader;
			set
			{
				_shader = value;
				if (godot != null && value?.godot != null)
					godot.Shader = value.godot;
			}
		}

		// ---- Color ----
		public Color color
		{
			get => _color;
			set
			{
				_color = value;
				godot?.SetShaderParameter("_Color", value.toGodot());
			}
		}

		// ---- Textures ----
		public Texture2D? mainTexture
		{
			get => _mainTexture;
			set
			{
				_mainTexture = value;
				if (value != null)
					godot?.SetShaderParameter("_MainTex", value.godotTex);
			}
		}

		public Vector2 mainTextureOffset { get; set; } = Vector2.zero;
		public Vector2 mainTextureScale  { get; set; } = Vector2.one;

		// ---- Shader parameters ----
		public void SetTexture(string name, Texture? tex)
		{
			if (tex != null)
				godot?.SetShaderParameter(name, tex.godotTex);
		}

		public void SetFloat(string name, float value)
			=> godot?.SetShaderParameter(name, value);

		public void SetInt(string name, int value)
			=> godot?.SetShaderParameter(name, value);

		public void SetColor(string name, Color value)
			=> godot?.SetShaderParameter(name, value.toGodot());

		public void SetVector(string name, Vector4 value)
			=> godot?.SetShaderParameter(name, value.toGodot());

		public void SetVector(string name, Vector3 value)
			=> godot?.SetShaderParameter(name, (Godot.Vector3)value);

		public void SetMatrix(string name, Matrix4x4 value)
		{
			// Not directly supported; log warning
			Debug.LogWarning($"Material.SetMatrix(\"{name}\"): not implemented.");
		}

		public float GetFloat(string name)
		{
			var v = godot?.GetShaderParameter(name);
			return v.HasValue && v.Value.VariantType == Variant.Type.Float ? v.Value.AsSingle() : 0f;
		}

		public Color GetColor(string name)
		{
			var v = godot?.GetShaderParameter(name);
			if (v.HasValue && v.Value.VariantType == Variant.Type.Color)
			{
				var c = v.Value.AsColor();
				return new Color(c.R, c.G, c.B, c.A);
			}
			return Color.black;
		}

		public bool HasProperty(string name) => true; // Can't easily query Godot shader params

		public void EnableKeyword(string keyword) { }
		public void DisableKeyword(string keyword) { }
		public bool IsKeywordEnabled(string keyword) => false;

		public int renderQueue { get; set; } = 2000;
	}
}
