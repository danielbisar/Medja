using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class TextBlockRenderer : SkiaControlRendererBase<TextBlock>
	{
		private readonly SKPaint _textPaint;
		private readonly SKPaint _textDisabledPaint;
		
		private bool _isControlInitialized;
		
		public TextBlockRenderer()
		{
			_textPaint = new SKPaint();
			_textPaint.IsAntialias = true;
			
			_textDisabledPaint = new SKPaint();
			_textDisabledPaint.IsAntialias = true;
		}
		
		protected override void InternalRender()
		{
			RenderBackground();

			// IsLayoutUpdated = true is necessary to be able to call GetLines
			if (string.IsNullOrWhiteSpace(_control.Text) || !_control.IsLayoutUpdated)
				return;

			if (!_isControlInitialized)
				InitControl();

			var paint = _control.IsEnabled ? _textPaint : _textDisabledPaint;
			var lines = _control.GetLines();
			var lineHeight = _paint.FontSpacing;

			var pos = _control.Position.ToSKPoint();
			// add the height also for the first line
			// else it seems the text is drawn at a 
			// too high position
			pos.Y += lineHeight;

			for (int i = 0; i < lines.Length && pos.Y <= _rect.Bottom; i++)
			{
				_canvas.DrawText(lines[i], pos, paint);
				pos.Y += lineHeight;
			}
		}

		private void InitControl()
		{
			var font = _control.Font;
			
			_textPaint.Typeface = SKTypeface.FromFamilyName(font.Name);
			_textPaint.TextSize = font.Size;
			_textPaint.Color = _control.Foreground.ToSKColor();

			_textDisabledPaint.Typeface = _textPaint.Typeface;
			_textDisabledPaint.TextSize = font.Size;
			_textDisabledPaint.Color = _control.Foreground.GetLighter(0.25f).ToSKColor();

			font.GetWidth = _textPaint.MeasureText;
			
			// TODO not supported scenarios:
			// - changing of foreground color of the control
			// - changing of any value in font of the control
			// - using the actual font and size defined in the font object

			_isControlInitialized = true;
		}
	}
}
