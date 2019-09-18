
namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Trivial triangle implementation. Do not use to render large amounts of triangles. There are much better ways
    /// (VBO, Quads etc) 
    /// </summary>
    public class GLTriangle : GLColoredModel
    {
        public GLTriangle()
        : base(new float[]
        {
            0, 0, 0,
            1, 0, 0,
            0, 1, 0
        })
        {
        }
    }
}