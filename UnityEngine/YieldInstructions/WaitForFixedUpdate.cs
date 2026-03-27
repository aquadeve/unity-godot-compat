using System.Collections;

namespace UnityEngine
{
	/// <summary>
	/// Suspends the coroutine until the next fixed-rate frame update.
	/// In Godot, yields one physics frame.
	/// </summary>
	public sealed class WaitForFixedUpdate : IEnumerator
	{
		public object Current => null!;
		public bool MoveNext() => false;
		public void Reset() { }
	}
}
