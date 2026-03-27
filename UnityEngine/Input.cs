using Godot;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public static class Input
	{
		// ---- Mouse ----
		public static Vector3 mousePosition
		{
			get
			{
				var vp = UnityEngineAutoLoad.Instance?.GetViewport();
				if (vp == null) return Vector3.zero;
				var pos = vp.GetMousePosition();
				return new Vector3(pos.X, pos.Y, 0f);
			}
		}

		public static bool GetMouseButton(int button) => Godot.Input.IsMouseButtonPressed((MouseButton)(button + 1));
		public static bool GetMouseButtonDown(int button) => false; // Would need event-based tracking
		public static bool GetMouseButtonUp(int button) => false;

		// ---- KeyCode mapping ----
		private static readonly Dictionary<KeyCode, Key> _keyMap = BuildKeyMap();

		private static Dictionary<KeyCode, Key> BuildKeyMap()
		{
			var map = new Dictionary<KeyCode, Key>();
			map[KeyCode.None] = Key.None;
			map[KeyCode.Space] = Key.Space;
			map[KeyCode.Return] = Key.Enter;
			map[KeyCode.Escape] = Key.Escape;
			map[KeyCode.Backspace] = Key.Backspace;
			map[KeyCode.Tab] = Key.Tab;
			map[KeyCode.Delete] = Key.Delete;
			map[KeyCode.UpArrow] = Key.Up;
			map[KeyCode.DownArrow] = Key.Down;
			map[KeyCode.LeftArrow] = Key.Left;
			map[KeyCode.RightArrow] = Key.Right;
			map[KeyCode.LeftShift] = Key.Shift;
			map[KeyCode.RightShift] = Key.Shift;
			map[KeyCode.LeftControl] = Key.Ctrl;
			map[KeyCode.RightControl] = Key.Ctrl;
			map[KeyCode.LeftAlt] = Key.Alt;
			map[KeyCode.RightAlt] = Key.Alt;

			// Alpha keys
			for (int i = 0; i <= 9; i++)
				map[(KeyCode)((int)KeyCode.Alpha0 + i)] = (Key)((int)Key.Key0 + i);

			// Letters
			for (int i = 0; i < 26; i++)
				map[(KeyCode)((int)KeyCode.A + i)] = (Key)((int)Key.A + i);

			// Function keys
			for (int i = 0; i < 12; i++)
				map[(KeyCode)((int)KeyCode.F1 + i)] = (Key)((int)Key.F1 + i);

			return map;
		}

		private static Key ToGodotKey(KeyCode key)
		{
			return _keyMap.TryGetValue(key, out var godotKey) ? godotKey : Key.None;
		}

		// ---- Key input ----
		public static bool GetKey(KeyCode key)
		{
			var gk = ToGodotKey(key);
			return gk != Key.None && Godot.Input.IsKeyPressed(gk);
		}

		public static bool GetKeyDown(KeyCode key)
		{
			// Godot doesn't have a direct frame-based key-down check outside _Input.
			// This returns pressed state; for accurate down detection, use _Input in MonoBehaviour.
			return GetKey(key);
		}

		public static bool GetKeyUp(KeyCode key)
		{
			return false; // Requires event-based tracking
		}

		public static bool GetKey(string name) => GetKey(NameToKeyCode(name));
		public static bool GetKeyDown(string name) => GetKeyDown(NameToKeyCode(name));
		public static bool GetKeyUp(string name) => GetKeyUp(NameToKeyCode(name));

		// ---- Axis ----
		public static float GetAxis(string axisName)
		{
			switch (axisName)
			{
				case "Horizontal":
					float h = 0f;
					if (Godot.Input.IsKeyPressed(Key.D) || Godot.Input.IsKeyPressed(Key.Right)) h += 1f;
					if (Godot.Input.IsKeyPressed(Key.A) || Godot.Input.IsKeyPressed(Key.Left))  h -= 1f;
					return h;
				case "Vertical":
					float v = 0f;
					if (Godot.Input.IsKeyPressed(Key.W) || Godot.Input.IsKeyPressed(Key.Up))   v += 1f;
					if (Godot.Input.IsKeyPressed(Key.S) || Godot.Input.IsKeyPressed(Key.Down)) v -= 1f;
					return v;
				case "Mouse X":
				case "Mouse Y":
					return 0f; // Would need per-frame delta tracking
				default:
					return 0f;
			}
		}

		public static float GetAxisRaw(string axisName) => GetAxis(axisName);

		public static bool GetButton(string buttonName) => false;
		public static bool GetButtonDown(string buttonName) => false;
		public static bool GetButtonUp(string buttonName) => false;

		// ---- Touch ----
		public static int touchCount => 0;
		public static bool touchSupported => false;
		public static Touch GetTouch(int index) => default;

		// ---- Misc ----
		public static bool anyKey => Godot.Input.IsAnythingPressed();
		public static bool anyKeyDown => false;
		public static string inputString => "";

		private static KeyCode NameToKeyCode(string name)
		{
			if (Enum.TryParse<KeyCode>(name, true, out var kc)) return kc;
			return name.ToLower() switch
			{
				"space" => KeyCode.Space,
				"return" or "enter" => KeyCode.Return,
				"escape" => KeyCode.Escape,
				"up" => KeyCode.UpArrow,
				"down" => KeyCode.DownArrow,
				"left" => KeyCode.LeftArrow,
				"right" => KeyCode.RightArrow,
				_ => KeyCode.None
			};
		}
	}
}
