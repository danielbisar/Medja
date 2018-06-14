using Medja.Theming;
using SkiaSharp;
using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public class ProgressBarRenderer : ControlRendererBase<SKCanvas, ProgressBar>
	{
		protected override void Render(SKCanvas context, ProgressBar control)
		{
			var position = control.Position;

			var backgroundRect = position.ToSKRect();

			var filledRect = new SKRect(backgroundRect.Left,
										  backgroundRect.Top,
										  backgroundRect.Left + backgroundRect.Width * control.Percentage,
										  backgroundRect.Bottom);

			using (var paint = new SKPaint())
			{
				paint.IsAntialias = true;

				paint.Color = SKColors.Black;
				context.DrawRect(backgroundRect, paint);

				paint.Color = SKColors.Red;
				context.DrawRect(filledRect, paint);
			}
		}
	}
}
