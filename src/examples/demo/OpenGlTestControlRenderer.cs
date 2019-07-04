using Medja.OpenTk.Components3D;
using Medja.OpenTk.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.Demo
{
	public class OpenGlTestControlRenderer : OpenTKControlRendererBase<OpenGlTestControl>
	{
        private readonly GLPerspectiveCamera _camera;
        private readonly GLSphere _glSphere;
        private readonly GLTriangle _glTriangle;
        private float _rotation = 0;
        
		public OpenGlTestControlRenderer(OpenGlTestControl control)
		: base(control)
		{
            _camera = new GLPerspectiveCamera();
            
            _glSphere = new GLSphere();
            _glSphere.Camera = _camera;
        }
		
        float z = 0;
        
		protected override void InternalRender()
		{
            // theoretically this should just be a one time setup
            /*GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.ShadeModel(ShadingModel.Smooth);*/
            
            //GL.Material(MaterialFace.Front, MaterialParameter.Emission, new Vector4(0.2f,1,0.5f,1f));
            //GL.Material(MaterialFace.Front, MaterialParameter.Specular, new Vector4(0.2f,1,0.5f,1));
            //GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 10);
           
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
           
            _camera.AspectRatio = _control.Position.Width / _control.Position.Height;
            _camera.Render();

            //GL.Light(LightName.Light0, LightParameter.Position, new Vector4(0, 2, -2, 0));
            
            GL.Color3(1.0f, 0.2f, 1.0f);
            
            var scale = 2.5f - ((_rotation / 100.0f) % 5.0f);
            _glSphere.Scale = new Vector3(scale);
            _glSphere.Rotation = new Vector3( 0, _rotation++, 0);
            _glSphere.Render();
		}
	}
}
