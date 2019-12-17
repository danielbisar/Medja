using System;

namespace Medja.Properties.Binding
{
    public static class MultiBindingFactories
    {
        /// <summary>
        /// Updates the target property based on changes of the source properties.
        /// </summary>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="source1">The first source.</param>
        /// <param name="source2">The second source.</param>
        /// <param name="converter">The converter.</param>
        /// <typeparam name="TTarget">Target property type.</typeparam>
        /// <typeparam name="TSource1">Source 1 property type.</typeparam>
        /// <typeparam name="TSource2">Source 2 property type.</typeparam>
        /// <returns></returns>
        public static MultiBinding<TTarget> UpdateFrom<TTarget, TSource1, TSource2>(
            this Property<TTarget> targetProperty,
            Property<TSource1> source1,
            Property<TSource2> source2,
            Func<TSource1, TSource2, TTarget> converter)
        {
            var result = new MultiBinding<TTarget>(targetProperty, 
                properties => 
                    converter(((Property<TSource1>) properties[0]).Get(), 
                    ((Property<TSource2>) properties[1]).Get()));

            result.AddSource(source1);
            result.AddSource(source2);

            result.Update();
            
            return result;
        }
    }
}