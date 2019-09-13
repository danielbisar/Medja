using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class VertexArrayObject : IDisposable
    {
        private int _nextVertexAttributeIndex;
        private int _elementCount;
        private List<VertexAttribute> _attributes;
        
        public int Id { get; }
        
        public VertexArrayObject()
        {
            Id = GL.GenVertexArray();
            
            _nextVertexAttributeIndex = 0;
            _attributes = new List<VertexAttribute>();
        }

        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The id of the attribute.</returns>
        public VertexAttribute AddVertexAttribute(VertexAttributeType type, VertexBufferObject vertexBufferObject)
        {
            if(type == VertexAttributeType.Positions)
                _elementCount = vertexBufferObject.ElementCount;
            
            var attribute = new VertexAttribute(type, this, _nextVertexAttributeIndex);
            attribute.Enable();
            attribute.DefinePointer(vertexBufferObject);
            
            _attributes.Add(attribute);
            _nextVertexAttributeIndex++;
            
            return attribute;
        }

        public void Render()
        {
            Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _elementCount);
        }

        /// <summary>
        /// Gets the header for the OpenGL shader based on the current attribute configuration.
        /// </summary>
        /// <returns>The in variables based on the configured attributes.</returns>
        public string GetAttributeLayoutCode()
        {
            var sb = new StringBuilder();

            foreach (var attribute in _attributes)
            {
                var variableName = attribute.Type switch
                { 
                    VertexAttributeType.Positions => "position",
                    VertexAttributeType.Colors => "color",
                    _ => throw new ArgumentOutOfRangeException()
                };

                sb.AppendFormat("layout(location = {0}) in vec{1} {2};",
                    attribute.Id,
                    attribute.VertexBufferObject.ComponentsPerVertex,
                    variableName);
                sb.AppendLine();
            }
            
            return sb.ToString();
        }

        public void Dispose()
        {
            foreach(var attribute in _attributes)
                attribute.Dispose();
            
            _attributes.Clear();
            GL.DeleteVertexArray(Id);
        }
    }
}