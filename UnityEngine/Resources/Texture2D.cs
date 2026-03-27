using System;
using System.Collections.Generic;
using Godot;

namespace UnityEngine
{
	public class Texture2D : Texture
	{
		private Godot.Texture2D? _tex;

		public override int width  => _tex?.GetWidth()  ?? _width;
		public override int height => _tex?.GetHeight() ?? _height;
		private int _width;
		private int _height;

		public override Godot.Texture2D? godotTex => _tex;

		// Internal Godot texture for material usage
		public Godot.Texture2D? godot
		{
			get => _tex;
			set => _tex = value;
		}

		public TextureFormat format { get; private set; } = TextureFormat.RGBA32;
		public bool alphaIsTransparency { get; set; } = true;
		public int mipmapCount => 1;
		public bool isReadable { get; private set; } = true;

		// ---- Constructors ----
		public Texture2D(int width, int height, TextureFormat format = TextureFormat.RGBA32, bool mipmap = false, bool linear = false)
		{
			_width = width;
			_height = height;
			this.format = format;
			var img = Image.CreateEmpty(width, height, mipmap, Image.Format.Rgba8);
			var tex = new ImageTexture();
			tex.CreateFromImage(img);
			_tex = tex;
		}

		internal Texture2D(Godot.Texture2D godotTexture)
		{
			_tex = godotTexture;
		}

		internal Texture2D(byte[] data)
		{
			LoadRawTextureData(data);
		}

		// ---- Statics ----
		public static Texture2D? whiteTexture
		{
			get
			{
				var t = new Texture2D(4, 4, TextureFormat.RGBA32);
				t.SetPixels(new Color[] { Color.white, Color.white, Color.white, Color.white });
				t.Apply();
				return t;
			}
		}

		public static Texture2D? blackTexture
		{
			get
			{
				var t = new Texture2D(4, 4, TextureFormat.RGBA32);
				t.SetPixels(new Color[] { Color.black, Color.black, Color.black, Color.black });
				t.Apply();
				return t;
			}
		}

		// ---- Pixel operations ----
		private Color[]? _pixelData;

		public void SetPixel(int x, int y, Color color)
		{
			EnsurePixelData();
			int idx = y * _width + x;
			if (idx >= 0 && idx < _pixelData!.Length)
				_pixelData[idx] = color;
		}

		public Color GetPixel(int x, int y)
		{
			EnsurePixelData();
			int idx = y * _width + x;
			if (idx >= 0 && idx < _pixelData!.Length)
				return _pixelData[idx];
			return Color.black;
		}

		public Color GetPixelBilinear(float u, float v)
		{
			int x = Mathf.FloorToInt(u * (_width - 1));
			int y = Mathf.FloorToInt(v * (_height - 1));
			return GetPixel(x, y);
		}

		public Color[] GetPixels(int miplevel = 0)
		{
			EnsurePixelData();
			return _pixelData ?? Array.Empty<Color>();
		}

		public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, int miplevel = 0)
		{
			EnsurePixelData();
			var result = new Color[blockWidth * blockHeight];
			for (int j = 0; j < blockHeight; j++)
				for (int i = 0; i < blockWidth; i++)
					result[j * blockWidth + i] = GetPixel(x + i, y + j);
			return result;
		}

		public void SetPixels(Color[] colors, int miplevel = 0)
		{
			EnsurePixelData();
			int count = Math.Min(colors.Length, _pixelData!.Length);
			Array.Copy(colors, _pixelData, count);
		}

		public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, int miplevel = 0)
		{
			for (int j = 0; j < blockHeight; j++)
				for (int i = 0; i < blockWidth; i++)
				{
					int idx = j * blockWidth + i;
					if (idx < colors.Length)
						SetPixel(x + i, y + j, colors[idx]);
				}
		}

		public void Apply(bool updateMipmaps = true, bool makeNoLongerReadable = false)
		{
			if (_pixelData == null) return;
			var bytes = new byte[_pixelData.Length * 4];
			for (int i = 0; i < _pixelData.Length; i++)
			{
				bytes[i * 4]     = (byte)Mathf.RoundToInt(_pixelData[i].r * 255);
				bytes[i * 4 + 1] = (byte)Mathf.RoundToInt(_pixelData[i].g * 255);
				bytes[i * 4 + 2] = (byte)Mathf.RoundToInt(_pixelData[i].b * 255);
				bytes[i * 4 + 3] = (byte)Mathf.RoundToInt(_pixelData[i].a * 255);
			}
			var img = Image.CreateFromData(_width, _height, false, Image.Format.Rgba8, bytes);
			var tex = new ImageTexture();
			tex.CreateFromImage(img);
			_tex = tex;
			if (makeNoLongerReadable) isReadable = false;
		}

		public void LoadRawTextureData(byte[] data)
		{
			try
			{
				var img = new Image();
				var err = img.LoadJpgFromBuffer(data);
				if (err != Error.Ok)
					err = img.LoadPngFromBuffer(data);
				if (err == Error.Ok)
				{
					_width  = img.GetWidth();
					_height = img.GetHeight();
					var tex = new ImageTexture();
					tex.CreateFromImage(img);
					_tex = tex;
				}
				else
				{
					Debug.LogWarning("Texture2D.LoadRawTextureData: failed to load image data.");
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Texture2D.LoadRawTextureData error: {e.Message}");
			}
		}

		public byte[] GetRawTextureData()
		{
			if (_tex is ImageTexture it)
			{
				var img = it.GetImage();
				if (img != null)
					return img.GetData();
			}
			return Array.Empty<byte>();
		}

		public bool Resize(int width, int height, TextureFormat format = TextureFormat.RGBA32, bool hasMipMap = false)
		{
			_width = width;
			_height = height;
			this.format = format;
			_pixelData = null;
			var img = Image.CreateEmpty(width, height, hasMipMap, Image.Format.Rgba8);
			var tex = new ImageTexture();
			tex.CreateFromImage(img);
			_tex = tex;
			return true;
		}

		private void EnsurePixelData()
		{
			if (_pixelData == null)
				_pixelData = new Color[_width * _height];
		}

		// ---- Implicit conversions ----
		public static implicit operator Godot.Texture2D?(Texture2D? t) => t?._tex;

		public static implicit operator Texture2D?(Godot.Texture2D? t)
			=> t == null ? null : new Texture2D(t);

		public static bool GenerateAtlas(Vector2[] sizes, int padding, int atlasSize, List<Rect> results)
		{
			if (sizes == null) throw new ArgumentException("sizes cannot be null");
			if (results == null) throw new ArgumentException("results cannot be null");
			results.Clear();
			return sizes.Length == 0;
		}
	}
}

