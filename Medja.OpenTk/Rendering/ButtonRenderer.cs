using Medja.Controls;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class ButtonRenderer : ControlRendererBase<SKCanvas, Button>
    {
        protected override void Render(SKCanvas skiaCanvas, Button button)
        {
            var position = button.Position;
            var skRect = position.ToSKRect();

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;

                paint.Color = button.InputState.IsMouseOver ? SKColors.Red : SKColors.Black;
                skiaCanvas.DrawRect(skRect, paint);

                paint.Color = SKColors.Red;
                skiaCanvas.DrawLine(skRect.Left, skRect.Bottom, skRect.Right, skRect.Bottom, paint);

                var text = button.Text;

                if (string.IsNullOrEmpty(text))
                    return;

                paint.Color = SKColors.White;

                var width = paint.MeasureText(text);
                var height = paint.TextSize;

                skiaCanvas.DrawText(text, skRect.MidX - width / 2, skRect.MidY + height / 2, paint);
            }
        }
    }
}
