using Medja.Controls;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class PopupRenderer : PopupRendererBase
    {
        public PopupRenderer(Popup control) 
            : base(control)
        {
        }
        
        protected override void InternalRender()
        {
            _canvas.DrawRect(_rect, _backgroundPaint);
        }
    }
}