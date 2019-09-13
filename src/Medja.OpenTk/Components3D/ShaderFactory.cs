using System;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public static class ShaderFactory
    {
        /// <summary>
        /// 
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

            var shader = new OpenGLShader(ShaderType.FragmentShader);
            shader.Source = string.Format(@"#version 420

in vec{0} {1};
out vec4 fragmentColor;

void main()
{{
    fragmentColor = {2};
}}", inColorComponentCount, inColorName, GetVec4Expression(inColorName, inColorComponentCount));
            
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
    }
}