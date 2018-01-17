using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Medja.Controls.Panels
{
    public class StackPanel : Control
    {
        public List<Control> Children { get; }

        private readonly Property<float> _spaceBetweenChildrenProperty;
        public float SpaceBetweenChildren
        {
            get { return _spaceBetweenChildrenProperty.Get(); }
            set { _spaceBetweenChildrenProperty.Set(value); }
        }

        public StackPanel()
        {
            _spaceBetweenChildrenProperty = AddProperty<float>();

            Children = new List<Control>();
            SpaceBetweenChildren = 0.01f;
        }

        public void Add(Control control)
        {
            Children.Add(control ?? throw new ArgumentNullException(nameof(control)));
            IsLayoutUpdated = false;
        }

        protected override void Layout()
        {
            Debug.WriteLine("StackPanel.Layout Children.Count = " + Children.Count);

            base.Layout();

            var curX = X;
            var curY = Y;
            var spacingY = SpaceBetweenChildren;

            foreach (var child in Children)
            {
                child.X = curX;
                child.Y = curY;

                child.UpdateLayout();

                Debug.WriteLine("child.X = " + child.X + ", child.Y = " + child.Y + ", child.Width = " + child.Width + ", child.Height = " + child.Height);

                curY += child.Height + spacingY;
            }
        }
    }
}
