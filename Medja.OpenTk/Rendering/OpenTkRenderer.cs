using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;
using System;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// This class is the main entry point for the rendering of controls for OpenTk with Skia.
	/// </summary>
	public class OpenTkRenderer : IRenderer
	{
		private SkiaGLLayer _skia;
		private SKCanvas _canvas;

		private int _previous3DControlCount;
		private int _previous2DControlCount;

		public Action Before3D { get; set; }

		public OpenTkRenderer()
		{
			_previous2DControlCount = 0;
			_previous3DControlCount = 0;

			_skia = new SkiaGLLayer();

			Before3D = InternalBefore3D;
		}

		private void InternalBefore3D()
		{
			GL.ClearColor(Color.Gray);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
		}

		public void SetSize(Rectangle rectangle)
		{
			_skia.Resize(rectangle.Width, rectangle.Height);
		}

		public void Render(IEnumerable<Control> controls)
		{
			// we don't expect the controls to change a lot, so we cache at least the size of the lists
			var controls3d = new List<Control3D>(_previous3DControlCount);
			var controls2d = new List<Control>(_previous2DControlCount);

			foreach (var control in controls)
			{
				if (control.IsVisible)
				{
					if (control is Control3D control3d)
						controls3d.Add(control3d);
					else
						controls2d.Add(control);
				}
			}

			_previous3DControlCount = controls3d.Count;
			_previous2DControlCount = controls2d.Count;

			Before3D();

			foreach (var control3d in controls3d)
				Render(control3d);

			OpenGLState.KeepState(() =>
			{
				_skia.ResetContext();
				_canvas = _skia.Canvas;

				foreach (var control in controls2d)
				{
					Render(control);
				}

				_canvas.Flush();
			});
		}

		private void Render(Control control)
		{
			// it can be that this property has changed during rendering
			// this should fix an issue with the progressbar being displayed
			// even though it should not be visible anymore
			if (!control.IsVisible)
				return;

			if (control.Renderer != null)
				control.Renderer.Render(_canvas, control);
		}

		public void Dispose()
		{
			_skia.Dispose();
		}
	}
}
