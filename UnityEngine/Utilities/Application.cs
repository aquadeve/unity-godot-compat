using Godot;
using System;

namespace UnityEngine
{
	public static class Application
	{
		public static Action<string, string, LogType> logMessageReceived;

		public static int targetFrameRate
		{
			get
			{
				return Engine.MaxFps;
			}

			set
			{
				Engine.MaxFps = value;
			}
		}


		public static string dataPath
		{
			get
			{
				Debug.Log("Application.dataPath not implemented");
				return "";
			}
		}


		public static string persistentDataPath
		{
			get
			{
				string baseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

				switch ( OS.GetName() )
				{
					case "X11":
						baseFolder += "/.local/share/";
						break;
					case "OSX":
						baseFolder += "/Library/Application Support/";
						break;
					case "Windows":
						baseFolder += @"\";
						break;
					default:
						Debug.Log("Application.persistentDataPath not implemented for " + OS.GetName());
						return baseFolder;
				}

				return baseFolder + ProjectSettings.GetSetting("application/config/name");
			}
		}


		public static RuntimePlatform platform
		{
			get
			{
				if ( Engine.IsEditorHint() )
				{
					switch ( OS.GetName() )
					{
						case "X11":
							return RuntimePlatform.LinuxEditor;
						case "OSX":
							return RuntimePlatform.OSXEditor;
						case "Windows":
							return RuntimePlatform.WindowsEditor;
					}
				}
				else
				{
					switch ( OS.GetName() )
					{
						case "X11":
							return RuntimePlatform.LinuxPlayer;
						case "OSX":
							return RuntimePlatform.OSXPlayer;
						case "Windows":
							return RuntimePlatform.WindowsPlayer;
						case "ANDROID":
							return RuntimePlatform.Android;
						case "IOS":
							return RuntimePlatform.IPhonePlayer;
					}
				}

				return RuntimePlatform.WindowsPlayer;
			}
		}


		public static bool isEditor { get { return Engine.IsEditorHint(); } }
		public static bool isPlaying { get { return true; } }


		public static void OpenURL(string url)
		{
			OS.ShellOpen(url);
		}


		public static void Quit()
		{
			UnityEngineAutoLoad.Instance.Quit();
		}

		public static string productName
		{
			get => (string)ProjectSettings.GetSetting("application/config/name");
		}

		public static string companyName { get; set; } = "";
		public static string version { get; set; } = "1.0";
		public static string unityVersion => "2021.3.0f1"; // compat stub
		public static bool isFocused => true;
		public static bool isMobilePlatform => platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer;
		public static SystemLanguage systemLanguage => SystemLanguage.English;
		public static string streamingAssetsPath => "res://StreamingAssets";
		public static string temporaryCachePath => OS.GetCacheDir();
	}


	public enum RuntimePlatform
	{
		OSXEditor,
		OSXPlayer,
		WindowsPlayer,
		OSXDashboardPlayer,
		WindowsEditor = 7,
		IPhonePlayer,
		Android = 11,
		LinuxPlayer = 13,
		LinuxEditor = 16,
		WebGLPlayer,
		WSAPlayerX86 = 18,
		WSAPlayerX64 = 19,
		WSAPlayerARM = 20,
		TizenPlayer,
		PSP2,
		PS4,
		PSM,
		XboxOne,
		SamsungTVPlayer,
		WiiU = 30,
		tvOS,
		Switch
	}
}