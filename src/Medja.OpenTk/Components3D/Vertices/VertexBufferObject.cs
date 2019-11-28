using System;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D.Vertices
{
    public class VertexBufferObject : IDisposable
    {
        public int Id { get; }
        public BufferUsageHint UsageHint { get; set; }

        /// <summary>
        /// How much components per vertex (1, 2, 3 or 4). 4 is the OpenGL default value. This is the second parameter
        /// for glVertexAttribPointer.
        /// </summary>
        public int ComponentsPerVertex { get; set; }

        /// <summary>
        /// The number of positions/colors etc defined.
        /// </summary>
        public int ElementCount { get; private set; }

        public VertexBufferObject()
        {
            Id = GL.GenBuffer();
            UsageHint = BufferUsageHint.StaticDraw;
            ComponentsPerVertex = 4;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        }

        public void SetData(float[] data)
        {
            // sanity check
            if (data.Length % ComponentsPerVertex != 0)
                throw new InvalidOperationException("Invalid data length. Each vertex should consist of " +
                                                    ComponentsPerVertex + " float values. The data array contains: " +
                                                    data.Length + " elements");

            // copy data to GPU memory
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, UsageHint);

            ElementCount = data.Length / ComponentsPerVertex;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}