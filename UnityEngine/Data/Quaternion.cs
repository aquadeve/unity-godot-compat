using System;
using Godot;

namespace UnityEngine
{
public struct Quaternion
{
private static readonly Quaternion identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);

public const float kEpsilon = 1E-06f;

public float x;
public float y;
public float z;
public float w;

public static Quaternion identity => identityQuaternion;

public Vector3 eulerAngles
{
get
{
var v = Internal_ToEulerRad(this) * Mathf.Rad2Deg;
return Internal_MakePositive(v);
}
set => this = Internal_FromEulerRad(value * Mathf.Deg2Rad);
}

public Quaternion normalized => Normalize(this);

public Quaternion(float x, float y, float z, float w)
{
this.x = x; this.y = y; this.z = z; this.w = w;
}

public float this[int index]
{
get { return index switch { 0 => x, 1 => y, 2 => z, 3 => w, _ => throw new IndexOutOfRangeException() }; }
set { switch (index) { case 0: x = value; break; case 1: y = value; break; case 2: z = value; break; case 3: w = value; break; default: throw new IndexOutOfRangeException(); } }
}

// ---- Static Methods ----
public static float Angle(Quaternion a, Quaternion b)
{
float dot = Mathf.Abs(Dot(a, b));
return IsEqualUsingDot(dot) ? 0f : Mathf.Acos(Mathf.Min(dot, 1f)) * 2f * Mathf.Rad2Deg;
}

public static Quaternion AngleAxis(float angle, Vector3 axis)
{
var v = (Godot.Vector3)axis;
v = v.Normalized();
float rad = angle * Mathf.Deg2Rad * 0.5f;
float s = Mathf.Sin(rad);
return new Quaternion(v.X * s, v.Y * s, v.Z * s, Mathf.Cos(rad));
}

public static float Dot(Quaternion a, Quaternion b)
=> a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

public static Quaternion Euler(float x, float y, float z)
=> Internal_FromEulerRad(new Vector3(x, y, z) * Mathf.Deg2Rad);

public static Quaternion Euler(Vector3 euler)
=> Euler(euler.x, euler.y, euler.z);

public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
{
var from = ((Godot.Vector3)fromDirection).Normalized();
var to   = ((Godot.Vector3)toDirection).Normalized();
var axis = from.Cross(to);
float dot = from.Dot(to);
if (dot >= 1f) return identity;
if (dot <= -1f)
{
var perp = Mathf.Abs(from.X) < 0.9f ? new Godot.Vector3(1, 0, 0) : new Godot.Vector3(0, 1, 0);
axis = from.Cross(perp).Normalized();
return new Quaternion(axis.X, axis.Y, axis.Z, 0f);
}
float s = Mathf.Sqrt((1f + dot) * 2f);
axis /= s;
return new Quaternion(axis.X, axis.Y, axis.Z, s * 0.5f);
}

public static Quaternion Inverse(Quaternion rotation)
{
float n = rotation.x * rotation.x + rotation.y * rotation.y + rotation.z * rotation.z + rotation.w * rotation.w;
if (n == 0f) return identity;
return new Quaternion(-rotation.x / n, -rotation.y / n, -rotation.z / n, rotation.w / n);
}

public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
{
t = Mathf.Clamp01(t);
return LerpUnclamped(a, b, t);
}

public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
{
if (Dot(a, b) < 0f)
b = new Quaternion(-b.x, -b.y, -b.z, -b.w);
var result = new Quaternion(
a.x + (b.x - a.x) * t,
a.y + (b.y - a.y) * t,
a.z + (b.z - a.z) * t,
a.w + (b.w - a.w) * t);
return Normalize(result);
}

public static Quaternion LookRotation(Vector3 forward, Vector3 upwards = default)
{
if (upwards == default) upwards = Vector3.up;
var fwd = ((Godot.Vector3)forward).Normalized();
var up  = ((Godot.Vector3)upwards).Normalized();
var right = up.Cross(fwd).Normalized();
up = fwd.Cross(right);
var basis = new Godot.Basis(right, up, -fwd);
return new Quaternion() { godot = basis.GetRotationQuaternion() };
}

public static Quaternion Normalize(Quaternion q)
{
float mag = Mathf.Sqrt(Dot(q, q));
if (mag < kEpsilon) return identity;
return new Quaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
}

public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
{
float angle = Angle(from, to);
if (angle == 0f) return to;
return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
}

public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
=> SlerpUnclamped(a, b, Mathf.Clamp01(t));

public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
{
float dot = Dot(a, b);
if (dot < 0f) { dot = -dot; b = new Quaternion(-b.x, -b.y, -b.z, -b.w); }
if (dot > 1f - kEpsilon) return LerpUnclamped(a, b, t);
float angle = Mathf.Acos(dot);
float sinAngle = Mathf.Sin(angle);
float ta = Mathf.Sin((1f - t) * angle) / sinAngle;
float tb = Mathf.Sin(t * angle) / sinAngle;
return new Quaternion(ta * a.x + tb * b.x, ta * a.y + tb * b.y, ta * a.z + tb * b.z, ta * a.w + tb * b.w);
}

// ---- Godot conversion ----
public Godot.Quaternion godot
{
get => new Godot.Quaternion(x, y, z, w);
set { x = value.X; y = value.Y; z = value.Z; w = value.W; }
}

public static implicit operator Godot.Quaternion(Quaternion q) => new Godot.Quaternion(q.x, q.y, q.z, q.w);
public static implicit operator Quaternion(Godot.Quaternion q) => new Quaternion(q.X, q.Y, q.Z, q.W);

// ---- Operators ----
public static Vector3 operator *(Quaternion rotation, Vector3 point)
{
var godotQ = new Godot.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
var godotV = new Godot.Vector3(point.x, point.y, point.z);
var result = godotQ * godotV;
return new Vector3(result.X, result.Y, result.Z);
}

public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
{
return new Quaternion(
lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
}

public static bool operator ==(Quaternion lhs, Quaternion rhs) => Dot(lhs, rhs) > 1f - kEpsilon;
public static bool operator !=(Quaternion lhs, Quaternion rhs) => !(lhs == rhs);

// ---- Methods ----
public void Set(float newX, float newY, float newZ, float newW) { x = newX; y = newY; z = newZ; w = newW; }

public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection) => this = FromToRotation(fromDirection, toDirection);
public void SetLookRotation(Vector3 view, Vector3 up = default) => this = LookRotation(view, up);

public void ToAngleAxis(out float angle, out Vector3 axis)
{
var q = Normalize(this);
angle = 2f * Mathf.Acos(q.w) * Mathf.Rad2Deg;
float s = Mathf.Sqrt(1f - q.w * q.w);
if (s < kEpsilon) axis = new Vector3(1, 0, 0);
else axis = new Vector3(q.x / s, q.y / s, q.z / s);
}

public override bool Equals(object? other) => other is Quaternion q && this == q;
public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2 ^ w.GetHashCode() >> 1;
public override string ToString() => $"({x:F1}, {y:F1}, {z:F1}, {w:F1})";

// ---- Internal helpers ----
private static bool IsEqualUsingDot(float dot) => dot > 1f - kEpsilon;

internal static Vector3 Internal_ToEulerRad(Quaternion rotation)
{
// Godot quaternion to euler
var q = new Godot.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
var euler = q.GetEuler();
return new Vector3(euler.X, euler.Y, euler.Z);
}

internal static Quaternion Internal_FromEulerRad(Vector3 euler)
{
var q = Godot.Quaternion.FromEuler(new Godot.Vector3(euler.x, euler.y, euler.z));
return new Quaternion(q.X, q.Y, q.Z, q.W);
}

internal static Vector3 Internal_MakePositive(Vector3 euler)
{
const float neg = -0.005729578f;
const float pos = 360f + neg;
if (euler.x < neg)       euler.x += 360f;
else if (euler.x > pos)  euler.x -= 360f;
if (euler.y < neg)       euler.y += 360f;
else if (euler.y > pos)  euler.y -= 360f;
if (euler.z < neg)       euler.z += 360f;
else if (euler.z > pos)  euler.z -= 360f;
return euler;
}
}
}
