using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
	public class TextBlockRenderer : TextControlRendererBase<TextBlock>
	{
		private readonly SKPaint _backgroundPaint;
		
		public TextBlockRenderer(TextBlock control)
			: base(control)
		{
			_backgroundPaint = new SKPaint();
			_backgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(4,4,4,4, new SKColor(0,0,0,100), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
			
			_control.AffectRendering(control.PropertyBackground);
		}

		protected override void DrawTextControlBackground()
		{
			_backgroundPaint.Color = _control.Background.ToSKColor();
			_canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);
		}

		protected override void Dispose(bool disposing)
		{
			_backgroundPaint.Dispose();
			base.Dispose(disposing);
		}
	}
}
