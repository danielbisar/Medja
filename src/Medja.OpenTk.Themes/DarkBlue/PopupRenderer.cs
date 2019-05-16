using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class PopupRenderer : SkiaControlRendererBase<Popup>
    {
        private readonly SKPaint _defaultBackgroundPaint;
        
        public PopupRenderer(Popup control) 
            : base(control)
        {
            _defaultBackgroundPaint = CreatePaint();
            _defaultBackgroundPaint.Color = control.Background.ToSKColor();
            _defaultBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
        }

        protected override void InternalRender()
        {
            _canvas.DrawRoundRect(_rect, 3, 3, _defaultBackgroundPaint);
        }
    }
}