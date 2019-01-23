using System;
using Google.Protobuf;

namespace Medja.ProtoBuf
{
    public static class MessageParserExtensions
    {
        /// <summary>
        /// Parses a message from an <see cref="ArraySegment{T}"/>.
        /// </summary>
        /// <param name="parser">The <see cref="MessageParser{T}"/>.</param>
        /// <param name="arraySegment">The <see cref="ArraySegment{T}"/>.</param>
        /// <typeparam name="T">The message type.</typeparam>
        /// <returns>A new instance parsed from the array segment.</returns>
        public static T ParseFrom<T>(this MessageParser<T> parser, ArraySegment<byte> arraySegment)
                where T : IMessage<T>
        {
            return parser.ParseFrom(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }
    }
}