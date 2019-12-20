using System;
using Medja.Controls;

namespace Medja.OpenTk.Controls
{
    /// <summary>
    /// Allows to place the OpenGL render code in the event handlers.
    /// </summary>
    public class OpenGLEventControl : Control3D
    {
        public event EventHandler Load;
        public event EventHandler RenderControl;

        public void RaiseLoad()
        {
            Load?.Invoke(this, EventArgs.Empty);
        }
        
        public void RaiseRender()
        {
            RenderControl?.Invoke(this, EventArgs.Empty);
        }
    }
}