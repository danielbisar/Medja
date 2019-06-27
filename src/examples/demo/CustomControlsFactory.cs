using Medja.OpenTk.Themes.DarkBlue;

namespace Medja.Demo
{
    public class CustomControlsFactory : DarkBlueTheme
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