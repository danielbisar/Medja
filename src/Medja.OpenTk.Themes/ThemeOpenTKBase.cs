using Medja.Controls.Container;
using Medja.OpenTk.Controls;
using Medja.Theming;

namespace Medja.OpenTk.Themes
{
    /// <summary>
    /// Base class for OpenTK Themes.
    /// </summary>
    public class ThemeOpenTKBase : ControlFactory
    {
        public ThemeOpenTKBase()
        {
            AddFactoryMethod(CreateOpenGLEventControl);
        }

        protected virtual OpenGLEventControl CreateOpenGLEventControl()
        {
            var result = new OpenGLEventControl();
            result.Renderer = new OpenGLEventControlRenderer(result);

            return result;
        }
        
        protected override TransformContainer CreateTransformContainer()
        {
            var result = base.CreateTransformContainer();
            result.Renderer = new TransformContainerRenderer(result);
            
            return result;
        }
    }
}
