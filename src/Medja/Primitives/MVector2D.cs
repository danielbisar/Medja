using Medja.Properties;

namespace Medja.Primitives
{
    /// <summary>
    /// Bindable vector with 2 dimensions.
    /// </summary>
    public class MVector2D
    {
        public readonly Property<float> PropertyX;
        public float X
        {
            get => PropertyX.Get();
            set => PropertyX.Set(value);
        }

        public readonly Property<float> PropertyY;
        public float Y
        {
            get => PropertyY.Get();
            set => PropertyY.Set(value);
        }
        
        public MVector2D()
        {
            PropertyX = new Property<float>();
            PropertyY = new Property<float>();
        }

        public override string ToString()
        {
            return $"MVector2D ({X}, {Y})";
        }
    }
}