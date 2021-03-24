using Medja.Properties;

namespace Medja.Controls.Container
{
    /// <summary>
    /// A container that allows transforming it's sub controls.
    /// </summary>
    public class TransformContainer : ContentControl
    {
        public readonly Property<float> PropertyRotation;

        /// <summary>
        /// The rotation in radians.
        /// </summary>
        public float Rotation
        {
            get => PropertyRotation.Get();
            set => PropertyRotation.Set(value);
        }

        public TransformContainer()
        {
            PropertyRotation = new Property<float>();
        }
    }
}
