using System;
using Medja.OpenTk.Components3D.Vertices;
using Medja.Primitives;
using Medja.Properties;

namespace Medja.OpenTk.Components3D
{
    public class GLLabel : GLModel
    {
        private VertexArrayObject _vao;
        private OpenGLProgram _program;
        private GLUniform _modelMatrixUniform;
        private GLUniform _viewProjectionMatrixUniform;
        private GLUniform _textureUniform;
        private bool _vaoNeedsUpdate;

        [NonSerialized]
        public readonly Property<string> PropertyText;
        public string Text
        {
            get { return PropertyText.Get(); }
            set { PropertyText.Set(value); }
        }

        public Font Font { get; }

        [NonSerialized]
        public readonly Property<float> PropertyScale;
        /// <summary>
        /// Gets or sets the scaling factor that is used from 2D text size to 3D.
        /// </summary>
        public float Scale
        {
            get { return PropertyScale.Get(); }
            set { PropertyScale.Set(value); }
        }

        public GLLabel()
        {
            Font = new Font();

            PropertyScale = new Property<float>();
            PropertyScale.SetSilent(100);
            PropertyText = new Property<string>();
            PropertyText.PropertyChanged += OnPropertyTextChanged;
            
            Font.PropertySize.PropertyChanged += OnFontPropertyChanged;
            
            _vaoNeedsUpdate = true;
        }

        private void OnFontPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO could also just update the texture?
            _vaoNeedsUpdate = true;
        }

        private void OnPropertyTextChanged(object sender, PropertyChangedEventArgs e)
        {
            _vaoNeedsUpdate = true;
        }

        private void CreateProgram()
        {
            var vertexShader = ShaderFactory.CreateDefaultVertexShader(new VertexShaderGenConfig
            {
                FixedColor = Colors.White,
                VertexArrayObject = _vao
            });
            //var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");
            var fragmentShader = ShaderFactory.CreatePassthroughTextureFragmentShader();

            _program = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);

            _modelMatrixUniform = _program.GetUniform("model");
            _viewProjectionMatrixUniform = _program.GetUniform("viewProjection");
            _textureUniform = _program.GetUniform("theTexture");
        }

        private void UpdateVao()
        {
            _vao?.Dispose();
            
            if(Text == null)
                return;
            
            var factory = new GLLabelVertexFactory();
            factory.FontTexture = new GLFontTexture(Font); // TODO cache?
            factory.Text = Text;
            factory.Scale = Scale;
            factory.Init();
            
            var vertices = factory.CreateVertices();
            var indices = factory.CreateIndices();
            var textureCoordinates = factory.CreateTextureCoordinates();
            
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 3;
            vbo.SetData(vertices);
            
            var vboTextureCoordinates = new VertexBufferObject();
            vboTextureCoordinates.ComponentsPerVertex = 2;
            vboTextureCoordinates.SetData(textureCoordinates);

            _vao = new VertexArrayObject();
            _vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);
            _vao.AddVertexAttribute(VertexAttributeType.TextureCoordinates, vboTextureCoordinates); 
            
            var ebo = _vao.CreateElementBufferObject();
            ebo.SetData(indices);
            
            // this must be done just once; the _vao is just needed for the 
            // attribute params
            // todo better would be to be able to describe the vao without 
            // creating it
            if(_program == null)
                CreateProgram();
        }

        public override void Render()
        {
            if(_vaoNeedsUpdate)
                UpdateVao();

            if (_vao != null)
            {
                base.Render();

                // TODO update only when changed?
                _modelMatrixUniform.Set(ref ModelMatrix._matrix);
                _viewProjectionMatrixUniform.Set(ref _viewProjectionMatrix);

                _program.Use();
                _vao.Render();
            }
        }
    }
}
