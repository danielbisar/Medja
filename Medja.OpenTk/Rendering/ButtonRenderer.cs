using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public class ButtonRenderer : SkiaControlRendererBase<Button>
	{
		protected override void InternalRender()
		{
			if (_control.IsEnabled)
			{
				if (_control.InputState.IsLeftMouseDown)
				{
					_paint.Color = ColorMap.SecondaryLight.ToSKColor();
					_canvas.DrawRect(_rect, _paint);
				}
				else if (_control.InputState.IsMouseOver)
				{
					_paint.Color = ColorMap.Secondary.ToSKColor();
					_canvas.DrawRect(_rect, _paint);
				}
				else
				{
					_paint.Color = ColorMap.Secondary.ToSKColor();
					_canvas.DrawLine(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom, _paint);
				}
			}

			_paint.Color = _control.IsEnabled ? ColorMap.PrimaryText.ToSKColor() : ColorMap.PrimaryLight.ToSKColor();
			RenderTextCentered(_control.Text, _control.Font);
		}
	}
}
