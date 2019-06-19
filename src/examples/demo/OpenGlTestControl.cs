using Medja.Controls;
using Medja.Properties;

namespace Medja.Demo
{
	public class OpenGlTestControl : Control3D
	{
		// rendering see OpenGlTestControlRenderer

        protected override void OnNeedsRenderingChanged(object sender, PropertyChangedEventArgs e)
        {
            if(Renderer != null && !NeedsRendering)
                NeedsRendering = true;
        }
    }
}
