namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Trivial triangle implementation. Do not use to render large amounts of triangles. There are much better ways
    /// (VBO, Quads etc) 
    /// </summary>
    public class GLTriangle : GLModel
    {
        public override void RenderModel()
        {
            /*GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);
            GL.Vertex3(0, 1, 0);
            GL.End();*/
        }
    }
}