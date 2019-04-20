using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// Base class for rendering 3D inside a control
    /// </summary>
    public abstract class OpenTKControlRendererBase<TControl> : ControlRendererBase<SKCanvas, TControl> 
            where TControl : Control
    {
        /// <summary>
        /// Set the clear buffer mask, is used in <see cref="Clear"/>.
        /// </summary>
        protected ClearBufferMask _clearBufferMask;
        protected int[] _originalViewport;

        public OpenTKControlRendererBase(TControl control)
            : base(control)
        {
            _clearBufferMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                    ClearBufferMask.StencilBufferBit;
            _originalViewport = new int[4];
        }

        /// <summary>
        /// Basic implementation what happens if you render a control with OpenTK.
        /// </summary>
        /// <param name="context">Is ignored.</param>
        protected override void Render(SKCanvas context)
        {
            Setup3D();
            InternalRender();
            Reset3D();
        }

        protected abstract void InternalRender();

        /// <summary>
        /// Setup OpenTK/OpenGL. If you need a different setup override this method. Important: you should enable
        /// ScissorTest and call GL.Viewport and GL.Scissor to limit the drawing area to the controls position. 
        /// </summary>
        protected virtual void Setup3D()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ScissorTest);

            var position = _control.Position;

            // precondtion: viewport was setup for full screen height before using this method
            GL.GetInteger(GetPName.Viewport, _originalViewport);

            // OpenGL expects the viewport to be defined with y as the lower point of the control
            // and its coordinates are turned upside down
            var translatedY = _originalViewport[3] - (position.Y + position.Height);
            
            GL.Viewport((int)position.X, (int)translatedY, (int)position.Width, (int)position.Height);
            GL.Scissor((int)position.X, (int)translatedY, (int)position.Width, (int)position.Height);
			
            Clear();
        }
        
        /// <summary>
        /// Override if you want to do something different than set the clear color to the controls background
        /// (or black) and use the <see cref="_clearBufferMask"/> when calling <see cref="GL.Clear"/>. 
        /// </summary>
        protected virtual void Clear()
        {
            var background = _control.Background ?? Colors.Black;
            
            //GL.ClearStencil(0);
            GL.ClearColor(background.Red, background.Green, background.Blue, 0);
            GL.Clear(_clearBufferMask);
        }

        /// <summary>
        /// Is called at the end of <see cref="Render"/> and disables
        /// <see cref="EnableCap.ScissorTest"/> and <see cref="EnableCap.DepthTest"/>.
        /// </summary>
        protected virtual void Reset3D()
        {
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.DepthTest);
            
            GL.Viewport(_originalViewport[0], _originalViewport[1], _originalViewport[2], _originalViewport[3]);
        }
    }
}