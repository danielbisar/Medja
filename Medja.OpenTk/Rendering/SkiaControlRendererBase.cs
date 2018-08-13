﻿using Medja.Controls;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// Skia control renderer base.
	/// 
	/// In Render call Render(canvas, control, ...) before using other render
	/// methods of this class.
	/// </summary>
	public abstract class SkiaControlRendererBase<TControl> : ControlRendererBase<SKCanvas, TControl>
		where TControl : Control
	{
		protected readonly SKPaint _paint;

		/// <summary>
		/// The canvas that is currently used.
		/// </summary>
		protected SKCanvas _canvas;

		/// <summary>
		/// The rect that is used for drawing the current control.
		/// </summary>
		protected SKRect _rect;

		protected TControl _control;

		public SkiaControlRendererBase()
		{
			_paint = new SKPaint();
			_paint.IsAntialias = true;
		}

		protected override void Render(SKCanvas context, TControl control)
		{
			try
			{
				_control = control;
				_canvas = context;
				_rect = control.Position.ToSKRect();

				InternalRender();
			}
			finally
			{
				_canvas = null;
				_control = null;
			}
		}

		protected abstract void InternalRender();

		protected void RenderBackground()
		{
			if (_control.Background == null)
				return;

			_paint.Color = _control.IsEnabled ?
				_control.Background.ToSKColor()
				: _control.Background.GetLighter(0.25f).ToSKColor();

			_canvas.DrawRect(_rect, _paint);
		}
	}
}
