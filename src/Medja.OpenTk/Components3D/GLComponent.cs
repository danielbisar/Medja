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

        [NonSerialized] 
        public readonly Property<float> PropertyScale;

        public float Scale
        {
            get { return PropertyScale.Get(); }
            set { PropertyScale.Set(value); }
        }

        protected GLComponent()
        {
            PropertyPosition = new Property<Vector3>();
            PropertyRotation = new Property<Vector3>();
            PropertyScale = new Property<float>();
            PropertyScale.UnnotifiedSet(1.0f);
        }
        
        public abstract void Render();
    }
}