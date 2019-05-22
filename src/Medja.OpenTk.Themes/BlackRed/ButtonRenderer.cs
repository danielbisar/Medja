using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class ButtonRenderer : TextControlRendererBase<Button>
	{
		private readonly SKPaint _mouseDownPaint;
		private readonly SKPaint _mouseOverPaint;
		private readonly SKPaint _defaultPaint;
		
		public ButtonRenderer(Button button)
			: base(button)
		{
			
			_defaultPaint = new SKPaint();
			_defaultPaint.IsAntialias = true;
			_defaultPaint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
			
			_mouseDownPaint = new SKPaint();
			_mouseDownPaint.IsAntialias = true;
			_mouseDownPaint.Color = BlackRedThemeValues.SecondaryLightColor.ToSKColor();
			
			_mouseOverPaint = new SKPaint();
			_mouseOverPaint.IsAntialias = true;
			_mouseOverPaint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
		}
		
		protected override void DrawTextControlBackground()
		{
			if (!_control.IsEnabled) 
				return;
			
			if (_control.InputState.IsLeftMouseDown)
				_canvas.DrawRect(_rect, _mouseDownPaint);
			else if (_control.InputState.IsMouseOver)
				_canvas.DrawRect(_rect, _mouseOverPaint);
			else
				_canvas.DrawLine(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom, _defaultPaint);
		}
	}
}
