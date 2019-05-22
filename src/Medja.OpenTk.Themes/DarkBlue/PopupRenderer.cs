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
            base.InternalRender();
            _canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);
        }
    }
}