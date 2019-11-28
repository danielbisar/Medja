namespace Medja.OpenTk.Components3D.Vertices
{
    /// <summary>
    /// A general VAO factory. Creates a VAO from a vertex array and an optional index array.
    /// </summary>
    public class VertexArrayObjectFactory : IVertexArrayObjectFactory
    {
        /// <summary>
        /// Indices if any.
        /// </summary>
        public uint[] Indices { get; set; }
        
        /// <summary>
        /// The vertices (3 components per vertex)
        /// </summary>
        public float[] Vertices { get; set; }
        
        public VertexArrayObject Create()
        {
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 3;
            vbo.SetData(Vertices);

            var vao = new VertexArrayObject();
            vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);

            if (Indices != null)
            {
                var ebo = vao.CreateElementBufferObject();
                ebo.SetData(Indices);
            }

            return vao;
        }
    }
}