using System;
using System.Collections.Generic;
using System.Text;

namespace Medja
{
    /// <summary>
    /// Base class for Medja objects
    /// </summary>
    public class MObject
    {
        protected virtual Property<T> AddProperty<T>()
        {
            return new Property<T>();
        }
    }
}
