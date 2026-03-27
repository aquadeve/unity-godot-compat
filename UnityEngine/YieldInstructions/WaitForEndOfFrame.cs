using System.Collections;

namespace UnityEngine
{
	/// <summary>
	/// Waits until the end of the frame after all rendering has completed.
	/// In Godot, this is equivalent to a single frame yield.
	/// </summary>
	public sealed class WaitForEndOfFrame : IEnumerator
	{
		public object Current => null!;
		public bool MoveNext() => false;
		public void Reset() { }
	}
}
