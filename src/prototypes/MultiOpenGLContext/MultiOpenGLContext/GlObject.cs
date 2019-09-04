using System;
using OpenTK.Graphics.OpenGL4;

namespace MultiOpenGLContext
{
    public class GlObject
    {
        private readonly int _vertexArrayId;
        private int _bufferId; // TODO cleanup/delete

        public GlObject(Vertex[] vertices)
        {
            _vertexArrayId = GL.GenVertexArray();
            //_bufferId = GL.GenBuffer();
            
            GL.CreateBuffers(1, out _bufferId);

            GL.BindVertexArray(_vertexArrayId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArrayId);

            // requires 4.5 or GL_ARB_direct_state_access
            GL.NamedBufferStorage(_bufferId, Vertex.Size * vertices.Length,
                vertices, BufferStorageFlags.None);

            GL.VertexArrayAttribBinding(_vertexArrayId, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayId, 0);
            GL.VertexArrayAttribFormat(_vertexArrayId, 0, 4, VertexAttribType.Float, false, 0);

            GL.VertexArrayAttribBinding(_vertexArrayId, 1, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayId, 1);
            GL.VertexArrayAttribFormat(_vertexArrayId, 1, 4, VertexAttribType.Float, false, 16);

            GL.VertexArrayVertexBuffer(_vertexArrayId, 0, _bufferId, IntPtr.Zero, (4 + 4) * 4);
        }

        public void Render()
        {
            GL.BindVertexArray(_vertexArrayId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }
    }
}