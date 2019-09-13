using Medja.OpenTk.Components3D;
using Medja.OpenTk.Rendering;
using Medja.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.Demo
{
    public class OpenGlTestControlRenderer : OpenTKControlRendererBase<OpenGlTestControl>
    {
        public OpenGlTestControlRenderer(OpenGlTestControl control)
        : base(control)
        {
        }
        
        GLTriangle _triangle;
        VertexArrayObject _vao;
        OpenGLProgram _program;
        
        public override void Initialize()
        {
            base.Initialize();

            //_triangle = new GLTriangle();

            var data = new float[]
            {
                0, 0,
                1, 0,
                0, 1
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

uniform mat4 combined;

void main()
{
    gl_Position = combined * vec4(position, 0, 1);
    outColor = vec3(1,1,1);
}";

            var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");

            var projection = Matrix4.CreatePerspectiveFieldOfView(0.79f, 4 / 3f, 0.1f, 100.0f);
            var view = Matrix4.Identity;
            var model = Matrix4.Identity;
            
            var trans = Matrix4.CreateTranslation(0, -0.5f, 0);
            var rotation = CreateRotation(0, (float)MedjaMath.Radians(35), 0);

            combined = /*projection * */ trans * rotation * view * model;
            
            

            _program = OpenGLProgram.CreateAndCompile(vertexShader, fragmentShader);
            var combinedUniform = _program.GetUniform("combined");
            
            combinedUniform.Set(ref combined);
        }

        

        Matrix4 combined;
        

        protected override void InternalRender()
        {
            //GL.ClearColor(1,1,1,1);
            //GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _program.Use();
            _vao.Render();
            
            //_triangle.Render();
        }

        protected override void Dispose(bool disposing)
        {
            _vao?.Dispose();
            _program?.Dispose();

            _triangle?.Dispose();
            
            base.Dispose(disposing);
        }
    }
}
