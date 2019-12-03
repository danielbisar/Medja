using Medja.OpenTk.Components3D.Vertices;
using Medja.Properties;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Class that uses an <see cref="VertexArrayObject"/> and shaders to render a model.
    /// </summary>
    public class GLVertexArrayModel : GLModel
    {
        public readonly Property<VertexArrayObject> PropertyVertexArrayObject;

        /// <summary>
        /// The <see cref="VertexArrayObject"/> this that should be rendered.
        /// </summary>
        public VertexArrayObject VertexArrayObject
        {
            get { return PropertyVertexArrayObject.Get(); }
            set { PropertyVertexArrayObject.Set(value); }
        }

        /// <summary>
        /// The <see cref="OpenGLProgram"/> that should be used.
        /// </summary>
        public OpenGLProgram ShaderProgram { get; set; }

        public GLVertexArrayModel()
        {
            PropertyVertexArrayObject = new Property<VertexArrayObject>();
        }
        
        public override void Render()
        {
            base.Render();

            ShaderProgram.Use();
            SetupProgram();
            
            VertexArrayObject.Render();
            ShaderProgram.Unuse();
        }

        /// <summary>
        /// Implement your setup code needed for the shader program here. (f.e. set your uniforms)
        /// </summary>
        protected virtual void SetupProgram()
        {
        }
    }
}