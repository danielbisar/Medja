using System;
using Medja.Theming;
using OpenTK.Graphics.OpenGL;

namespace MedjaOpenGlTestApp
{
	public class OpenGlTestControlRenderer : ControlRendererBase<object, OpenGlTestControl>
	{
		protected override void Render(object context, OpenGlTestControl control)
		{
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.Rotate(1, 0, 1, 0);
			//GL.Translate(0, 0, -1);

			GL.Color3(0, 1.0f, 0);
			GL.Begin(BeginMode.Triangles);

			GL.Vertex3(0, 0, -1);
			GL.Vertex3(1, 0, -1);
			GL.Vertex3(1, 1, -1);

			GL.End();
		}
	}
}
