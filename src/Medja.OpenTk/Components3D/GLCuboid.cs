using Medja.OpenTk.Components3D.Vertices;

namespace Medja.OpenTk.Components3D
{
    public class GLCuboid : GLColoredModel
    {
        public GLCuboid()
        {
            VertexArrayObject = new CubeFactory().Create();
        }
    }
}