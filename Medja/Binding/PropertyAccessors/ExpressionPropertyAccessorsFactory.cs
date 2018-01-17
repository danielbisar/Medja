using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Medja.Binding.PropertyAccessors
{
    /// <summary>
    /// Uses dynamic methods to invoke property accessors.
    /// </summary>
    public class ExpressionPropertyAccessorsFactory : PropertyAccessorFactoryBase
    {
        protected override Func<object, object> CreateGetter(Type type, PropertyInfo propertyInfo)
        {
            var methodInfo = propertyInfo.GetGetMethod();

            // {p => Convert(Convert(p, TestObj1).Int1, Object)}
            var target = Expression.Parameter(typeof(object));
            var targetToType = Expression.Convert(target, type);
            var propertyExpr = Expression.Property(targetToType, propertyInfo);
            var propToObject = Expression.Convert(propertyExpr, typeof(object));

            return Expression.Lambda<Func<object, object>>(propToObject, target).Compile();
        }

        protected override Action<object, object> CreateSetter(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo methodInfo = propertyInfo.GetSetMethod();

            if (methodInfo != null && methodInfo.GetParameters().Length == 1)
            {
                // (p1, p2) => ((TestObj1)p1).Int1 = (int)p2;
                var target = Expression.Parameter(typeof(object));
                var value = Expression.Parameter(typeof(object));

                var targetToType = Expression.Convert(target, type);
                var valueToType = Expression.Convert(value, propertyInfo.PropertyType);
                var propertyExpr = Expression.Property(targetToType, propertyInfo);
                var assign = Expression.Assign(propertyExpr, valueToType);
                
                return Expression.Lambda<Action<object, object>>(assign, target, value).Compile();
            }

            throw new NotImplementedException();
        }
    }
}
