using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering.Shaders;
using Medja.Rendering;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering
{
    public class ButtonRenderer : IControlRenderer
    {
        private readonly Program _program;
        private VertextBufferObject _vertexBuffer;
        
        public ButtonRenderer()
        {
            var vertextShader = new VertextShader(DefaultShaders.Simple3DVertexShader);
            var fragmentShader = new FragmentShader(DefaultShaders.SimpleFragmentShader);
            _program = new Program();

            _program.AddShader(vertextShader);
            _program.AddShader(fragmentShader);

            _program.CompileAndLink();
            _vertexBuffer = new VertextBufferObject();

            float[] vertices = {
        -0.5f, -0.5f, 0.0f, // left  
         0.5f, -0.5f, 0.0f, // right 
         0.0f,  0.5f, 0.0f  // top   
    };

            _vertexBuffer.Copy(vertices);
        }

        public void Render(Control control, RenderContext context)
        {
            Debug.WriteLine("Button.Render");
            var button = control as Button;

            /*var x = button.X;
            var y = button.Y;
            var xRight = x + button.Width;
            var yTop = y + button.Height;

            var vertices = new []
            {
               x, y,
               xRight, y,
               x, yTop,

               x, yTop,
               xRight, yTop,
               xRight, y
            };*/                      
      
            GL.UseProgram(_program.Id);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer.Id);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            var errorCode = GL.GetError();

            if (errorCode != ErrorCode.NoError)
                throw new InvalidOperationException("Found GL Error: " + errorCode);
            
            // todo render text

            button.IsRendered = true;
        }
    }
}
