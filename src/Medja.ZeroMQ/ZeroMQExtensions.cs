using System;
using ZeroMQ;

namespace Medja.ZeroMQ
{
	/// <summary>
	/// Provides extension methods for our ZeroMQ classes.
	/// </summary>
	public static class ZeroMQExtensions
	{
		/// <summary>
		/// Writes the specified bytes into the frame. (Shortcut for <see cref="ZFrame.Write(byte[],int,int)"/>)
		/// </summary>
		/// <param name="frame">The <see cref="ZFrame"/>.</param>
		/// <param name="data">Data.</param>
		public static void Write(this ZFrame frame, byte[] data)
		{
			frame.Write(data, 0, data.Length);
		}

		/// <summary>
		/// Writes the specified bytes into the frame. (Shortcut for <see cref="ZFrame.Write(byte[],int,int)"/>)
		/// </summary>
		/// <param name="frame">The <see cref="ZFrame"/>.</param>
		/// <param name="data">The data.</param>
		public static void Write(this ZFrame frame, ArraySegment<byte> data)
		{
			frame.Write(data.Array, data.Offset, data.Count);
		}
	}
}
