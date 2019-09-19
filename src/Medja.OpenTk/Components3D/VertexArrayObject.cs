using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// A vertex array object.
    /// </summary>
    public class VertexArrayObject : IDisposable
    {
        private int _nextVertexAttributeIndex;
        private int _elementCount;
        private List<VertexAttribute> _attributes;
        private ElementBufferObject _elementBufferObject;

        public int Id { get; }

        private PrimitiveType _primitiveType;
        public PrimitiveType PrimitiveType
        {
            get { return _primitiveType; }
            set
            {
                // quads are no longer supported by OpenGL so let the user know at some point
                // for old OpenGL it might work, but this API is mainly for modern OpenGL
                if(value == PrimitiveType.Quads)
                    throw new NotSupportedException("GL_QUADS is deprecated since version ~3"); 
                   
                _primitiveType = value;
            }
        }

        public VertexArrayObject()
        {
            Id = GL.GenVertexArray();
            PrimitiveType = PrimitiveType.Triangles;
            
            _nextVertexAttributeIndex = 0;
            _attributes = new List<VertexAttribute>();
        }

        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        /// <summary>
        /// Adds a vertex attribute to the vertex array object.
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

        public ElementBufferObject CreateElementBufferObject()
        {
            // without the bind the EBO will not be part of the VAO
            Bind();
            return _elementBufferObject = new ElementBufferObject();
        }

        public void Render()
        {
            Bind();
            
            if(_elementBufferObject == null)
                GL.DrawArrays(PrimitiveType, 0, _elementCount);
            else
                GL.DrawElements(PrimitiveType, _elementBufferObject.IndexCount, DrawElementsType.UnsignedInt, 0);
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
                    VertexAttributeType.TextureCoordinates => "textureCoord",
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

            _elementBufferObject?.Dispose();
            GL.DeleteVertexArray(Id);
        }

        public bool HasAttributeOfType(VertexAttributeType vertexAttributeType)
        {
            return _attributes.Any(p => p.Type == vertexAttributeType);
        }
    }
}