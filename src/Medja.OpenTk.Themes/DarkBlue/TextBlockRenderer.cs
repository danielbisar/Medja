using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
	public class TextBlockRenderer : TextControlRendererBase<TextBlock>
	{
		private readonly SKPaint _defaultBackgroundPaint;
		
		public TextBlockRenderer(TextBlock control)
			: base(control)
		{
			_defaultBackgroundPaint = new SKPaint();
			_defaultBackgroundPaint.Color = control.Background.ToSKColor();
			_defaultBackgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(4,4,4,4, new SKColor(0,0,0,100), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
		}

		protected override void DrawTextControlBackground()
		{
			_canvas.DrawRoundRect(_rect, 3, 3, _defaultBackgroundPaint);
		}
	}
}
