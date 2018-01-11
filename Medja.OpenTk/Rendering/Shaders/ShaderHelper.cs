using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering.Shaders
{
    public static class ShaderHelper
    {
        /// <summary>
        /// Compiles the shader and throws an exception on error.
        /// </summary>
        /// <param name="shaderId">The shader id.</param>
        public static void Compile(int shaderId)
        {
            GL.CompileShader(shaderId);

            int result;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out result);

            if (result != 0)
                throw new Exception("VertextShader compilation error: " + GL.GetShaderInfoLog(shaderId));
        }

        public static int CreateProgram(params Shader[] shaders)
        {
            return CreateProgram((IEnumerable<Shader>)shaders);
        }

        public static int CreateProgram(IEnumerable<Shader> shaders)
        {
            var programId = GL.CreateProgram();

            foreach(var shader in shaders)
                GL.AttachShader(programId, shader.Id);

            GL.LinkProgram(programId);

            int result;
            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out result);

            if (result != 0)
                throw new Exception("VertextShader compilation error: " + GL.GetProgramInfoLog(result));

            return programId;
        }
    }
}
