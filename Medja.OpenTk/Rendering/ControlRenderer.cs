using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public class ControlRenderer : SkiaControlRendererBase<Control>
	{
		protected override void InternalRender()
		{
			RenderBackground();
		}
	}
}
