using System;
using System.Linq;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using Medja.Utils;
using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace Medja.OpenTk.Components3D
{
    public class GLFontTexture : GLTexture
    {
        public const float MarginTop = 2; // always n pixels margin to the top

        public static readonly string Chars =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZÖÄÜ!\"§$%&/()=?{[]}\\^°`´@€*+~'#;,:._-<>|";

        public readonly CharTextureCoordinates Coordinates;
        
        public float LetterHeight { get { return Coordinates.LetterHeight; } }

        public GLFontTexture(Font font)
        {
            Coordinates = new CharTextureCoordinates(Chars.Max()+1);

            SetMinFilter(TextureMinFilter.Linear);
            SetMagFilter(TextureMagFilter.Linear);
            SetWrapS(TextureWrapMode.ClampToBorder);
            SetWrapT(TextureWrapMode.ClampToBorder);

            using (var bitmap = CreateFontBitmap(font))
                SetData(bitmap);
        }

        private SKBitmap CreateFontBitmap(Font font)
        {
            using (var typeface = SKTypeface.FromFamilyName(font.Name, font.GetSKFontStyle()))
            {
                using (var paint = new SKPaint())
                {
                    paint.Typeface = typeface;
                    paint.TextSize = font.Size;
                    paint.IsAntialias = true;
                    paint.IsAutohinted = true;
                    paint.SubpixelText = true;
                    
                    var textureSize = GetTextureSize(paint);
                    UpdateCharCoordinates(paint, textureSize);

                    var skImageInfo = new SKImageInfo(textureSize.Width, textureSize.Height);
                    skImageInfo.AlphaType = SKAlphaType.Opaque;
                    skImageInfo.ColorType = SKColorType.Bgra8888;

                    var bitmap = new SKBitmap(skImageInfo);

                    using (var canvas = new SKCanvas(bitmap))
                    {
                        canvas.Clear(SKColors.White);
                        canvas.DrawText(Chars, 0, -paint.FontMetrics.Ascent + MarginTop, paint);
                        canvas.Flush();
                    }

                    bitmap.SetImmutable();

                    return bitmap;
                }
            }
        }

        private void UpdateCharCoordinates(SKPaint paint, SizeInt textureSize)
        {
            var x = 0f;
            var widths = paint.GetGlyphWidths(Chars);
            
            // yes divided by width; since width is basically our unit system
            Coordinates.LetterHeight = GetFontHeight(paint) / textureSize.Width;
            
            for (int i = 0; i < Chars.Length; i++)
            {
                // since uv coordinates are in percentage we need to calculate the percentage
                var charWidth = widths[i] / textureSize.Width;
                
                Coordinates[Chars[i]] = new CharTextureCoordinate
                {
                    TopLeft = new TextureCoordinate(x, 1),
                    BottomLeft = new TextureCoordinate(x, 0),
                    TopRight = new TextureCoordinate(x + charWidth, 1),
                    BottomRight = new TextureCoordinate(x + charWidth, 0),
                    WidthPercentage = charWidth
                };

                x += charWidth;
            }
        }

        private SizeInt GetTextureSize(SKPaint paint)
        {
            // OpenGL textures can only be sized with values fitting the power of 2 values in width and height
            // tests showed that the unused area stays the same no matter how you divide the width
            // this is a real waste of memory; alternative would be to combine multiple fonts on one texture
            // for now we just create a texture that fits the requirement power of 2
            var textHeight = GetFontHeight(paint);
            var textWidth = paint.MeasureText(Chars);

            var textureWidth = (int) MedjaMath.GetNextPowerOfTwo((uint) textWidth);
            var textureHeight = (int) MedjaMath.GetNextPowerOfTwo((uint) textHeight);

            return new SizeInt(textureWidth, textureHeight);
        }

        private float GetFontHeight(SKPaint paint)
        {
            return paint.FontSpacing + MarginTop;
        }

        public CharTextureCoordinate GetCoordinates(char c)
        {
            return Coordinates[c];
        }
    }
}