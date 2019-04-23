using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class ProgressBarRenderer : SkiaControlRendererBase<ProgressBar>
	{
		public ProgressBarRenderer(ProgressBar control) 
		: base(control)
		{
		}
		
		protected override void InternalRender()
		{
			_paint.Color = SKColors.Black;
			_canvas.DrawRect(_rect, _paint);

			var filledRect = new SKRect(_rect.Left,
										_rect.Top,
										_rect.Left + _rect.Width * _control.Percentage,
										_rect.Bottom);

			_paint.Color = SKColors.Red;
			_canvas.DrawRect(filledRect, _paint);
		}
	}
}
