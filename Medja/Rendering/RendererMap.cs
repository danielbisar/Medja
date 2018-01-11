using System;
using System.Collections.Generic;
using Medja.Controls.Panels;

namespace Medja.Rendering
{
    /// <summary>
    /// Represents a RendererMap. Use <see cref="RendererFactory"/> wherever you need a renderer.
    /// </summary>
    public class RendererMap : Dictionary<Type, IControlRenderer>
    {
        public RendererMap()
        {
            // register the default renderers (which are platform indipendent)
            Add(typeof(StackPanel), new StackPanelRenderer());
        }

        /// <summary>
        /// Finds a renderer for the given type. If it does not exist the function tries to find a renderer for any of the baseclasses of the given type. If this also is not found an empty renderer will be returned (not null).
        /// </summary>
        /// <param name="type">The type you need a renderer for.</param>
        /// <returns>The IControlRenderer.</returns>
        public IControlRenderer Get(Type type)
        {
            IControlRenderer result;

            if (TryGetValue(type, out result))
                return result;

            ResolveBaseClassRenderer(type);

            // here we will have a renderer, even if it is an empty one.
            return this[type];
        }

        /// <summary>
        /// Searches if there is a renderer for any of the baseclasses of type and adds it as renderer for type.
        /// </summary>
        /// <param name="type">The type to find a renderer for.</param>
        internal void ResolveBaseClassRenderer(Type type)
        {
            var baseType = type.BaseType;

            while (baseType != null && baseType != typeof(object))
            {
                if (TryGetValue(baseType, out var baseTypeRenderer))
                {
                    // add the renderer which is actually for a base type 
                    // as renderer for the requested type, so it will be found
                    // next time.
                    Add(type, baseTypeRenderer);
                    return;
                }

                baseType = baseType.BaseType;
            }

            Add(type, EmptyControlRenderer.Default);
        }
    }
}
