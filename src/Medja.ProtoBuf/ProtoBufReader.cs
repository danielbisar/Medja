using System;
using System.IO;
using Google.Protobuf;

namespace Medja.ProtoBuf
{
    /// <summary>
    /// Allows reading of ProtoBuf messages from a stream. The expected format see <see cref="ProtoBufReaderWriterBase"/>.
    /// </summary>
    public class ProtoBufReader : ProtoBufReaderWriterBase
    {
        private ProtoBufStreamIndex _index;
        
        public ProtoBufReader(Stream stream, bool disposeStream) 
                : base(stream, disposeStream)
        {
            if(!stream.CanRead)
                throw new InvalidOperationException("Can't read from the given stream.");
            
            _index = new ProtoBufStreamIndex();
        }
        
        public IMessage Read()
        {
            var newIndex = _index.Add(_stream.Position);
            
            var typeId = ReadInt32();

            if (typeId == -1)
                typeId = ReadFirstTimeTypeInfo(!newIndex);

            // TODO there might be a much faster way to do this
            // f.e. provide factory methods for known/expected types
            // maybe protobuf even provides something (MessageParser also expects factory methods)
            var type = _typeRegistry.GetTypeById(typeId);
            var message = (IMessage)Activator.CreateInstance(type);
            
            message.MergeDelimitedFrom(_stream);

            return message;
        }

        /// <summary>
        /// Seeks to the n-th message.
        /// </summary>
        /// <param name="messageIndex">The (realtive) index of the message.</param>
        /// <exception cref="NotSupportedException">Currently only SeekOrigin.Begin is supported
        /// OR the stream does not support seeking.</exception>
        public void Seek(int messageIndex, SeekOrigin origin)
        {
            if (!_stream.CanSeek)
                throw new NotSupportedException();

            if (origin == SeekOrigin.Begin)
            {
                if(messageIndex < 0)
                    throw new Exception(); // TODO what exception would be correct
                
                if (_index.Count > messageIndex)
                {
                    var pos = _index[messageIndex];
                    _stream.Position = pos;
                }
                else if (_index.Count < messageIndex)
                {
                    while (_index.Count <= messageIndex)
                        Read();

                    _stream.Position = _index[messageIndex];
                }
            }
            else
                throw new NotSupportedException("Currently only SeekOrigin.Begin is supported.");
        }

        private int ReadInt32()
        {
            var buffer = Read(4);
            return BitConverter.ToInt32(buffer, 0);
        }
        
        private int ReadFirstTimeTypeInfo(bool wasReadBefore)
        {
            var expectedTypeId = ReadInt32();
            var typeNameLength = ReadInt32();
            var bytes = Read(typeNameLength);
            
            var typeName = _typeNameEncoding.GetString(bytes);
            var type = Type.GetType(typeName);

            if (!wasReadBefore)
            {
                var typeId = _typeRegistry.Add(type);

                // this could happen if
                // 1. we missed a type in the stream
                // 2. the type id written to the stream was wrong
                // 3. the type id generation pattern was changed (so ProtoBufTypeRegistry changed)
                // either way this is critical and we should not continue
                if (typeId != expectedTypeId)
                    throw new InvalidOperationException(
                            "Error in the stream. Expected type id does not match type id.");
            }

            return expectedTypeId;
        }

        private byte[] Read(int count)
        {
            var result = new byte[count];
            var read = _stream.Read(result, 0, count);
            
            if(read != count)
                throw new EndOfStreamException();

            return result;
        }
    }
}