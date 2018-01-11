using Medja.Controls;
using Medja.Rendering;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
    public class ButtonRenderer : IControlRenderer
    {
        public void Render(Control control)
        {
            var button = control as Button;

            var x = button.X;
            var y = button.Y;
            var xRight = x + button.Width;
            var yTop = y + button.Height;
            
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(x, y);
            GL.Vertex2(xRight, y);
            GL.Vertex2(x, yTop);

            GL.Vertex2(x, yTop);
            GL.Vertex2(xRight, yTop);
            GL.Vertex2(xRight, y);
            GL.End();

            button.IsRendered = true;
        }
    }
}
