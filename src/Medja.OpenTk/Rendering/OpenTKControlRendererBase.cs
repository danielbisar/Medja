using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// Base class for rendering 3D inside a control
    /// </summary>
    public abstract class OpenTKControlRendererBase<TControl> 
        : ControlRendererBase<SKCanvas, TControl>,
          IOpenTkControlRenderer 
          where TControl : Control3D
    {
        private GraphicsContext _gc;
        private IWindowInfo _windowInfo;

        public OpenTKControlRendererBase(TControl control)
            : base(control)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            var mainWindow = _control.GetRootControl() as OpenTkWindow;
            var gameWindow = mainWindow.GameWindow;
            _windowInfo = gameWindow.WindowInfo;
            
            _gc = new GraphicsContext(GraphicsMode.Default, _windowInfo, 4, 2,
                GraphicsContextFlags.ForwardCompatible);
            _gc.MakeCurrent(_windowInfo);
            
            GL.Enable(EnableCap.ScissorTest);
            Resize(_control.Position);
        }

        public override void Resize(MRect position)
        {
            if(!IsInitialized)
                return;
            
            _gc.MakeCurrent(_windowInfo);
            
            base.Resize(position);

            var x = (int) position.X;
            var width = (int) position.Width;
            var height = (int) position.Height;

            var windowPosition = _control.GetRootControl().Position;

            // OpenGL expects the viewport to be defined with y as the lower point of the control
            // and it's coordinates are turned upside down; this makes it fit to the axis of Skia
            var yb = position.Y + position.Height;
            var y = (int)windowPosition.Height - (int)yb;
            
            GL.Viewport(0, 0, (int)windowPosition.Width, (int)windowPosition.Height);
            GL.Scissor(x, y, width, height);
        }

        protected override void Render(SKCanvas context)
        {
            MakeContextCurrent();
            InternalRender();
            GL.Flush();
        }

        protected abstract void InternalRender();

        public void SwapBuffers()
        {
            if(IsInitialized)
                _gc.SwapBuffers();
        }

        public void MakeContextCurrent()
        {
            if(IsInitialized)
                _gc.MakeCurrent(_windowInfo);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _gc.Dispose();
            _windowInfo = null;
        }
    }
}