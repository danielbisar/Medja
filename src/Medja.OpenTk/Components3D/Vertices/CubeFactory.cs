namespace Medja.OpenTk.Components3D.Vertices
{
    public class CubeFactory : IVertexArrayObjectFactory
    {
        public static uint[] Indices =
        {
            0, 1, 3, 3, 1, 2, // front
            1, 7, 2, 2, 7, 6, // right
            4, 5, 6, 6, 7, 4, // back
            5, 4, 0, 0, 3, 5, // left
            3, 2, 5, 5, 2, 6, // top
            0, 4, 7, 7, 1, 0  // bottom
        };

        public float LengthX { get; set; }
        public float LengthY { get; set; }
        public float LengthZ { get; set; }

        public CubeFactory()
        {
            LengthX = LengthY = LengthZ = 1;
        }
        
        public VertexArrayObject Create()
        {
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 3;
            vbo.SetData(new[]
            {
                0, 0, 0,
                LengthX, 0, 0,
                LengthX, LengthY, 0,
                0, LengthY, 0,

                0, 0, -LengthZ,
                0, LengthY, -LengthZ,
                LengthX, LengthY, -LengthZ,
                LengthX, 0, -LengthZ
            });

            var vao = new VertexArrayObject();
            vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);
            var ebo = vao.CreateElementBufferObject();
            ebo.SetData(Indices);

            return vao;
        }
    }
}