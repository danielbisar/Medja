using Medja.OpenTk.Components3D.Vertices;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Base class for simple models with of one solid color.
    /// </summary>
    public class GLColoredModel : GLModel
    {
        private readonly VertexArrayObject _vao;
        private readonly OpenGLProgram _program;
        
        private readonly GLUniform _modelMatrixUniform;
        private readonly GLUniform _viewProjectionMatrixUniform;
        private readonly GLUniform _colorUniform;
        
        private Vector4 _color;

        public GLColoredModel(IVertexArrayObjectFactory factory)
        : this(factory.Create())
        {
        }

        public GLColoredModel(VertexArrayObject vao)
        {
            _vao = vao;

            var config = new VertexShaderGenConfig
            {
                HasColorParam = true,
                VertexArrayObject = _vao,
                ColorComponentCount = 4
            };
            var vertexShader = ShaderFactory.CreateDefaultVertexShader(config);
            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(config.ColorComponentCount, "outColor");

            _program = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);
            _modelMatrixUniform = _program.GetUniform("model");
            _viewProjectionMatrixUniform = _program.GetUniform("viewProjection");
            _colorUniform = _program.GetUniform("color");

            SetColor(Colors.White);
        }

        public void SetColor(Color color)
        {
            _color = color.ToVector4();
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