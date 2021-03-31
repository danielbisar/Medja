using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public class GLCamera : GLComponent
    {
        private bool _viewMatrixNeedsUpdate;
        protected bool _projectionMatrixNeedsUpdate;
        private bool _viewProjectionMatrixNeedsUpdate;

        [NonSerialized]
        public readonly Property<Vector3> PropertyPosition;
        public Vector3 Position
        {
            get { return PropertyPosition.Get(); }
            set { PropertyPosition.Set(value); }
        }

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

        [NonSerialized]
        public readonly Property<Matrix4> PropertyViewMatrix;
        public Matrix4 ViewMatrix
        {
            get
            {
                if(_viewMatrixNeedsUpdate)
                    UpdateViewMatrix();

                return PropertyViewMatrix.Get();
            }
            set { PropertyViewMatrix.Set(value); }
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

        [NonSerialized]
        public readonly Property<Matrix4> PropertyProjectionMatrix;
        public Matrix4 ProjectionMatrix
        {
            get
            {
                if(_projectionMatrixNeedsUpdate)
                    UpdateProjectionMatrix();

                return PropertyProjectionMatrix.Get();
            }
            set { PropertyProjectionMatrix.Set(value); }
        }

        [NonSerialized]
        public readonly Property<Matrix4> PropertyViewProjectionMatrix;
        public Matrix4 ViewProjectionMatrix
        {
            get
            {
                if(_viewMatrixNeedsUpdate)
                    UpdateViewMatrix();

                if(_projectionMatrixNeedsUpdate)
                    UpdateProjectionMatrix();

                if (_viewProjectionMatrixNeedsUpdate)
                    UpdateViewProjectionMatrix();

                return PropertyViewProjectionMatrix.Get();
            }
            set { PropertyViewProjectionMatrix.Set(value); }
        }

        public GLCamera()
        {
            PropertyViewMatrix = new Property<Matrix4>();
            PropertyViewMatrix.PropertyChanged += OnViewMatrixChanged;
            PropertyProjectionMatrix = new Property<Matrix4>();
            PropertyProjectionMatrix.PropertyChanged += OnProjectionMatrixChanged;
            PropertyViewProjectionMatrix = new Property<Matrix4>();

            PropertyTargetPosition = new Property<Vector3>();
            PropertyTargetPosition.SetSilent(Vector3.Zero);
            PropertyTargetPosition.PropertyChanged += OnViewMatrixPropertyChanged;

            PropertyUpVector = new Property<Vector3>();
            PropertyUpVector.SetSilent(new Vector3(0, 1, 0));
            PropertyUpVector.PropertyChanged += OnViewMatrixPropertyChanged;

            PropertyPosition = new Property<Vector3>();
            // +10 z means go back from 0,0,0 10 units
            PropertyPosition.SetSilent(new Vector3(0, 3, 10));
            PropertyPosition.PropertyChanged += OnViewMatrixPropertyChanged;

            PropertyZNear = new Property<float>();
            PropertyZNear.SetSilent(0.1f);

            PropertyZFar = new Property<float>();
            PropertyZFar.SetSilent(100.0f);

            _viewMatrixNeedsUpdate = true;
            _projectionMatrixNeedsUpdate = true;
            _viewProjectionMatrixNeedsUpdate = true;
        }

        private void OnProjectionMatrixChanged(object sender, PropertyChangedEventArgs e)
        {
            _viewProjectionMatrixNeedsUpdate = true;
        }

        private void OnViewMatrixChanged(object sender, PropertyChangedEventArgs e)
        {
            _viewProjectionMatrixNeedsUpdate = true;
        }

        private void OnViewMatrixPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _viewMatrixNeedsUpdate = true;
        }

        /// <summary>
        /// Updates the view matrix; is called automatically if needed before you get the value of <see cref="ViewMatrix"/>
        /// through the .NET property (not via PropertyViewMatrix.Get).
        /// </summary>
        public virtual void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(Position, TargetPosition, UpVector);
            _viewMatrixNeedsUpdate = false;
            _viewProjectionMatrixNeedsUpdate = true;
        }

        /// <summary>
        /// Updates the projection matrix; is called automatically if needed before you get the value of <see cref="ProjectionMatrix"/>
        /// through the .NET property (not via PropertyProjectionMatrix.Get).
        /// </summary>
        public virtual void UpdateProjectionMatrix()
        {
            _projectionMatrixNeedsUpdate = false;
            _viewProjectionMatrixNeedsUpdate = true;
        }

        /// <summary>
        /// Updates the view projection matrix; is called automatically before you get the value of <see cref="ViewProjectionMatrix"/>
        /// through the .NET property (not via PropertyViewProjectionMatrix.Get).
        /// </summary>
        public virtual void UpdateViewProjectionMatrix()
        {
            // OpenTK matrix multiplication order is from left to right
            ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
            _viewProjectionMatrixNeedsUpdate = false;
        }

        public override string ToString()
        {
            return "Camera: Pos(" + Position + "), TargetPos(" + TargetPosition + "), Z (near=" + ZNear + ", far=" + ZFar +")" + ToStringSubClass();
        }

        protected virtual string ToStringSubClass()
        {
            return "";
        }
    }
}
