using Medja.Controls;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class ButtonRenderer : ControlRendererBase<SKCanvas, Button>
	{
		protected override void Render(SKCanvas canvas, Button button)
		{
			var position = button.Position;
			var skRect = position.ToSKRect();

			using (var paint = new SKPaint())
			{
				paint.IsAntialias = true;

				paint.Color = button.IsEnabled && button.InputState.IsMouseOver ? SKColors.Red : SKColors.Black;
				canvas.DrawRect(skRect, paint);

				paint.Color = button.IsEnabled ? SKColors.Red : SKColors.DarkRed;
				canvas.DrawLine(skRect.Left, skRect.Bottom, skRect.Right, skRect.Bottom, paint);

				paint.Color = button.IsEnabled ? SKColors.White : SKColors.Gray;
				canvas.RenderTextCentered(button.Text, paint, skRect);
			}
		}
	}
}
