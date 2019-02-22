using Medja.Controls;
using SkiaSharp;
using Medja.OpenTk.Rendering;
using Medja.Primitives;

namespace Medja.Demo
{
	public class TextBlockRenderer : TextControlRendererBase<TextBlock>
	{
		private readonly SKPaint _defaultBackgroundPaint;
		
		public TextBlockRenderer(TextBlock control)
			: base(control)
		{
			_defaultBackgroundPaint = CreatePaint();
			_defaultBackgroundPaint.Color = control.Background.ToSKColor();
			_defaultBackgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(4,4,4,4, new SKColor(0,0,0,100), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
		}

		protected override SKPaint CreateTextPaint()
		{
			var result = CreatePaint();
			result.Color = DemoThemeValues.PrimaryTextColor.ToSKColor();

			return result;
		}

		protected override SKPaint CreateTextDisabledPaint()
		{
			var result = CreatePaint();
			result.Color = DemoThemeValues.PrimaryTextColor.GetDisabled().ToSKColor();

			return result;
		}

		protected override void DrawTextControlBackground()
		{
			_canvas.DrawRoundRect(_rect, 3, 3, _defaultBackgroundPaint);
		}
	}
}
