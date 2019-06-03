using System;
using Medja.Properties;

namespace Medja
{
    /// <summary>
    /// Represents a binding that forwards changes in both directions (source -> target, target -> source)
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    public class TwoWayBinding<TTarget, TSource> : BindingBase<TTarget, TSource>
    {
        private readonly Binding<TTarget, TSource> _sourceToTargetBinding;
        private readonly Binding<TSource, TTarget> _targetToSourceBinding;

        public TwoWayBinding(Property<TTarget> target, Property<TSource> source, Func<TSource, TTarget> sourceConverter, Func<TTarget, TSource> targetConverter)
        {
            _sourceToTargetBinding = new Binding<TTarget, TSource>(target, source, sourceConverter);
            _targetToSourceBinding = new Binding<TSource, TTarget>(source, target, targetConverter);
        }
        
        public override void Dispose()
        {
            _sourceToTargetBinding.Dispose();
            _targetToSourceBinding.Dispose();
        }
    }
}