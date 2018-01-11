using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.OpenTk.Rendering.Shaders
{
    public static class DefaultShaders
    {
        public static readonly string Simple3DVertexShader = 
      @"#version 330 core
        layout(location = 0) in vec3 aPos;

        void main()
        {
            gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
        }";

        public static readonly string SimpleFragmentShader = 
            @"#version 330 core
            out vec4 FragColor;
            
            void main()
            {
                FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
            }";
    }
}
