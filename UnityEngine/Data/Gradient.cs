using System;

namespace UnityEngine
{
	/// <summary>
	/// Represents a color gradient. Stub for API compatibility.
	/// </summary>
	[Serializable]
	public class Gradient
	{
		private GradientColorKey[] _colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(Color.white, 0f),
			new GradientColorKey(Color.white, 1f)
		};

		private GradientAlphaKey[] _alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		};

		public GradientColorKey[] colorKeys
		{
			get => _colorKeys;
			set => _colorKeys = value;
		}

		public GradientAlphaKey[] alphaKeys
		{
			get => _alphaKeys;
			set => _alphaKeys = value;
		}

		public Color Evaluate(float time)
		{
			time = Mathf.Clamp01(time);

			// Find color
			Color color = Color.white;
			if (_colorKeys.Length >= 2)
			{
				for (int i = 0; i < _colorKeys.Length - 1; i++)
				{
					if (time >= _colorKeys[i].time && time <= _colorKeys[i + 1].time)
					{
						float t = (_colorKeys[i + 1].time - _colorKeys[i].time) > 0f
							? (time - _colorKeys[i].time) / (_colorKeys[i + 1].time - _colorKeys[i].time)
							: 0f;
						color = Color.Lerp(_colorKeys[i].color, _colorKeys[i + 1].color, t);
						break;
					}
				}
			}
			else if (_colorKeys.Length == 1)
			{
				color = _colorKeys[0].color;
			}

			// Find alpha
			float alpha = 1f;
			if (_alphaKeys.Length >= 2)
			{
				for (int i = 0; i < _alphaKeys.Length - 1; i++)
				{
					if (time >= _alphaKeys[i].time && time <= _alphaKeys[i + 1].time)
					{
						float t = (_alphaKeys[i + 1].time - _alphaKeys[i].time) > 0f
							? (time - _alphaKeys[i].time) / (_alphaKeys[i + 1].time - _alphaKeys[i].time)
							: 0f;
						alpha = Mathf.Lerp(_alphaKeys[i].alpha, _alphaKeys[i + 1].alpha, t);
						break;
					}
				}
			}
			else if (_alphaKeys.Length == 1)
			{
				alpha = _alphaKeys[0].alpha;
			}

			color.a = alpha;
			return color;
		}

		public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys)
		{
			_colorKeys = colorKeys;
			_alphaKeys = alphaKeys;
		}
	}

	[Serializable]
	public struct GradientColorKey
	{
		public Color color;
		public float time;

		public GradientColorKey(Color col, float time)
		{
			this.color = col;
			this.time = time;
		}
	}

	[Serializable]
	public struct GradientAlphaKey
	{
		public float alpha;
		public float time;

		public GradientAlphaKey(float alpha, float time)
		{
			this.alpha = alpha;
			this.time = time;
		}
	}
}
