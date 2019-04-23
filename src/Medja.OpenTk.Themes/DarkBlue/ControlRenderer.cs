using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Themes.BlackRed;

namespace Medja.OpenTk.Themes.DarkBlue
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