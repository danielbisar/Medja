using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// This class is the main entry point for the rendering of controls for OpenTk with Skia.
	/// </summary>
	public class OpenTk2DOnlyRenderer : IRenderer
	{
		protected readonly SkiaGlLayer _skia;
		protected bool _needsRendering;
		protected SKCanvas _canvas;
		
		public OpenTk2DOnlyRenderer()
		{
			_skia = new SkiaGlLayer();
		}

		public void SetSize(Rectangle rectangle)
		{
			_skia.Resize(rectangle.Width, rectangle.Height);
			_needsRendering = true;
		}

		public virtual bool Render(IList<Control> controls)
		{
			if (!NeedsRendering(controls))
				return false;

			_needsRendering = false;
			
			_skia.Canvas.Clear();
			_canvas = _skia.Canvas;
			
			for (int i = 0; i < controls.Count; i++)
			{
				Render(controls[i]);
			}
			
			_canvas.Flush();
			return true;
		}

		protected bool NeedsRendering(IList<Control> controls)
		{
			// && !_needsRendering
			
			//_canvas.

			for (int i = 0; i < controls.Count; i++)
			{
				if (controls[i].NeedsRendering)
				{
					_needsRendering = true;
					Console.WriteLine("R: " + controls[i] + ": " + controls[i].Position);
				}
			}
			
			if(_needsRendering)
				Console.WriteLine("<----");

			return _needsRendering;
		}

		protected void Render(Control control)
		{
			control.NeedsRendering = false;

			// it can be that this property has changed during rendering
			// this should fix an issue with the progressbar being displayed
			// even though it should not be visible anymore
			if (!control.IsVisible)
				return;

			control.Renderer?.Render(_canvas);
		}

		public void Dispose()
		{
			_skia.Dispose();
		}
	}
}
