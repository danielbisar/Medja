using System;
using System.IO;
using System.Text;

namespace Medja.ProtoBuf
{
    /// <remarks>
    /// Layout of the data.
    ///
    /// Per Message
    /// - int32 type id
    /// - [type name info] - details see below
    /// - int32 message length
    /// - ProtoBuf message
    ///
    ///
    /// type name info - this field is only written if type id = -1, this is the case if a specific type is
    /// written for the first time into the stream
    ///
    /// - int32 type id
    /// - int32 length of the string
    /// - bytes (UTF-8 encoded) assembly qualified typename
    ///
    /// Note: currently we use the systems endianness. So sharing between little and big endian systems is not yet
    /// supported.
    /// </remarks>
    public class ProtoBufReaderWriterBase
    {
        protected readonly Stream _stream;
        protected readonly ProtoBufTypeRegistry _typeRegistry;
        protected readonly Encoding _typeNameEncoding;
        protected readonly byte[] _minusOneAsBytes;
        private readonly bool _disposeStream;

        protected ProtoBufReaderWriterBase(Stream stream, bool disposeStream = true)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            _disposeStream = disposeStream;
            _stream = stream;
            _typeRegistry = new ProtoBufTypeRegistry();
            _typeNameEncoding = Encoding.UTF8;
            _minusOneAsBytes = BitConverter.GetBytes(-1);
        }
        
        public void Dispose()
        {
            if(_disposeStream)
                _stream?.Dispose();
        }
    }
}