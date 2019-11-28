using System;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D.Vertices
{
    /// <summary>
    /// Is an OpenGL buffer containing indices for rendering objects with VertexBuffers.
    /// </summary>
    public class ElementBufferObject : IDisposable
    {
        public int Id { get; }
        public BufferUsageHint UsageHint { get; set; }
        
        public int IndexCount { get; private set; }

        /// <summary>
        /// Creates a new instance. This should not be used directly, use
        /// <see cref="VertexArrayObject.CreateElementBufferObject"/> instead, since
        /// OpenGL registers GL.BindBuffer calls with BufferTarget.ElementArrayBuffer,
        /// when an VertexArrayObject is bound.
        /// </summary>
        internal ElementBufferObject()
        {
            Id = GL.GenBuffer();
            UsageHint = BufferUsageHint.StaticDraw;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
        }

        public void SetData(uint[] indices)
        {
            IndexCount = indices.Length;
                
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexCount * sizeof(uint), indices, UsageHint);
        }
        
        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}