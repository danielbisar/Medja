using Medja.Primitives;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Configuration for generating a vertex shader.
    /// </summary>
    public class VertexShaderGenConfig
    {
        /// <summary>
        /// If true the shader will have a uniform named color as input. Else the fixed color will be used.
        /// </summary>
        public bool HasColorParam { get; set; }
        
        /// <summary>
        /// Is only used if <see cref="HasColorParam"/> is false. Sets a fixed color.
        /// </summary>
        public Color FixedColor { get; set; }
        
        public int ColorComponentCount { get; set; }
        
        public VertexArrayObject VertexArrayObject { get; set; }

        public VertexShaderGenConfig()
        {
            ColorComponentCount = 3;
        }
    }
}