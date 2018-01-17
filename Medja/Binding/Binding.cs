using System;
using System.ComponentModel;
using Medja.Binding.Reflection;

namespace Medja.Binding
{
    /// <summary>
    /// Not thread-safe.
    /// </summary>
    public class Binding
    {
        private readonly INotifyPropertyChanged _source;
        private readonly string _sourcePropertyName;

        private readonly INotifyPropertyChanged _target;
        private readonly string _targetPropertyName;

        private readonly BindingMode _mode;
        private bool _selfUpdated;

        private Action<object, object> updateTarget;
        private Action<object, object> updateSource;

        public Binding(INotifyPropertyChanged source, string sourcePropertyName, INotifyPropertyChanged target, string targetPropertyName, BindingMode mode)
        {
            _source = source;
            _sourcePropertyName = sourcePropertyName;
            _target = target;
            _targetPropertyName = targetPropertyName;
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

            if (e.PropertyName == _sourcePropertyName)
                UpdateTarget();
        }
        
        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_selfUpdated)
                return;

            if (e.PropertyName == _targetPropertyName)
                UpdateSource();
        }       

        private void UpdateTarget()
        {
            _selfUpdated = true;
            updateTarget(_target, _source);
            _selfUpdated = false;
        }

        private void UpdateSource()
        {
            _selfUpdated = true;
            updateSource(_source, _target);
            _selfUpdated = false;            
        }
    }
}
