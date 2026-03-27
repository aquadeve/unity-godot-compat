using System;

namespace UnityEngine
{
	/// <summary>A 4x4 matrix for transformations.</summary>
	public struct Matrix4x4
	{
		public float m00, m01, m02, m03;
		public float m10, m11, m12, m13;
		public float m20, m21, m22, m23;
		public float m30, m31, m32, m33;

		public static readonly Matrix4x4 identity = new Matrix4x4
		{
			m00 = 1, m11 = 1, m22 = 1, m33 = 1
		};

		public static readonly Matrix4x4 zero = new Matrix4x4();

		public Vector4 GetColumn(int index)
		{
			return index switch
			{
				0 => new Vector4(m00, m10, m20, m30),
				1 => new Vector4(m01, m11, m21, m31),
				2 => new Vector4(m02, m12, m22, m32),
				3 => new Vector4(m03, m13, m23, m33),
				_ => throw new IndexOutOfRangeException()
			};
		}

		public Vector4 GetRow(int index)
		{
			return index switch
			{
				0 => new Vector4(m00, m01, m02, m03),
				1 => new Vector4(m10, m11, m12, m13),
				2 => new Vector4(m20, m21, m22, m23),
				3 => new Vector4(m30, m31, m32, m33),
				_ => throw new IndexOutOfRangeException()
			};
		}

		public Vector3 MultiplyPoint(Vector3 point)
		{
			float w = m30 * point.x + m31 * point.y + m32 * point.z + m33;
			if (Math.Abs(w) < float.Epsilon) return Vector3.zero;
			return new Vector3(
				(m00 * point.x + m01 * point.y + m02 * point.z + m03) / w,
				(m10 * point.x + m11 * point.y + m12 * point.z + m13) / w,
				(m20 * point.x + m21 * point.y + m22 * point.z + m23) / w);
		}

		public Vector3 MultiplyVector(Vector3 vector)
			=> new Vector3(
				m00 * vector.x + m01 * vector.y + m02 * vector.z,
				m10 * vector.x + m11 * vector.y + m12 * vector.z,
				m20 * vector.x + m21 * vector.y + m22 * vector.z);

		public static Matrix4x4 TRS(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			var m = identity;
			// Rotation matrix from quaternion
			float x = rot.x, y = rot.y, z = rot.z, w = rot.w;
			m.m00 = (1 - 2 * (y * y + z * z)) * scale.x;
			m.m01 = 2 * (x * y - w * z) * scale.y;
			m.m02 = 2 * (x * z + w * y) * scale.z;
			m.m10 = 2 * (x * y + w * z) * scale.x;
			m.m11 = (1 - 2 * (x * x + z * z)) * scale.y;
			m.m12 = 2 * (y * z - w * x) * scale.z;
			m.m20 = 2 * (x * z - w * y) * scale.x;
			m.m21 = 2 * (y * z + w * x) * scale.y;
			m.m22 = (1 - 2 * (x * x + y * y)) * scale.z;
			m.m03 = pos.x; m.m13 = pos.y; m.m23 = pos.z; m.m33 = 1;
			return m;
		}

		public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
		{
			var m = new Matrix4x4();
			m.m00 = a.m00*b.m00 + a.m01*b.m10 + a.m02*b.m20 + a.m03*b.m30;
			m.m01 = a.m00*b.m01 + a.m01*b.m11 + a.m02*b.m21 + a.m03*b.m31;
			m.m02 = a.m00*b.m02 + a.m01*b.m12 + a.m02*b.m22 + a.m03*b.m32;
			m.m03 = a.m00*b.m03 + a.m01*b.m13 + a.m02*b.m23 + a.m03*b.m33;
			m.m10 = a.m10*b.m00 + a.m11*b.m10 + a.m12*b.m20 + a.m13*b.m30;
			m.m11 = a.m10*b.m01 + a.m11*b.m11 + a.m12*b.m21 + a.m13*b.m31;
			m.m12 = a.m10*b.m02 + a.m11*b.m12 + a.m12*b.m22 + a.m13*b.m32;
			m.m13 = a.m10*b.m03 + a.m11*b.m13 + a.m12*b.m23 + a.m13*b.m33;
			m.m20 = a.m20*b.m00 + a.m21*b.m10 + a.m22*b.m20 + a.m23*b.m30;
			m.m21 = a.m20*b.m01 + a.m21*b.m11 + a.m22*b.m21 + a.m23*b.m31;
			m.m22 = a.m20*b.m02 + a.m21*b.m12 + a.m22*b.m22 + a.m23*b.m32;
			m.m23 = a.m20*b.m03 + a.m21*b.m13 + a.m22*b.m23 + a.m23*b.m33;
			m.m30 = a.m30*b.m00 + a.m31*b.m10 + a.m32*b.m20 + a.m33*b.m30;
			m.m31 = a.m30*b.m01 + a.m31*b.m11 + a.m32*b.m21 + a.m33*b.m31;
			m.m32 = a.m30*b.m02 + a.m31*b.m12 + a.m32*b.m22 + a.m33*b.m32;
			m.m33 = a.m30*b.m03 + a.m31*b.m13 + a.m32*b.m23 + a.m33*b.m33;
			return m;
		}
	}
}
