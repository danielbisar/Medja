using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Medja.Binding.Reflection
{
    public static class ExpressionHelper
    {
        public static Expression<Action<TTarget, TSource>> SetPropFromProp<TTarget, TSource>(
            TTarget target, string targetPropertyName,
            TSource source, string sourcePropertyName)
        {
            // (p1, p2) => p1.Value = p2.Value
            var targetP = Expression.Parameter(typeof(TTarget));
            var sourceP = Expression.Parameter(typeof(TSource));

            MemberExpression sourceProp;

            // means the user specified TSource manually, so we need to do a convert
            if(typeof(TSource) != source.GetType())
                sourceProp = Expression.Property(Expression.Convert(sourceP, source.GetType()), sourcePropertyName);
            else
                sourceProp = Expression.Property(sourceP, sourcePropertyName);

            MemberExpression targetProp;

            if (typeof(TTarget) != target.GetType())
                targetProp = Expression.Property(Expression.Convert(targetP, target.GetType()), targetPropertyName);
            else
                targetProp = Expression.Property(targetP, targetPropertyName);

            Expression assign;

            var sourcePropertyInfo = (PropertyInfo)sourceProp.Member;
            var targetPropertyInfo = (PropertyInfo)targetProp.Member;

            if (sourcePropertyInfo.PropertyType != targetPropertyInfo.PropertyType)
                assign = Expression.Assign(targetProp, Expression.Convert(sourceProp, targetPropertyInfo.PropertyType));
            else
                assign = Expression.Assign(targetProp, sourceProp);
            
            return Expression.Lambda<Action<TTarget, TSource>>(assign, targetP, sourceP);
        }
    }

}
