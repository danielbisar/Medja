using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Defines an Orthographic camera.
    /// </summary>
    public class GLOrthographicCamera : GLCamera
    {
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

            _projectionMatrixNeedsUpdate = true;
        }

        private void OnProjectionMatrixPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _projectionMatrixNeedsUpdate = true;
        }

        public override void UpdateProjectionMatrix()
        {
            base.UpdateProjectionMatrix();
            ProjectionMatrix = Matrix4.CreateOrthographic(Width, Height, ZNear, ZFar);
        }

        protected override string ToStringSubClass()
        {
            return " ortho, width=" + Width + ", height=" + Height;
        }
    }
}
