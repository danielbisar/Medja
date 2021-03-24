using System;
using Medja.Primitives;

namespace Medja.OpenTk.Rendering
{
    public class GlContextActions
    {
        public Action OnInit;
        public Action<RectInt> OnResize;

        /// <summary>
        /// Returns true if anything was drawn, else false.
        /// </summary>
        public Func<bool> OnRender;
    }
}
