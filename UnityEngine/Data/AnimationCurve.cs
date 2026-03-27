using System;
using System.Collections.Generic;

namespace UnityEngine
{
	/// <summary>
	/// Store a collection of Keyframes that can be evaluated over time.
	/// Stub implementation for API compatibility.
	/// </summary>
	[Serializable]
	public class AnimationCurve
	{
		private readonly List<Keyframe> _keys = new();

		public AnimationCurve() { }

		public AnimationCurve(params Keyframe[] keys)
		{
			_keys.AddRange(keys);
			_keys.Sort((a, b) => a.time.CompareTo(b.time));
		}

		public Keyframe[] keys
		{
			get => _keys.ToArray();
			set { _keys.Clear(); _keys.AddRange(value); _keys.Sort((a, b) => a.time.CompareTo(b.time)); }
		}

		public int length => _keys.Count;

		public float Evaluate(float time)
		{
			if (_keys.Count == 0) return 0f;
			if (_keys.Count == 1) return _keys[0].value;
			if (time <= _keys[0].time) return _keys[0].value;
			if (time >= _keys[_keys.Count - 1].time) return _keys[_keys.Count - 1].value;

			for (int i = 0; i < _keys.Count - 1; i++)
			{
				if (time >= _keys[i].time && time <= _keys[i + 1].time)
				{
					float t = (time - _keys[i].time) / (_keys[i + 1].time - _keys[i].time);
					// Simple hermite interpolation
					float t2 = t * t;
					float t3 = t2 * t;
					float h1 = 2f * t3 - 3f * t2 + 1f;
					float h2 = -2f * t3 + 3f * t2;
					return h1 * _keys[i].value + h2 * _keys[i + 1].value;
				}
			}
			return _keys[_keys.Count - 1].value;
		}

		public int AddKey(float time, float value)
		{
			var key = new Keyframe(time, value);
			_keys.Add(key);
			_keys.Sort((a, b) => a.time.CompareTo(b.time));
			return _keys.IndexOf(key);
		}

		public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return new AnimationCurve(new Keyframe(timeStart, valueStart), new Keyframe(timeEnd, valueEnd));
		}

		public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return new AnimationCurve(
				new Keyframe(timeStart, valueStart, 0f, 0f),
				new Keyframe(timeEnd, valueEnd, 0f, 0f));
		}

		public static AnimationCurve Constant(float timeStart, float timeEnd, float value)
		{
			return new AnimationCurve(
				new Keyframe(timeStart, value, 0f, 0f),
				new Keyframe(timeEnd, value, 0f, 0f));
		}
	}

	/// <summary>
	/// A single keyframe that can be injected into an animation curve.
	/// </summary>
	[Serializable]
	public struct Keyframe
	{
		public float time;
		public float value;
		public float inTangent;
		public float outTangent;

		public Keyframe(float time, float value)
		{
			this.time = time;
			this.value = value;
			this.inTangent = 0f;
			this.outTangent = 0f;
		}

		public Keyframe(float time, float value, float inTangent, float outTangent)
		{
			this.time = time;
			this.value = value;
			this.inTangent = inTangent;
			this.outTangent = outTangent;
		}
	}
}
