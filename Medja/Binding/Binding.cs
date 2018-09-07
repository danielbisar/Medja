using System;

namespace Medja
{
    public class Binding<TTarget, TSource> : IDisposable
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

        public void Dispose()
        {
            _source.PropertyChanged -= OnSourcePropertyChanged;
            _source = null;
            _target = null;
            _sourceConverter = null;
        }
    }
}
