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
        private int _vertexBufferId;

        private readonly VertextShader _vertextShader;
        private readonly FragmentShader _fragmentShader;
        private int _shaderProgramId;

        public ButtonRenderer()
        {
            _vertextShader = new VertextShader(@"#version 330 core
        layout(location = 0) in vec3 aPos;

        void main()
        {
            gl_Position = vec4(aPos.x, aPos.y, 0, 1.0);
        }");

            _fragmentShader = new FragmentShader(@"#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
} ");

            _shaderProgramId = ShaderHelper.CreateProgram(_vertextShader, _fragmentShader);

            // TODO using
            _fragmentShader.Dispose();
            _vertextShader.Dispose();

            _vertexBufferId = GL.GenBuffer();
        }

        public void Render(Control control, RenderContext context)
        {
            Debug.WriteLine("Button.Render");
            var button = control as Button;

            var x = button.X;
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
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.DrawElements(BeginMode.Triangles, vertices.Length / 2, );

            // todo use shaders
            // todo render text

            button.IsRendered = true;
        }
    }
}
