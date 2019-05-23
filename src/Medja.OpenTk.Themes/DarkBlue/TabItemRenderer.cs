using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TabItemRenderer : SkiaControlRendererBase<TabItem>
    {
        private readonly SKPaint _backgroundPaint;
        
        public TabItemRenderer(TabItem control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
        }

        protected override void InternalRender()
        {
            _backgroundPaint.Color = _control.Background.ToSKColor();
            var rect = _control.Position.ToSKRect();
            
            _canvas.DrawRect(rect, _backgroundPaint);
        }
    }
}