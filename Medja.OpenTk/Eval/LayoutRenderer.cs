using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Medja.OpenTk.Eval
{
    public class LayoutRenderer
    {
        private readonly Random _rnd = new Random();

        public void Render(Layout layout)
        {
            // items in the back are coming first, the items in the front
            // possible optimization just draw what is really visible, but this would be overhead for this simple prototype
            foreach (var kvp in layout)
            {
                var item = kvp.Key;
                var positionInfo = kvp.Value;

                if (item is Menu)
                {
                    GL.Color3(0.3, 0.3, 0.3);
                    DrawRect(positionInfo);
                }
                else if (item is MenuEntry me)
                {
                    GL.Color3(0.35, 0.35, 0.35);
                    DrawRect(positionInfo);
                    GL.Color3(0, 0, 0);

                    DrawText(positionInfo, me.Text);
                }
                else
                    DrawRect(positionInfo);
            }
        }

        private bool _isFirstText = true;

        private void DrawText(PositionInfo positionInfo, string text)
        {
            // https://developer.xamarin.com/guides/xamarin-forms/advanced/skiasharp/basics/text/

            using (var face = SKTypeface.FromFamilyName("Arial"))
            {
                using (var paint = new SKPaint
                {
                    Typeface = face,
                    TextSize = 12,
                    IsAntialias = true,
                    Color = SKColors.Black
                })
                {
                    var width = (int)(paint.MeasureText(text) + 1);
                    var height = 13;

                    using (var bitmap = new SKBitmap(width, height))
                    {
                        using (var canvas = new SKCanvas(bitmap))
                        {
                            canvas.Clear(SKColors.Green);
                            canvas.DrawText(text, 0, 10, paint);
                            canvas.Flush();
                        }

                        if (_isFirstText)
                        {
                            _isFirstText = false;

                            using (var image = SKImage.FromBitmap(bitmap))
                            {
                                var data = image.Encode();

                                using (var stream = new FileStream("output.png", FileMode.Create, FileAccess.Write))
                                    data.SaveTo(stream);

                                var texture = GL.GenTexture();
                                GL.BindTexture(TextureTarget.ProxyTexture2D, texture);
                                //GL.TexImage2D(TextureTarget2d, 0, PixelInternalFormat.Rgb, )
                                // http://www.opengl-tutorial.org/beginners-tutorials/tutorial-5-a-textured-cube/

                                //GL.Rect(positionInfo.X, positionInfo.Y, positionInfo.X + positionInfo.Width, positionInfo.Y + positionInfo.Height);
                            }
                        }
                    }
                }
            }
        }

        private void DrawRect(PositionInfo positionInfo)
        {
            GL.Rect(positionInfo.X, positionInfo.Y, positionInfo.X + positionInfo.Width, positionInfo.Y + positionInfo.Height);
        }
    }
}
