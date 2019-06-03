using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Medja.Properties;
using Medja.Utils.Reflection;

namespace Medja.Utils
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Get all fields implementing <see cref="IProperty"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The fields of that object implementing IProperty if any.</returns>
        /// <exception cref="ArgumentNullException">If obj == null.</exception>
        public static IEnumerable<FieldInfo> GetMedjaPropertyFields(this object obj)
        {
            if(obj == null)
                throw new ArgumentNullException(nameof(obj));
            
            var type = obj.GetType();
            var allFields = type.GetFields();
            
            return allFields.Where(p => p.FieldType.Implements(typeof(IProperty)));
        }
    }
}