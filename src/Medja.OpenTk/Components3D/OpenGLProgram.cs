using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class OpenGLProgram : IDisposable
    {
        public static OpenGLProgram CreateAndCompile(params OpenGLShader[] shaders)
        {
            var program = new OpenGLProgram();

            foreach (var shader in shaders)
            {
                if (!shader.IsCompiled)
                    shader.Compile();

                program.Attach(shader);
            }
            
            program.Link();
            
            return program;
        }

        private List<OpenGLShader> _attachedShaders;
        private bool _isDisposed;
        private bool _isLinked;
       
        private readonly int _id;
        /// <summary>
        /// Gets the OpenGL program id 
        /// </summary>
        public int Id
        {
            get { return _id; }
        }
        
        public OpenGLProgram()
        {
            _id = GL.CreateProgram();
            _attachedShaders = new List<OpenGLShader>();
        }

        public void Attach(OpenGLShader shader)
        {
            if(!shader.IsCompiled)
                throw new InvalidOperationException("compile the shader first");
            
            _attachedShaders.Add(shader);
            GL.AttachShader(_id, shader.Id);
        }

        public void Link()
        {
            if(_isLinked)
                throw new InvalidOperationException("Program was linked already");
            
            GL.LinkProgram(_id);
            GL.GetProgram(_id, GetProgramParameterName.LinkStatus, out var linkStatus);

            if (linkStatus != 1) // 1 for true
                throw new Exception("Shader program compilation error: " + GL.GetProgramInfoLog(_id));

            DisposeAttachedShaders();
            _isLinked = true;
        }

        public void Use()
        {
            GL.UseProgram(_id);
        }

        public void Unuse()
        {
            GL.UseProgram(0);
        }

        private void DisposeAttachedShaders()
        {
            if(_attachedShaders == null)
                return;
            
            foreach (var shader in _attachedShaders)
                shader.Dispose();

            _attachedShaders = null;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Unuse();
                DisposeAttachedShaders();
                GL.DeleteProgram(_id);
                _isDisposed = true;
            }
        }

        public GLUniform GetUniform(string name)
        {
            Use();
            return new GLUniform(Id, name);
        }
    }
}
