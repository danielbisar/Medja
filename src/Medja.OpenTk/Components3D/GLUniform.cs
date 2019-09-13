using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class GLUniform
    {
        public string Name {get;}
        public int Id {get;}

        public GLUniform(int programId, string name)
        {
            Name = name;
            Id = GL.GetUniformLocation(programId, name);
        }

        public void Set(ref Matrix4 value)
        {
            GL.UniformMatrix4(Id, false, ref value);
        }
    }
}