using System;
using Medja.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Defines an Orthographic camera.
    /// </summary>
    /// <remarks>
    /// - Scale.Z component has no effect
    /// - 
    /// </remarks>
    public class GLOrthographicCamera : GLCamera
    {
        private Matrix4 _projectionMatrix;
        
        [NonSerialized] 
        public readonly Property<float> PropertyWidth;

        public float Width
        {
            get { return PropertyWidth.Get(); }
            set { PropertyWidth.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<float> PropertyHeight;

        public float Height
        {
            get { return PropertyHeight.Get(); }
            set { PropertyHeight.Set(value); }
        }

        public GLOrthographicCamera()
        {
            PropertyWidth = new Property<float>();
            PropertyWidth.SetSilent(12);
            
            PropertyHeight = new Property<float>();
            PropertyHeight.SetSilent(9);
            
            PropertyWidth.PropertyChanged += OnProjectionMatrixPropertyChanged;
            PropertyHeight.PropertyChanged += OnProjectionMatrixPropertyChanged;
            PropertyZFar.PropertyChanged += OnProjectionMatrixPropertyChanged;
            PropertyZNear.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            UpdateProjectionMatrix();
        }

        private void OnProjectionMatrixPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateProjectionMatrix();
        }

        private void UpdateProjectionMatrix()
        {
            _projectionMatrix = Matrix4.CreateOrthographic(Width * Scale.X, Height * Scale.Y, ZNear, ZFar);
        }

        public override void Render()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _projectionMatrix);
        }
    }
}