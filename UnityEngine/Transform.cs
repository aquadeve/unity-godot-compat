using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace UnityEngine
{
	public class Transform : Component, IEnumerable<Transform>
	{
		private Transform? _parent;

		// ---- Position / Rotation / Scale ----
		public Vector3 localPosition
		{
			get => gameObject.godot.Position;
			set => gameObject.godot.Position = value;
		}

		public Quaternion localRotation
		{
			get => gameObject.godot.Quaternion;
			set => gameObject.godot.Quaternion = value;
		}

		public Vector3 localEulerAngles
		{
			get => gameObject.godot.RotationDegrees;
			set => gameObject.godot.RotationDegrees = value;
		}

		public Vector3 localScale
		{
			get => gameObject.godot.Scale;
			set => gameObject.godot.Scale = value;
		}

		public Vector3 position
		{
			get => gameObject.godot.GlobalPosition;
			set => gameObject.godot.GlobalPosition = value;
		}

		public Quaternion rotation
		{
			get => gameObject.godot.GlobalTransform.Basis.GetRotationQuaternion();
			set
			{
				var gt = gameObject.godot.GlobalTransform;
				gt.Basis = new Basis(value);
				gameObject.godot.GlobalTransform = gt;
			}
		}

		public Vector3 eulerAngles
		{
			get => rotation.eulerAngles;
			set => rotation = Quaternion.Euler(value);
		}

		public Vector3 lossyScale => gameObject.godot.GlobalTransform.Basis.Scale;

		// ---- Direction Vectors ----
		public Vector3 forward
		{
			get => -gameObject.godot.GlobalTransform.Basis.Z;
			set => LookAt(position + value, Vector3.up);
		}

		public Vector3 right => gameObject.godot.GlobalTransform.Basis.X;
		public Vector3 up => gameObject.godot.GlobalTransform.Basis.Y;

		// ---- Hierarchy ----
		public Transform? parent
		{
			get => _parent;
			set => SetParent(value);
		}

		public int childCount => gameObject.godot.GetChildCount();

		public void SetParent(Transform? newParent, bool worldPositionStays = true)
		{
			var currentParent = gameObject.godot.GetParent();
			if (currentParent != null)
				currentParent.CallDeferred(Node.MethodName.RemoveChild, gameObject.godot);

			if (newParent != null)
				newParent.gameObject.godot.CallDeferred(Node.MethodName.AddChild, gameObject.godot);

			_parent = newParent;
		}

		public Transform? GetChild(int index)
		{
			var child = gameObject.godot.GetChild(index);
			if (child is Node3D n3d)
				return UGGameObjectHelper.GetOrCreate(n3d).transform;
			return null;
		}

		public bool IsChildOf(Transform parent) => gameObject.godot.IsAncestorOf(parent.gameObject.godot) == false && parent.gameObject.godot.IsAncestorOf(gameObject.godot);

		public Transform? Find(string name)
		{
			var node = gameObject.godot.FindChild(name);
			if (node is Node3D n3d) return UGGameObjectHelper.GetOrCreate(n3d).transform;
			return null;
		}

		// ---- Rotation helpers ----
		public void Rotate(Vector3 eulerDegrees, Space relativeTo = Space.Self)
		{
			if (relativeTo == Space.Self)
			{
				var q = gameObject.godot.Quaternion;
				q *= Godot.Quaternion.FromEuler((Godot.Vector3)eulerDegrees * Mathf.Deg2Rad);
				gameObject.godot.Quaternion = q;
			}
			else
			{
				rotation = Quaternion.Euler(eulerAngles + eulerDegrees);
			}
		}

		public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self)
			=> Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);

		public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self)
		{
			var q = Quaternion.AngleAxis(angle, axis);
			if (relativeTo == Space.Self)
				localRotation = localRotation * q;
			else
				rotation = q * rotation;
		}

		public void RotateAround(Vector3 point, Vector3 axis, float angle)
		{
			var q = Quaternion.AngleAxis(angle, axis);
			var diff = position - point;
			diff = q * diff;
			position = point + diff;
			Rotate(axis, angle, Space.World);
		}

		public void LookAt(Vector3 worldPosition, Vector3 worldUp = default)
		{
			if (worldUp == default) worldUp = Vector3.up;
			gameObject.godot.LookAt(worldPosition, worldUp);
		}

		public void LookAt(Transform target, Vector3 worldUp = default)
			=> LookAt(target.position, worldUp);

		// ---- Translate ----
		public void Translate(Vector3 translation, Space relativeTo = Space.Self)
		{
			if (relativeTo == Space.Self)
				localPosition += rotation * translation;
			else
				position += translation;
		}

		public void Translate(float x, float y, float z, Space relativeTo = Space.Self)
			=> Translate(new Vector3(x, y, z), relativeTo);

		// ---- Coordinate transforms ----
		public Vector3 TransformPoint(Vector3 point)
			=> gameObject.godot.ToGlobal(point);

		public Vector3 InverseTransformPoint(Vector3 point)
			=> gameObject.godot.ToLocal(point);

		public Vector3 TransformDirection(Vector3 direction)
			=> rotation * direction;

		public Vector3 InverseTransformDirection(Vector3 direction)
			=> Quaternion.Inverse(rotation) * direction;

		// ---- IEnumerable ----
		public IEnumerator<Transform> GetEnumerator()
		{
			for (int i = 0; i < childCount; i++)
			{
				var c = GetChild(i);
				if (c != null) yield return c;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public enum Space { World, Self }
}
