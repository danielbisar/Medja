using System;

namespace Medja.Properties.Binding
{
    /// <summary>
    /// Extension methods to create bindings.
    /// </summary>
    public static class BindingExtensions
    {
        /// <summary>
        /// Creates a new TwoWayBinding.
        /// </summary>
        public static TwoWayBinding<TTarget, TSource> BindTwoWay<TTarget, TSource>(this Property<TTarget> targetProperty,
                                                        Property<TSource> sourceProperty,
                                                        Func<TSource, TTarget> sourceConverter,
                                                        Func<TTarget, TSource> targetConverter)
        {
            return new TwoWayBinding<TTarget, TSource>(targetProperty, sourceProperty, sourceConverter,
                                                       targetConverter);
        }

        public static TwoWayBinding<TTarget, TSource> BindTwoWay<TTarget, TSourceObject, TSource>(
                this Property<TTarget> targetProperty, 
                TSourceObject obj, 
                Action<TSourceObject, TSource> setSourceValue,
                Func<TSourceObject, TSource> getSourceValue, 
                Func<TSource, TTarget> sourceConverter,
                Func<TTarget, TSource> targetConverter)
        {
            var wrapper = new PropertyWrapper<TSourceObject, TSource>(obj, getSourceValue, setSourceValue);
            targetProperty.Set(sourceConverter(getSourceValue(obj)));
            
            return new TwoWayBinding<TTarget, TSource>(targetProperty, wrapper, sourceConverter, targetConverter);
        }

        public static TwoWayBinding<T, T> BindTwoWay<T>(this Property<T> targetProperty, Property<T> sourceProperty)
        {
            return new TwoWayBinding<T, T>(targetProperty, sourceProperty, p => p, p => p);
        }
        
        // UpdateFrom
        public static Binding<TValue, TValue> UpdateFrom<TValue>(this Property<TValue> targetProperty,
            Property<TValue> sourceProperty)
        {
            var result = new Binding<TValue, TValue>(targetProperty, sourceProperty, p => p);
            result.Update();

            return result;
        }
        
        public static Binding<TTarget, TSource> UpdateFrom<TTarget, TSource>(
            this Property<TTarget> targetProperty,
            Property<TSource> sourceProperty,
            Func<TSource, TTarget> converter)
        {
            var result = new Binding<TTarget,TSource>(targetProperty, sourceProperty, converter);
            result.Update();

            return result;
        }
    }
}