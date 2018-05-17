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
		private SKSurface _surface;
        private SKCanvas _canvas;
        private bool _isDisposed;

		private int _previous3DControlCount;
		private int _previous2DControlCount;

		public Action SetupOpenGL { get; set; }

        public OpenTkRenderer()
        {
			_previous2DControlCount = 0;
			_previous3DControlCount = 0;
            _skia = new SkiaGLLayer();

			SetupOpenGL = InternalSetupOpenGL;
        }

        private void InternalSetupOpenGL()
		{
			GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(0);
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

            foreach(var control in controls)
            {
                if (control is Control3D control3d)
                    controls3d.Add(control3d);
                else
                    controls2d.Add(control);
            }

			_previous3DControlCount = controls3d.Count;
			_previous2DControlCount = controls2d.Count;

			SetupOpenGL();

			foreach (var control3d in controls3d)
				Render(control3d);
            
            //GL.PushClientAttrib(ClientAttribMask.ClientAllAttribBits);

            _surface = _skia.CreateSurface();
            _canvas = _surface.Canvas;

            foreach (var control in controls2d)
            {
                Render(control);
            }

            _canvas.Flush();
			_canvas.Dispose();
            _surface.Dispose();

            //GL.PopClientAttrib();
        }

        private void Render(Control control)
        {
			if (control.Renderer != null)
				control.Renderer.Render(_canvas, control);
        }

        public void Dispose()
        {
			if (!_isDisposed)
			{
				if (_surface != null)
					_surface.Dispose();

				if (_skia != null)
					_skia.Dispose();

				_isDisposed = true;
			}
        }
    }
}
