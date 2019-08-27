using System.Collections.Generic;
using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// This class is the main entry point for the rendering of controls for OpenTk with Skia.
    /// </summary>
    public class OpenTk2DAnd3DRenderer : OpenTk2DOnlyRenderer
    {
        public override bool Render(IList<Control> controls)
        {
            if (!NeedsRendering(controls))
                return false;

            _needsRendering = false;
            
            _skia.Canvas.Clear();

            var previousWas3DControl = false;
            var state = new OpenGLState();
            
            state.Save();
            _skia.ResetContext();
            _canvas = _skia.Canvas;
            
            for (int i = 0; i < controls.Count; i++)
            {
                var control = controls[i];
                var is3DControl = control is Control3D;

                if (is3DControl && !previousWas3DControl)
                {
                    _canvas.Flush();
                    state.Restore();
                }
                else if (!is3DControl && previousWas3DControl)
                {
                    state.Save();
                    _skia.ResetContext();
                    _canvas = _skia.Canvas;
                }
                
                if(is3DControl)
                    OpenGLState.KeepState(() => Render(control));
                else
                    Render(control);

                previousWas3DControl = is3DControl;
            }

            _canvas.Flush();
            state.TryRestore();

            return true;
        }
    }
}
