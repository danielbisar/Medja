using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class GLUniform
    {
        public int Id { get; }
        public string Name { get; }
        
        public GLUniform(int programId, string name)
        {
            Name = name;
            Id = GL.GetUniformLocation(programId, name);
            
            if(Id == -1)
                throw new ArgumentException("The uniform '" + name + "' does not exist in program with id " + programId, nameof(name));
        }

        public void Set(ref Matrix4 value)
        {
            GL.UniformMatrix4(Id, false, ref value);
        }

        public void Set(ref Vector3 vec3)
        {
            GL.Uniform3(Id, vec3);
        }
    }
}