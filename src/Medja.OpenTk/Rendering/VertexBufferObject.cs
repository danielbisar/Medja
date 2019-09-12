using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// Abstracts the creation and drawing of an OpenGL VBO (VertexBufferObject).
    /// </summary>
	public class VertexBufferObject : IDisposable
	{
		private int _vertexBufferObjectId;
		private float[] _data;
		private bool _isDisposed;
		private int _dataLengthDiv3;
        private Action<PrimitiveType> _draw;
        private int _stride; // the length of each data package in bytes
        
		public int Id
		{
			get { return _vertexBufferObjectId; }
		}

		public int FloatValueCount
		{
			get { return _data == null ? -1 : _data.Length; }
		}

		/// <summary>
		/// Gets or sets the amount of vertices to draw.
		/// </summary>
		/// <value>The vertex draw limit. -1 = no limit, another value = even though Data might contain more vertices, only draw the given number.</value>
		public int VertexDrawLimit
		{
			get; set;
		}

        private VBODataType _dataType;
        public VBODataType DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                UpdateDrawMethod();
            }
        }

        public VertexBufferObject()
		{
			_vertexBufferObjectId = -1;
			VertexDrawLimit = -1;
            UpdateDrawMethod();
		}

        /// <summary>
        /// Gets the internal data array.
        /// </summary>
        /// <returns>The float value array.</returns>
        public float[] GetData()
        {
            return _data;
        }

		/// <summary>
		/// Creates the data array without filling it.
		/// </summary>
		/// <param name="length">Length.</param>
		/// <param name="bufferUsageHint">Buffer usage hint.</param>
		public void CreateData(int length, BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw)
		{
			// length of data of VBO can't be changed so we need to recreate it
			if (_vertexBufferObjectId != -1)
				GL.DeleteBuffer(_vertexBufferObjectId);

			_data = new float[length];
			_dataLengthDiv3 = _data.Length / 3;

			_vertexBufferObjectId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.BufferData(BufferTarget.ArrayBuffer, _data.Length * sizeof(float), _data, bufferUsageHint);
		}

        /// <summary>
        /// Updates the data. If the data array wasn't create before it will be.
        /// </summary>
        /// <param name="data">The vertices.</param>
        /// <param name="bufferUsageHint">Hint for OpenGL how often the vertices will change.</param>
		public void UpdateData(List<Vector3> data, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
		{
			var newDataLength = data.Count * 3;

			if (newDataLength != FloatValueCount)
				CreateData(newDataLength, bufferUsageHint);

			UpdateData(ptr =>
			{
				unsafe
				{
					float* vertexArray = (float*)ptr;

					for (int i = 0; i < data.Count; i++)
					{
						vertexArray[i * 3] = data[i].X;
						vertexArray[i * 3 + 1] = data[i].Y;
						vertexArray[i * 3 + 2] = data[i].Z;
					}
				}
			});
		}

		private void UpdateData(Action<IntPtr> update, BufferAccess bufferAccess = BufferAccess.WriteOnly)
		{
			/*GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.VertexPointer(3, VertexPointerType.Float, _stride, 0);

			var verticesPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, bufferAccess);

			update(verticesPtr);

			GL.UnmapBuffer(BufferTarget.ArrayBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableClientState(ArrayCap.VertexArray);*/
		}

		public void Draw(PrimitiveType type)
		{
            _draw(type);
		}

        private void DrawVertices(PrimitiveType type)
        {
            /*GL.EnableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
            GL.VertexPointer(3, VertexPointerType.Float, _stride, 0);

            GL.DrawArrays(type, 0, GetVerticesCount());

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);*/
        }

        private void DrawVerticesAndNormals(PrimitiveType type)
        {
            /*GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
            
            GL.VertexPointer(3, VertexPointerType.Float, _stride, 0);
            GL.NormalPointer(NormalPointerType.Float, _stride, Vector3.SizeInBytes);

            // todo try performance of DrawElements vs DrawArrays
            GL.DrawArrays(type, 0, GetVerticesCount());
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.VertexArray);*/
        }

		public void UpdateAndDraw(PrimitiveType type, Action<IntPtr> update, BufferAccess bufferAccess = BufferAccess.WriteOnly)
		{
			/*GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.VertexPointer(3, VertexPointerType.Float, _stride, 0);

			var verticesPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, bufferAccess);

			update(verticesPtr);

			GL.UnmapBuffer(BufferTarget.ArrayBuffer);

			GL.DrawArrays(type, 0, GetVerticesCount());

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableClientState(ArrayCap.VertexArray);*/
		}

        private void UpdateDrawMethod()
        {
            switch (_dataType)
            {
                case VBODataType.Vertices:
                    _draw = DrawVertices;
                    _stride = Vector3.SizeInBytes;
                    break;
                case VBODataType.VerticesAndNormals:
                    _draw = DrawVerticesAndNormals;
                    _stride = Vector3.SizeInBytes * 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

		private int GetVerticesCount()
		{
			return VertexDrawLimit == -1 ? _dataLengthDiv3 : Math.Min(_dataLengthDiv3, VertexDrawLimit);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				GL.DeleteBuffer(_vertexBufferObjectId);
			}
		}
	}
}
