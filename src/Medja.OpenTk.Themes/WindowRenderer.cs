using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public class WindowRenderer : SkiaControlRendererBase<Medja.Controls.Window>
    {
        private readonly SKPaint _backgroundPaint;

        public WindowRenderer(Medja.Controls.Window control)
            : base(control)
        {
            _backgroundPaint = new SKPaint();
        }

        protected override void InternalRender()
        {
            var color = _control.IsEnabled ? _control.Background : _control.Background.GetDisabled();
            _backgroundPaint.Color = color.ToSKColor();

            _canvas.DrawRect(0, 0, _control.Position.Width, _control.Position.Height, _backgroundPaint);
        }
    }
}
