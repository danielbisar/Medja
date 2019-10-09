using System;
using OpenTK.Graphics.OpenGL4;

namespace MultiOpenGLContext
{
    public class FrameBufferObject : IDisposable
    {
        private int _depthBufferId;
        public int Id {get;}
        
        public FrameBufferObject()
        {
            Id = GL.GenFramebuffer();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, Id);
        }

        void dos()
        {
            _depthBufferId = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _depthBufferId);
            // width and height must be power of two
            // except if a specific??? extension is present
            // http://docs.gl/gl4/glGenFramebuffers
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Rgba8, width, height);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(Id);
        }
    }
}