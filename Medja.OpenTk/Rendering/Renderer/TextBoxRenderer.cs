using Medja.Controls;
using SkiaSharp;
using System.Diagnostics;
using Medja.Primitives;

namespace Medja.OpenTk.Rendering
{
	public class TextBoxRenderer : SkiaControlRendererBase<TextBox>
	{
		private readonly Stopwatch _caretStopWatch;

		public TextBoxRenderer()
		{
			_caretStopWatch = Stopwatch.StartNew();
		}

		protected override void InternalRender()
		{
			RenderBottomLine();

			var pos = _control.Position.ToSKPoint();
			var padding = new Thickness(5, 0);

			//if (control.TextWrapping == Primitives.TextWrapping.None)
			//{
			//  // shorten the text to renderable length
			//  while (paint.MeasureText(text) > rect.Width
			//         && text.Length > 1)
			//      text = text.Substring(0, text.Length - 2);
			//}
			
			_paint.Color = _control.IsEnabled
				? _control.Foreground.ToSKColor()
				: _control.Foreground.GetLighter(0.25f).ToSKColor();

			_rect.Left += padding.Left;
			pos.Y += _paint.FontSpacing + padding.Top;
			pos.X += padding.Left;
			RenderText(_control.Text, _control.Font, pos);

			if (_control.IsFocused && _caretStopWatch.ElapsedTicks % 10000000 <= 5000000)
			{
				var textLength = _paint.MeasureText(_control.Text);
				var caretLeft = _rect.Left + textLength;
				var top = _rect.Top + _paint.FontSpacing - _paint.TextSize;
				var bottom = top + _paint.FontSpacing;

				_canvas.DrawLine(new SKPoint(caretLeft, top), new SKPoint(caretLeft, bottom), _paint);
			}
			
			//paint.BreakText
		}

		private void RenderBottomLine()
		{
			if (!_control.IsEnabled)
				_paint.Color = ColorMap.PrimaryLight.ToSKColor();
			else
			{
				if (_control.IsFocused)
				{
					_paint.Color = ColorMap.Secondary.ToSKColor();
				}
				else
				{
					_paint.Color = ColorMap.PrimaryLight.ToSKColor();
				}
			}

			_canvas.DrawLine(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom, _paint);
		}
	}
}
