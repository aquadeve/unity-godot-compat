using Godot;

namespace UnityEngine
{
	/// <summary>
	/// Stores and accesses player preferences between game sessions.
	/// Uses Godot's ConfigFile for persistence.
	/// </summary>
	public static class PlayerPrefs
	{
		private static readonly ConfigFile _config = new();
		private static readonly string _path = "user://playerprefs.cfg";
		private static bool _loaded = false;
		private const string Section = "PlayerPrefs";

		private static void EnsureLoaded()
		{
			if (_loaded) return;
			_loaded = true;
			if (FileAccess.FileExists(_path))
				_config.Load(_path);
		}

		public static void SetInt(string key, int value)
		{
			EnsureLoaded();
			_config.SetValue(Section, key, value);
		}

		public static int GetInt(string key, int defaultValue = 0)
		{
			EnsureLoaded();
			if (_config.HasSectionKey(Section, key))
				return (int)_config.GetValue(Section, key);
			return defaultValue;
		}

		public static void SetFloat(string key, float value)
		{
			EnsureLoaded();
			_config.SetValue(Section, key, value);
		}

		public static float GetFloat(string key, float defaultValue = 0f)
		{
			EnsureLoaded();
			if (_config.HasSectionKey(Section, key))
				return (float)_config.GetValue(Section, key);
			return defaultValue;
		}

		public static void SetString(string key, string value)
		{
			EnsureLoaded();
			_config.SetValue(Section, key, value);
		}

		public static string GetString(string key, string defaultValue = "")
		{
			EnsureLoaded();
			if (_config.HasSectionKey(Section, key))
				return (string)_config.GetValue(Section, key);
			return defaultValue;
		}

		public static bool HasKey(string key)
		{
			EnsureLoaded();
			return _config.HasSectionKey(Section, key);
		}

		public static void DeleteKey(string key)
		{
			EnsureLoaded();
			if (_config.HasSectionKey(Section, key))
				_config.EraseSectionKey(Section, key);
		}

		public static void DeleteAll()
		{
			_config.Clear();
			Save();
		}

		public static void Save()
		{
			EnsureLoaded();
			_config.Save(_path);
		}
	}
}
