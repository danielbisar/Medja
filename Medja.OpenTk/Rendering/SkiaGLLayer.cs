using System;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
	public class SkiaGLLayer : IDisposable
	{
		public readonly GRContext _grContext;
		public GRBackendRenderTargetDesc _renderTarget;
		public SKSurface _surface;

		public SKCanvas Canvas { get; private set; }

		public SkiaGLLayer()
		{
			_grContext = GRContext.Create(GRBackend.OpenGL);
			_renderTarget = CreateRenderTarget();

			//UpdateSurface();
		}

		public void ResetContext()
		{
			_grContext.ResetContext();
		}

		public void Resize(int width, int height)
		{
			_renderTarget.Width = width;
			_renderTarget.Height = height;

			UpdateSurface();
		}

		private void UpdateSurface()
		{
			//if (Canvas != null)
			//	Canvas.Dispose();

			//if (_surface != null)
			//_surface.Dispose();

			GL.PushClientAttrib(ClientAttribMask.ClientAllAttribBits);
			_surface = SKSurface.Create(_grContext, _renderTarget);
			GL.PopClientAttrib();
			//Canvas = _surface.Canvas;
			//Canvas.

			//ResetContext();
		}

		public void Dispose()
		{
			if (Canvas != null)
				Canvas.Dispose();

			if (_surface != null)
				_surface.Dispose();

			_grContext.Dispose();
		}

		private GRBackendRenderTargetDesc CreateRenderTarget()
		{
			int frameBuffer;
			int stencil;
			int samples;
			int bufferWidth = 0;
			int bufferHeight = 0;

			GL.GetInteger(GetPName.FramebufferBinding, out frameBuffer);
			GL.GetInteger(GetPName.StencilBits, out stencil);
			GL.GetInteger(GetPName.Samples, out samples);

			return new GRBackendRenderTargetDesc
			{
				Width = bufferWidth,
				Height = bufferHeight,
				Config = GRPixelConfig.Bgra8888,
				Origin = GRSurfaceOrigin.BottomLeft,
				SampleCount = samples,
				StencilBits = stencil,
				RenderTargetHandle = (IntPtr)frameBuffer,
			};
		}
	}
}
