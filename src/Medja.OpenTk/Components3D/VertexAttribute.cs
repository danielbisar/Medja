using System;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public // A vertex attribute is anything from position (vertex), color, indices, normals, ...
        class VertexAttribute : IDisposable
    {
        public VertexBufferObject VertexBufferObject { get; set; }

        public VertexArrayObject Owner { get; }
        public int Id { get; }
        public VertexAttributeType Type { get; }

        public VertexAttribute(VertexAttributeType type, VertexArrayObject owner, int vertexAttributeId)
        {
            Type = type;
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Id = vertexAttributeId;
        }

        public void Enable()
        {
            // necessary for all OpenGL version < 4.5
            // see https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/glEnableVertexAttribArray.xhtml
            Owner.Bind();

            GL.EnableVertexAttribArray(Id);
        }

        public void DefinePointer(VertexBufferObject vbo)
        {
            if (VertexBufferObject != null)
                throw new InvalidOperationException("The vertex attribute pointer can only be defined once");

            VertexBufferObject = vbo ?? throw new ArgumentNullException(nameof(vbo));

            // necessary for all OpenGL versions < 4.5
            // see https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/glEnableVertexAttribArray.xhtml
            Owner.Bind();
            vbo.Bind();

            GL.VertexAttribPointer(Id, vbo.ComponentsPerVertex, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void Dispose()
        {
            Owner.Bind();
            GL.DisableVertexAttribArray(Id);

            VertexBufferObject.Dispose();
            VertexBufferObject = null;
        }
    }
}