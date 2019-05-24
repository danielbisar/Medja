namespace Medja.Controls
{
    /// <summary>
    /// Represents an axis configuration for the <see cref="Graph2D"/> control.
    /// </summary>
    public class Graph2DAxis : Control
    {
        // orientation

        public Property<bool> PropertyAutoAdjust;
        /// <summary>
        /// Gets or sets if the min/max values should be calculated automatically or not.
        /// </summary>
        public bool AutoAdjust
        {
            get { return PropertyAutoAdjust.Get(); }
            set { PropertyAutoAdjust.Set(value); }
        }

        public Graph2DAxis()
        {
            PropertyAutoAdjust = new Property<bool>();
        }
    }
}