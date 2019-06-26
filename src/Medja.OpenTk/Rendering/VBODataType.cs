namespace Medja.OpenTk.Rendering
{
    public enum VBODataType
    {
        /// <summary>
        /// Just vertices (3 values x, y, z coordinate)
        /// </summary>
        Vertices,
        
        /// <summary>
        /// Vertices followed by normal values (Vertex + 3 normal coordinates)
        /// </summary>
        VerticesAndNormals
    }
}