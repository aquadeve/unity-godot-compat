using Godot;

namespace UnityEngine
{
	public class Texture : Object
	{
		public virtual int width  => 0;
		public virtual int height => 0;

		public FilterMode filterMode { get; set; } = FilterMode.Bilinear;
		public TextureWrapMode wrapMode { get; set; } = TextureWrapMode.Repeat;
		public int anisoLevel { get; set; } = 1;

		public virtual Godot.Texture2D? godotTex => null;

		public static AnisotropicFiltering anisotropicFiltering { get; set; } = AnisotropicFiltering.Enable;
		public static int masterTextureLimit { get; set; } = 0;
	}
}

