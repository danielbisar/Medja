using System.IO;

namespace Medja.Utils.Serialization
{
    /// <summary>
    /// Interface to serialize .net objects.
    /// </summary>
    public interface IMedjaSerializer
    {
        void WriteTo(Stream stream);
        void ReadFrom(Stream stream);
    }
}
