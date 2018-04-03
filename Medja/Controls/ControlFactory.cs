using System;
using System.Collections.Generic;
using System.Text;
using Medja.Primitives;

namespace Medja.Controls
{
    public class ControlFactory
    {
        // is used so we don't need reflection
        private readonly Dictionary<Type, Func<object>> _factoryMethods;

        public ControlFactory()
        {
            _factoryMethods = new Dictionary<Type, Func<object>>();
            _factoryMethods.Add(typeof(Button), CreateButton);
        }

        protected virtual Button CreateButton()
        {
            return new Button();
        }

        public T Create<T>()
            where T : Control
        {
            return (T)_factoryMethods[typeof(T)]();
        }

        public T Create<T>(Action<T> applyCustomStyle)
            where T : Control
        {
            var result = Create<T>();
            applyCustomStyle(result);

            return result;
        }
    }
}
