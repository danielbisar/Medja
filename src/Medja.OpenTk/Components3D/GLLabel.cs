using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using Medja.Utils;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using Font = Medja.Primitives.Font;

namespace Medja.OpenTk.Components3D
{
    public class CharCoordinates
    {
        public double X;
        public double Y;

        public double U; // like X from left to right
        public double V; // unlike Y from bottom to top

        /// <summary>
        /// Width in texture coordinates. For Height use FontTexture.LetterHeight
        /// </summary>
        public double Width;
        
        public double Width3D;
        public double Height3D;
    }
    
    /// <summary>
    /// A texture representing a font.
    /// </summary>
    public class FontTexture : IDisposable
    {
        public static readonly string Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZÖÄÜ!\"§$%&/()=?{[]}\\^°`´@€*+~'#;,:._-<>|";
        public static readonly int[] Index;

        static FontTexture()
        {
            var maxValue = 0;

            for (int i = 0; i < Chars.Length; i++)
            {
                var n = (int)Chars[i];
                
                if(n > maxValue)
                    maxValue = n;
            }
            
            Index = new int[maxValue + 1];
            
            for(int i = 0; i < Chars.Length; i++)
            {
                var c = (int)Chars[i];
                Index[c] = i;
            }
        }
        
        private readonly SKTypeface _typeface;
        public readonly List<CharCoordinates> _charCoordinates;
        public readonly int TextureId;
        
        /// <summary>
        /// Is the height of a letter in UV coordinates.
        /// </summary>
        public double LetterHeight {get; private set;}

        public FontTexture(Font font)
        {
            _typeface = SKTypeface.FromFamilyName(font.Name, font.GetSKFontStyle());

            using (var paint = new SKPaint())
            {
                paint.Typeface = _typeface;
                paint.TextSize = font.Size;
                paint.IsAntialias = true;
                paint.IsAutohinted = true;
                paint.SubpixelText = true;
                
                var textureSize = GetTextureSize(paint);
                _charCoordinates = CalculateCoordinates(paint, textureSize);

                var skImageInfo = new SKImageInfo(textureSize.Width, textureSize.Height);
                skImageInfo.AlphaType = SKAlphaType.Opaque;
                skImageInfo.ColorType = SKColorType.Bgra8888;

                using (var bitmap = new SKBitmap(skImageInfo))
                {
                    using (var canvas = new SKCanvas(bitmap))
                    {
                        canvas.Clear(SKColors.White);
                        
                        /*var positions = _charCoordinates.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
                        canvas.DrawPositionedText(Chars, positions, paint);*/
                        
                        // use ascent, which is a negative value, this makes the upper border of the
                        // text staying at a fixed position
                        canvas.DrawText(Chars, 0, -paint.FontMetrics.Ascent, paint);
                        canvas.Flush();
                    }

                    bitmap.SetImmutable();

//                    Console.WriteLine("IsImmutable = " + bitmap.IsImmutable);
//                    Console.WriteLine("BytesPerPixel = " + bitmap.BytesPerPixel);
//                    Console.WriteLine("ReadyToDraw = " + bitmap.ReadyToDraw);
//                    Console.WriteLine("AlphaType = " + bitmap.AlphaType);
//                    Console.WriteLine("ColorType = " + bitmap.ColorType);

                    TextureId = CreateOpenGlTexture(bitmap);
                }
            }
        }

