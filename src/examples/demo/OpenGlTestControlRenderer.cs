using System;
using Medja.OpenTk.Components3D;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.Demo
{
	public class OpenGlTestControlRenderer : OpenTKControlRendererBase<OpenGlTestControl>
	{
        private readonly GLPerspectiveCamera _camera;
        private readonly GLSphere _glSphere;
        private readonly GLLabel _label;
        private float _rotation = 0;
        
		public OpenGlTestControlRenderer(OpenGlTestControl control)
		: base(control)
		{
            _camera = new GLPerspectiveCamera();
            
            //_glSphere = new GLSphere();
            //_glSphere.Camera = _camera;

            _label = new GLLabel();
            _label.Camera = _camera;
        }
        
		protected override void InternalRender()
		{
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
           
            _camera.AspectRatio = _control.Position.Width / _control.Position.Height;
            _camera.Render();

            _label.Position = new Vector3(-40,0,-50);
            _label.Render();
            
            //var scale = 2.5f - ((_rotation / 100.0f) % 5.0f);
            //_glSphere.Scale = new Vector3(scale);
            //_glSphere.Rotation = new Vector3( 0, _rotation++, 0);
            //_glSphere.Render();
		}
	}
}
