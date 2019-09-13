using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Trivial triangle implementation. Do not use to render large amounts of triangles. There are much better ways
    /// (VBO, Quads etc) 
    /// </summary>
    public class GLTriangle : GLModel
    {
        private readonly VertexArrayObject _vao;
        private readonly OpenGLProgram _program;

        public GLTriangle()
        {
            var data = new float[]
            {
                0,0,
                1,0,
                0,1
            };
            
            var vbo = new VertexBufferObject();
            vbo.ComponentsPerVertex = 2;
            vbo.SetData(data);
            
            _vao = new VertexArrayObject();
            _vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);

            var vertexShader = new OpenGLShader(ShaderType.VertexShader);
            vertexShader.Source = @"#version 420

" + _vao.GetAttributeLayoutCode() + @"

out vec3 outColor;

void main()
{
    gl_Position = vec4(position, 0, 1);
    outColor = vec3(1,1,1);
}";
            
            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");
            
            _program = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);
        }
        
        
        public override void Render()
        {
            _program.Use();
            _vao.Render();
        }
    }
}