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

            //float[] vertices = {
            //    -0.5f, 0.5f, 0, // left top
            //     0.5f, 0.5f, 0, // right top
            //     0.5f, 0, 0,    // right low
            //     -0.5f, 0, 0,    // left low  
            //};

            //_vertexBuffer.Copy(vertices);
        }

        public void Render(Control control, RenderContext context)
        {            
            var button = control as Button;

            var x = button.X;
            var y = button.Y;
            var xRight = x + button.Width;
            var yTop = y + button.Height;

            float[] vertices = {
                 x, yTop, 0, // left top
                 xRight, yTop, 0, // right top
                 xRight, y, 0,    // right low
                 x, y, 0,    // left low  
            };

            //Debug.WriteLine("Button.Render");
            //Debug.WriteLine("" + x + ", " + y);
            //Debug.WriteLine("" + xRight + ", " + yTop);

            //float[] vertices = {
            //    -0.5f, 0.5f, 0, // left top
            //     0.5f, 0.5f, 0, // right top
            //     0.5f, 0, 0,    // right low
            //     -0.5f, 0, 0,    // left low  
            //};

            _vertexBuffer.Copy(vertices);

            /*var vertices = new []
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
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            //GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            var errorCode = GL.GetError();

            if (errorCode != ErrorCode.NoError)
                throw new InvalidOperationException("Found GL Error: " + errorCode);
            
            // todo render text

            button.IsRendered = true;
        }
    }
}
