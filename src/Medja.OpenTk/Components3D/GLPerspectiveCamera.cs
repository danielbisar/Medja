using System;
using Medja.Properties;
using Medja.Utils.Math;
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

        [NonSerialized] 
        public readonly Property<float> PropertyZNear;

        /// <summary>
        /// Distance of the near clipping plane. Default: 0.1f
        /// </summary>
        public float ZNear
        {
            get { return PropertyZNear.Get(); }
            set { PropertyZNear.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<float> PropertyZFar;

        /// <summary>
        /// Distance of the far clipping plane. Default: 100.0f
        /// </summary>
        public float ZFar
        {
            get { return PropertyZFar.Get(); }
            set { PropertyZFar.Set(value); }
        }

        public GLPerspectiveCamera()
        {
            PropertyFieldOfViewAngle = new Property<float>();
            PropertyFieldOfViewAngle.UnnotifiedSet(0.79f);
            PropertyFieldOfViewAngle.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            PropertyAspectRatio = new Property<float>();
            PropertyAspectRatio.UnnotifiedSet(4f / 3);
            PropertyAspectRatio.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            PropertyZNear = new Property<float>();
            PropertyZNear.UnnotifiedSet(0.1f);
            PropertyZNear.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            PropertyZFar = new Property<float>();
            PropertyZFar.UnnotifiedSet(100.0f);
            PropertyZFar.PropertyChanged += OnProjectionMatrixPropertyChanged;
            
            Position = new Vector3(0, 3, -10);
        
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