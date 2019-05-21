using Medja.OpenTk.Utils;
using SkiaSharp;
using Xunit;

namespace Medja.OpenTk.Test.Utils
{
    public class SkiaExtensionsTest
    {
        [Fact]
        public void DrawTextDoesNotThrowExceptionOnNullOrEmptyText()
        {
            using (var bitmap = new SKBitmap())
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    using (var paint = new SKPaint())
                    {
                        canvas.DrawTextSafe(null, 0, 0, paint);
                    }
                }
            }
        }
    }
}