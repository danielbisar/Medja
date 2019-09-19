using System;
using System.Collections.Generic;
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
        private bool _vaoNeedsUpdate;

        [NonSerialized]
        public readonly Property<string> PropertyText;

        public string Text
        {
            get { return PropertyText.Get(); }
            set { PropertyText.Set(value); }
        }

        public GLLabel()
        {
            PropertyText = new Property<string>();
            PropertyText.PropertyChanged += OnPropertyTextChanged;
            
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
            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");

            _program = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);

            _modelMatrixUniform = _program.GetUniform("model");
            _viewProjectionMatrixUniform = _program.GetUniform("viewProjection");
        }

        private void UpdateVao()
        {
            _vao?.Dispose();
            
            var factory = new GLLabelVertexFactory(Text, 1);
            var vertices = factory.CreateVertices();
            var indices = factory.CreateIndices();
            
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 3;
            vbo.SetData(vertices);

            _vao = new VertexArrayObject();
            _vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);
            
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
            
            base.Render();

            // TODO update only when changed?
            _modelMatrixUniform.Set(ref ModelMatrix._matrix);
            _viewProjectionMatrixUniform.Set(ref _viewProjectionMatrix);
            
            _program.Use();
            _vao.Render();
        }
    }
}
