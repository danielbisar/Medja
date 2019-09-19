using System;
using System.Collections.Generic;
using Medja.Properties;

namespace Medja.OpenTk.Components3D
{
    
    // original code is in VSCode
    // thoughts:
    // use draw_dynamic or static (stream doesn't make sense)
    // use one or more vbos for labels (fixed size and could be transformed; what about the texture)
    // how fast is recreating the vbo? (measure)
    
    public class GLLabel : GLModel
    {
        private VertexArrayObject _vao;
        private readonly OpenGLProgram _program;
        private readonly GLUniform _modelMatrixUniform;
        private readonly GLUniform _viewProjectionMatrixUniform;
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
            
            _program = CreateProgram();
            _modelMatrixUniform = _program.GetUniform("model");
            _viewProjectionMatrixUniform = _program.GetUniform("viewProjection");
            _vaoNeedsUpdate = true;
        }

        private void OnPropertyTextChanged(object sender, PropertyChangedEventArgs e)
        {
            _vaoNeedsUpdate = true;
        }

        private OpenGLProgram CreateProgram()
        {
            var vertexShader = ShaderFactory.CreateDefaultVertexShader(new VertexShaderGenConfig
            {
                HasColorParam = true,
                VertexArrayObject = _vao
            });
            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");

            return OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);
        }

        private void UpdateVao()
        {
            _vao?.Dispose();

            /*var vertices = CreateVertices();
            
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 3;
            vbo.SetData(data);

            _vao = new VertexArrayObject();
            _vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);

            if (indices != null)
            {
                var ebo = _vao.CreateElementBufferObject();
                ebo.UsageHint =
                    ebo.SetData(indices);
            }

            _vao = new VertexArrayObject();
            _vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);*/
        }

        

        public override void Render()
        {
            if(_vaoNeedsUpdate)
                UpdateVao();
            
            base.Render();
            
            _program.Use();
            _vao.Render();
        }
    }
}
