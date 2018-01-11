namespace Medja.Controls
{
    public class Button : Control
    {
        private readonly IProperty<string> _text;

        public string Text
        {
            get { return _text.Get(); }
            set { _text.Set(value); }
        }

        public Button()
        {
            _text = AddProperty<string>();
        }
    }
}
