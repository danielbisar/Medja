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
        private GLLabel _label;
        private float _ry = 0;
        
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
            /*_scene.Camera = new GLOrthographicCamera
            {
                Width = 3,
                Height = 3,
                Position = new Vector3(0, 2, 10),
                TargetPosition = new Vector3(0, 0.5f, 0)
            };*/
            _scene.Camera = new GLPerspectiveCamera
            {
                Position = new Vector3(0, 2, 10),
                TargetPosition = new Vector3(0, 0.5f, 0)
            };
            /*_scene.AddRenderWrapper(new GLTriangle(), triangle =>
            {
                _ry += 0.01f;
                triangle.ModelMatrix.Rotation = new Vector3(0, _ry, 0);
                
                for (int r = 0; r < 360; r += 60)
                {
                    triangle.SetColor(Colors.ColorsArray[r / 60]);
                    triangle.ModelMatrix.AddRotationY((float) MedjaMath.Radians(60));
                    triangle.Render();
                }
            });*/
            
            var cube = new GLCuboid();
            cube.SetColor(new Color(0.5f, 0.5f, 1, 0.25f));
            
            _scene.AddRenderWrapper(cube, cuboid =>
            {
                cuboid.ModelMatrix.AddRotationX((float) MedjaMath.Radians(1));
                cuboid.ModelMatrix.AddRotationY((float) MedjaMath.Radians(1));

                cuboid.Render();
            });
            _scene.AddRenderWrapper(_label = new GLLabel(), label =>
            {
                label.ModelMatrix.AddRotationX((float) MedjaMath.Radians(1));
                label.ModelMatrix.AddRotationY((float) MedjaMath.Radians(1));

                label.Render();
            });
            _label.Text = "!#iftÖg";
            _label.Font.Size = 24;
        }
        
        protected override void InternalRender()
        {
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
