using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Base class for simple colored models
    /// </summary>
    public class GLColoredModel : GLModel
    {
        private readonly VertexArrayObject _vao;
        private readonly OpenGLProgram _program;
        private readonly GLUniform _modelMatrixUniform;
        private readonly GLUniform _viewProjectionMatrixUniform;
        private readonly GLUniform _colorUniform;
        private Vector3 _color;

        public GLColoredModel(float[] data, uint[] indices = null)
        {
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 3;
            vbo.SetData(data);

            _vao = new VertexArrayObject();
            _vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);

            if (indices != null)
            {
                var ebo = _vao.CreateElementBufferObject();
                ebo.SetData(indices);
            }
            
            var vertexShader = new OpenGLShader(ShaderType.VertexShader);
            vertexShader.Source = @"#version 420

" + _vao.GetAttributeLayoutCode() + @"

uniform mat4 viewProjection;
uniform mat4 model;
uniform vec3 color;

out vec3 outColor;

void main()
{
    gl_Position = viewProjection * model * vec4(position, 1);
    outColor = color; //vec3(1,1,1);
}";

            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");

            _program = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);
            _modelMatrixUniform = _program.GetUniform("model");
            _viewProjectionMatrixUniform = _program.GetUniform("viewProjection");
            _colorUniform = _program.GetUniform("color");

            SetColor(Colors.White);
        }

        public void SetColor(Color color)
        {
            _color = color.ToVector3();
            _colorUniform.Set(ref _color);
        }

        public override void Render()
        {
            base.Render();

            _modelMatrixUniform.Set(ref ModelMatrix._matrix);
            _viewProjectionMatrixUniform.Set(ref _viewProjectionMatrix);

            _program.Use();
            _vao.Render();
        }
    }
}