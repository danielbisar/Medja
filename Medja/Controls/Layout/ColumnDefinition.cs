namespace Medja.Controls.Layout
{
    public class ColumnDefinition
    {
        public float? Width { get; }

        public ColumnDefinition()
        {
            
        }

        public ColumnDefinition(float? width)
        {
            Width = width;
        }
    }
}