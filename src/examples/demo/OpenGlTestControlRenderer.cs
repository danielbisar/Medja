using System;
using System.Collections.Generic;
using Medja.OpenTk.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Medja.Demo
{
	public class OpenGlTestControlRenderer : OpenTKControlRendererBase<OpenGlTestControl>
	{
		private static readonly Random _rnd = new Random();
		
		private static double GetRandomNumber(double minimum, double maximum)
		{ 
			return _rnd.NextDouble() * (maximum - minimum) + minimum;
		}
		
		private readonly VertextBufferObject _vbo;
		private float _rotation;
		
		public OpenGlTestControlRenderer(OpenGlTestControl control)
		: base(control)
		{
			_rotation = (float) GetRandomNumber(1, 100);
			var vertices = new List<Vector3>();

			var depth = 1000;
			var width = 1000;
			
			for (int i = 0; i <= depth; i++)
			{
				for (int j = 0; j < width; j++)
				{
					var x = j / 100.0f;
					var y = (float)Math.Sin(x * Math.PI);
					var z = -(1 + i / 100.0f);

					var vt = new Vector3(x, y, z);

					vertices.Add(vt);
				}
			}

			vertices = ReorderPointsForQuads(vertices, width);

			_vbo = new VertextBufferObject();
			_vbo.UpdateData(vertices);
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
		
		
		protected override void InternalRender()
		{
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Rotate(_rotation += 1, new Vector3(0, 1, 0));

			GL.Translate(new Vector3(-0.5f, -0.5f, 1.0f));
			GL.Color4(Color4.White);
			
			_vbo.Draw(PrimitiveType.Points);
		}
	}
}
