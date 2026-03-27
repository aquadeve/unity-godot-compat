using System;
using System.Collections;

namespace UnityEngine
{
	/// <summary>
	/// Suspends the coroutine execution until the supplied delegate evaluates to true.
	/// </summary>
	public sealed class WaitUntil : CustomYieldInstruction
	{
		private readonly Func<bool> _predicate;

		public WaitUntil(Func<bool> predicate)
		{
			_predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
		}

		public override bool keepWaiting => !_predicate();
	}
}
