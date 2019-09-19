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
        public static readonly string Chars =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZÖÄÜ!\"§$%&/()=?{[]}\\^°`´@€*+~'#;,:._-<>|";

        public readonly CharTextureCoordinate[] Coordinates;

        public GLFontTexture(Font font)
        {
            Coordinates = new CharTextureCoordinate[Chars.Max()+1];

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
                        // use ascent, which is a negative value, this makes the upper border of the
                        // text staying at a fixed position
                        canvas.DrawText(Chars, 0, -paint.FontMetrics.Ascent, paint);
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
            // TODO var y = -paint.FontMetrics.Ascent;
            var widths = paint.GetGlyphWidths(Chars);

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
            var spacing = paint.FontSpacing;
            var textWidth = paint.MeasureText(Chars);

            var textureWidth = (int) MedjaMath.GetNextPowerOfTwo((uint) textWidth);
            var textureHeight = (int) MedjaMath.GetNextPowerOfTwo((uint) spacing);

            // spacing is the actual height we need, get the difference, translate it to uv coordinates (uv are
            // percentages of the texture) inverse this value (basically reduce the 100% height by the height we don't
            // need)
            //LetterHeightUV = 1 - (textureHeight - paint.FontSpacing) / (double) textureHeight;

            return new SizeInt(textureWidth, textureHeight);
        }

        public CharTextureCoordinate GetCoordinates(char c)
        {
            return Coordinates[c];
        }
    }
}