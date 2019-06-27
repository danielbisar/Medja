using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public abstract class GLCamera : GLComponent
    {
        [NonSerialized] public readonly Property<Vector3> PropertyTargetPosition;

        /// <summary>
        /// The position the viewer looks at.
        /// </summary>
        public Vector3 TargetPosition
        {
            get { return PropertyTargetPosition.Get(); }
            set { PropertyTargetPosition.Set(value); }
        }

        [NonSerialized] public readonly Property<Vector3> PropertyUpVector;

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

        protected GLCamera()
        {
            PropertyViewMatrix = new Property<Matrix4>();

            PropertyTargetPosition = new Property<Vector3>();
            PropertyTargetPosition.UnnotifiedSet(Vector3.Zero);
            PropertyTargetPosition.PropertyChanged += OnViewMatrixPropertyChanged;

            PropertyUpVector = new Property<Vector3>();
            PropertyUpVector.UnnotifiedSet(new Vector3(0, 1, 0));
            PropertyUpVector.PropertyChanged += OnViewMatrixPropertyChanged;
            
            PropertyPosition.PropertyChanged += OnViewMatrixPropertyChanged;
            
            UpdateViewMatrix();
        }

        private void OnViewMatrixPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateViewMatrix();
        }

        protected virtual void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(Position, TargetPosition, UpVector);
        }
    }
}