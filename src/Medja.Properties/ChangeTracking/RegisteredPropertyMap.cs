using System;
using System.Collections.Generic;
using System.Linq;

namespace Medja.Properties.ChangeTracking
{
    internal class RegisteredPropertyMap : IDisposable
    {
        private readonly Dictionary<string, RegisteredProperty> _properties;
        private readonly Dictionary<IProperty, string> _names;

        public RegisteredPropertyMap()
        {
            _properties = new Dictionary<string, RegisteredProperty>();
            _names = new Dictionary<IProperty, string>();
        }

        public void Add(RegisteredProperty property)
        {
            _properties.Add(property.Name, property);
            _names.Add(property.Property, property.Name);
        }

        public RegisteredProperty GetByName(string name)
        {
            return _properties[name];
        }

        public string GetName(IProperty property)
        {
            return _names[property];
        }

        public IEnumerable<IProperty> GetProperties()
        {
            return _properties.Values.Select(p => p.Property);
        }

        public IEnumerable<RegisteredProperty> GetRegisteredProperties()
        {
            return _properties.Values;
        }

        public void Dispose()
        {
            foreach(var property in _properties.Values)
                property.Dispose();
            
            _properties.Clear();
            _names.Clear();
        }
    }
}