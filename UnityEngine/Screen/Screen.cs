using Godot;

namespace UnityEngine
{
	public static class Screen
	{
		public static int width
		{
			get
			{
				var vp = UnityEngineAutoLoad.Instance?.GetViewport();
				return vp != null ? (int)vp.GetVisibleRect().Size.X : 1920;
			}
		}

		public static int height
		{
			get
			{
				var vp = UnityEngineAutoLoad.Instance?.GetViewport();
				return vp != null ? (int)vp.GetVisibleRect().Size.Y : 1080;
			}
		}

		public static bool fullScreen
		{
			get => DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
			set => DisplayServer.WindowSetMode(value ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
		}

		public static Resolution currentResolution
		{
			get => new Resolution { width = width, height = height };
		}

		public static float dpi => DisplayServer.ScreenGetDpi();

		public static ScreenOrientation orientation { get; set; } = ScreenOrientation.AutoRotation;

		public static void SetResolution(int width, int height, bool fullScreen)
		{
			DisplayServer.WindowSetSize(new Vector2I(width, height));
			if (fullScreen)
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			else
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	public enum ScreenOrientation
	{
		Portrait = 1,
		PortraitUpsideDown = 2,
		LandscapeLeft = 3,
		LandscapeRight = 4,
		AutoRotation = 5
	}
}
