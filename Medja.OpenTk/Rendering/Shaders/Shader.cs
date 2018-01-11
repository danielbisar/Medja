using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering.Shaders
{
    public class Shader : IDisposable
    {
        private bool _isDisposed;

        public int Id { get; protected set; }
        public string Source { get; protected set; }

        public void Compile()
        {
            GL.CompileShader(Id);

            int result;
            GL.GetShader(Id, ShaderParameter.CompileStatus, out result);

            if (result != 1)
                throw new Exception("VertextShader compilation error: " + GL.GetShaderInfoLog(Id));
        }

        private void ReleaseUnmanagedResources()
        {
            if (Id != 0)
            {
                GL.DeleteShader(Id);
                Id = 0;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    // free managed resources
                }

                ReleaseUnmanagedResources();                
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            Dispose(false);
        }
    }
}
