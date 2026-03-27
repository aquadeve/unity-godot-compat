using System;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public struct Mathf
	{
		//
		// Static Fields
		//
		public const float PI = 3.14159274f;

		public const float Infinity = float.PositiveInfinity;

		public const float NegativeInfinity = float.NegativeInfinity;

		public const float Deg2Rad = 0.0174532924f;

		public const float Rad2Deg = 57.29578f;

		public static readonly float Epsilon = (!MathfInternal.IsFlushToZeroEnabled) ? MathfInternal.FloatMinDenormal : MathfInternal.FloatMinNormal;

		//
		// Static Methods
		//
		public static int Abs (int value)
		{
			return Math.Abs (value);
		}

		public static float Abs (float f)
		{
			return Math.Abs (f);
		}

		public static float Acos (float f)
		{
			return (float)Math.Acos ((double)f);
		}

		public static bool Approximately (float a, float b)
		{
			return Mathf.Abs (b - a) < Mathf.Max (1E-06f * Mathf.Max (Mathf.Abs (a), Mathf.Abs (b)), Mathf.Epsilon * 8f);
		}

		public static float Asin (float f)
		{
			return (float)Math.Asin ((double)f);
		}

		public static float Atan (float f)
		{
			return (float)Math.Atan ((double)f);
		}

		public static float Atan2 (float y, float x)
		{
			return (float)Math.Atan2 ((double)y, (double)x);
		}

		public static float Ceil (float f)
		{
			return (float)Math.Ceiling ((double)f);
		}

		public static int CeilToInt (float f)
		{
			return (int)Math.Ceiling ((double)f);
		}

		public static float Clamp (float value, float min, float max)
		{
			if (value < min) {
				value = min;
			} else if (value > max) {
				value = max;
			}
			return value;
		}

		public static int Clamp (int value, int min, int max)
		{
			if (value < min) {
				value = min;
			} else if (value > max) {
				value = max;
			}
			return value;
		}

		public static float Clamp01 (float value)
		{
			float result;
			if (value < 0f) {
				result = 0f;
			} else if (value > 1f) {
				result = 1f;
			} else {
				result = value;
			}
			return result;
		}

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		public static int ClosestPowerOfTwo (int value)
		{
			if (value <= 0) return 1;
			int prev = 1; int next = 1;
			while (next < value) { prev = next; next <<= 1; }
			return (value - prev) < (next - value) ? prev : next;
		}

		public static Color CorrelatedColorTemperatureToRGB (float kelvin)
		{
			// Tanner Helland approximation
			float temp = kelvin / 100f;
			float r, g, b;
			if (temp <= 66f) { r = 1f; g = Clamp01((float)(0.39008157876901960784 * Math.Log(temp) - 0.63184144378862745098)); }
			else { r = Clamp01((float)(1.29293618606274509804 * Math.Pow(temp - 60, -0.1332047592))); g = Clamp01((float)(1.12989086089529411765 * Math.Pow(temp - 60, -0.0755148492))); }
			if (temp >= 66f) b = 1f;
			else if (temp <= 19f) b = 0f;
			else b = Clamp01((float)(0.54320678911019607843 * Math.Log(temp - 10) - 1.19625408914));
			return new Color(r, g, b, 1f);
		}

		public static float Cos (float f)
		{
			return (float)Math.Cos ((double)f);
		}

		public static float DeltaAngle (float current, float target)
		{
			float num = Mathf.Repeat (target - current, 360f);
			if (num > 180f) {
				num -= 360f;
			}
			return num;
		}

		public static float Exp (float power)
		{
			return (float)Math.Exp ((double)power);
		}

		public static ushort FloatToHalf (float val)
		{
			// Simple half-float conversion
			uint bits = BitConverter.SingleToUInt32Bits(val);
			uint sign = (bits >> 31) & 1;
			int exp = (int)((bits >> 23) & 0xFF) - 127 + 15;
			uint mant = (bits >> 13) & 0x3FF;
			if (exp <= 0) return (ushort)(sign << 15);
			if (exp >= 31) return (ushort)((sign << 15) | 0x7C00);
			return (ushort)((sign << 15) | (exp << 10) | mant);
		}

		public static float Floor (float f)
		{
			return (float)Math.Floor ((double)f);
		}

		public static int FloorToInt (float f)
		{
			return (int)Math.Floor ((double)f);
		}

		public static float Gamma (float value, float absmax, float gamma)
		{
			bool flag = false;
			if (value < 0f) {
				flag = true;
			}
			float num = Mathf.Abs (value);
			float result;
			if (num > absmax) {
				result = ((!flag) ? num : (-num));
			} else {
				float num2 = Mathf.Pow (num / absmax, gamma) * absmax;
				result = ((!flag) ? num2 : (-num2));
			}
			return result;
		}

		public static float GammaToLinearSpace (float value)
		{
			if (value <= 0.04045f) return value / 12.92f;
			return Pow((value + 0.055f) / 1.055f, 2.4f);
		}

		public static float HalfToFloat (ushort val)
		{
			uint sign = (uint)((val >> 15) & 1);
			int exp = (val >> 10) & 0x1F;
			uint mant = (uint)(val & 0x3FF);
			if (exp == 0) { if (mant == 0) return sign == 0 ? 0f : -0f; exp = 1; }
			else if (exp == 31) return sign == 0 ? float.PositiveInfinity : float.NegativeInfinity;
			uint bits = (sign << 31) | (((uint)(exp - 15 + 127)) << 23) | (mant << 13);
			return BitConverter.UInt32BitsToSingle(bits);
		}

		private static void INTERNAL_CALL_CorrelatedColorTemperatureToRGB (float kelvin, out Color value)
		{
			value = CorrelatedColorTemperatureToRGB(kelvin);
		}

		public static float InverseLerp (float a, float b, float value)
		{
			float result;
			if (a != b) {
				result = Mathf.Clamp01 ((value - a) / (b - a));
			} else {
				result = 0f;
			}
			return result;
		}

		public static bool IsPowerOfTwo (int value)
		{
			return value > 0 && (value & (value - 1)) == 0;
		}

		public static float Lerp (float a, float b, float t)
		{
			return a + (b - a) * Mathf.Clamp01 (t);
		}

		public static float LerpAngle (float a, float b, float t)
		{
			float num = Mathf.Repeat (b - a, 360f);
			if (num > 180f) {
				num -= 360f;
			}
			return a + num * Mathf.Clamp01 (t);
		}

		public static float LerpUnclamped (float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		public static float LinearToGammaSpace (float value)
		{
			if (value <= 0.0031308f) return 12.92f * value;
			return 1.055f * Pow(value, 1f / 2.4f) - 0.055f;
		}

		internal static bool LineIntersection (Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num * num4 - num2 * num3;
			bool result2;
			if (num5 == 0f) {
				result2 = false;
			} else {
				float num6 = p3.x - p1.x;
				float num7 = p3.y - p1.y;
				float num8 = (num6 * num4 - num7 * num3) / num5;
				result = new Vector2 (p1.x + num8 * num, p1.y + num8 * num2);
				result2 = true;
			}
			return result2;
		}

		internal static bool LineSegmentIntersection (Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num * num4 - num2 * num3;
			bool result2;
			if (num5 == 0f) {
				result2 = false;
			} else {
				float num6 = p3.x - p1.x;
				float num7 = p3.y - p1.y;
				float num8 = (num6 * num4 - num7 * num3) / num5;
				if (num8 < 0f || num8 > 1f) {
					result2 = false;
				} else {
					float num9 = (num6 * num2 - num7 * num) / num5;
					if (num9 < 0f || num9 > 1f) {
						result2 = false;
					} else {
						result = new Vector2 (p1.x + num8 * num, p1.y + num8 * num2);
						result2 = true;
					}
				}
			}
			return result2;
		}

		public static float Log (float f, float p)
		{
			return (float)Math.Log ((double)f, (double)p);
		}

		public static float Log (float f)
		{
			return (float)Math.Log ((double)f);
		}

		public static float Log10 (float f)
		{
			return (float)Math.Log10 ((double)f);
		}

		public static float Max (float a, float b)
		{
			return (a <= b) ? b : a;
		}

		public static int Max (int a, int b)
		{
			return (a <= b) ? b : a;
		}

		public static int Max (params int[] values)
		{
			int num = values.Length;
			int result;
			if (num == 0) {
				result = 0;
			} else {
				int num2 = values [0];
				for (int i = 1; i < num; i++) {
					if (values [i] > num2) {
						num2 = values [i];
					}
				}
				result = num2;
			}
			return result;
		}

		public static float Max (params float[] values)
		{
			int num = values.Length;
			float result;
			if (num == 0) {
				result = 0f;
			} else {
				float num2 = values [0];
				for (int i = 1; i < num; i++) {
					if (values [i] > num2) {
						num2 = values [i];
					}
				}
				result = num2;
			}
			return result;
		}

		public static float Min (float a, float b)
		{
			return (a >= b) ? b : a;
		}

		public static int Min (params int[] values)
		{
			int num = values.Length;
			int result;
			if (num == 0) {
				result = 0;
			} else {
				int num2 = values [0];
				for (int i = 1; i < num; i++) {
					if (values [i] < num2) {
						num2 = values [i];
					}
				}
				result = num2;
			}
			return result;
		}

		public static int Min (int a, int b)
		{
			return (a >= b) ? b : a;
		}

		public static float Min (params float[] values)
		{
			int num = values.Length;
			float result;
			if (num == 0) {
				result = 0f;
			} else {
				float num2 = values [0];
				for (int i = 1; i < num; i++) {
					if (values [i] < num2) {
						num2 = values [i];
					}
				}
				result = num2;
			}
			return result;
		}

		public static float MoveTowards (float current, float target, float maxDelta)
		{
			float result;
			if (Mathf.Abs (target - current) <= maxDelta) {
				result = target;
			} else {
				result = current + Mathf.Sign (target - current) * maxDelta;
			}
			return result;
		}

		public static float MoveTowardsAngle (float current, float target, float maxDelta)
		{
			float num = Mathf.DeltaAngle (current, target);
			float result;
			if (-maxDelta < num && num < maxDelta) {
				result = target;
			} else {
				target = current + num;
				result = Mathf.MoveTowards (current, target, maxDelta);
			}
			return result;
		}

		public static int NextPowerOfTwo (int value)
		{
			if (value <= 0) return 1;
			value--;
			value |= value >> 1; value |= value >> 2;
			value |= value >> 4; value |= value >> 8;
			value |= value >> 16;
			return value + 1;
		}

		public static float PerlinNoise (float x, float y)
		{
			// Simple hash-based approximation (not true Perlin but functional)
			return (float)(0.5 + 0.5 * Math.Sin(x * 1.2f + Math.Cos(y * 0.7f) * 3.1f));
		}

		public static float PingPong (float t, float length)
		{
			t = Mathf.Repeat (t, length * 2f);
			return length - Mathf.Abs (t - length);
		}

		public static float Pow (float f, float p)
		{
			return (float)Math.Pow ((double)f, (double)p);
		}

		internal static long RandomToLong (System.Random r)
		{
			byte[] array = new byte[8];
			r.NextBytes (array);
			return (long)(BitConverter.ToUInt64 (array, 0) & 9223372036854775807uL);
		}

		public static float Repeat (float t, float length)
		{
			return Mathf.Clamp (t - Mathf.Floor (t / length) * length, 0f, length);
		}

		public static float Round (float f)
		{
			return (float)Math.Round ((double)f);
		}

		public static int RoundToInt (float f)
		{
			return (int)Math.Round ((double)f);
		}

		public static float Sign (float f)
		{
			return (f < 0f) ? -1f : 1f;
		}

		public static float Sin (float f)
		{
			return (float)Math.Sin ((double)f);
		}

		[ExcludeFromDocs]
		public static float SmoothDamp (float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Mathf.SmoothDamp (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static float SmoothDamp (float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return Mathf.SmoothDamp (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothDamp (float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue ("Mathf.Infinity")] float maxSpeed, [DefaultValue ("Time.deltaTime")] float deltaTime)
		{
			smoothTime = Mathf.Max (0.0001f, smoothTime);
			float num = 2f / smoothTime;
			float num2 = num * deltaTime;
			float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
			float num4 = current - target;
			float num5 = target;
			float num6 = maxSpeed * smoothTime;
			num4 = Mathf.Clamp (num4, -num6, num6);
			target = current - num4;
			float num7 = (currentVelocity + num * num4) * deltaTime;
			currentVelocity = (currentVelocity - num * num7) * num3;
			float num8 = target + (num4 + num7) * num3;
			if (num5 - current > 0f == num8 > num5) {
				num8 = num5;
				currentVelocity = (num8 - num5) / deltaTime;
			}
			return num8;
		}

		[ExcludeFromDocs]
		public static float SmoothDampAngle (float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Mathf.SmoothDampAngle (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static float SmoothDampAngle (float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return Mathf.SmoothDampAngle (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothDampAngle (float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue ("Mathf.Infinity")] float maxSpeed, [DefaultValue ("Time.deltaTime")] float deltaTime)
		{
			target = current + Mathf.DeltaAngle (current, target);
			return Mathf.SmoothDamp (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static float SmoothStep (float from, float to, float t)
		{
			t = Mathf.Clamp01 (t);
			t = -2f * t * t * t + 3f * t * t;
			return to * t + from * (1f - t);
		}

		public static float Sqrt (float f)
		{
			return (float)Math.Sqrt ((double)f);
		}

		public static float Tan (float f)
		{
			return (float)Math.Tan ((double)f);
		}
	}
}
