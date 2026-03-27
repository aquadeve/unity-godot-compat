namespace UnityEngine
{
	/// <summary>
	/// Enumeration of common event types for use with Event.
	/// </summary>
	public enum EventType
	{
		MouseDown = 0,
		MouseUp = 1,
		MouseMove = 2,
		MouseDrag = 3,
		KeyDown = 4,
		KeyUp = 5,
		ScrollWheel = 6,
		Repaint = 7,
		Layout = 8,
		DragUpdated = 9,
		DragPerform = 10,
		DragExited = 15,
		Ignore = 11,
		Used = 12,
		ValidateCommand = 13,
		ExecuteCommand = 14,
		ContextClick = 16,
		MouseEnterWindow = 20,
		MouseLeaveWindow = 21,
		TouchDown = 30,
		TouchUp = 31,
		TouchMove = 32,
		TouchEnter = 33,
		TouchLeave = 34,
		TouchStationary = 35
	}

	/// <summary>
	/// Enumeration for the cursor lock state.
	/// </summary>
	public enum CursorLockMode
	{
		None = 0,
		Locked = 1,
		Confined = 2
	}

	/// <summary>
	/// Describes whether a touch is a direct, indirect, or stylus.
	/// </summary>
	public enum TouchType
	{
		Direct = 0,
		Indirect = 1,
		Stylus = 2
	}

	/// <summary>
	/// Describes the phase of a touch event.
	/// </summary>
	public enum TouchPhase
	{
		Began = 0,
		Moved = 1,
		Stationary = 2,
		Ended = 3,
		Canceled = 4
	}

	/// <summary>
	/// Structure describing the status of a finger touching the screen.
	/// </summary>
	public struct Touch
	{
		public int fingerId;
		public Vector2 position;
		public Vector2 rawPosition;
		public Vector2 deltaPosition;
		public float deltaTime;
		public int tapCount;
		public TouchPhase phase;
		public TouchType type;
	}

	/// <summary>
	/// Render mode for a camera.
	/// </summary>
	public enum RenderMode
	{
		ScreenSpaceOverlay = 0,
		ScreenSpaceCamera = 1,
		WorldSpace = 2
	}

	/// <summary>
	/// Shadow casting mode for renderers.
	/// </summary>
	public enum ShadowCastingMode
	{
		Off = 0,
		On = 1,
		TwoSided = 2,
		ShadowsOnly = 3
	}

	/// <summary>
	/// Describes the hiding state of a renderer.
	/// </summary>
	public enum HideFlags
	{
		None = 0,
		HideInHierarchy = 1,
		HideInInspector = 2,
		DontSaveInEditor = 4,
		NotEditable = 8,
		DontSaveInBuild = 16,
		DontUnloadUnusedAsset = 32,
		DontSave = 52,
		HideAndDontSave = 61
	}

	/// <summary>
	/// How the material interacts with lightmaps.
	/// </summary>
	public enum LightmapBakeType
	{
		Realtime = 4,
		Baked = 2,
		Mixed = 1
	}

	/// <summary>
	/// A selection of force modes for ForceMode on Rigidbody.
	/// </summary>
	public enum ForceMode
	{
		Force = 0,
		Acceleration = 5,
		Impulse = 1,
		VelocityChange = 2
	}

	/// <summary>
	/// 2D force mode for Rigidbody2D.
	/// </summary>
	public enum ForceMode2D
	{
		Force = 0,
		Impulse = 1
	}

	/// <summary>
	/// Determines how collisions are detected.
	/// </summary>
	public enum CollisionDetectionMode
	{
		Discrete = 0,
		Continuous = 1,
		ContinuousDynamic = 2,
		ContinuousSpeculative = 3
	}

	/// <summary>
	/// Send message options for SendMessage.
	/// </summary>
	public enum SendMessageOptions
	{
		RequireReceiver = 0,
		DontRequireReceiver = 1
	}
}
