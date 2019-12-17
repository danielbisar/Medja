using Xunit;

namespace Medja.OpenTk.Test.Utils
{
    public class SkiaExtensionsTest
    {
        [Fact]
        public void DrawTextDoesNotThrowExceptionOnNullOrEmptyText()
        {
            // todo does currently not run on azure ci
            // see # 459
            //Medja.OpenTk.Test.Utils.SkiaExtensionsTest.DrawTextDoesNotThrowExceptionOnNullOrEmptyText [FAIL]
            //   [xUnit.net 00:00:02.76]       System.BadImageFormatException : An attempt was made to load a program with an incorrect format. (Exception from HRESULT: 0x8007000B)
            //    [xUnit.net 00:00:02.77]       Stack Trace:
            //    [xUnit.net 00:00:02.77]            at SkiaSharp.SkiaApi.sk_bitmap_new()
            //    [xUnit.net 00:00:02.77]            at SkiaSharp.SKBitmap..ctor()
            //    [xUnit.net 00:00:02.77]         src\Medja.OpenTk.Test\Utils\SkiaExtensionsTest.cs(12,0): at Medja.OpenTk.Test.Utils.SkiaExtensionsTest.DrawTextDoesNotThrowExceptionOnNullOrEmptyText()
            //    [xUnit.net 00:00:02.77]   Finished:    Medja.OpenTk.Test
            // https://baruchcodes.visualstudio.com/Medja/_build/results?buildId=265
            
//            using (var bitmap = new SKBitmap())
//            {
//                using (var canvas = new SKCanvas(bitmap))
//                {
//                    using (var paint = new SKPaint())
//                    {
//                        canvas.DrawTextSafe(null, 0, 0, paint);
//                    }
//                }
//            }
        }
    }
}