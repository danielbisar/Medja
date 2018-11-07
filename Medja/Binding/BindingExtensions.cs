using System;

namespace Medja
{
    public static class BindingExtensions
    {
        /// <summary>
        /// Binds the give source property to the given target. Also initializes the initial value.
        /// </summary>
        /// <param name="target">The property that should be updated on change of source.</param>
        /// <param name="source">The source property (meaning providing the value changes)</param>
        /// <param name="converter">A function that converts values from TSource to TTarget.</param>
        /// <typeparam name="TTarget">The targets value type.</typeparam>
        /// <typeparam name="TSource">The sources value type.</typeparam>
        /// <returns>The binding object (can be ignored).</returns>
        public static Binding<TTarget, TSource> BindTo<TTarget, TSource>(this Property<TTarget> target, Property<TSource> source,
                                                    Func<TSource, TTarget> converter)
        {
            target.Set(converter(source.Get()));
            return BindToWithoutInit(target, source, converter);
        }
        
        /// <summary>
        /// Binds the give source property to the given target. Also initializes the initial value.
        /// </summary>
        /// <param name="target">The property that should be updated on change of source.</param>
        /// <param name="source">The source property (meaning providing the value changes)</param>
        /// <typeparam name="TValue">The value type of the source and target property.</typeparam>
        /// <returns>The binding object (can be ignored).</returns>
        public static Binding<TValue, TValue> BindTo<TValue>(this Property<TValue> target, Property<TValue> source)
        {
            target.Set(source.Get());
            return BindToWithoutInit(target, source);
        }
        
        /// <summary>
        /// Same as BindTo but does not read the initial value from source.
        /// </summary>
        /// <param name="target">The property that should be updated on change of source.</param>
        /// <param name="source">The source property (meaning providing the value changes)</param>
        /// <typeparam name="TValue">The value type of the source and target property.</typeparam>
        /// <returns>The binding object (can be ignored).</returns>
        public static Binding<TValue, TValue> BindToWithoutInit<TValue>(this Property<TValue> target, Property<TValue> source)
        {
            return new Binding<TValue, TValue>(target, source, p => p);
        }

        /// <summary>
        /// Same as BindTo but does not read the initial value from source.
        /// </summary>
        /// <param name="target">The property that should be updated on change of source.</param>
        /// <param name="source">The source property (meaning providing the value changes)</param>
        /// <param name="converter">A function that converts values from TSource to TTarget.</param>
        /// <typeparam name="TTarget">The targets value type.</typeparam>
        /// <typeparam name="TSource">The sources value type.</typeparam>
        /// <returns>The binding object (can be ignored).</returns>
        public static Binding<TTarget, TSource> BindToWithoutInit<TTarget, TSource>(this Property<TTarget> target, Property<TSource> source,
                                                                         Func<TSource, TTarget> converter)
        {
            return new Binding<TTarget, TSource>(target, source, converter);
        }
    }
}