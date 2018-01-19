namespace Medja.Controls
{
    public class Button : Control
    {
        public readonly Property<string> PropertyText;
        public string Text
        {
            get { return PropertyText.Get(); }
            set { PropertyText.Set(value); }
        }

        public Button()
        {
            PropertyText = new Property<string>();
        }
    }
}
