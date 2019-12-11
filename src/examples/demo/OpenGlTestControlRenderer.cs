using Medja.OpenTk.Components3D;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using Medja.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.Demo
{
    public class OpenGlTestControlRenderer : OpenTKControlRendererBase<OpenGlTestControl>
    {
        private GLScene _scene;
        private GLCuboid _cube;
        private GLCuboid _cube2;
        
        public OpenGlTestControlRenderer(OpenGlTestControl control)
        : base(control)
        {
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            
            _scene = new GLScene();
            _scene.Camera = new GLPerspectiveCamera
            {
                Position = new Vector3(0, 2, 10),
                TargetPosition = new Vector3(0, 0.5f, 0),
            };
            
            _cube = new GLCuboid();
            _cube.SetColor(new Color(0.5f, 0.5f, 1, 0.25f));
            _cube.ModelMatrix.Position = new Vector3(1,0,0);

            _cube2 = new GLCuboid();
            _cube2.SetColor(new Color(1.0f, 0.5f, 0.5f, 0.5f));
            _cube2.ModelMatrix.Position = new Vector3(-2, 0, 0);

            _scene.Add(_cube);
            _scene.Add(_cube2);
        }

        protected override void InternalRender()
        {
            _cube.ModelMatrix.AddRotationX((float) MedjaMath.Radians(1));
            
            _cube2.ModelMatrix.AddRotationY((float) MedjaMath.Radians(1));
            
            GL.ClearColor(0.8f, 0.8f, 0.8f, 1); 
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _scene.Render();
        }

        protected override void Dispose(bool disposing)
        {
            _scene?.Dispose();
            base.Dispose(disposing);
        }
    }
}
