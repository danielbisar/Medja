using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class VerticalScrollBarRenderer : SkiaControlRendererBase<VerticalScrollBar>
    {
        private readonly SKPaint _background;
        private readonly SKPaint _fill;
        
        public VerticalScrollBarRenderer(VerticalScrollBar control) 
            : base(control)
        {
            _background = new SKPaint();
            _background.IsAntialias = true;
            _background.Style = SKPaintStyle.Fill;

            _fill = new SKPaint();
            _fill.IsAntialias = true;
            _fill.Style = SKPaintStyle.Fill;
            _fill.Color = ThemeDarkBlueValues.PrimaryColor.ToSKColor();

            _control.AffectsRendering(_control.PropertyValue);
            _control.AffectsRendering(_control.PropertyMaxValue);
        }

        protected override void InternalRender()
        {
            _background.Color = _control.Background.ToSKColor();
            _canvas.DrawRect(_rect, _background);

            if (_control.MaxValue <= 0)
                return;

            var notVisibleHeight = _control.MaxValue;
            var height = _rect.Height + notVisibleHeight;
            var fillHeight = _rect.Height * (_rect.Height / height);

            if (fillHeight < 10)
                fillHeight = 10;

            var pos = _rect.Top + _control.Percentage * (_rect.Height - fillHeight);
            _canvas.DrawRect(_rect.Left, pos, _rect.Width, fillHeight, _fill);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            _background.Dispose();
            _fill.Dispose();
        }
    }
}