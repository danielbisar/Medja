using System;
using System.IO;

namespace Medja.Utils.IO
{
    /// <summary>
    /// Extensions methods for <see cref="System.IO.Stream"/> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Writes a sequence of bytes to the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream instance.</param>
        /// <param name="buffer">The buffer.</param>
        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Reads a sequence of bytes from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream instance.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The amount of bytes read <see cref="Stream.Read"/>.</returns>
        public static int Read(this Stream stream, byte[] buffer)
        {
            return stream.Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Reads a sequence of bytes from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream instance.</param>
        /// <param name="count">The count of bytes to read.</param>
        /// <returns>A buffer exactly <see cref="count"/> bytes long.</returns>
        /// <exception cref="EndOfStreamException">If less than count bytes could be read from the stream.</exception>
        public static byte[] Read(this Stream stream, int count)
        {
            var result = new byte[count];
            
            if(stream.Read(result) != count)
                throw new EndOfStreamException();

            return result;
        }
    }
}