using System;
using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace Medja.OpenTk.Components3D
{
    public class GLTexture : IDisposable
    {
        public int Id { get; }
        
        public GLTexture()
        {
            Id = GL.GenTexture();
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public void SetData(SKBitmap bitmap)
        {
            if(bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            
            if(bitmap.ColorType != SKColorType.Bgra8888)
                throw new NotSupportedException("Only Bgra8888 textures are supported!");

            Bind();
            // copy the pixel data from SKBitmap to GPU memory
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                bitmap.Width, bitmap.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, bitmap.GetPixels());
        }

        public void SetMinFilter(TextureMinFilter filter)
        {
            Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) filter);
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) filter);
        }

        public void SetWrapS(TextureWrapMode wrapMode)
        {
            Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) wrapMode);
        }
        
        public void SetWrapT(TextureWrapMode wrapMode)
        {
            Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) wrapMode);
        }

        public void GenerateMipmap()
        {
            Bind();
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Dispose()
        {
            GL.DeleteTexture(Id);
        }
    }
}