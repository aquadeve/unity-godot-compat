using System;
using Godot;

namespace UnityEngine
{
	public static class Debug
	{
		const string nullString = "Null";

		public static bool isDebugBuild { get { return OS.IsDebugBuild(); } }

		// ---- Log ----
		public static void Log(object? obj)
		{
			GD.Print(obj?.ToString() ?? nullString);
		}

		public static void Log(object? obj, Object? context)
		{
			GD.Print(obj?.ToString() ?? nullString);
		}

		public static void LogFormat(string format, params object?[] args)
		{
			GD.Print(string.Format(format, args));
		}

		public static void LogFormat(Object? context, string format, params object?[] args)
		{
			GD.Print(string.Format(format, args));
		}

		// ---- Warning ----
		public static void LogWarning(object? obj)
		{
			GD.Print("WARNING: " + (obj?.ToString() ?? nullString));
		}

		public static void LogWarning(object? obj, Object? context)
		{
			GD.Print("WARNING: " + (obj?.ToString() ?? nullString));
		}

		public static void LogWarningFormat(string format, params object?[] args)
		{
			GD.Print("WARNING: " + string.Format(format, args));
		}

		public static void LogWarningFormat(Object? context, string format, params object?[] args)
		{
			GD.Print("WARNING: " + string.Format(format, args));
		}

		// ---- Error ----
		public static void LogError(object? obj)
		{
			GD.PrintErr(obj?.ToString() ?? nullString);
		}

		public static void LogError(object? obj, Object? context)
		{
			GD.PrintErr(obj?.ToString() ?? nullString);
		}

		public static void LogErrorFormat(string format, params object?[] args)
		{
			GD.PrintErr(string.Format(format, args));
		}

		public static void LogErrorFormat(Object? context, string format, params object?[] args)
		{
			GD.PrintErr(string.Format(format, args));
		}

		// ---- Exception ----
		public static void LogException(Exception exception)
		{
			GD.PrintErr($"Exception: {exception.Message}\n{exception.StackTrace}");
		}

		public static void LogException(Exception exception, Object? context)
		{
			GD.PrintErr($"Exception: {exception.Message}\n{exception.StackTrace}");
		}

		// ---- Assert ----
		public static void Assert(bool condition)
		{
			if (!condition) GD.PrintErr("Assertion failed!");
		}

		public static void Assert(bool condition, string? message)
		{
			if (!condition) GD.PrintErr("Assertion failed: " + (message ?? ""));
		}

		public static void Assert(bool condition, object? message)
		{
			if (!condition) GD.PrintErr("Assertion failed: " + (message?.ToString() ?? ""));
		}

		public static void AssertFormat(bool condition, string format, params object?[] args)
		{
			if (!condition) GD.PrintErr("Assertion failed: " + string.Format(format, args));
		}
	}
}