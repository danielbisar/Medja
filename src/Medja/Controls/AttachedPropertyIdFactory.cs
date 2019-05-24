using System.Threading;

namespace Medja.Controls
{
	/// <summary>
	/// Provides ids for attached properties.
	/// </summary>
	public static class AttachedPropertyIdFactory
	{
		private static int _nextId;

		public static int NewId()
		{
			var newValue = Interlocked.Increment(ref _nextId);
			return newValue;
		}
	}
}
