using Medja.Controls;
using Medja.OpenTk.Rendering;

namespace Medja.Demo
{
    public class ControlRenderer : SkiaControlRendererBase<Control>
    {
        public ControlRenderer(Control control) 
            : base(control)
        {
        }
		
        protected override void InternalRender()
        {
            RenderBackground();
        }
    }
}