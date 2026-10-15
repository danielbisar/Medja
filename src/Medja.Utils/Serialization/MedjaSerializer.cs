using System;
using System.IO;

namespace Medja.Utils.Serialization
{
    /// <summary>
    /// Unsupported placeholder for a serializer using medja properties only.
    /// </summary>
    [Obsolete("Medja serialization is not implemented. Use a supported serializer instead.", true)]
    public class MedjaSerializer : IMedjaSerializer
    {
        public MedjaSerializer(object obj)
        {
            
        }
        
        public void WriteTo(Stream stream)
        {
            throw new NotSupportedException("Medja serialization is not implemented.");
        }

        public void ReadFrom(Stream stream)
        {
            throw new NotSupportedException("Medja serialization is not implemented.");
        }
    }
}