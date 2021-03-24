using System;

namespace Medja.Properties.ChangeTracking.Changes
{
    [Serializable]
    public abstract class PropertyChange
    {
        public string Name { get; set; }

        protected PropertyChange(string name)
        {
            Name = name;
        }
    }
}
