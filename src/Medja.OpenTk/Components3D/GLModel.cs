using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public class GLModel : GLComponent
    {
        private Vector3 _rotation;
        private Vector3 _translation;

        /// <summary>
        /// Gets the model matrix (contains position, rotation, ... of the model)
        /// </summary>
        public Matrix4 Matrix { get; set; }

        private Matrix4 _rotationMatrix;
        private bool _isRotationChanged;
        public Vector3 Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _isRotationChanged = true;
            }
        }

        private bool _isTranslationChanged;
        private Matrix4 _translationMatrix;
        public Vector3 Translation
        {
            get { return _translation; }
            set
            {
                _translation = value;
                _isTranslationChanged = true;
            }
        }

        public GLModel()
        {
            Matrix = Matrix4.Identity;
            _rotationMatrix = Matrix4.Identity;
            _translationMatrix = Matrix4.Identity;
        }
        
        public override void Render()
        {
            if (_isRotationChanged || _isTranslationChanged)
            {
                if (_isRotationChanged)
                {
                    _rotationMatrix = CreateRotation(Rotation);
                    _isRotationChanged = false;
                }

                if (_isTranslationChanged)
                {
                    _translationMatrix = Matrix4.CreateTranslation(Translation);
                    _isTranslationChanged = false;
                }

                Matrix = Matrix4.CreateTranslation(Translation) * CreateRotation(Rotation);
            }
        }

        /// <summary>
        /// Creates the rotation matrix.
        /// </summary>
        /// <returns>The rotation matrix.</returns>
        private Matrix4 CreateRotation(Vector3 rotation)
        {
            var result = Matrix4.Identity;

            result *= Matrix4.CreateRotationX(rotation.X);
            result *= Matrix4.CreateRotationY(rotation.Y);
            result *= Matrix4.CreateRotationZ(rotation.Z);

            return result;
        }
    }
}