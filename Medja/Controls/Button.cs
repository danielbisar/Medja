namespace Medja.Controls
{
    public class Button : Control
    {
        private readonly IProperty<float> _x;
        public float X
        {
            get { return _x.Get(); }
            set { _x.Set(value); }
        }

        private readonly IProperty<float> _y;
        public float Y
        {
            get { return _y.Get(); }
            set { _y.Set(value); }
        }

        private readonly IProperty<float> _width;
        public float Width
        {
            get { return _width.Get(); }
            set { _width.Set(value); }
        }

        private readonly IProperty<float> _height;
        public float Height
        {
            get { return _height.Get(); }
            set { _height.Set(value); }
        }

        private readonly IProperty<string> _text;

        public string Text
        {
            get { return _text.Get(); }
            set { _text.Set(value); }
        }

        public Button()
        {
            _x = AddProperty<float>();
            _y = AddProperty<float>();
            _width = AddProperty<float>();
            _height = AddProperty<float>();
            _text = AddProperty<string>();
        }
    }
}
