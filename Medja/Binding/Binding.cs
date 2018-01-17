using System;
using System.ComponentModel;
using Medja.Binding.PropertyAccessors;
using Medja.Binding.Reflection;

namespace Medja.Binding
{
    /// <summary>
    /// Not thread-safe.
    /// </summary>
    public class Binding
    {
        private readonly INotifyPropertyChanged _source;
        private readonly BoundProperty _sourceProperty;

        private readonly INotifyPropertyChanged _target;
        private readonly BoundProperty _targetProperty;

        private readonly BindingMode _mode;
        private bool _selfUpdated;

        private Action<object, object> updateTarget;
        private Action<object, object> updateSource;

        public Binding(INotifyPropertyChanged source, string sourcePropertyName, INotifyPropertyChanged target, string targetPropertyName, BindingMode mode)
        {
            // currently under evaluation: use property accessor or update methods
            // binding creation time vs memory usage vs access time

            _source = source;
            _sourceProperty = new BoundProperty(source.GetType(), sourcePropertyName);
            _target = target;
            _targetProperty = new BoundProperty(target.GetType(), targetPropertyName);
            _mode = mode;

            AttachEventHandler();
            updateTarget = ExpressionHelper.SetPropFromProp<object, object>(_target, targetPropertyName, _source, sourcePropertyName).Compile();
            updateSource = ExpressionHelper.SetPropFromProp<object, object>(_source, sourcePropertyName, _target, targetPropertyName).Compile();
        }        

        private void AttachEventHandler()
        {
            _source.PropertyChanged += OnSourcePropertyChanged;

            if (_mode == BindingMode.TwoWay)
                _target.PropertyChanged += OnTargetPropertyChanged;
        }

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_selfUpdated)
                return;

            if (e.PropertyName == _sourceProperty.PropertyName)
                UpdateTarget();
        }
        
        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_selfUpdated)
                return;

            if (e.PropertyName == _targetProperty.PropertyName)
                UpdateSource();
        }

        

        private void UpdateTarget()
        {
            _selfUpdated = true;
            updateTarget(_target, _source);
            _selfUpdated = false;
            //Update(_targetProperty.Accessors.Value, _target, _sourceProperty.Accessors.Value, _source);
        }

        private void UpdateSource()
        {
            _selfUpdated = true;
            updateSource(_source, _target);
            _selfUpdated = false;

            //Update(_sourceProperty.Accessors.Value, _source, _targetProperty.Accessors.Value, _target);
        }

        //private void Update(PropertyAccessor targetAccessor, object target, PropertyAccessor sourceAccessor, object source)
        //{
        //    _selfUpdated = true;
        //    // todo converter
        //    targetAccessor.Set(target, sourceAccessor.Get(source));
        //    _selfUpdated = false;
        //}
    }
}
