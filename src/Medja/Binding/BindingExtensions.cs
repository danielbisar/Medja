using System;
using System.Linq;
using System.Reflection;
using Medja.Controls;
using Medja.Utils.Reflection;

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

        /// <summary>
        /// Tells the control that the layout needs to be updated every time given property changes.
        /// </summary>
        /// <param name="property">The property that affects the layout.</param>
        /// <param name="control">The control that is affected by the layout. This should be the control
        /// containing the property.</param>
        /// <typeparam name="T">The properties value type.</typeparam>
        public static MarkLayoutDirtyHelper<T> AffectsLayout<T>(this Property<T> property, Control control)
        {
            return new MarkLayoutDirtyHelper<T>(control, property);
        }

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
                this Property<TTarget> targetProperty, TSourceObject obj, Action<TSourceObject, TSource> setSourceValue,
                Func<TSourceObject, TSource> getSourceValue, Func<TSource, TTarget> sourceConverter,
                Func<TTarget, TSource> targetConverter)
        {
            var wrapper = new PropertyWrapper<TSourceObject, TSource>(obj, setSourceValue, getSourceValue);
            targetProperty.Set(sourceConverter(getSourceValue(obj)));
            
            return new TwoWayBinding<TTarget, TSource>(targetProperty, wrapper.PropertyValue, sourceConverter, targetConverter);
        }

        public static void OnAnyPropertyChanged(this object medjaObject, Action action)
        {
            if(medjaObject == null)
                throw new ArgumentNullException(nameof(medjaObject));
            
            if(action == null)
                throw new ArgumentNullException(nameof(action));
            
            var type = medjaObject.GetType();
            var allFields = type.GetFields();
            var medjaPropertyFields = allFields.Where(p => p.FieldType.Implements(typeof(IProperty)))
                .ToList();
            
            if(medjaPropertyFields.Count == 0)
                throw new InvalidOperationException("No field of type IProperty found on object of type " + type.Name);

            foreach (var field in medjaPropertyFields)
            {
                (field.GetValue(medjaObject) as IProperty).PropertyChanged += (s, e) => action();
            }
        }
    }
}