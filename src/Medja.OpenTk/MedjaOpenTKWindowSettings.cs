using Medja.Theming;

namespace Medja.OpenTk
{
    public class MedjaOpenTKWindowSettings
    {
        public IControlFactory ControlFactory { get; set; }
        public OpenGLVersion OpenGLVersion { get; set; }
        public int MaxFramesPerSecond { get; set; }

        public MedjaOpenTKWindowSettings()
        {
            OpenGLVersion = new OpenGLVersion(4, 2);
            MaxFramesPerSecond = 60;
        }
    }
}