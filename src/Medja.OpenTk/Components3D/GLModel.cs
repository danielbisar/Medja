using System;
using Medja.Properties;

namespace Medja.OpenTk.Components3D
{
    public class GLModel : GLComponent
    {
        /// <summary>
        /// Applies the model view matrix and calls render.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="camera"></param>
        /// <param name="render"></param>
        public static void ApplyModelViewMatrix(GLComponent component, GLCamera camera, Action render)
        {
            if(camera == null)
                return;

            var matrix = camera.ViewMatrix;

            /*GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadMatrix(ref matrix);

            var rotation = component.Rotation;
            
            // even if deprecated, this calls are faster than manually creating the matrix
            GL.Rotate(rotation.X, 1, 0, 0);
            GL.Rotate(rotation.Y, 0, 1, 0);
            GL.Rotate(rotation.Z, 0, 0, 1);

            GL.Translate(component.Position);
            GL.Scale(component.Scale);
            
            render();
            
            GL.PopMatrix();*/
        }
    
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

        /// <summary>
        /// Calls RenderModel inside <see cref="ApplyModelViewMatrix"/>.
        /// </summary>
        public override void Render()
        {
            ApplyModelViewMatrix(this, Camera, RenderModel);
        }
        
        /// <summary>
        /// Renders the model, without applying transformation matrix.
        /// </summary>
        public virtual void RenderModel()
        {
        }
    }
}