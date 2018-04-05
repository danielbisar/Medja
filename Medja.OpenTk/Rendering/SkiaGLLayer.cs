using System;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Medja.OpenTk.Rendering
{
    public class SkiaGLLayer : IDisposable
    {
        private GRContext _grContext;
        private GRBackendRenderTargetDesc _renderTarget;

        public SkiaGLLayer()
        {
            _grContext = GRContext.Create(GRBackend.OpenGL);
            _renderTarget = CreateRenderTarget();
        }

        public SKSurface CreateSurface()
        {
            return SKSurface.Create(_grContext, _renderTarget);
        }

        public void Resize(int width, int height)
        {
            _renderTarget.Width = width;
            _renderTarget.Height = height;
        }        

        public void Dispose()
        {
            _grContext.Dispose();
        }

        private GRBackendRenderTargetDesc CreateRenderTarget()
        {
            GL.GetInteger(GetPName.FramebufferBinding, out int framebuffer);
            GL.GetInteger(GetPName.StencilBits, out int stencil);
            GL.GetInteger(GetPName.Samples, out int samples);

            int bufferWidth = 0;
            int bufferHeight = 0;

            return new GRBackendRenderTargetDesc
            {
                Width = bufferWidth,
                Height = bufferHeight,
                Config = GRPixelConfig.Bgra8888,
                Origin = GRSurfaceOrigin.BottomLeft,
                SampleCount = samples,
                StencilBits = stencil,
                RenderTargetHandle = (IntPtr)framebuffer,
            };
        }        
    }
}
