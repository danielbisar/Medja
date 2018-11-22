using System;

namespace Medja.Controls
{
    public class CheckBox : TextControl
    {
        public readonly Property<bool> PropertyIsChecked;
        public bool IsChecked
        {
            get { return PropertyIsChecked.Get(); }
            set { PropertyIsChecked.Set(value); }
        }

        public CheckBox()
        {
            PropertyIsChecked = new Property<bool>();
            InputState.MouseClicked += OnMouseClicked;
        }

        private void OnMouseClicked(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
        }
    }
}