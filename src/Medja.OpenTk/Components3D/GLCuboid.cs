using Medja.OpenTk.Components3D.Vertices;

namespace Medja.OpenTk.Components3D
{
    public class GLCuboid : GLColoredModel
    {
        public GLCuboid(CubeFactory factory = null)
            : base(factory ?? new CubeFactory())
        {
        }
    }
}