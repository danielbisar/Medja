using System;
using Medja.Properties;

namespace Medja
{
    /// <summary>
    /// Represents the connection of a source property to a target property.
    /// </summary>
    /// <typeparam name="TTarget">The target value type.</typeparam>
    /// <typeparam name="TSource">The source value type.</typeparam>
    /// <remarks>Dispose this object to unregister the binding.</remarks>
    public class Binding<TTarget, TSource> : IBinding
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
        /// <remarks>
        /// The binding does not initialize the target property. Means the target property gets updated only on the
        /// first change of the source property. If you need a different behavior use
        /// <see cref="BindingExtensions.BindTo{TTarget,TSource}"/>.
        /// </remarks>
        public Binding(Property<TTarget> target, 
                       Property<TSource> source, 
                       Func<TSource, TTarget> sourceConverter)
        {
            _isDisposed = false;
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _source.PropertyChanged += OnSourcePropertyChanged;
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _sourceConverter = sourceConverter ?? throw new ArgumentNullException(nameof(sourceConverter));
        }        

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            Update();
        }

        /// <summary>
        /// Reevaluates the target value.
        /// </summary>
        public void Update()
        {
            _target.Set(_sourceConverter(_source.Get()));
        }

        /// <summary>
        /// Clears all references and event handlers.
        /// </summary>
        public void Dispose()
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
