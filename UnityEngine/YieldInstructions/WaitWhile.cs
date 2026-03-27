using System;
using System.Collections;

namespace UnityEngine
{
	/// <summary>
	/// Suspends the coroutine execution until the supplied delegate evaluates to false.
	/// </summary>
	public sealed class WaitWhile : CustomYieldInstruction
	{
		private readonly Func<bool> _predicate;

		public WaitWhile(Func<bool> predicate)
		{
			_predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
		}

		public override bool keepWaiting => _predicate();
	}
}
