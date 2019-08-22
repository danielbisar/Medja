using System;
using System.Runtime.InteropServices;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Medja.OpenTk.Components3D
{
    public class FontTexture : IDisposable
    {
        public static readonly string Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"§$%&/()=?{[]}\\^°`´@€*+~'#;,:._-<>|";
        private static readonly int[] Index;

        static FontTexture()
        {
            Index = new int[char.MaxValue];
            
            for(int i = 0; i < Chars.Length; i++)
            {
                var c = Chars[i];
                Index[c] = i;
            }
        }
        
        private readonly SKTypeface _typeface;
        public readonly int TextureId;

        public FontTexture(Font font)
        {
            _typeface = SKTypeface.FromFamilyName(font.Name, font.GetSKFontStyle());

            using (var paint = new SKPaint())
            {
                paint.Typeface = _typeface;
                paint.TextSize = font.Size;
                
                var width = (int)Math.Round(paint.MeasureText(Chars), 0, MidpointRounding.AwayFromZero);
                var top = paint.FontMetrics.Top;
                var bottom = paint.FontMetrics.Bottom;
                var height = (int) Math.Round(bottom - top, 0, MidpointRounding.AwayFromZero);
                
                Console.WriteLine("Width: " + width + ", height: " + height);

                var skImageInfo = new SKImageInfo(width, height);
                
                using (var bitmap = new SKBitmap(skImageInfo))
                {
                    using (var canvas = new SKCanvas(bitmap))
                    {
                        canvas.DrawText(Chars, 0, 0, paint);
                    }

                    TextureId = GL.GenTexture();
                    
                    Console.WriteLine("Texture id: " + TextureId);
                    
                    GL.BindTexture(TextureTarget.Texture2D, TextureId);
                    GL.TexImage2D(TextureTarget.Texture2D, 0,
                        PixelInternalFormat.Rgb, width, height, 0,
                        PixelFormat.Rgb, PixelType.UnsignedByte, bitmap.Pixels);
                    
                    // might not be the best option for fonts to create mipmaps
                    //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); 
                }
            }
        }

        public void Dispose()
        {
            _typeface?.Dispose();
        }
    }
    
    public class GLLabel : GLModel
    {
        private readonly GLMesh _mesh;
        private readonly FontTexture _fontTexture;

        public GLLabel()
        {
            _fontTexture = new FontTexture(new Font { Name = "SourceCode Pro" });
            
            /*_mesh = new GLMesh();
            _mesh.PrimitiveType = PrimitiveType.Quads;

            _mesh.AddTexCoord(0, 1);
            _mesh.AddVertex(0, 0, 0);

            _mesh.AddTexCoord(1, 1);
            _mesh.AddVertex(width, 0, 0);

            _mesh.AddTexCoord(1, 0);
            _mesh.AddVertex(width, height, 0);

            _mesh.AddTexCoord(0, 0);
            _mesh.AddVertex(0, height, 0);
            
            _mesh.CreateBuffers();*/
        }

        public override void RenderModel()
        {
            float w, h;
            w = 100;
            h = 30;
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _fontTexture.TextureId);
            
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0,0,0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(w, 0, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(w, h, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, h, 0);
            GL.End();
        }
    }
}