using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public class GLModel : GLComponent, IViewProjectionMatrix
    {
        public ModelMatrix ModelMatrix { get; }

        /// <summary>
        /// this field is necessary to allow passing the matrix as ref to a GL.Uniform
        /// do not set this field directly from outside this class.
        /// </summary>
        protected Matrix4 _viewProjectionMatrix;
        
        [NonSerialized]
        public readonly Property<Matrix4> PropertyViewProjectionMatrix;
        public Matrix4 ViewProjectionMatrix
        {
            get { return PropertyViewProjectionMatrix.Get(); }
            set { PropertyViewProjectionMatrix.Set(value); }
        }

        public GLModel()
        {
            ModelMatrix = new ModelMatrix();
            PropertyViewProjectionMatrix = new Property<Matrix4>();
            PropertyViewProjectionMatrix.PropertyChanged += OnViewProjectionMatrixChanged;
        }

        private void OnViewProjectionMatrixChanged(object sender, PropertyChangedEventArgs e)
        {
            _viewProjectionMatrix = ViewProjectionMatrix;
        }

        public override void Render()
        {
            ModelMatrix.UpdateModelMatrix();
        }
    }
}