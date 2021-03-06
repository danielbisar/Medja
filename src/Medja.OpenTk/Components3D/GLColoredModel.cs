using Medja.OpenTk.Components3D.Vertices;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Renders solid color models with VAO and a shader program.
    /// </summary>
    public class GLColoredModel : GLVertexArrayModel
    {
        private GLUniform _modelMatrixUniform;
        private GLUniform _viewProjectionMatrixUniform;
        private GLUniform _colorUniform;
        
        private Vector4 _color;

        public GLColoredModel(IVertexArrayObjectFactory factory)
        : this(factory.Create())
        {
        }

        public GLColoredModel(VertexArrayObject vao)
        : this()
        {
            VertexArrayObject = vao;
        }

        public GLColoredModel()
        {
            PropertyVertexArrayObject.PropertyChanged += OnVertexArrayObjectChanged;
            SetColor(Colors.White);
        }

        private void OnVertexArrayObjectChanged(object sender, PropertyChangedEventArgs eventargs)
        {
            CreateShader();
        }

        private void CreateShader()
        {
            ShaderProgram?.Dispose();

            if (VertexArrayObject == null)
                return;
            
            var config = new VertexShaderGenConfig
            {
                HasColorParam = true,
                VertexArrayObject = VertexArrayObject,
                ColorComponentCount = 4
            };
            var vertexShader = ShaderFactory.CreateDefaultVertexShader(config);
            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(config.ColorComponentCount, "outColor");
            
            ShaderProgram = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);
            _modelMatrixUniform = ShaderProgram.GetUniform("model");
            _viewProjectionMatrixUniform = ShaderProgram.GetUniform("viewProjection");
            _colorUniform = ShaderProgram.GetUniform("color");
            _colorUniform.Set(ref _color);
        }

        public void SetColor(Color color)
        {
            _color = color.ToVector4();
            _colorUniform?.Set(ref _color);
        }

        protected override void SetupProgram()
        {
            _modelMatrixUniform.Set(ref ModelMatrix._matrix);
            _viewProjectionMatrixUniform.Set(ref _viewProjectionMatrix);
        }
    }
}