using System;

namespace Medja.Utils.Collections.Generic
{
	public class RingBufferEventArgs : EventArgs
	{
		public RingBufferOperation Operation { get; }

		public RingBufferEventArgs(RingBufferOperation operation)
		{
			Operation = operation;
		}
	}
}
