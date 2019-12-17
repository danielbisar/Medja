using System;

namespace Medja.Properties.Binding
{
    /// <summary>
    /// Represents a binding that forwards changes in both directions (source -> target, target -> source)
    /// </summary>
    /// <typeparam name="TTarget">The target value type.</typeparam>
    /// <typeparam name="TSource">The source value type.</typeparam>
    /// <remarks>Dispose this object to unregister the binding.</remarks>
    public class TwoWayBinding<TTarget, TSource> : IBinding
    {
        private readonly Binding<TTarget, TSource> _sourceToTargetBinding;
        private readonly Binding<TSource, TTarget> _targetToSourceBinding;

        public TwoWayBinding(Property<TTarget> target, 
                             Property<TSource> source, 
                             Func<TSource, TTarget> sourceConverter, 
                             Func<TTarget, TSource> targetConverter)
        {
            _sourceToTargetBinding = new Binding<TTarget, TSource>(target, source, sourceConverter);
            _targetToSourceBinding = new Binding<TSource, TTarget>(source, target, targetConverter);
        }

        public void Update()
        {
            _sourceToTargetBinding.Update();
        }

        public void UpdateFromTarget()
        {
            _targetToSourceBinding.Update();
        }
        
        public void Dispose()
        {
            _sourceToTargetBinding.Dispose();
            _targetToSourceBinding.Dispose();
        }
    }
}