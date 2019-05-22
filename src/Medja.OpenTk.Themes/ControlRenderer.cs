using Medja.Controls;

namespace Medja.OpenTk.Themes
{
	public class ControlRenderer : SkiaControlRendererBase<Control>
	{
		private readonly BackgroundRenderer _backgroundRenderer;
		
		public ControlRenderer(Control control) 
		: base(control)
		{
			_backgroundRenderer = new BackgroundRenderer(control);
		}
		
		protected override void InternalRender()
		{
			_backgroundRenderer.Render(_canvas);
		}

		protected override void Dispose(bool disposing)
		{
			_backgroundRenderer.Dispose();
			base.Dispose(disposing);
		}
	}
}
