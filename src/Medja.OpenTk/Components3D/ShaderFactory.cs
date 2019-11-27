using System;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public static class ShaderFactory
    {
        /// <summary>
        /// Creates a simple fragment shader that does not do anything with the color.
        /// </summary>
        /// <param name="inColorComponentCount">Is the input from the previous shader a vec3 or vec4.</param>
        /// <param name="inColorName">The name of the variable that contains the color values from the previous shader.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static OpenGLShader CreatePassthroughFragmentShader(int inColorComponentCount, string inColorName)
        {
            if (inColorComponentCount != 3 && inColorComponentCount != 4)
                throw new ArgumentOutOfRangeException(nameof(inColorComponentCount));

            if (string.IsNullOrEmpty(inColorName))
                throw new ArgumentException("must contain a value that is not null or whitespaces",
                    nameof(inColorName));

            if (inColorName == "fragmentColor")
                throw new ArgumentException("cannot contain the value 'fragmentColor'", nameof(inColorName));

            var source = string.Format(@"#version 420

in vec{0} {1};
out vec4 fragmentColor;

void main()
{{
    fragmentColor = {2};
}}", inColorComponentCount, inColorName, GetVec4Expression(inColorName, inColorComponentCount));
            
            var shader = new OpenGLShader(ShaderType.FragmentShader);
            shader.Source = source;
            
            return shader;
        }

        public static OpenGLShader CreatePassthroughTextureFragmentShader()
        {
            var source = @"#version 420

uniform sampler2D theTexture;

in vec2 outTextureCoord;

out vec4 fragmentColor;

void main()
{
    fragmentColor = texture(theTexture, outTextureCoord);
}
";
            var shader = new OpenGLShader(ShaderType.FragmentShader);
            shader.Source = source;

            return shader;
        }

        private static string GetVec4Expression(string inVarName, int inComponentCount)
        {
            if (string.IsNullOrEmpty(inVarName))
                throw new ArgumentException("must contain a value that is not null or whitespaces",
                    nameof(inVarName));
            
            if(inComponentCount == 3)
                return "vec4("+inVarName+", 1)";
            else if(inComponentCount == 4)
                return inVarName;
            else
                throw new ArgumentOutOfRangeException(nameof(inComponentCount));
        }

        /// <summary>
        /// Creates a default shader that applies the viewProjection and model matrix (as uniform) and gets the configured
        /// attributes from the given <see cref="VertexArrayObject"/>.
        /// </summary>
        /// <param name="config">The vertex shader generation config object.</param>
        /// <returns>The generated OpenGLShader</returns>
        public static OpenGLShader CreateDefaultVertexShader(VertexShaderGenConfig config)
        {
            var mainBody = new StringBuilder();
            mainBody.AppendLine("gl_Position = viewProjection * model * vec4(position, 1);");
            
            var hasColorInput = config.VertexArrayObject.HasAttributeOfType(VertexAttributeType.Colors);
            
            if(config.HasColorParam)
                mainBody.Append("outColor = color;");
            else if (hasColorInput)
            {
                if (config.ColorComponentCount == 3)
                {
                    mainBody.AppendFormat("outColor = vec3({0}, {1}, {2});",
                        config.FixedColor.Red,
                        config.FixedColor.Green,
                        config.FixedColor.Blue);
                }
                else if (config.ColorComponentCount == 4)
                {
                    mainBody.AppendFormat("outColor = vec4({0}, {1}, {2}, {3});",
                        config.FixedColor.Red,
                        config.FixedColor.Green,
                        config.FixedColor.Blue,
                        config.FixedColor.Alpha);
                }
                else
                    throw new NotSupportedException($"invalid value for {nameof(config.ColorComponentCount)}");
            }

            mainBody.AppendLine();


            var hasTextureCoordinates =
                config.VertexArrayObject.HasAttributeOfType(VertexAttributeType.TextureCoordinates);
            
            if(hasTextureCoordinates)
                mainBody.AppendLine("outTextureCoord = textureCoord;"); 
            

            var source = CreateDefaultVertexShaderCode(config, mainBody.ToString());
            
            var shader = new OpenGLShader(ShaderType.VertexShader);
            shader.Source = source;
            
            return shader;
        }

        /// <summary>
        /// Creates a default shader that applies the viewProjection and model matrix (as uniform) and gets the configured
        /// attributes from the given <see cref="VertexArrayObject"/>.
        /// </summary>
        /// <param name="config">The vertex shader generation config object.</param>
        /// <param name="mainBody">The body of the main method (without brackets)</param>
        /// <returns>The generated shader code.</returns>
        /// <remarks>
        /// Attribute layout code: the layout(location = 0) in vec3 position; part, as generated by
        /// <see cref="VertexBufferObject.GetAttributeLayoutCode"/>.
        ///
        /// contains 3 uniforms: viewProjection => the mat4 viewProjection matrix, mat4 model => the model matrix, vec3 color the input color.
        ///
        /// out vec3 outColor;
        /// </remarks>
        public static string CreateDefaultVertexShaderCode(VertexShaderGenConfig config, string mainBody)
        {
            if(config.VertexArrayObject == null)
                throw new NullReferenceException("config: no VertexArrayObject set");
            
            var hasTextureCoordinates =
                config.VertexArrayObject.HasAttributeOfType(VertexAttributeType.TextureCoordinates);
            
            var sb = new StringBuilder();
            
            sb.AppendLine("#version 420");
            sb.AppendLine();
            sb.AppendLine(config.VertexArrayObject.GetAttributeLayoutCode());
            sb.AppendLine();
            sb.AppendLine("uniform mat4 viewProjection;");
            sb.AppendLine("uniform mat4 model;");

            if (config.HasColorParam)
            {
                sb.AppendFormat("uniform vec{0} color;", config.ColorComponentCount);
                sb.AppendLine();
            }

            if(hasTextureCoordinates)
                sb.AppendLine("out vec2 outTextureCoord;");
            else
            {
                sb.AppendFormat("out vec{0} outColor;", config.ColorComponentCount);
                sb.AppendLine();
            }
            
            sb.AppendLine();
            sb.AppendLine("void main()");
            sb.AppendLine("{");
            sb.AppendLine(mainBody);
            sb.AppendLine("}");
            
            return sb.ToString();
        }
    }
}