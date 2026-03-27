using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequiredByNativeCode]
	[StructLayout (LayoutKind.Sequential)]
	public sealed class Coroutine : CustomYieldInstruction
	{
		internal IEnumerator enumerator;


		public Coroutine (IEnumerator routine)
		{
			this.enumerator = routine;
		}


		public override bool keepWaiting
		{
			get
			{
				return enumerator.MoveNext();
			}
		}
	}
}
