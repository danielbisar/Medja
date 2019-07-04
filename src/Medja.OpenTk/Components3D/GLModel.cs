using System;
using Medja.Properties;
using Medja.Utils.Math;
using Medja.Utils.Performance;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Components3D
{
    public class GLModel : GLComponent
    {
        [NonSerialized] 
        public readonly Property<GLCamera> PropertyCamera;

        /// <summary>
        /// Gets or sets the <see cref="GLCamera"/> used to view this object.
        /// </summary>
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

        private void BeginRender()
        {
            if (Camera != null)
            {
                var matrix = Camera.ViewMatrix;
                
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadMatrix(ref matrix);

                // even if deprecated, this calls are faster than manually creating the matrix
                GL.Rotate(Rotation.X, 1, 0, 0);
                GL.Rotate(Rotation.Y, 0, 1, 0);
                GL.Rotate(Rotation.Z, 0, 0, 1);

                GL.Translate(Position);
                GL.Scale(Scale);
            }
        }

        protected virtual void RenderModel()
        {
        }

        private void EndRender()
        {
            if(Camera != null)
                GL.PopMatrix();
        }
    }
}