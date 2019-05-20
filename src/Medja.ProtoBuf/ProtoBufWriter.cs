using System;
using System.IO;
using Google.Protobuf;

namespace Medja.ProtoBuf
{
    /// <summary>
    /// Allows writing of ProtoBuf messages to a stream. The format see <see cref="ProtoBufReaderWriterBase"/>.
    /// </summary>
    public class ProtoBufWriter : ProtoBufReaderWriterBase
    {
        public ProtoBufWriter(Stream stream, bool disposeStream)
            : base(stream, disposeStream)
        {
            if(!stream.CanWrite)
                throw new InvalidOperationException("Can't write to the given stream.");
        }
        
        public void Write<T>(IMessage<T> message) 
                where T : IMessage<T>
        {
            // or via static class and cache as public static readonly of a generic static class
            var isKnownType = _typeRegistry.HasType(typeof(T)); 

            if (isKnownType)
                WriteTypeInfo(typeof(T));   
            else
                WriteFirstTimeTypeInfo(typeof(T));

            message.WriteDelimitedTo(_stream);
        }

        private void WriteFirstTimeTypeInfo(Type type)
        {
            var bytes = _typeNameEncoding.GetBytes(type.AssemblyQualifiedName);
            var typeId = _typeRegistry.Add(type);
            
            Write(_minusOneAsBytes);
            Write(BitConverter.GetBytes(typeId));
            Write(BitConverter.GetBytes(bytes.Length));
            Write(bytes);
        }

        private void WriteTypeInfo(Type type)
        {
            var typeId = _typeRegistry.GetTypeId(type);
            Write(BitConverter.GetBytes(typeId));
        }

        private void Write(byte[] data)
        {
            _stream.Write(data, 0, data.Length);
        }
    }
}