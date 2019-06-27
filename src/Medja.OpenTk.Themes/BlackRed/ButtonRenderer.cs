using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class ButtonRenderer : TextControlRendererBase<Button>
	{
		private readonly SKPaint _backgroundPaint;
		
		public ButtonRenderer(Button button)
			: base(button)
		{
			_backgroundPaint = new SKPaint();
			_backgroundPaint.IsAntialias = true;
			
			_control.AffectRendering(_control.PropertyBackground);
		}
		
		protected override void DrawTextControlBackground()
		{
			if (_control.Background == null) 
				return;

			var rect = _control.Position.ToSKRect();
			_backgroundPaint.Color = _control.Background.ToSKColor();

			if (_control.InputState.IsLeftMouseDown)
				_canvas.DrawRect(rect, _backgroundPaint);
			else
				_canvas.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom, _backgroundPaint);
		}

		protected override void Dispose(bool disposing)
		{
			_backgroundPaint.Dispose();
			base.Dispose(disposing);
		}
	}
}
