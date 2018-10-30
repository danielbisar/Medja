using System;
using Medja.Primitives;

namespace Medja.Controls
{
    public class ScrollableContainer : ContentControl
    {
        public readonly Property<float> PropertyMaxHeight;
        /// <summary>
        /// Gets the maximum height (either the Height of this element or if the Content is bigger, the height
        /// of the Content).
        /// </summary>
        public float MaxHeight
        {
            get { return PropertyMaxHeight.Get(); }
            protected set { PropertyMaxHeight.Set(value); }
        }
        
        public readonly Property<float> PropertyVerticalScrollingPos;
        public float VerticalScrollingPos
        {
            get { return PropertyVerticalScrollingPos.Get(); }
            set { PropertyVerticalScrollingPos.Set(value); }
        }

        public readonly Property<bool> PropertyCanScroll;
        public bool CanScroll
        {
            get { return PropertyCanScroll.Get(); }
            set { PropertyCanScroll.Set(value); }
        }

        
        public ScrollableContainer()
        {
            PropertyMaxHeight = new Property<float>();
            PropertyMaxHeight.PropertyChanged += OnMaxHeightChanged;
            PropertyVerticalScrollingPos = new Property<float>();
            PropertyCanScroll = new Property<bool>();
        }

        private void OnMaxHeightChanged(object sender, PropertyChangedEventArgs e)
        {
            CanScroll = MaxHeight > Position.Height;
        }

        // TODO
        //public override Size Measure(Size availableSize)
        
        public override void Arrange(Size availableSize)
        {
            var area = Rect.Subtract(Position, Margin);
            area.Subtract(Padding);
            
            area.Y += VerticalScrollingPos;
			
            ContentArranger.Position(area);
            ContentArranger.Stretch(area);

            if (Content != null)
            {
                MaxHeight = Math.Max(area.Height, Content.Position.Height);
                
                if (VerticalScrollingPos > MaxHeight)
                    VerticalScrollingPos = MaxHeight;
            }
        }
    }
}