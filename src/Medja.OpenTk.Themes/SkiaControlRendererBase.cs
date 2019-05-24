using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Themes
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
		/// <summary>
		/// The canvas that is currently used.
		/// </summary>
		protected SKCanvas _canvas;

		/// <summary>
		/// The rect that is used for drawing the current control.
		/// </summary>
		/// todo this should not be here
		protected SKRect _rect;

		public SkiaControlRendererBase(TControl control)
			: base(control)
		{
		}

		protected override void Render(SKCanvas context)
		{
			try
			{
				_canvas = context;
				
				var hasClipping = !_control.ClippingArea.IsEmpty;

				if (hasClipping)
				{
					_canvas.Save();
					_canvas.ClipRect(_control.ClippingArea.ToSKRect());
				}

				_rect = _control.Position.ToSKRect();

				InternalRender();
				
				if(hasClipping)
					_canvas.Restore();
			}
			finally
			{
				_canvas = null;
			}
		}

		protected abstract void InternalRender();
	}
}
