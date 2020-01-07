using Medja.OpenTk.Components3D.Vertices;

namespace Medja.OpenTk.Components3D
{
    public class GLCuboid : GLColoredModel
    {
        public GLCuboid()
        {
            VertexArrayObject = new CubeFactory().Create();
        }

        public GLCuboid(float length)
        : this(length, length, length)
        {
        }

        public GLCuboid(float lengthX, float lengthY, float lengthZ)
        {
            var factory = new CubeFactory();
            factory.LengthX = lengthX;
            factory.LengthY = lengthY;
            factory.LengthZ = lengthZ;

            VertexArrayObject = factory.Create();
        }
    }
}