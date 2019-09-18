namespace Medja.OpenTk.Components3D
{
    public class GLCuboid : GLColoredModel
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
        
        private static float[] CreateVertices(float xLength, float yLength, float zLength)
        {
            return new []
            {
                0,0,0,
                xLength, 0, 0,
                xLength, yLength, 0,
                0, yLength, 0,
                
                0, 0, zLength,
                0, yLength, zLength,
                xLength, yLength, zLength,
                xLength, 0, zLength,
            };
        }
        
        public GLCuboid(float xLength, float yLength, float zLength)
        : base(CreateVertices(xLength, yLength, zLength), Indices)
        {
        }

        public GLCuboid(float length)
            : this(length, length, length)
        {
        }

        public GLCuboid()
            : this(1)
        {
        }
    }
}