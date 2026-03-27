using System;

namespace UnityEngine
{
	public sealed class Random
	{
		private static System.Random _rng = new System.Random();

		//
		// Static Properties
		//
		public static float value
		{
			get { return (float)_rng.NextDouble(); }
		}

		public static Vector2 insideUnitCircle
		{
			get
			{
				float angle = value * Mathf.PI * 2f;
				float r = Mathf.Sqrt(value);
				return new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
			}
		}

		public static Vector3 insideUnitSphere
		{
			get
			{
				float theta = value * Mathf.PI * 2f;
				float phi = Mathf.Acos(2f * value - 1f);
				float r = Mathf.Pow(value, 1f / 3f);
				float sinPhi = Mathf.Sin(phi);
				return new Vector3(
					r * sinPhi * Mathf.Cos(theta),
					r * sinPhi * Mathf.Sin(theta),
					r * Mathf.Cos(phi));
			}
		}

		public static Vector3 onUnitSphere
		{
			get
			{
				float theta = value * Mathf.PI * 2f;
				float phi = Mathf.Acos(2f * value - 1f);
				float sinPhi = Mathf.Sin(phi);
				return new Vector3(
					sinPhi * Mathf.Cos(theta),
					sinPhi * Mathf.Sin(theta),
					Mathf.Cos(phi));
			}
		}

		public static Quaternion rotation
		{
			get
			{
				return Quaternion.Euler(value * 360f, value * 360f, value * 360f);
			}
		}

		public static Quaternion rotationUniform
		{
			get
			{
				float u1 = value;
				float u2 = value;
				float u3 = value;
				float sqrt1MinusU1 = Mathf.Sqrt(1f - u1);
				float sqrtU1 = Mathf.Sqrt(u1);
				return new Quaternion(
					sqrt1MinusU1 * Mathf.Sin(2f * Mathf.PI * u2),
					sqrt1MinusU1 * Mathf.Cos(2f * Mathf.PI * u2),
					sqrtU1 * Mathf.Sin(2f * Mathf.PI * u3),
					sqrtU1 * Mathf.Cos(2f * Mathf.PI * u3));
			}
		}

		public static Random.State state { get; set; }

		//
		// Static Methods
		//
		public static void InitState(int seed)
		{
			_rng = new System.Random(seed);
		}

		public static float Range(float min, float max)
		{
			return min + (float)_rng.NextDouble() * (max - min);
		}

		public static int Range(int min, int max)
		{
			return _rng.Next(min, max);
		}

		[Obsolete("Use Random.Range instead")]
		public static float RandomRange(float min, float max) => Range(min, max);

		[Obsolete("Use Random.Range instead")]
		public static int RandomRange(int min, int max) => Range(min, max);

		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
		{
			float h = Mathf.Lerp(hueMin, hueMax, Random.value);
			float s = Mathf.Lerp(saturationMin, saturationMax, Random.value);
			float v = Mathf.Lerp(valueMin, valueMax, Random.value);
			Color result = Color.HSVToRGB(h, s, v, true);
			result.a = Mathf.Lerp(alphaMin, alphaMax, Random.value);
			return result;
		}

		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
			=> ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f);

		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
			=> ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 0f, 1f, 1f, 1f);

		public static Color ColorHSV(float hueMin, float hueMax)
			=> ColorHSV(hueMin, hueMax, 0f, 1f, 0f, 1f, 1f, 1f);

		public static Color ColorHSV()
			=> ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);

		//
		// Nested Types
		//
		[Serializable]
		public struct State
		{
			[SerializeField]
			private int s0;

			[SerializeField]
			private int s1;

			[SerializeField]
			private int s2;

			[SerializeField]
			private int s3;
		}
	}
}
