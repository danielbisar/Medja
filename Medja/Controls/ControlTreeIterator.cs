using System.Collections;
using System.Collections.Generic;

namespace Medja.Controls
{
    public static class ControlTreeIterator
    {
        /// <summary>
        /// Gets all controls including the root and all it's children as IEnumerable.
        /// </summary>
        /// <param name="root">The control to start at.</param>
        /// <returns></returns>
        public static IEnumerable<Control> GetAllControls(this Control root)
        {
            yield return root;

            var children = root.GetChildren();

            foreach (var child in children)
            {
                //yield return child;
                
                foreach (var control in GetAllControls(child))
                    yield return control;
            }
        }
    }
}