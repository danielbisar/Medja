using System;
using System.Collections.Generic;
using Medja.Theming;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Medja.OpenTk.Rendering;

namespace MedjaOpenGlTestApp
{
	public class OpenGlTestControlRenderer : ControlRendererBase<object, OpenGlTestControl>
	{
		// TODO regarding the viewport setup: make a base class that can be used by other controls that automatically setup the viewport
		
		private static readonly Random _rnd = new Random();
		
		private VertextBufferObject _vbo;
		private float _rotation = 1;
		
		private static double GetRandomNumber(double minimum, double maximum)
		{ 
			return _rnd.NextDouble() * (maximum - minimum) + minimum;
		}

		public OpenGlTestControlRenderer()
		{
			_rotation = (float) GetRandomNumber(1, 100);
			var vertices = new List<Vector3>();

			for (int i = 0; i <= 10; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					var x = j / 100.0f;
					var y = (float)Math.Sin(x * Math.PI);
					var z = -(1 + i / 100.0f);

					var vt = new Vector3(x, y, z);

					vertices.Add(vt);

					//Console.Write(vt + ", ");
				}

				//Console.WriteLine();
			}

			vertices = ReorderPointsForQuads(vertices, 100);

			_vbo = new VertextBufferObject();
			_vbo.UpdateData(vertices);
			//_vbo.VertexDrawLimit = 1000;*/
		}

		private List<Vector3> ReorderPointsForQuads(List<Vector3> vertices, int xyItemCount)
		{
			// expect data order x,y on one z line
			var result = new List<Vector3>();

			for (int i = 0; i + 1 + xyItemCount < vertices.Count; i++)
			{
				// last vertex in current row is always ignored
				if ((i + 1) % xyItemCount != 0)
				{
					result.Add(vertices[i]);
					result.Add(vertices[i + 1]);
					result.Add(vertices[i + 1 + xyItemCount]);
					result.Add(vertices[i + xyItemCount]);
				}
			}

			return result;
		}

		protected override void Render(object context, OpenGlTestControl control)
		{
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.ScissorTest);

			GL.Viewport((int)control.Position.X, (int)control.Position.Y, (int)control.Position.Width, (int)control.Position.Height);
			GL.Scissor((int)control.Position.X, (int)control.Position.Y, (int)control.Position.Width, (int)control.Position.Height);
			
			//GL.ClearStencil(0);
			GL.ClearColor(0, 0, 0, 0);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
			
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Rotate(_rotation += 1, new Vector3(0, 1, 0));

			GL.Translate(new Vector3(-0.5f, -0.5f, 1.0f));
			GL.Color4(Color4.White);
			
			_vbo.Draw(PrimitiveType.Quads);
			
			GL.Disable(EnableCap.ScissorTest);
		}
	}
}
