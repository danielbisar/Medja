using SkiaSharp;

namespace Medja.Rendering.Font
{
    public class FontRenderer
    {
        private readonly string _fontFamily;
        private readonly int _size;

        public FontRenderer(string fontFamily, int size)
        {
            _fontFamily = fontFamily;
            _size = size;
        }

        public Bitmap Render(string text)
        {
            // https://developer.xamarin.com/guides/xamarin-forms/advanced/skiasharp/basics/text/
            using (var bitmap = new SKBitmap(1024, 768))
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    using (var face = SKTypeface.FromFamilyName(_fontFamily))
                    {
                        using (var paint = new SKPaint
                        {
                            Typeface = face,
                            TextSize = _size,
                            IsAntialias = true,
                            Color = SKColors.Green
                        })
                        {
                            var width = paint.MeasureText(text);

                            canvas.Clear(SKColors.Yellow);
                            canvas.DrawText(text, 50, 50, paint);
                            canvas.Flush();
                        }   
                    }
                }

                using (var image = SKImage.FromBitmap(bitmap))
                {
                    var data = image.Encode();
                    
                    /*using (var stream = new FileStream("output.png", FileMode.Create, FileAccess.Write))
                        data.SaveTo(stream);*/

                    return new Bitmap(data.ToArray(), 1024, 768);
                }
            }
        }
    }
}
