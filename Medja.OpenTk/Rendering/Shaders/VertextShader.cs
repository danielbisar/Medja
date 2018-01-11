using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering.Shaders
{
    public class VertextShader : Shader
    {
        public VertextShader(string source)
        {
            Source = source;
            Id = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(Id, Source);
        }
    }
}
