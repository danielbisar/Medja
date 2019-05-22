using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// This class is the main entry point for the rendering of controls for OpenTk with Skia.
	/// </summary>
	public class OpenTkRenderer : IRenderer
	{
		private readonly SkiaGlLayer _skia;
		private SKCanvas _canvas;

		public OpenTkRenderer()
		{
			_skia = new SkiaGlLayer();
		}

		public void SetSize(Rectangle rectangle)
		{
			_skia.Resize(rectangle.Width, rectangle.Height);
		}
		
		public void Render(IEnumerable<Control> controls)
		{
			_skia.Canvas.Clear();

			var previousWas3DControl = false;
			var state = new OpenGLState();
			
			state.Save();
			_skia.ResetContext();
			_canvas = _skia.Canvas;
			
			foreach (var control in controls)
			{
				var is3DControl = control is Control3D;

				if (is3DControl && !previousWas3DControl)
				{
					_canvas.Flush();
					state.Restore();
				}
				else if (!is3DControl && previousWas3DControl)
				{
					state.Save();
					_skia.ResetContext();
					_canvas = _skia.Canvas;
				}
				
				if(is3DControl)
					OpenGLState.KeepState(() => Render(control));
				else
					Render(control);

				previousWas3DControl = is3DControl;
			}
			
			_canvas.Flush();
			state.TryRestore();
		}

		private void Render(Control control)
		{
			// it can be that this property has changed during rendering
			// this should fix an issue with the progressbar being displayed
			// even though it should not be visible anymore
			if (!control.IsVisible)
				return;

			var renderer = control.Renderer;

			if (renderer == null)
				return;

			renderer.Render(_canvas);
		}

		public void Dispose()
		{
			_skia.Dispose();
		}
	}
}
