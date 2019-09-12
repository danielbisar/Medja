namespace Medja.Controls
{
    /// <summary>
    /// A base class to specify that the control will have a different render path.
    /// </summary>
    public abstract class Control3D : Control
    {
        public Control3D()
        {
            Is3D = true;
        }
    }
}
