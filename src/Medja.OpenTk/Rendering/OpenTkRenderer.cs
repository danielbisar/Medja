using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;
using Medja.Utils.Linq;
using OpenTK;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// This class is the main entry point for the rendering of controls for OpenTk with Skia.
    /// </summary>
    public class OpenTkRenderer
    {
        private readonly GameWindow _gameWindow;
        protected readonly SkiaGlLayer _skia;
        protected bool _needsRendering;
        protected SKCanvas _canvas;
        
        public OpenTkRenderer(GameWindow gameWindow)
        {
            _gameWindow = gameWindow;
            _skia = new SkiaGlLayer();
        }

        public void SetSize(Rectangle rectangle)
        {
            _gameWindow.MakeCurrent();
            _skia.Resize(rectangle.Width, rectangle.Height);
            _needsRendering = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <returns></returns>
        /// <remarks>IList would make it slower.</remarks>
        public virtual void Render(List<Control> controls)
        {
            if (!NeedsRendering(controls)) 
                return;

            _needsRendering = false;

            _gameWindow.MakeCurrent();
            
            _canvas = _skia.Canvas;
            _canvas.Clear();
            
            Control last3DControl = null;
            
            controls.ForEachSplitSeq(p => !p.Is3D, 
                Render, 
                () => _canvas.Flush(), 
                p =>
                {
                    Render(p);
                    last3DControl = p;
                });
            
            SwapBuffers(last3DControl);
        }

        private void SwapBuffers(Control last3DControl)
        {
            if (last3DControl != null && last3DControl.Renderer is IOpenTkControlRenderer x)
            {
                x.MakeContextCurrent();
                x.SwapBuffers();
                _gameWindow.Context.MakeCurrent(_gameWindow.WindowInfo);
            }
            else
            {
                _gameWindow.Context.MakeCurrent(_gameWindow.WindowInfo);
                _gameWindow.Context.SwapBuffers();
            }
        }

        protected bool NeedsRendering(List<Control> controls)
        {
            for (int i = 0; i < controls.Count && !_needsRendering; i++)
            {
                if (controls[i].NeedsRendering)
                    _needsRendering = true;
            }
            
            return _needsRendering;
        }

        protected void Render(Control control)
        {
            control.NeedsRendering = false;

            // it can be that this property has changed during rendering
            // this should fix an issue with the progressbar being displayed
            // even though it should not be visible anymore
            if (!control.IsVisible)
                return;

            var renderer = control.Renderer;
            
            if (renderer != null)
            {
                if(!renderer.IsInitialized)
                    renderer.Initialize();
                    
                renderer.Render(_canvas);
            }
        }

        public void Dispose()
        {
            _skia.Dispose();
        }
    }
}
