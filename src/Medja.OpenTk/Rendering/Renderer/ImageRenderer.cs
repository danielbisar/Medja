using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class ImageRenderer : SkiaControlRendererBase<Image>
    {
        public ImageRenderer(Image control) 
                : base(control)
        {
        }

        protected override void InternalRender()
        {
            if(_control.Bitmap?.BackendBitmap != null)
                _canvas.DrawBitmap((SKBitmap)_control.Bitmap.BackendBitmap, _rect);
        }
    }
}