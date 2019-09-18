using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public interface IViewProjectionMatrix
    {
        Matrix4 ViewProjectionMatrix { get; set; }
    }
}