using System;
using System.Collections.Generic;
using System.Text;
using Medja.Binding.PropertyAccessors;

namespace Medja.Binding
{
    public static class BindingLibrary
    {

        public static IPropertyAccessorFactory PropertyAccessorFactory { get; set; }
        public static IValueFactory ValueFactory { get; set; }

        static BindingLibrary()
        {
            //PropertyAccessorFactory = new PropertyInfoPropertyAccessorFactory();
            PropertyAccessorFactory = new ExpressionPropertyAccessorsFactory();
            ValueFactory = new DefaultValueFactory();
        }
    }
}
