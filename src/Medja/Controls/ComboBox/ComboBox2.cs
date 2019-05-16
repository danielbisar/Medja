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

        public ComboBox2()
        {
            PropertyTitle = new Property<string>();
        }
    }
}
