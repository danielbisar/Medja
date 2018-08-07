using System;
using Medja.Theming;
using SkiaSharp;
using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public class ControlRenderer : ControlRendererBase<SKCanvas, Control>
	{
		protected override void Render(SKCanvas canvas, Control control)
		{
			var position = control.Position;
			var skRect = position.ToSKRect();

			using (var paint = new SKPaint())
			{
				paint.IsAntialias = true;

				if (control.Background != null)
				{
					paint.Color = control.Background.ToSKColor();
					canvas.DrawRect(skRect, paint);
				}
			}
		}
	}
}
