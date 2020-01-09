using Medja.Controls;
using Medja.Theming;

namespace Medja.OpenTk.Themes
{
    /// <summary>
    /// Base class for OpenTK Themes.
    /// </summary>
    public class ThemeOpenTKBase : ControlFactory
    {
        protected override TransformContainer CreateTransformContainer()
        {
            var result = base.CreateTransformContainer();
            result.Renderer = new TransformContainerRenderer(result);
            
            return result;
        }
    }
}