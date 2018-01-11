using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Rendering.Shaders
{
    public class FragmentShader : Shader
    {
        public FragmentShader(string source)
        {
            Source = source;
            Id = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(Id, Source);
            ShaderHelper.Compile(Id);
        }
    }
}
