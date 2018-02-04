namespace Medja.Layers.Layouting
{
    public class PositionInfo
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public override string ToString()
        {
            return "X = " + X + ", Y = " + Y + ", Width = " + Width + ", Height = " + Height;
        }
    }
}
