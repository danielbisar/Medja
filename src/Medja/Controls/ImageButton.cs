using Medja.Theming;

namespace Medja.Controls
{
    /// <summary>
    /// A button containing an image.
    /// </summary>
    public class ImageButton : ContentControl, IButton
    {
        public readonly Property<bool> PropertyIsSelected;
        public bool IsSelected
        {
            get { return PropertyIsSelected.Get(); }
            set { PropertyIsSelected.Set(value); }
        }
        
        public readonly Property<Image> PropertyImage;
        public Image Image
        {
            get { return PropertyImage.Get(); }
            private set { PropertyImage.Set(value); }
        }
        
        public readonly Property<Image> PropertyMouseOverImage;
        public Image MouseOverImage
        {
            get { return PropertyMouseOverImage.Get(); }
            private set { PropertyMouseOverImage.Set(value); }
        }

        public readonly Property<Image> PropertyMouseDownImage;
        public Image MouseDownImage
        {
            get { return PropertyMouseDownImage.Get(); }
            private set { PropertyMouseDownImage.Set(value); }
        }

        public readonly Property<Image> PropertySelectedImage;
        public Image SelectedImage
        {
            get { return PropertySelectedImage.Get(); }
            private set { PropertySelectedImage.Set(value); }
        }

        
        public readonly Property<bool> PropertyIsAutoSizeToBitmap;
        public bool IsAutoSizeToBitmap
        {
            get { return PropertyIsAutoSizeToBitmap.Get(); }
            set { PropertyIsAutoSizeToBitmap.Set(value); }
        }
        
        public ImageButton(IControlFactory controlFactory)
        {
            PropertyImage = new Property<Image>();
            PropertyMouseOverImage = new Property<Image>();
            PropertyMouseDownImage = new Property<Image>();
            PropertySelectedImage = new Property<Image>();
            PropertyIsSelected = new Property<bool>();
            PropertyIsAutoSizeToBitmap = new Property<bool>();
            
            InputState.OwnsMouseEvents = true;
            InputState.PropertyIsMouseOver.PropertyChanged += OnMouseOverChanged;
            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnLeftMouseDownChanged;

            PropertyIsSelected.PropertyChanged += OnSelectedChanged;
            
            // TODO how to behave for resizes of this control, instead of resizing to bitmap?
            IsAutoSizeToBitmap = true;
            
            Image = controlFactory.Create<Image>();
            Image.PropertyBitmap.PropertyChanged += OnImageBitmapChanged;

            MouseDownImage = controlFactory.Create<Image>();
            MouseOverImage = controlFactory.Create<Image>();
            SelectedImage = controlFactory.Create<Image>();
            
            Content = Image;
        }

        private void OnSelectedChanged(object sender, PropertyChangedEventArgs eventargs)
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            if (InputState.IsMouseOver)
            {
                if (InputState.IsLeftMouseDown)
                    Content = MouseDownImage;
                else
                    Content = MouseOverImage;
            }
            else if (IsSelected)
                Content = SelectedImage;
            else
                Content = Image;
        }

        private void OnMouseOverChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateContent();
        }
        
        private void OnLeftMouseDownChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateContent();
        }

        private void OnImageBitmapChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAutoSizeToBitmap)
            {
                Position.Width = Image.Bitmap.Width;
                Position.Height = Image.Bitmap.Height;
            }
        }
    }
}