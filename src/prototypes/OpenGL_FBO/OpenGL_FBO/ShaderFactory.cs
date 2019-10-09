using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace MultiOpenGLContext
{
    public static class ShaderFactory
    {
        public static int CreateShaderProgram()
        {
            var result = GL.CreateProgram();
            var shaders = new List<int>();
            shaders.Add(CompileShader(ShaderType.VertexShader,
                @"#version 420 core

layout (location = 0) in vec4 position;
layout (location = 1) in vec4 color;

out vec4 vs_color;

void main(void)
{
    gl_Position = position;
    vs_color = color;
}"));

            shaders.Add(CompileShader(ShaderType.FragmentShader,
                @"#version 420 core

in vec4 vs_color;

out vec4 color;

void main(void)
{
    color = vs_color;
}
"));
            foreach (var shader in shaders)
                GL.AttachShader(result, shader);

            GL.LinkProgram(result);

            var info = GL.GetProgramInfoLog(result);

            if (!string.IsNullOrWhiteSpace(info))
                throw new Exception($"CompileShaders ProgramLinking had errors: {info}");

            foreach (var shader in shaders)
            {
                GL.DetachShader(result, shader);
                GL.DeleteShader(shader);
            }
            
            return result;
        }
        
        public static int CreateTexShaderProgram()
        {
            var result = GL.CreateProgram();
            var shaders = new List<int>();
            shaders.Add(CompileShader(ShaderType.VertexShader,
                @"#version 420 core

layout (location = 0) in vec4 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec2 texCoords;

out vec2 oTexCoords;

void main(void)
{
    gl_Position = position;
    oTexCoords = texCoords;
}"));

            shaders.Add(CompileShader(ShaderType.FragmentShader,
                @"#version 420 core

in vec2 oTexCoords;

uniform sampler2D screenTex;

out vec4 color;

void main(void)
{
    color = texture(screenTex, oTexCoords);
}
"));
            foreach (var shader in shaders)
                GL.AttachShader(result, shader);

            GL.LinkProgram(result);

            var info = GL.GetProgramInfoLog(result);

            if (!string.IsNullOrWhiteSpace(info))
                throw new Exception($"CompileShaders ProgramLinking had errors: {info}");

            foreach (var shader in shaders)
            {
                GL.DetachShader(result, shader);
                GL.DeleteShader(shader);
            }
            
            return result;
        }

        private static int CompileShader(ShaderType shaderType, string source)
        {
            var shader = GL.CreateShader(shaderType);

            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            var info = GL.GetShaderInfoLog(shader);

            if (!string.IsNullOrWhiteSpace(info))
                throw new Exception($"CompileShader {shaderType} had errors: {info}");

            return shader;
        }
    }
}