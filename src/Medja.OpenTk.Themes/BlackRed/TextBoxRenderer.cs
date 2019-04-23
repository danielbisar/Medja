using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TextBoxRenderer : SkiaControlRendererBase<TextBox>
	{
		private readonly Stopwatch _caretStopWatch;

		public TextBoxRenderer(TextBox textBox)
		: base(textBox)
		{
			_caretStopWatch = Stopwatch.StartNew();
		}

		protected override void InternalRender()
		{
			RenderBottomLine();

			var pos = _control.Position.ToSKPoint();
			var padding = new Thickness(5, 0);
			
			// TODO textwrapping (use class TextWrapper)
			
			_paint.Color = _control.IsEnabled
				? _control.TextColor.ToSKColor()
				: _control.TextColor.GetLighter(0.25f).ToSKColor();

			_rect.Left += padding.Left;
			pos.Y += _paint.FontSpacing + padding.Top;
			pos.X += padding.Left;
			RenderText(_control.Text, _control.Font, pos);

			if (_control.IsFocused && _caretStopWatch.ElapsedTicks % 10000000 <= 5000000)
			{
				var textLength = GetTextWidth(_paint, _control.Text);
				var caretLeft = _rect.Left + textLength;
				var top = _rect.Top + _paint.FontSpacing - _paint.TextSize;
				var bottom = top + _paint.FontSpacing;

				_canvas.DrawLine(new SKPoint(caretLeft, top), new SKPoint(caretLeft, bottom), _paint);
			}
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
