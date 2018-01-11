using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering.Shaders
{
    public class Program : IDisposable
    {
        private readonly List<Shader> _shaders;
        private bool _isDisposed = false;

        public int Id { get; protected set; }        

        public Program()
        {
            _shaders = new List<Shader>();
        }

        public void AddShader(Shader shader)
        {
            _shaders.Add(shader);
        }

        public void CompileAndLink()
        {
            foreach (var shader in _shaders)
                shader.Compile();

            Link();
        }

        public void Link()
        {
            var programId = GL.CreateProgram();

            foreach (var shader in _shaders)
                GL.AttachShader(programId, shader.Id);

            GL.LinkProgram(programId);

            int result;
            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out result);

            if (result != 1)
                throw new Exception("ShaderProgram compilation error: " + GL.GetProgramInfoLog(result));

            foreach (var shader in _shaders)
                shader.Dispose();

            _shaders.Clear();
            
            Id = programId;
        }        

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposing)
                {
                    foreach (var shader in _shaders)
                        shader.Dispose();

                    _shaders.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.                
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Program() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
