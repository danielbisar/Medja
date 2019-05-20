namespace Medja.Controls
{
    /// <summary>
    /// Represents a single menu item.
    /// </summary>
    public class MenuItem : Control
    {
        public readonly Property<bool> PropertyIsSelected;
        /// <summary>
        /// Gets or sets if the <see cref="MenuItem"/> is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return PropertyIsSelected.Get(); }
            set { PropertyIsSelected.Set(value); }
        }
        
        public readonly Property<string> PropertyTitle;
        /// <summary>
        /// Gets or sets the title of the menu item.
        /// </summary>
        public string Title
        {
            get { return PropertyTitle.Get(); }
            set { PropertyTitle.Set(value); }
        }

        public MenuItem()
        {
            PropertyIsSelected = new Property<bool>();
            PropertyTitle = new Property<string>();
            
            InputState.OwnsMouseEvents = true;
            InputState.PropertyIsMouseOver.PropertyChanged += OnMouseOverChanged;
        }

        private void OnMouseOverChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateIsSelected();
        }

        private void UpdateIsSelected()
        {
            IsSelected = InputState.IsMouseOver;
            // todo also check for IsFocused or KeyboardFocus?
        }
    }
}