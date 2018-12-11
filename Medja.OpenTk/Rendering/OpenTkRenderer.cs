using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;
using System.Text;
using Medja.Theming;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// This class is the main entry point for the rendering of controls for OpenTk with Skia.
	/// </summary>
	public class OpenTkRenderer : IRenderer
	{
		private readonly SkiaGLLayer _skia;
		private SKCanvas _canvas;
		private readonly SKTypeface _defaultTypeface;

		public OpenTkRenderer()
		{
			_skia = new SkiaGLLayer();
			
			// TODO remove 
			_defaultTypeface = SKTypeface.FromFamilyName("Monospace");
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

			if (renderer is ISkiaRenderer skiaRenderer)
			{
				skiaRenderer.DefaultTypeFace = _defaultTypeface;
				skiaRenderer.Render(_canvas, control);
			}
			else if (renderer is IControlRenderer controlRenderer)
			{
				controlRenderer.Render(_canvas, control);
			}
		}

		public void Dispose()
		{
			_defaultTypeface.Dispose();
			_skia.Dispose();
		}
	}
}
