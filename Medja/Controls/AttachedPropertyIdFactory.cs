using System;

namespace Medja.Controls
{
	public static class AttachedPropertyIdFactory
	{
		private static int _nextId;
		private static readonly object _lock = new object();

		public static int NewId()
		{
			lock (_lock)
				return _nextId++;
		}
	}
}
