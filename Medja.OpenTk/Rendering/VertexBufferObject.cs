using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
	public class VertextBufferObject : IDisposable
	{
		private int _vertexBufferObjectId;
		private float[] _data;
		private bool _isDisposed;
		private int _dataLengthDiv3;

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

		public float[] GetData()
		{
			return _data;
		}

		public VertextBufferObject()
		{
			_vertexBufferObjectId = -1;
			VertexDrawLimit = -1;
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
			GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);

			var verticesPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, bufferAccess);

			update(verticesPtr);

			GL.UnmapBuffer(BufferTarget.ArrayBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableClientState(ArrayCap.VertexArray);
		}

		public void Draw(PrimitiveType type)
		{
			GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);

			GL.DrawArrays(type, 0, GetVerticesCount());

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableClientState(ArrayCap.VertexArray);
		}

		public void UpdateAndDraw(PrimitiveType type, Action<IntPtr> update, , BufferAccess bufferAccess = BufferAccess.WriteOnly)
		{
			GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);

			var verticesPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, bufferAccess);

			update(verticesPtr);

			GL.UnmapBuffer(BufferTarget.ArrayBuffer);

			GL.DrawArrays(type, 0, GetVerticesCount());

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableClientState(ArrayCap.VertexArray);
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
