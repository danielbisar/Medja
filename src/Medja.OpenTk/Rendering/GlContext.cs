using System;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace Medja.OpenTk
{
    public class GlContext : IDisposable
    {
        public readonly IGraphicsContext _context;
        public readonly GlContextActions Actions;

        public GlContext(IGraphicsContext context)
        {
            Actions = new GlContextActions();
            _context = context;
        }

        public void MakeCurrent(IWindowInfo windowInfo)
        {
            _context.MakeCurrent(windowInfo);
        }

        public void SwapBuffers()
        {
            _context.SwapBuffers();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}