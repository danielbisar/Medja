using System;

namespace Medja.Properties
{
    
    public class PropertyContainer
    {
        private readonly IProperty _property;
        private readonly Func<IProperty, object> _get;
        private readonly Action<IProperty, object> _set;
        
        public IProperty Property
        {
            get { return _property; }
        }

        public PropertyContainer(IProperty property, Func<IProperty, object> get, Action<IProperty, object> set)
        {
            _property = property;
            
            _get = get ?? throw new ArgumentNullException(nameof(get));
            _set = set ?? throw new ArgumentNullException(nameof(set));
        }

        public object Get()
        {
            return _get(_property);
        }

        public void Set(object value)
        {
            _set(_property, value);
        }
    }
}