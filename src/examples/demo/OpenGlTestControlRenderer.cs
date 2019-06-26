using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Medja.OpenTk.Rendering;
using Medja.Utils.Math;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Medja.Demo
{
	public class OpenGlTestControlRenderer : OpenTKControlRendererBase<OpenGlTestControl>
	{
        private readonly GLSphere _glSphere;
        private float _rotation = 0;
        
		public OpenGlTestControlRenderer(OpenGlTestControl control)
		: base(control)
		{
            _glSphere = new GLSphere();
            _glSphere.Create(1, 32, 32);
        }
		
		protected override void InternalRender()
		{
            /*GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);*/
            GL.Enable(EnableCap.CullFace);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var fovRadian = (float) MedjaMath.Radians(45);
            var aspecRatio = 4 / 3;
            var zNear = 0.1f;
            var zFar = 100.0f;

            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fovRadian, aspecRatio, zNear, zFar);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
            
            var eye = new Vector3(0, 3, -10);
            var target = new Vector3();
            var up = new Vector3(0, 1, 0);

            var viewMatrix = Matrix4.LookAt(eye, target, up);
            var modelMatrix = Matrix4.Identity; // model position
            
            //var mvp = projectionMatrix * viewMatrix * modelMatrix; // get applied from right to left
            
			GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);
            
            //GL.Translate(new Vector3(0, 0, 10.0f));
            GL.Rotate(_rotation += 1, new Vector3(0, 1, 0));

			GL.Color3(1.0f, 0.2f, 1.0f);
			
            _glSphere.Draw();
		}
	}
}
