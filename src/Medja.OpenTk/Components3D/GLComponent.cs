using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public abstract class GLComponent : IDisposable
    {
        [NonSerialized] 
        public readonly Property<Vector3> PropertyPosition;
        
        public Vector3 Position
        {
            get { return PropertyPosition.Get(); }
            set { PropertyPosition.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<Vector3> PropertyRotation;

        public Vector3 Rotation
        {
            get { return PropertyRotation.Get(); }
            set { PropertyRotation.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<Vector3> PropertyScale;

        public Vector3 Scale
        {
            get { return PropertyScale.Get(); }
            set { PropertyScale.Set(value); }
        }

        protected GLComponent()
        {
            PropertyPosition = new Property<Vector3>();
            PropertyRotation = new Property<Vector3>();
            PropertyScale = new Property<Vector3>();
            PropertyScale.SetSilent(new Vector3(1));
        }
        
        public abstract void Render();

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}