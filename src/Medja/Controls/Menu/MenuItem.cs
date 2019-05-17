namespace Medja.Controls
{
    /// <summary>
    /// Represents a single menu item.
    /// </summary>
    public class MenuItem : Control
    {
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
            PropertyTitle = new Property<string>();
            InputState.OwnsMouseEvents = true;
        }
    }
}