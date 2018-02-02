using System.Collections.Generic;
using Medja.Controls;

namespace Medja.Layers.Layouting
{
    public interface ILayout
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">The current controls (this) state.</param>
        /// <returns></returns>
        IEnumerable<ControlState> GetLayout(ControlState state);
    }
}