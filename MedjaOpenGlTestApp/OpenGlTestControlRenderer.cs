using System;
using System.Collections.Generic;
using Medja.Theming;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MedjaOpenGlTestApp
{
	public class OpenGlTestControlRenderer : ControlRendererBase<object, OpenGlTestControl>
	{
		private readonly int _programId;
		private readonly int _vboId;
		private readonly int _vboColorsId;
		private float[] _data;

		public OpenGlTestControlRenderer()
		{
			//// define the shaders
			//var vertexShader = @"

			//         #version 130

			//         in vec3 aPosition;
			//         //out vec4 vertexColor;

			//         void main()
			//         {
			//             gl_Position = vec4(aPosition, 1);
			//             //vertexColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
			//         }
			//         ";

			//var fragmentShader = @"
			//         #version 130

			//         //in vec4 vertexColor;
			//         out vec4 fragColor;

			//         void main()
			//         {
			//             fragColor = vec4(0,1,1);
			//         }";

			//var vertShaderId = GL.CreateShader(ShaderType.VertexShader);
			//GL.ShaderSource(vertShaderId, vertexShader);
			//GL.CompileShader(vertShaderId);


			//int result;
			//GL.GetShader(vertShaderId, ShaderParameter.CompileStatus, out result);
			//if (result != 1)
			//	throw new Exception("VertextShader compilation error: " + GL.GetShaderInfoLog(vertShaderId));



			//var fragShaderId = GL.CreateShader(ShaderType.FragmentShader);
			//GL.ShaderSource(fragShaderId, fragmentShader);
			//GL.CompileShader(fragShaderId);

			//GL.GetShader(vertShaderId, ShaderParameter.CompileStatus, out result);
			//if (result != 1)
			//	throw new Exception("FragmentShader compilation error: " + GL.GetShaderInfoLog(fragShaderId));




			//var programId = GL.CreateProgram();
			//GL.AttachShader(programId, vertShaderId);
			////GL.AttachShader(programId, fragShaderId);
			//GL.LinkProgram(programId);

			//GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out result);

			//Console.WriteLine(GL.GetProgramInfoLog(programId));

			//if (result != 1)
			//	throw new Exception("ShaderProgram compilation error: " + GL.GetProgramInfoLog(programId));

			//GL.DeleteShader(vertShaderId);
			//GL.DeleteShader(fragShaderId);

			//_programId = programId;

			var vertices = new List<Vector3>();


			for (float i = 0; i <= 1; i += 0.01f)
				vertices.Add(new Vector3(i, 0, -1));

			vertices.Add(new Vector3(1, 1, -1));



			_data = new float[vertices.Count * 3];

			for (int i = 0; i < vertices.Count; i++)
			{
				_data[i * 3] = vertices[i].X;
				_data[i * 3 + 1] = vertices[i].Y;
				_data[i * 3 + 2] = vertices[i].Z;
			}

			float[] colors = new float[vertices.Count * 3];
			float[] prClrs = new float[] { 0, 1 };

			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = prClrs[i % 2];
			}

			_vboId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
			GL.BufferData(BufferTarget.ArrayBuffer, _data.Length * Vector3.SizeInBytes, _data, BufferUsageHint.StaticDraw);

			_vboColorsId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vboColorsId);
			GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

		}

		protected override void Render(object context, OpenGlTestControl control)
		{
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.Rotate(1, 0, 1, 0);
			//GL.Translate(0, 0, -1);

			//GL.Color3(0, 1.0f, 0);
			//GL.Begin(BeginMode.Triangles);

			//GL.Vertex3(0, 0, -1);
			//GL.Vertex3(1, 0, -1);
			//GL.Vertex3(1, 1, -1);

			//GL.End();

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.ColorArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
			GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vboColorsId);
			GL.ColorPointer(3, ColorPointerType.Float, 0, 0);

			GL.DrawArrays(PrimitiveType.Points, 0, _data.Length / 3);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.DisableClientState(ArrayCap.VertexArray);
		}
	}
}
