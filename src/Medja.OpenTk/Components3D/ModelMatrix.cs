using System;
using System.Diagnostics;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Represents the model matrix plus helper methods to modify it.
    /// </summary>
    public class ModelMatrix
    {
        /// <summary>
        /// Creates the rotation matrix.
        /// </summary>
        /// <param name="rotation">Rotation vector (each component defines the
        /// rotation around the corresponding axis in radians)</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4 CreateRotation(Vector3 rotation)
        {
            var result = Matrix4.Identity;

            result *= Matrix4.CreateRotationX(rotation.X);
            result *= Matrix4.CreateRotationY(rotation.Y);
            result *= Matrix4.CreateRotationZ(rotation.Z);

            return result;
        }

        public static Matrix4 CreateRotation(float x, float y, float z)
        {
            return CreateRotation(new Vector3(x, y, z));
        }
        
        private bool _rotationMatrixNeedsUpdate;
        private bool _scalingMatrixNeedsUpdate;
        private bool _translationMatrixNeedsUpdate;
        
        private Matrix4 _rotationMatrix;
        private Matrix4 _scalingMatrix;
        private Matrix4 _translationMatrix;

        public Matrix4 _matrix;

        /// <summary>
        /// Gets the model matrix (contains position, rotation, ... of the model)
        /// </summary>
        public Matrix4 Matrix
        {
            get { return _matrix; }
        }

        [NonSerialized]
        public readonly Property<Vector3> PropertyPosition;
        public Vector3 Position
        {
            get { return PropertyPosition.Get(); }
            set { PropertyPosition.Set(value); }
        }

        [NonSerialized]
        public readonly Property<Vector3> PropertyScaling;
        public Vector3 Scaling
        {
            get { return PropertyScaling.Get(); }
            set { PropertyScaling.Set(value); }
        }

        [NonSerialized]
        public readonly Property<Vector3> PropertyRotation;
        public Vector3 Rotation
        {
            get { return PropertyRotation.Get(); }
            set { PropertyRotation.Set(value); }
        }

        public ModelMatrix()
        {
            PropertyPosition = new Property<Vector3>();
            PropertyScaling = new Property<Vector3>();
            PropertyRotation = new Property<Vector3>();

            _matrix = Matrix4.Identity;
            _rotationMatrix = Matrix4.Identity;
            _translationMatrix = Matrix4.Identity;
            _scalingMatrix = Matrix4.Identity;

            // property changed handlers
            PropertyPosition.PropertyChanged += OnPositionChanged;
            PropertyScaling.PropertyChanged += OnScalingChanged;
            PropertyRotation.PropertyChanged += OnRotationChanged;
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            _translationMatrixNeedsUpdate = true;
        }

        private void OnScalingChanged(object sender, PropertyChangedEventArgs e)
        {
            _scalingMatrixNeedsUpdate = true;
        }

        private void OnRotationChanged(object sender, PropertyChangedEventArgs e)
        {
            _rotationMatrixNeedsUpdate = true;
        }

        /// <summary>
        /// Adds to the current rotation rotation around X axis by <see cref="angle"/> radians.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        public void AddRotationX(float angle)
        {
            Rotation = new Vector3(Rotation.X + angle, Rotation.Y, Rotation.Z);
        }

        /// <summary>
        /// Adds to the current rotation rotation around Y axis by <see cref="angle"/> radians.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        public void AddRotationY(float angle)
        {
            Rotation = new Vector3(Rotation.X, Rotation.Y + angle, Rotation.Z);
        }

        /// <summary>
        /// Adds to the current rotation rotation around Z axis by <see cref="angle"/> radians.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        public void AddRotationZ(float angle)
        {
            Rotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z + angle);
        }

        public void SetRotationX(float angle)
        {
            Rotation = new Vector3(angle, Rotation.Y, Rotation.Z);
        }

        public void SetRotationY(float angle)
        {
            Rotation = new Vector3(Rotation.X, angle, Rotation.Z);
        }

        public void SetRotationZ(float angle)
        {
            Rotation = new Vector3(Rotation.X, Rotation.Y, angle);
        }

        /// <summary>
        /// Updates the model matrix if necessary.
        /// </summary>
        public void UpdateModelMatrix()
        {
            if (_rotationMatrixNeedsUpdate || _translationMatrixNeedsUpdate || _scalingMatrixNeedsUpdate)
            {
                if (_rotationMatrixNeedsUpdate)
                {
                    _rotationMatrix = CreateRotation(Rotation);
                    _rotationMatrixNeedsUpdate = false;
                }

                if (_scalingMatrixNeedsUpdate)
                {
                    _scalingMatrix = Matrix4.CreateScale(Scaling);
                    _scalingMatrixNeedsUpdate = false;
                }

                if (_translationMatrixNeedsUpdate)
                {
                    _translationMatrix = Matrix4.CreateTranslation(Position);
                    _translationMatrixNeedsUpdate = false;
                }

                _matrix = _scalingMatrix * _rotationMatrix * _translationMatrix;
            }
        }
    }
}