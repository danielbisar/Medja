using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TabItemRenderer : SkiaControlRendererBase<TabItem>
    {
        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _textPaint;
        
        public TabItemRenderer(TabItem control, Color textColor) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            
            _textPaint = new SKPaint();
            _textPaint.IsAntialias = true;
            _textPaint.Color = textColor.ToSKColor();
            _textPaint.TextSize = 16;
            
            _control.AffectRendering(_control.PropertyHeader);
        }

        protected override void InternalRender()
        {
            _backgroundPaint.Color = _control.Background.ToSKColor();
            
            var rect = _control.Position.ToSKRect();
            
            // todo padding for header
            
            _canvas.DrawRect(rect, _backgroundPaint);
            _canvas.DrawTextSafe(_control.Header, _control.Position.X + 10, _control.Position.Y + _textPaint.TextSize + 2, _textPaint);
        }
    }
}