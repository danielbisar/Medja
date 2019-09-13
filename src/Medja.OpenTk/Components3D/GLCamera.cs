using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public abstract class GLCamera : GLComponent
    {
        [NonSerialized] 
        public readonly Property<Vector3> PropertyTargetPosition;

        /// <summary>
        /// The position the viewer looks at.
        /// </summary>
        public Vector3 TargetPosition
        {
            get { return PropertyTargetPosition.Get(); }
            set { PropertyTargetPosition.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<Vector3> PropertyUpVector;

        /// <summary>
        /// Defines where is up of the viewers eyes. Default: x=0, y=1, z=0
        /// </summary>
        public Vector3 UpVector
        {
            get { return PropertyUpVector.Get(); }
            set { PropertyUpVector.Set(value); }
        }
        
        [NonSerialized] public readonly Property<Matrix4> PropertyViewMatrix;

        public Matrix4 ViewMatrix
        {
            get { return PropertyViewMatrix.Get(); }
            set { PropertyViewMatrix.Set(value); }
        }

        [NonSerialized] public readonly Property<float> PropertyZNear;

        /// <summary>
        /// Distance of the near clipping plane. Default: 0.1f
        /// </summary>
        public float ZNear
        {
            get { return PropertyZNear.Get(); }
            set { PropertyZNear.Set(value); }
        }

        [NonSerialized] public readonly Property<float> PropertyZFar;

        /// <summary>
        /// Distance of the far clipping plane. Default: 100.0f
        /// </summary>
        public float ZFar
        {
            get { return PropertyZFar.Get(); }
            set { PropertyZFar.Set(value); }
        }

        protected GLCamera()
        {
            PropertyViewMatrix = new Property<Matrix4>();

            PropertyTargetPosition = new Property<Vector3>();
            PropertyTargetPosition.SetSilent(Vector3.Zero);
            PropertyTargetPosition.PropertyChanged += OnViewMatrixPropertyChanged;

            PropertyUpVector = new Property<Vector3>();
            PropertyUpVector.SetSilent(new Vector3(0, 1, 0));
            PropertyUpVector.PropertyChanged += OnViewMatrixPropertyChanged;

            // +10 z means go back from 0,0,0 10 units
            PropertyPosition.SetSilent(new Vector3(0, 3, 10));
            PropertyPosition.PropertyChanged += OnViewMatrixPropertyChanged;

            PropertyZNear = new Property<float>();
            PropertyZNear.SetSilent(0.1f);

            PropertyZFar = new Property<float>();
            PropertyZFar.SetSilent(100.0f);
            
            UpdateViewMatrix();
        }

        private void OnViewMatrixPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(Position, TargetPosition, UpVector);
        }
    }
}