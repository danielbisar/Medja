namespace Medja.OpenTk.Rendering
{
    public interface IOpenTkControlRenderer
    {
        void SwapBuffers();
        void MakeContextCurrent();
    }
}