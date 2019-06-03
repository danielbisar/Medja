using System.IO;

namespace Medja.Utils.Serialization
{
    /// <summary>
    /// Serializer using medja properties only.
    /// </summary>
    public class MedjaSerializer : IMedjaSerializer
    {
        public MedjaSerializer(object obj)
        {
            
        }
        
        public void WriteTo(Stream stream)
        {
            
        }

        public void ReadFrom(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}