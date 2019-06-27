using System;
using Medja.Properties;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public abstract class GLComponent
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

        protected GLComponent()
        {
            PropertyPosition = new Property<Vector3>();
            PropertyRotation = new Property<Vector3>();
        }
        
        public abstract void Render();
    }
}