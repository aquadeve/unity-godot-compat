#if !UNITY_2017_1_OR_NEWER

using Godot;
using System.Collections.Generic;

namespace UnitedGodot
{
	/// <summary>
	/// A Godot Node3D that wraps a UnityEngine.GameObject.
	/// Place this in the Godot scene tree so that child MonoBehaviours
	/// can discover their owning GameObject automatically.
	/// </summary>
	public partial class UGGameObject : Node3D
	{
		private static readonly Dictionary<Node3D, UnityEngine.GameObject> _nodeToGameObject = new();

		/// <summary>
		/// Returns the UnityEngine.GameObject for the given Node3D.
		/// If the Node3D is a UGGameObject, returns its inner GameObject.
		/// Otherwise creates (or retrieves from cache) a wrapper.
		/// </summary>
		public static UnityEngine.GameObject FromNode3D(Node3D node3D)
		{
			if (node3D is UGGameObject ugGo)
			{
				if (ugGo.InnerGameObject == null)
					UnityEngine.Debug.LogError("UGGameObject.FromNode3D: InnerGameObject is null!");
				return ugGo.InnerGameObject;
			}

			if (_nodeToGameObject.TryGetValue(node3D, out var go))
				return go;

			go = new UnityEngine.GameObject(null, node3D);
			_nodeToGameObject[node3D] = go;
			return go;
		}

		/// <summary>
		/// The inner UnityEngine.GameObject created lazily.
		/// </summary>
		public UnityEngine.GameObject InnerGameObject
		{
			get
			{
				_innerGameObject ??= new UnityEngine.GameObject(null, this);
				return _innerGameObject;
			}
		}

		private UnityEngine.GameObject? _innerGameObject;
	}
}

#endif
