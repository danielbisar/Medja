using System;
using Medja.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Components3D
{
    public class GLModel : GLComponent
    {
        [NonSerialized] 
        public readonly Property<GLCamera> PropertyCamera;

        public GLCamera Camera
        {
            get { return PropertyCamera.Get(); }
            set { PropertyCamera.Set(value); }
        }
        
        public GLModel()
        {
            PropertyCamera = new Property<GLCamera>();
        }

        public override void Render()
        {
            BeginRender();
            RenderModel();
            EndRender();            
        }

        protected virtual void BeginRender()
        {
            if (Camera != null)
            {
                var matrix = Matrix4.Identity;
                matrix = Camera.ViewMatrix;

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadMatrix(ref matrix);

                GL.Rotate(Rotation.X, 1, 0, 0);
                GL.Rotate(Rotation.Y, 0, 1, 0);
                GL.Rotate(Rotation.Z, 0, 0, 1);

                GL.Translate(Position);
            }
        }

        protected virtual void RenderModel()
        {
        }

        protected virtual void EndRender()
        {
            GL.PopMatrix();
        }
    }
}