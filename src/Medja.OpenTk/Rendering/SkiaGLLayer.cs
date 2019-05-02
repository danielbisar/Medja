using System;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// Acts as glue between OpenTK and SkiaSharp (uses the existing OpenGL context and creates a SkiaSurface from it)
	/// </summary>
	/// <remarks>Before rendering call <see cref="Resize"/> at least once.</remarks>
	public class SkiaGlLayer : IDisposable
	{
		public readonly GRContext _grContext;
		public GRBackendRenderTarget _renderTarget;
		public SKSurface _surface;

		public SKCanvas Canvas { get; private set; }

		public SkiaGlLayer()
		{
			_grContext = GRContext.CreateGl();
		}

		public void ResetContext()
		{
			_grContext.ResetContext();
		}

		public void Resize(int width, int height)
		{
			_renderTarget?.Dispose();
			_renderTarget = CreateRenderTarget(width, height);
			UpdateSurface();
		}

		private void UpdateSurface()
		{
			Canvas?.Dispose();
			_surface?.Dispose();

			OpenGLState.KeepState(() => 
			{
				_surface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, GetColorType());
                Canvas = _surface.Canvas;
                Canvas.Scale(0.5f);
                ResetContext();
            });
		}

		public void Dispose()
		{
			Canvas?.Dispose();
			_surface?.Dispose();
			_renderTarget?.Dispose();
			_grContext.Dispose();
		}

		/// <summary>
		/// Creates the <see cref="GRBackendRenderTarget"/> based on the current OpenGL values.
		/// </summary>
		/// <param name="width">The width the target should have.</param>
		/// <param name="height">The height the target should have.</param>
		/// <returns>The <see cref="GRBackendRenderTarget"/>.</returns>
		private GRBackendRenderTarget CreateRenderTarget(int width, int height)
		{
			GL.GetInteger(GetPName.FramebufferBinding, out var fboId);
			GL.GetInteger(GetPName.Samples, out var sampleCount);
			GL.GetInteger(GetPName.StencilBits, out var stencilBits);
			
			var glInfo = new GRGlFramebufferInfo((uint)fboId, GetColorType().ToGlSizedFormat());
			var renderTarget = new GRBackendRenderTarget(width, height, sampleCount, stencilBits, glInfo);

			return renderTarget;
		}

		private SKColorType GetColorType()
		{
			// TODO operation system specific implementation
			// SKImageInfo.PlatformColorType should theoretically return a valid value but doesn't
			// get the color bit values from OpenGL via
			/*GL.GetInteger(GetPName.RedBits, out var red);
			GL.GetInteger(GetPName.GreenBits, out var green);
			GL.GetInteger(GetPName.BlueBits, out var blue);
			GL.GetInteger(GetPName.AlphaBits, out var alpha);*/

			return SKColorType.Rgba8888;
		}
	}
}
