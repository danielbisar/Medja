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
        public ProtoBufReader(Stream stream, bool disposeStream) 
                : base(stream, disposeStream)
        {
            if(!stream.CanRead)
                throw new InvalidOperationException("Can't read from the given stream.");
        }
        
        public IMessage Read()
        {
            var typeId = ReadInt32();

            if (typeId == -1)
                typeId = ReadFirstTimeTypeInfo();

            // TODO there might be a much faster way to do this
            // f.e. provide factory methods for known/expected types
            // maybe protobuf even provides something (MessageParser also expects factory methods)
            var type = _typeRegistry.GetTypeById(typeId);
            var message = (IMessage)Activator.CreateInstance(type);
            
            message.MergeDelimitedFrom(_stream);

            return message;
        }

        private int ReadInt32()
        {
            var buffer = Read(4);
            return BitConverter.ToInt32(buffer, 0);
        }
        
        private int ReadFirstTimeTypeInfo()
        {
            var expectedTypeId = ReadInt32();
            var typeNameLength = ReadInt32();
            var bytes = Read(typeNameLength);

            var typeName = _typeNameEncoding.GetString(bytes);
            var type = Type.GetType(typeName);

            var typeId = _typeRegistry.Add(type);
            
            // this could happen if
            // 1. we missed a type in the stream
            // 2. the type id written to the stream was wrong
            // 3. the type id generation pattern was changed (so ProtoBufTypeRegistry changed)
            // either way this is critical and we should not continue
            if(typeId != expectedTypeId)
                throw new InvalidOperationException("Error in the stream. Expected type id does not match type id.");
            
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