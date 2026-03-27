namespace UnitedGodot
{
	/// <summary>
	/// Global flags for detecting the runtime environment.
	/// Provides compatibility with the UnitedGodot reference project.
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// True when running under Godot, false when running under Unity.
		/// </summary>
		public const bool isGodot = true;
	}
}
