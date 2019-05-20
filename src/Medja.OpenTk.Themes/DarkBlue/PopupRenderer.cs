using Medja.Controls;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class PopupRenderer : PopupRendererBase
    {
        public PopupRenderer(Popup control) 
            : base(control)
        {
        }
        
        protected override void InternalRender()
        {
            _canvas.DrawRoundRect(_rect, 3, 3, DefaultBackgroundPaint);
        }
    }
}