using System;
using System.Collections.Generic;
using System.Linq;
using Medja.OpenTk.Utils;
using Medja.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// A mesh, defines the vertices and indexes that make up the mesh.
    /// </summary>
    public class GLMesh : GLModel
    {
        private List<int> _indices;
        private List<Vector3> _vertices;
        private int _vboId, _vbaId;

        [NonSerialized] 
        public readonly Property<PrimitiveType> PropertyPrimitiveType;
        public PrimitiveType PrimitiveType
        {
            get { return PropertyPrimitiveType.Get(); }
            set { PropertyPrimitiveType.Set(value); }
        }
        
        public GLMesh()
        {
            PropertyPrimitiveType = new Property<PrimitiveType>();
            PropertyPrimitiveType.UnnotifiedSet(PrimitiveType.Triangles);
            
            _indices = new List<int>();
            _vertices = new List<Vector3>();
            _vbaId = _vboId = -1;
        }
        
        public int AddVertex(Vector3 vertex)
        {
            var index = _vertices.IndexOf(vertex);
            
            if(index != -1)
                return index;
            
            _vertices.Add(vertex);
            
            return _vertices.Count - 1;
        }

        public int AddVertex(float x, float y, float z)
        {
            return AddVertex(new Vector3(x, y, z));
        }

        public void AddIndices(int i1, int i2, int i3)
        {
            _indices.Add(i1);
            _indices.Add(i2);
            _indices.Add(i3);
        }

        public void AddIndices(params int[] indices)
        {
            _indices.AddRange(indices);
        }

        public void CreateBuffers()
        {
            if(_vboId != -1 || _vbaId != -1)
                throw new InvalidOperationException("VBO or VBA was already created");

            CreateVBO();

            if (_indices.Count > 0)
                CreateVBA();
        }

        private void CreateVBA()
        {
            if (_vbaId != -1)
                throw new InvalidOperationException("VBA was already created");

            var indexDataArray = _indices.Select(p => (byte) p).ToArray();

            GL.EnableClientState(ArrayCap.VertexArray);

            _vbaId = GL.GenBuffer();
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vbaId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexDataArray.Length * sizeof(byte), indexDataArray,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
        }

        private void CreateVBO()
        {
            if(_vboId != -1)
                throw new InvalidOperationException("VBO was already created");

            var vertexDataArray = _vertices.SelectMany(p => p.Iterate()).ToArray();

            GL.EnableClientState(ArrayCap.VertexArray);
            _vboId = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexDataArray.Length * sizeof(float), vertexDataArray,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
        
        public void SubdivideFaces(Func<Vector3, Vector3, Vector3> getMiddle)
        {
            var result = new List<int>();

            for (int i = 0; i < _indices.Count; i+=3)
            {
                var v1 = _vertices[_indices[i]];
                var v2 = _vertices[_indices[i + 1]];
                var v3 = _vertices[_indices[i + 2]];

                var midV1V2 = getMiddle(v1, v2);
                var midV2V3 = getMiddle(v2, v3);
                var midV3V1 = getMiddle(v3, v1);
                
                // does not add items multiple times but if it is already
                // present returns the index of the vertex
                var midV1V2Index = AddVertex(midV1V2);
                var midV2V3Index = AddVertex(midV2V3);
                var midV3V1Index = AddVertex(midV3V1);
                
                result.Add(_indices[i]);
                result.Add(midV1V2Index);
                result.Add(midV3V1Index);

                result.Add(_indices[i+1]);
                result.Add(midV2V3Index);
                result.Add(midV1V2Index);

                result.Add(_indices[i+2]);
                result.Add(midV3V1Index);
                result.Add(midV2V3Index);

                result.Add(midV1V2Index);
                result.Add(midV2V3Index);
                result.Add(midV3V1Index);
            }
            
            _indices.Clear();
            _indices.AddRange(result);
        }

        public void NormalizeVectors()
        {
            for(int i = 0; i < _vertices.Count; i++)
                _vertices[i] = _vertices[i].Normalized();
        }

        protected override void RenderModel()
        {
            if (_vboId == -1)
                throw new InvalidOperationException("call " + nameof(CreateBuffers) + " first");

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);

            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);

            if (_vbaId == -1)
                GL.DrawArrays(PrimitiveType, 0, _vertices.Count);
            else
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vbaId);
                // potential optimization: use small element types like: unsigned byte if it is possible
                GL.DrawElements(PrimitiveType, _indices.Count, DrawElementsType.UnsignedByte, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
    }
}