        private SizeInt GetTextureSize(SKPaint paint)
        {
            // OpenGL textures can only be sized with values fitting the power of 2 values in width and height
            // tests showed that the unused area stays the same no matter how you divide the width
            // this is a real waste of memory; alternative would be to combine multiple fonts on one texture
            // for now we just create a texture that fits the requirement power of 2

            var textWidth = paint.MeasureText(Chars);
            var spacing = paint.FontSpacing;

            var textureWidth = (int)MedjaMath.GetNextPowerOfTwo((uint) textWidth);
            var textureHeight = (int)MedjaMath.GetNextPowerOfTwo((uint) spacing);

            // spacing is the actual height we need, get the difference, translate it to uv coordinates (uv are
            // percentages of the texture) inverse this value (basically reduce the 100% height by the height we don't
            // need)
            LetterHeight = 1 - (textureHeight - paint.FontSpacing) / (double) textureHeight;

            return new SizeInt(textureWidth, textureHeight);
            /*
             How the different sizes were tested
             
            Console.WriteLine("Chars width: " + textWidth);
            Console.WriteLine("Font spacing: " + spacing);

            uint x = 1;
            for (int i = 0; i < 5; i++)
            {
                var width = textWidth / x;
                var height = x * spacing;
                
                var pow2Width = MedjaMath.GetNextPowerOfTwo((uint) width);
                var pow2Height = MedjaMath.GetNextPowerOfTwo((uint) height);
                
                var usedArea = width*height;
                var actualArea = pow2Width*pow2Height;

                Console.WriteLine("width: " + width + " -> " + pow2Width);
                Console.WriteLine("height: " + height + " -> " + pow2Height);
                Console.WriteLine("used area: " + usedArea);
                Console.WriteLine("actual area: " + actualArea);
                Console.WriteLine("difference: " + (actualArea - usedArea)); 
                
                x *= 2;
            }*/
        }

        

        private List<CharCoordinates> CalculateCoordinates(SKPaint paint, SizeInt size)
        {
            var result = new List<CharCoordinates>();
            var x = 0d;
            var y = -paint.FontMetrics.Ascent;
            var scale = 5.0;
            var widths = paint.GetGlyphWidths(Chars);
            
            for (int i = 0; i < Chars.Length; i++)
            {
                var charWidth = widths[i];
               
                result.Add(new CharCoordinates
                {
                    X = x,
                    Y = y,
                    
                    U = x / size.Width,
                    V = 0,
                    
                    // uv width
                    Width = charWidth / (double)size.Width,
                    
                    Width3D = charWidth / scale,
                    Height3D = paint.FontSpacing / scale
                });
                
                Console.WriteLine("Char '" + Chars[i] + "' - width = " + charWidth);
                
                x += charWidth;
            }
            
            return result;
        }

        private int CreateOpenGlTexture(SKBitmap bitmap)
        {
            Contract.Assert(bitmap.ColorType == SKColorType.Bgra8888, "Only BGRA8888 is supported");
            
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            var textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            // copy the pixel data from SKBitmap to GPU memory
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                bitmap.Width, bitmap.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, bitmap.GetPixels());

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.ClampToBorder);
            
            // alternative to the above settings we could use mipmaps via GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            return textureId;
        }

        public void Dispose()
        {
            _typeface?.Dispose();
            GL.DeleteTexture(TextureId);
        }
    }
    
    public class GLLabel : GLModel
    {
        private readonly FontTexture _fontTexture;

        public GLLabel()
        : this(new Font {Name = "Source Code Pro"})
        {
        }

        public GLLabel(Font font)
        : this(new FontTexture(font))
        {
            
        }

        public GLLabel(FontTexture fontTexture)
        {
            _fontTexture = fontTexture;
        }

        public override void RenderModel()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _fontTexture.TextureId);
            
            var text = "#PQR_{}!#iftÖ";

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var coord = _fontTexture._charCoordinates[FontTexture.Index[(int)c]];
                
                var halfWidth = coord.Width3D / 2.0d;
                var halfHeight = coord.Height3D / 2.0d;
                
                GL.Translate(coord.Width3D,0,0);


                // yea, texture coordinates
                // start with 0, 0 at the lower left corner
                // end with 1, 1 at top right corner
                // similar we define the names: u0, v0 is the lower left corner start of the letter
                var u0 = coord.U;
                var v0 = coord.V;
                var u1 = coord.U + coord.Width;
                var v1 = coord.V + _fontTexture.LetterHeight;
                

                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(u0, v0);
                GL.Vertex3(-halfWidth, halfHeight, 0); // left top corner

                GL.TexCoord2(u0, v1);
                GL.Vertex3(-halfWidth, -halfHeight, 0); // left bottom corner

                GL.TexCoord2(u1, v1);
                GL.Vertex3(halfWidth, -halfHeight, 0); // right bottom corner

                GL.TexCoord2(u1, v0);
                GL.Vertex3(halfWidth, halfHeight, 0); // right top corner
                GL.End();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _fontTexture.Dispose();
            base.Dispose(disposing);
        }
    }
}
