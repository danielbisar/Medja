using Medja.OpenTk.Themes;

namespace Medja.Demo
{
    public class CustomControlsFactory : BlackRedTheme
    {
        public CustomControlsFactory()
        {
            AddFactoryMethod(CreateOpenGlTestControl);
        }

        public virtual OpenGlTestControl CreateOpenGlTestControl()
        {
            var result = new OpenGlTestControl();
            result.Renderer = new OpenGlTestControlRenderer(result);
            
            return result;
        }
    }
}