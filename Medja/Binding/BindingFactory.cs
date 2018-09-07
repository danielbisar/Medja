using System;

namespace Medja
{
    public static class BindingFactory
    {
        public static Binding<T, T> Create<T>(Property<T> target, Property<T> source)
        {
            return new Binding<T, T>(target, source, p => p);
        }

        public static Binding<TTarget, TSource> Create<TTarget, TSource>(Property<TTarget> target, Property<TSource> source, Func<TSource, TTarget> converter)
        {
            return new Binding<TTarget, TSource>(target, source, converter);
        }
    }
}