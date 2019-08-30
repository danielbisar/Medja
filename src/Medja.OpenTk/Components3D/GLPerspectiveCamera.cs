using System;
using Medja.Properties;
using Medja.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Components3D
{
    public class GLPerspectiveCamera : GLCamera
    {
        private Matrix4 _projectionMatrix;
        
        [NonSerialized] 
        public readonly Property<float> PropertyFieldOfViewAngle;
        /// <summary>
        /// The field of view angle in radians. Default value: 45° => 0.79 radians. Use <see cref="MedjaMath.Radians"/>.
        /// This is also the "zoom", <see cref="GLComponent.Scale"/> is not applied in this class.
        /// </summary>
        public float FieldOfViewAngle
        {
            get { return PropertyFieldOfViewAngle.Get(); }
            set { PropertyFieldOfViewAngle.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<float> PropertyAspectRatio;

        /// <summary>
        /// The x to y aspect ratio. Default value: 4/3
        /// </summary>
        public float AspectRatio
        {
            get { return PropertyAspectRatio.Get(); }
            set { PropertyAspectRatio.Set(value); }
        }

        public GLPerspectiveCamera()
        {
            PropertyFieldOfViewAngle = new Property<float>();
            PropertyFieldOfViewAngle.SetSilent(0.79f);
            PropertyFieldOfViewAngle.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            PropertyAspectRatio = new Property<float>();
            PropertyAspectRatio.SetSilent(4f / 3);
            PropertyAspectRatio.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            PropertyZNear.PropertyChanged += OnProjectionMatrixPropertyChanged;
            PropertyZFar.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            UpdateProjectionMatrix();
        }

        private void OnProjectionMatrixPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateProjectionMatrix();
        }

        private void UpdateProjectionMatrix()
        {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfViewAngle, AspectRatio, ZNear, ZFar);
        }

        public override void Render()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _projectionMatrix);
        }
    }
}