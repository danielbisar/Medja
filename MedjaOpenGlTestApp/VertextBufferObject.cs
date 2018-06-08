using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MedjaOpenGlTestApp
{
	public class VertextBufferObject : IDisposable
	{
		private readonly int _vertexBufferObjectId;
		private float[] _data;
		private bool _isDisposed;

		public int Id
		{
			get { return _vertexBufferObjectId; }
		}

		public int Length
		{
			get { return _data.Length; }
		}

		private int _divider;
		public int Divider { get { return _divider; } }

		public VertextBufferObject()
		{
			_vertexBufferObjectId = GL.GenBuffer();
			_divider = 1;
		}

		public float[] GetData()
		{
			return _data;
		}

		public void UpdateData(List<Vector3> data, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
		{
			_data = new float[data.Count * 3];

			for (int i = 0; i < data.Count; i++)
			{
				_data[i * 3] = data[i].X;
				_data[i * 3 + 1] = data[i].Y;
				_data[i * 3 + 2] = data[i].Z;
			}

			UpdateData(bufferUsageHint);
		}

		public void UpdateData(BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);
			GL.BufferData(BufferTarget.ArrayBuffer, _data.Length * Vector3.SizeInBytes, _data, bufferUsageHint);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		/*public void DynamicUpdateData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);

            unsafe
            {

                IntPtr intPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

                if (intPtr == IntPtr.Zero)
                    return;

                Vector3* vertexArray = (Vector3*)intPtr;

                for (int i = 0; i < 
            }

            GL.UnmapBuffer(BufferTarget.ArrayBuffer);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }*/

		public void Draw(PrimitiveType type)
		{
			GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectId);

			GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);

			GL.DrawArrays(type, 0, _data.Length / 3);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.DisableClientState(ArrayCap.VertexArray);
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
