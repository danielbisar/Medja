using System;
using Medja.Primitives;

namespace Medja.Controls
{
    /// <summary>
    /// A ComboBox or DropDown Control
    /// </summary>
    public class ComboBox2 : Control
    {
        //public Property<bool> PropertyIsEditable;

        public Property<string> PropertyTitle;
        /// <summary>
        /// The title that is currently displayed.
        /// </summary>
        public string Title
        {
            get { return PropertyTitle.Get(); }
            set { PropertyTitle.Set(value); }
        }
        
        public Property<bool> PropertyIsDropDownOpen;

        public bool IsDropDownOpen
        {
            get { return PropertyIsDropDownOpen.Get(); }
            set { PropertyIsDropDownOpen.Set(value); }
        }
        
        public ComboBox2()
        {
            PropertyTitle = new Property<string>();
            PropertyIsDropDownOpen = new Property<bool>();
            
            InputState.Clicked += OnClicked;
        }

        protected virtual void OnClicked(object sender, EventArgs e)
        {
            var mousePos = InputState.PointerPosition;

            if (Position.IsWithin(mousePos))
                IsDropDownOpen = !IsDropDownOpen;
        }
    }
}
