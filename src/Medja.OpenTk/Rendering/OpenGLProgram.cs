using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
	public class OpenGLProgram : IDisposable
	{
		private readonly int _programId;
		private bool _isDisposed;

		public OpenGLProgram(IEnumerable<OpenGLShader> shaders, Action<int> setupOpenGL)
		{
			_programId = GL.CreateProgram();

			var shaderIds = new List<int>();

			foreach (var shader in shaders)
			{
				// TODO move into shader
				var id = GL.CreateShader(shader.Type);
				shaderIds.Add(id);

				GL.ShaderSource(id, shader.Source);
				GL.CompileShader(id);

				int result;
				GL.GetShader(id, ShaderParameter.CompileStatus, out result);

				if (result != 1)
					throw new Exception(shader.Type + " compilation error: " + GL.GetShaderInfoLog(id));

				GL.AttachShader(_programId, id);
			}

			setupOpenGL?.Invoke(_programId);

			GL.LinkProgram(_programId);
			GL.GetProgram(_programId, GetProgramParameterName.LinkStatus, out var linkStatus);

			if (linkStatus != 1)
				throw new Exception("Shader program compilation error: " + GL.GetProgramInfoLog(_programId));

			foreach (var id in shaderIds)
				GL.DeleteShader(id);
		}

		public void Use()
		{
			GL.UseProgram(_programId);
		}

		public void Unuse()
		{
			GL.UseProgram(0);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				Unuse();
				GL.DeleteProgram(_programId);
				_isDisposed = true;
			}
		}
	}
}
