using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls
{
    /// <summary>
    /// Represents an axis configuration for the <see cref="Graph2D"/> control.
    /// </summary>
    public class Graph2DAxis : Control
    {
        public readonly Property<bool> PropertyAutoAdjust;
        /// <summary>
        /// Gets or sets if the min/max values should be calculated automatically or not.
        /// </summary>
        public bool AutoAdjust
        {
            get { return PropertyAutoAdjust.Get(); }
            set { PropertyAutoAdjust.Set(value); }
        }

        public readonly Property<float> PropertyMax;

        /// <summary>
        /// Gets or sets the maximum value displayed.
        /// </summary>
        public float Max
        {
            get { return PropertyMax.Get(); }
            set { PropertyMax.Set(value); }
        }

        public readonly Property<float> PropertyMin;

        /// <summary>
        /// Gets or sets the minimum value displayed.
        /// </summary>
        public float Min
        {
            get { return PropertyMin.Get(); }
            set { PropertyMin.Set(value); }
        }

        public readonly Property<AxisOrientation> PropertyOrientation;

        /// <summary>
        /// Gets or set the orientation of this control.
        /// </summary>
        public AxisOrientation Orientation
        {
            get { return PropertyOrientation.Get(); }
            set { PropertyOrientation.Set(value); }
        }
        
        public Font Font { get; } 

        public Graph2DAxis()
        {
            PropertyAutoAdjust = new Property<bool>();
            PropertyMax = new Property<float>();
            PropertyMin = new Property<float>();
            PropertyOrientation = new Property<AxisOrientation>();
            
            Font = new Font();
        }
    }
}