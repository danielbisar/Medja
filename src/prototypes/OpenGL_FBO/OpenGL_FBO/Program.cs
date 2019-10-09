using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MultiOpenGLContext;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace OpenGL_FBO
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }
        
        private GameWindow _gameWindow;
        private int _fboId, _programNoTexId, _programTexId, _textureId, _wireTextureId;
        private GlObject _glObject;
        private GlObject _plane;
        
        public Program()
        {
        }

        public void Run()
        {
            using (_gameWindow = new GameWindow(800,600,
                GraphicsMode.Default, "FBO", GameWindowFlags.Default, 
                DisplayDevice.Default, 4, 2, 
                GraphicsContextFlags.ForwardCompatible))
            {
                _gameWindow.Load += OnLoad;
                _gameWindow.Resize += OnResize;
                _gameWindow.RenderFrame += OnRender;
                _gameWindow.Run();
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            
            // debug test: texture
            _wireTextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _programTexId);
            
            using (var bitmap = new Bitmap(Path.GetFullPath("wire.jpg")))
            {
                var data = bitmap.LockBits(
                    new Rectangle(0,0,bitmap.Width, bitmap.Height), 
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, 
                    PixelInternalFormat.Rgb, 
                    data.Width, data.Height, 0,
                    PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data); 
                
                //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, 
                //    bitmap.Height, 0, PixelFormat.Bgra,
                //    PixelType.UnsignedByte,
                //    bitmap.GetPixels());
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            // since render buffers can't be shared between opengl contexts we use a texture as color render target
            // additionally most tutorials show how to use a texture, so this seems the most common way
            // post processing is also not possible on the render buffer directly
            /*_textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 800, 600, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 
                IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            */GL.BindTexture(TextureTarget.Texture2D, 0);
          
            /*var drbi = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, drbi);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, 800, 600);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            
            _fboId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fboId);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _textureId, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, drbi);
            
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            
            if(status != FramebufferErrorCode.FramebufferComplete)
                throw new Exception("Framebuffer is incomplete");
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            
            
            _programNoTexId = ShaderFactory.CreateShaderProgram();
            */_programTexId = ShaderFactory.CreateTexShaderProgram();
            /*
            /*_glObject = new GlObject(new[]
                            {
                                new Vertex(new Vector4(-0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                                new Vertex(new Vector4(0.0f, -0.25f, 0.5f, 1f), Color4.Black),
                                new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                            });*/
            
            _plane = new GlObject(new[]
                            {
                                // bottom left
                                new Vertex(new Vector4(-0.25f, -0.25f, 0.5f, 1f), new Vector2(0,0)),
                                // top right
                                new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1f), new Vector2(1,1)),
                                // top left
                                new Vertex(new Vector4(-0.25f, 0.25f, 0.5f, 1f), new Vector2(0,1)),
                                
                                // top right
                                new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1f), new Vector2(1,1)),
                                // bottom left
                                new Vertex(new Vector4(-0.25f, -0.25f, 0.5f, 1f), new Vector2(0,0)),
                                // bottom right
                                new Vertex(new Vector4(0.25f, -0.25f, 0.5f, 1f), new Vector2(1,0)),
                            });
        }

        private void OnResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            GL.Viewport(0,0, clientRect.Width, clientRect.Height);
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            GL.ClearColor(0f, 0.2f, 0.8f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);
         
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _wireTextureId);
            //GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.UseProgram(_programTexId);
            
            _plane.Render(); 
            
            GL.UseProgram(0);
            GL.Disable(EnableCap.Texture2D);
            
            /*GL.UseProgram(_programNoTexId);
            _glObject.Render();
            GL.UseProgram(0);*/
                                          
            
            // render triangle to frame buffer object
            /*GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fboId);
            GL.ClearColor(0, 1f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_programNoTexId);
            _glObject.Render();
            GL.UseProgram(0);
                        
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);*/
            
            
            _gameWindow.SwapBuffers();
        }
    }
}
