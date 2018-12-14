using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// Base class for rendering 3D inside a control
    /// </summary>
    public abstract class OpenTKControlRendererBase<TControl> : ControlRendererBase<SKCanvas, TControl> 
            where TControl : Control
    {
        protected TControl _control;
        
        /// <summary>
        /// Set the clear buffer mask, is used in <see cref="Clear"/>.
        /// </summary>
        protected ClearBufferMask _clearBufferMask;

        public OpenTKControlRendererBase()
        {
            _clearBufferMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                    ClearBufferMask.StencilBufferBit;
        }

        /// <summary>
        /// Basic implementation what happens if you render a control with OpenTK.
        /// </summary>
        /// <param name="context">Is ignored.</param>
        /// <param name="control">The control that should be rendered. Saved into <see cref="_control"/>.</param>
        protected override void Render(SKCanvas context, TControl control)
        {
            _control = control;
            
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
            
            GL.Viewport((int)position.X, (int)position.Y, (int)position.Width, (int)position.Height);
            GL.Scissor((int)position.X, (int)position.Y, (int)position.Width, (int)position.Height);
			
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
        }
    }
}