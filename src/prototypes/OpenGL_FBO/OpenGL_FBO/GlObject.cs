using System;
using OpenTK.Graphics.OpenGL4;

namespace MultiOpenGLContext
{
    public class GlObject
    {
        private readonly int _vertexArrayId;
        private int _bufferId; // TODO cleanup/delete
        private int _verticesLength;

        public GlObject(Vertex[] vertices)
        {
            _vertexArrayId = GL.GenVertexArray();
            //_bufferId = GL.GenBuffer();
            
            GL.CreateBuffers(1, out _bufferId);

            GL.BindVertexArray(_vertexArrayId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArrayId);
            
            _verticesLength = vertices.Length;
            
            // requires 4.5 or GL_ARB_direct_state_access
            GL.NamedBufferStorage(_bufferId, Vertex.Size * vertices.Length,
                vertices, BufferStorageFlags.None);

            // position
            GL.VertexArrayAttribBinding(_vertexArrayId, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayId, 0);
            GL.VertexArrayAttribFormat(_vertexArrayId, 0, 4, VertexAttribType.Float, false, 0);

            // color
            GL.VertexArrayAttribBinding(_vertexArrayId, 1, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayId, 1);
            GL.VertexArrayAttribFormat(_vertexArrayId, 1, 4, VertexAttribType.Float, false, 16);

            // tex coords
            GL.VertexArrayAttribBinding(_vertexArrayId, 2, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayId, 2);
            GL.VertexArrayAttribFormat(_vertexArrayId, 2, 2, VertexAttribType.Float, false, 32);
            
            GL.VertexArrayVertexBuffer(_vertexArrayId, 0, _bufferId, IntPtr.Zero, (4 + 4 + 2) * 4);
        }

        public void Render()
        {
            GL.BindVertexArray(_vertexArrayId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticesLength);
        }
    }
}