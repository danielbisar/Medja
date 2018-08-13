using Medja.Controls;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class ButtonRenderer : SkiaControlRendererBase<Button>
	{
		protected override void InternalRender()
		{
			_paint.Color = _control.IsEnabled && _control.InputState.IsMouseOver ? SKColors.Red : SKColors.Black;
			_canvas.DrawRect(_rect, _paint);

			_paint.Color = _control.IsEnabled ? SKColors.Red : SKColors.DarkRed;
			_canvas.DrawLine(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom, _paint);

			_paint.Color = _control.IsEnabled ? SKColors.White : SKColors.Gray;
			_canvas.RenderTextCentered(_control.Text, _paint, _rect);
		}
	}
}
