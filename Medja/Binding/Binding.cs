using System;

namespace Medja
{
    /// <summary>
    /// Represents the connection of one source property to a target property.
    /// </summary>
    /// <typeparam name="TTarget">The target value type.</typeparam>
    /// <typeparam name="TSource">The source value type.</typeparam>
    /// <remarks>Dispose the object to unregister the binding.</remarks>
    public class Binding<TTarget, TSource> : BindingBase<TTarget, TSource>
    {
        private Property<TTarget> _target;
        private Property<TSource> _source;
        private Func<TSource, TTarget> _sourceConverter;

        public Binding(Property<TTarget> target, Property<TSource> source, Func<TSource, TTarget> sourceConverter)
        {
            source.PropertyChanged += OnSourcePropertyChanged;
            _source = source;
            _target = target;
            _sourceConverter = sourceConverter;
        }        

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            _target.Set(_sourceConverter(_source.Get()));
        }

        public override void Dispose()
        {
            _source.PropertyChanged -= OnSourcePropertyChanged;
            _source = null;
            _target = null;
            _sourceConverter = null;
        }
    }
}
