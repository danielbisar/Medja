namespace Medja.NativeInterfaces
{
    public class MedjaScreen
    {
        public float ScaleX { get; }
        public float ScaleY { get; }

        internal MedjaScreen(float scaleX, float scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
    }
}