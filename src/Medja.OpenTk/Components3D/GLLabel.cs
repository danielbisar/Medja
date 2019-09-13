using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using Medja.Properties;
using Medja.Utils;
using OpenTK.Graphics.OpenGL4;
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
        public double LetterHeightUV { get; private set;}
        
        public double LetterHeight3D { get; private set; }

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
            LetterHeightUV = 1 - (textureHeight - paint.FontSpacing) / (double) textureHeight;

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

        /// <summary>
        /// Measures the width of a text in 3D units (using Width3D).
        /// </summary>
        /// <param name="str">The string to measure.</param>
        /// <returns>The width in 3D units.</returns>
        public double MeasureWidth(string str)
        {
            return str.Sum(c => _charCoordinates[Index[c]].Width3D);
        }

        /// <summary>
        /// Measures the width of a text in 3D units (using Width3D). Stops if maxWidth is reached.
        /// </summary>
        /// <param name="str">The string to measure.</param>
        /// <param name="maxWidth">The maximum width allowed.</param>
        /// <returns>The width in 3D units.</returns>
        public double MeasureWidth(string str, int maxWidth)
        {
            var width = 0d;
            
            foreach (var newWidth in str.Select(c => width + _charCoordinates[Index[c]].Width3D))
            {
                if(newWidth >= maxWidth)
                    return width;
                
                width = newWidth;
            }
            
            return width;
        }

        public void Dispose()
        {
            _typeface?.Dispose();
            GL.DeleteTexture(TextureId);
        }
    }
    
    public class GLLabel : GLModel
    {
        //private readonly FontTexture _fontTexture;
        //private GLMesh _mesh;

        [NonSerialized,] 
        public readonly Property<string> PropertyText;
        public string Text
        {
            get { return PropertyText.Get(); }
            set { PropertyText.Set(value); }
        }

        public GLLabel()
        : this(new Font {Name = "Source Code Pro"})
        {
        }

        public GLLabel(Font font)
        : this((FontTexture)null /*new FontTexture(font)*/)
        {
            
        }

        public GLLabel(FontTexture fontTexture)
        {
            //_fontTexture = fontTexture;
            //_mesh = new GLMesh();
            
            PropertyText = new Property<string>();
            PropertyText.SetSilent("#PQR_{}!#iftÖ");
            PropertyText.SetSilent("0");
            
            UpdateVertices();
        }
        
        int vboId = -1;

        private void UpdateVertices()
        {
            vboId = GL.GenBuffer();
            
            var buffer = new float[]
            {
                0, 1, 0, // top    left
                0, 0, 0, // bottom left
                1, 0, 0, // bottom right
                1, 1, 0, // top    right

                0, 1,
                0, 0,
                1, 0,
                1, 1
            };
            var bufferSize = buffer.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, buffer, BufferUsageHint.StaticDraw);
            
            /*var shader = new OpenGLShader();
            shader.Source = @"#version 420

";
            
            var program = new OpenGLProgram();*/
            
            /*_mesh?.Dispose();

            if (vboId != -1)
            {
                GL.DeleteBuffer(vboId);
            }

            GL.EnableClientState(ArrayCap.VertexArray);
            //GL.EnableClientState(ArrayCap.TextureCoordArray);
            
            
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer,
                bufferSize,
                buffer,
                BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
            //GL.DisableClientState(ArrayCap.TextureCoordArray);
            
            /*_mesh = new GLMesh();
            // we cannot use quad strip, because we need multiple 
            // texture coordinates for same vertices
            _mesh.PrimitiveType = PrimitiveType.Quads;
            
            if(Text.Length == 0)
                return;

            var x = 0d;
            
            foreach (var c in Text)
            {
                var coord = _fontTexture._charCoordinates[FontTexture.Index[Text[0]]];

                // texture coordinates
                // start with 0, 0 at the lower left corner
                // end with 1, 1 at top right corner
                // similar we define the names: u0, v0 is the lower left corner start of the letter
                var u0 = coord.U;
                var v0 = coord.V;
                var u1 = coord.U + coord.Width;
                var v1 = coord.V + _fontTexture.LetterHeightUV;
                var letterHeight = (float)coord.Height3D;
                
                _mesh.AddTexCoord((float)u0, (float) v0);
                _mesh.AddVertex((float)x, letterHeight, 0, false); // left top corner

                _mesh.AddTexCoord((float) u0, (float) v1);
                _mesh.AddVertex((float)x, 0, 0, false); // left bottom corner
                
                x += coord.Width3D;

                _mesh.AddTexCoord((float) u1, (float) v1);
                _mesh.AddVertex((float)x, 0, 0, false); // right bottom corner

                _mesh.AddTexCoord((float) u1, (float) v0);
                _mesh.AddVertex((float)x, letterHeight, 0, false); // right top corner
            }
            
            _mesh.CreateBuffers();*/
            
            
        }

        public override void Render()
        {
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DisableVertexAttribArray(0);

            if (GL.GetError() != ErrorCode.NoError)
            {
                Console.WriteLine("Found OpenGL error: " + GL.GetError());
            }
            
            //GL.Enable(EnableCap.Texture2D);
            //GL.BindTexture(TextureTarget.Texture2D, _fontTexture.TextureId);
            /*GL.EnableClientState(ArrayCap.VertexArray);
            //GL.EnableClientState(ArrayCap.TextureCoordArray);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            //GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 12);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Texture2D);*/
            
            /*var text = "#PQR_{}!#iftÖ";

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var coord = _fontTexture._charCoordinates[FontTexture.Index[(int)c]];
                
                var halfWidth = coord.Width3D / 2.0d;
                var halfHeight = coord.Height3D / 2.0d;


                // texture coordinates
                // start with 0, 0 at the lower left corner
                // end with 1, 1 at top right corner
                // similar we define the names: u0, v0 is the lower left corner start of the letter
                var u0 = coord.U;
                var v0 = coord.V;
                var u1 = coord.U + coord.Width;
                var v1 = coord.V + _fontTexture.LetterHeight;

                void addLetter(char ch)
                {
                    GL.Vertex3(
                }
                
                /*GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(u0, v0);
                GL.Vertex3(-halfWidth, halfHeight, 0); // left top corner

                GL.TexCoord2(u0, v1);
                GL.Vertex3(-halfWidth, -halfHeight, 0); // left bottom corner

                GL.TexCoord2(u1, v1);
                GL.Vertex3(halfWidth, -halfHeight, 0); // right bottom corner

                GL.TexCoord2(u1, v0);
                GL.Vertex3(halfWidth, halfHeight, 0); // right top corner
                GL.End();*/
            //}
        }

        protected override void Dispose(bool disposing)
        {
            //_fontTexture.Dispose();
            base.Dispose(disposing);
        }
    }
}
