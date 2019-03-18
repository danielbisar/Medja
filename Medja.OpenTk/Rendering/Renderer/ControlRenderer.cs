using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public class ControlRenderer : SkiaControlRendererBase<Control>
	{
		public ControlRenderer(Control control) 
		: base(control)
		{
		}
		
		protected override void InternalRender()
		{
			RenderBackground();
		}
	}
}
