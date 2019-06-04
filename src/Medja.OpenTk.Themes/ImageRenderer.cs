using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public class ImageRenderer : SkiaControlRendererBase<Image>
    {
        public ImageRenderer(Image control) 
                : base(control)
        {
            _control.AffectsRendering(_control.PropertyBitmap);
        }

        protected override void InternalRender()
        {
            if(_control.Bitmap?.BackendBitmap != null)
                _canvas.DrawBitmap((SKBitmap)_control.Bitmap.BackendBitmap, _rect);
        }
    }
}