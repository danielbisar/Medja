using System;
using Medja.Properties;

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
        private bool _isDisposed;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="target">The target property (will be updated on change of source)</param>
        /// <param name="source">The source property (updates the target on change)</param>
        /// <param name="sourceConverter">Function that converts the source value to the target value. Default p => p.</param>
        public Binding(Property<TTarget> target, Property<TSource> source, Func<TSource, TTarget> sourceConverter)
        {
            _isDisposed = false;
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _source.PropertyChanged += OnSourcePropertyChanged;
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _sourceConverter = sourceConverter ?? throw new ArgumentNullException(nameof(sourceConverter));
        }        

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            _target.Set(_sourceConverter(_source.Get()));
        }

        /// <summary>
        /// Clears all references and event handlers hold.
        /// </summary>
        public override void Dispose()
        {
            if (_isDisposed) 
                return;
            
            _isDisposed = true;
            _source.PropertyChanged -= OnSourcePropertyChanged;
            _source = null;
            _target = null;
            _sourceConverter = null;
        }
    }
}
