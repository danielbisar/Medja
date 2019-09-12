namespace Medja.OpenTk
{
    public struct OpenGLVersion
    {
        public int Major { get; }
        public int Minor { get; }

        public OpenGLVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }
}