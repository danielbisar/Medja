using Medja.Controls.Images;
using Medja.Properties;

namespace Medja.Controls
{
    /// <summary>
    /// Displays an image.
    /// </summary>
    public class Image : Control
    {
        public readonly Property<string> PropertyPath;
        public string Path
        {
            get { return PropertyPath.Get(); }
            set { PropertyPath.Set(value); }
        }
        
        public readonly Property<Bitmap> PropertyBitmap;
        public Bitmap Bitmap
        {
            get { return PropertyBitmap.Get(); }
            private set { PropertyBitmap.Set(value); }
        }

        public Image()
        {
            PropertyPath = new Property<string>();
            PropertyBitmap = new Property<Bitmap>();

            PropertyPath.PropertyChanged += OnPathChanged;
        }

        public void SizeToContent()
        {
            if (Bitmap == null)
                return;

            Position.Width = Bitmap.Width;
            Position.Height = Bitmap.Height;
        }

        private void OnPathChanged(object sender, PropertyChangedEventArgs e)
        {
            Bitmap = MedjaApplication.Instance.Library.BitmapFactory.Get((string) e.NewValue);
        }
    }
}