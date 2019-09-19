using System;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class OpenGLShader : IDisposable
    {
        public int Id {get;}

        public bool IsCompiled { get; private set;}
        
        private string _source; // makes debugging easier
        public string Source
        {
            set
            {
                _source = value;
                GL.ShaderSource(Id, value);
            }
        }

        public ShaderType Type { get; }

        public OpenGLShader(ShaderType type)
        {
            Type = type;
            Id = GL.CreateShader(type);
        }

        public void Compile()
        {
            if(IsCompiled)
                throw new InvalidOperationException("Shader was compiled already");
            
            GL.CompileShader(Id);
            GL.GetShader(Id, ShaderParameter.CompileStatus, out var result);

            if (result != 1)
                throw new Exception(Type + " compilation error: " + GL.GetShaderInfoLog(Id));

            _source = null;
            IsCompiled = true;
        }

        public void Dispose()
        {
            GL.DeleteShader(Id);
        }
    }
}
