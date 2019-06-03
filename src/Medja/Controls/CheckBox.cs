using System;
using Medja.Properties;

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
            InputState.Clicked += OnClicked;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
        }
    }
}