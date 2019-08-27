using System;
using Medja.Properties;

namespace Medja.Controls
{
    /// <summary>
    /// A checkbox control.
    /// </summary>
    public class CheckBox : TextControl
    {
        public readonly Property<bool> PropertyIsChecked;
        /// <summary>
        /// Gets or sets if the <see cref="CheckBox"/> is checked.
        /// </summary>
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