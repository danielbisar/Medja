using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class ProgressBarRenderer : SkiaControlRendererBase<ProgressBar>
	{
		private readonly SKPaint _borderPaint;
		private readonly SKPaint _fillPaint;
		
		public ProgressBarRenderer(ProgressBar control) 
		: base(control)
		{
			_borderPaint = new SKPaint();
			_borderPaint.Color = SKColors.Black;
			_borderPaint.IsAntialias = true;
			
			_fillPaint = new SKPaint();
			_fillPaint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
			_fillPaint.IsAntialias = true;
		}
		
		protected override void InternalRender()
		{
			_canvas.DrawRect(_rect, _borderPaint);

			var filledRect = new SKRect(_rect.Left,
										_rect.Top,
										_rect.Left + _rect.Width * _control.Percentage,
										_rect.Bottom);

			_canvas.DrawRect(filledRect, _fillPaint);
		}

		protected override void Dispose(bool disposing)
		{
			_borderPaint.Dispose();
			_fillPaint.Dispose();
			
			base.Dispose(disposing);
		}
	}
}
