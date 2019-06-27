using System;

namespace Medja.Properties
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