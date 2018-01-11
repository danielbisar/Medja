using System;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering
{
    public class VertextBufferObject : IDisposable
    {
        private bool _isDisposed = false;
        public int Id { get; }

        public VertextBufferObject()
        {
            Id = GL.GenBuffer();
        }
        
        public void Copy(float []vertices)
        {
            // copy data to ArrayBuffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            
            // set vertex attr pointer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // unbind
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    GL.DeleteBuffer(Id);
                }
                                
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _isDisposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
