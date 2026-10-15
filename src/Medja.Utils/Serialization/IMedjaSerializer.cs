using System;
using System.IO;

namespace Medja.Utils.Serialization
{
    /// <summary>
    /// Unsupported placeholder for serializing .NET objects.
    /// </summary>
    [Obsolete("Medja serialization is not implemented. Use a supported serializer instead.", true)]
    public interface IMedjaSerializer
    {
        void WriteTo(Stream stream);
        void ReadFrom(Stream stream);
    }
}
