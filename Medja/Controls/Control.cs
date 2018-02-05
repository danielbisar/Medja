using System;
using Medja.Layers.Layouting;

namespace Medja.Controls
{
    /// <summary>
    /// Any control should inherit from this class.
    /// </summary>
    /// <remarks>
    /// 
    /// ? Background/Foreground colors should be designed via renderers?
    /// 
    /// </remarks>
    public class Control : MObject
    {
        public PositionInfo PositionInfo { get; set; }

        public float MinWidth { get; set; }
        public float Width { get; set; }
        public float MaxWidth { get; set; }
        public float ActualWidth { get; set; }

        public float MinHeight { get; set; }
        public float Height { get; set; }
        public float MaxHeight { get; set; }
        public float ActualHeight { get; set; }

        public Thickness Margin { get; set; }

        public Control()
        {
        }

        public virtual Size Measure(Size availableSize)
        {
            var width = Math.Max(MinWidth, availableSize.Width);
            width = Math.Min(width, MaxWidth);

            var height = Math.Max(MinHeight, availableSize.Height);
            height = Math.Min(height, MaxHeight);

            return new Size(width, height);
        }        

        protected virtual void Arrange(PositionInfo position)
        {
            PositionInfo = new PositionInfo
            {
                X = position.X,
                Y = position.Y,
                Width = position.Width,
                Height = position.Height
            };
        }

        public void Layout()
        {
            var size = Measure(new Size(float.PositiveInfinity, float.PositiveInfinity));
            Arrange(new PositionInfo
            {
                Width = size.Width,
                Height = size.Height
            });
        }
    }
}
