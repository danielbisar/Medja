using System;
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
        
        private bool _rotationMatrixNeedsUpdate;
        private bool _scalingMatrixNeedsUpdate;
        private bool _translationMatrixNeedsUpdate;
        
        private Matrix4 _rotationMatrix;
        private Matrix4 _scalingMatrix;
        private Matrix4 _translationMatrix;

        public Matrix4 _matrix;

        [NonSerialized]
        public readonly Property<Matrix4> PropertyMatrix;
        /// <summary>
        /// Gets the model matrix (contains position, rotation, ... of the model)
        /// </summary>
        public Matrix4 Matrix
        {
            get => PropertyMatrix.Get();
            protected set => PropertyMatrix.Set(value);
        }

        [NonSerialized]
        public readonly Property<Vector3> PropertyTranslation;
        public Vector3 Translation
        {
            get => PropertyTranslation.Get();
            set => PropertyTranslation.Set(value);
        }

        [NonSerialized]
        public readonly Property<Vector3> PropertyScaling;
        public Vector3 Scaling
        {
            get => PropertyScaling.Get();
            set => PropertyScaling.Set(value);
        }

        [NonSerialized]
        public readonly Property<Vector3> PropertyRotation;
        public Vector3 Rotation
        {
            get => PropertyRotation.Get();
            set => PropertyRotation.Set(value);
        }

        [NonSerialized]
        public readonly Property<bool> PropertyRotateBeforeTranslate;
        /// <summary>
        /// If true (default value) the rotation matrix is applied before the translation matrix, else the other way around.
        /// </summary>
        public bool RotateBeforeTranslate
        {
            get => PropertyRotateBeforeTranslate.Get();
            set => PropertyRotateBeforeTranslate.Set(value);
        }

        public ModelMatrix()
        {
            PropertyMatrix = new Property<Matrix4>();
            PropertyMatrix.SetSilent(Matrix4.Identity);
            PropertyTranslation = new Property<Vector3>();
            PropertyScaling = new Property<Vector3>();
            PropertyRotation = new Property<Vector3>();
            PropertyRotateBeforeTranslate = new Property<bool>();
            PropertyRotateBeforeTranslate.SetSilent(true);
            
            _matrix = Matrix4.Identity;
            _rotationMatrix = Matrix4.Identity;
            _translationMatrix = Matrix4.Identity;
            _scalingMatrix = Matrix4.Identity;

            // property changed handlers
            PropertyMatrix.PropertyChanged += OnMatrixChanged;
            PropertyTranslation.PropertyChanged += OnTranslationChanged;
            PropertyScaling.PropertyChanged += OnScalingChanged;
            PropertyRotation.PropertyChanged += OnRotationChanged;
            PropertyRotateBeforeTranslate.PropertyChanged += OnRotateBeforeTranslateChanged;
        }

        private void OnMatrixChanged(object sender, PropertyChangedEventArgs e)
        {
            _matrix = (Matrix4) e.NewValue;
        }

        private void OnTranslationChanged(object sender, PropertyChangedEventArgs e)
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

        private void OnRotateBeforeTranslateChanged(object sender, PropertyChangedEventArgs e)
        {
            _rotationMatrixNeedsUpdate = true;
        }

        /// <summary>
        /// Updates the model matrix if necessary.
        /// </summary>
        public void UpdateModelMatrix()
        {
            if (!_rotationMatrixNeedsUpdate 
                && !_translationMatrixNeedsUpdate 
                && !_scalingMatrixNeedsUpdate) 
                return;
            
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
                _translationMatrix = Matrix4.CreateTranslation(Translation);
                _translationMatrixNeedsUpdate = false;
            }

            CalculateMatrix();
        }

        private void CalculateMatrix()
        {
            if(RotateBeforeTranslate)
                Matrix = _scalingMatrix * _rotationMatrix * _translationMatrix;
            else
                Matrix = _scalingMatrix * _translationMatrix * _rotationMatrix;
        }
        
        // MOVEMENT HELPER METHODS
        
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

        public void AddTranslationX(float x)
        {
            Translation = new Vector3(Translation.X + x, Translation.Y, Translation.Z);
        }

        public void AddTranslationY(float y)
        {
            Translation = new Vector3(Translation.X, Translation.Y + y, Translation.Z);
        }

        public void AddTranslationZ(float z)
        {
            Translation = new Vector3(Translation.X, Translation.Y, Translation.Z + z);
        }

        public void SetTranslationX(float x)
        {
            Translation = new Vector3(x, Translation.Y, Translation.Z);
        }

        public void SetTranslationY(float y)
        {
            Translation = new Vector3(Translation.X, y, Translation.Z);
        }

        public void SetTranslationZ(float z)
        {
            Translation = new Vector3(Translation.X, Translation.Y, z);
        }
    }
}