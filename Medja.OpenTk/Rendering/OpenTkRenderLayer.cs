using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Medja.Controls;
using Medja.Layers;
using Medja.Layers.Layouting;
using Medja.OpenTk.Eval;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class OpenTkRenderLayer : ILayer
    {
        public IEnumerable<ControlState> Apply(IEnumerable<ControlState> states)
        {
            Debug.WriteLine(" --- OpenTkRenderLayer --- ");
            
            foreach(var state in states)
            {
                var item = state.Control;
                var position = state.Position;

                Debug.WriteLine(" - Control: " + item.GetType().Name);

                if (item is Button b)
                {
                    GL.Color3(0.35, 0.35, 0.35);
                    DrawRect(position);
                    GL.Color3(0, 0, 0);

                    DrawText(position, b.Text);
                }
                else
                    DrawRect(position);
            }

            return states;
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
