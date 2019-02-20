using Medja.Controls;
using SkiaSharp;
using Medja.OpenTk.Rendering;
using Medja.Primitives;

namespace Medja.Demo
{
	public class TextBlockRenderer : SkiaControlRendererBase<TextBlock>
	{
		private readonly SKPaint _defaultBackgroundPaint;
		private readonly SKPaint _textPaint;
		private readonly SKPaint _textDisabledPaint;
		
		private bool _isControlInitialized;
		
		public TextBlockRenderer(TextBlock control)
			: base(control)
		{
			_defaultBackgroundPaint = CreatePaint();
			_defaultBackgroundPaint.Color = control.Background.ToSKColor();
			_defaultBackgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(4,4,4,4, new SKColor(0,0,0,100), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
			
			_textPaint = new SKPaint();
			_textPaint.IsAntialias = true;
			
			_textDisabledPaint = new SKPaint();
			_textDisabledPaint.IsAntialias = true;
		}
		
		protected override void InternalRender()
		{
			_canvas.DrawRoundRect(_rect, 3, 3, _defaultBackgroundPaint);	

			// IsLayoutUpdated = true is necessary to be able to call GetLines
			if (string.IsNullOrWhiteSpace(_control.Text) || !_control.IsLayoutUpdated)
				return;

			if (!_isControlInitialized)
				InitControl();

			var paint = _control.IsEnabled ? _textPaint : _textDisabledPaint;
			var lines = _control.GetLines();
			var lineHeight = paint.TextSize * 1.3f;

			var pos = _control.Position.ToSKPoint();
			// add the height also for the first line
			// else it seems the text is drawn at a 
			// too high position
			pos.Y += paint.TextSize + _control.Padding.Top;
			pos.X += _control.Padding.Left;
			
			for (int i = 0; i < lines.Length && pos.Y <= _rect.Bottom; i++)
			{
				_canvas.DrawText(lines[i], pos, paint);
				pos.Y += lineHeight;
			}
		}

		private void InitControl()
		{
			var font = _control.Font;
			
			_textPaint.Typeface = font.Name == null ? DefaultTypeFace : SKTypeface.FromFamilyName(font.Name);
			_textPaint.TextSize = font.Size;
			_textPaint.Color = _control.TextColor.ToSKColor();

			_textDisabledPaint.Typeface = _textPaint.Typeface;
			_textDisabledPaint.TextSize = font.Size;
			_textDisabledPaint.Color = _control.TextColor.GetDisabled().ToSKColor();

			font.GetWidth = GetTextWidth;
			
			// TODO not supported scenarios:
			// - changing of foreground color of the control
			// - changing of any value in font of the control
			// - using the actual font and size defined in the font object

			_isControlInitialized = true;
		}

		private float GetTextWidth(string text)
		{
			return GetTextWidth(_textPaint, text);
		}
	}
}
