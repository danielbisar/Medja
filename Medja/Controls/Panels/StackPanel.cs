using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Medja.Controls.Panels
{
    public class StackPanel : Control
    {
        public List<Control> Children { get; }

        public readonly Property<float> PropertySpaceBetweenChildrenProperty;
        public float SpaceBetweenChildren
        {
            get { return PropertySpaceBetweenChildrenProperty.Get(); }
            set { PropertySpaceBetweenChildrenProperty.Set(value); }
        }

        public StackPanel()
        {
            PropertySpaceBetweenChildrenProperty = new Property<float>();

            Children = new List<Control>();
            SpaceBetweenChildren = 0.012f;
        }

        public void Add(Control control)
        {
            Children.Add(control ?? throw new ArgumentNullException(nameof(control)));
            IsLayoutUpdated = false;
        }

        internal override void Layout(Size availableSize)
        {
            //Debug.WriteLine("StackPanel.Layout Children.Count = " + Children.Count);

            base.Layout(availableSize);

            var curX = X;
            var curY = Y;
            var spacingY = SpaceBetweenChildren;
            var count = Children.Count;
            var childAvailableSize = new Size(availableSize.Width, (availableSize.Height / count) - spacingY * count);
            Control child;

            for(int i = 0; i < count; i++)
            {
                child = Children[i];
                child.X = curX;
                child.Y = curY;

                child.Layout(childAvailableSize);

                //Debug.WriteLine("child.X = " + child.X + ", child.Y = " + child.Y + ", child.Width = " + child.Width + ", child.Height = " + child.Height);

                curY += child.Height + spacingY;
            }
        }
    }
}
