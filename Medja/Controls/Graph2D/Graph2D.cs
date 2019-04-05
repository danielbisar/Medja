using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    public class Graph2D : Control
    {
        // idea: if necessary reuse point objects
        // idea: use a type of list that adds points ordered by x so that max and min methods could operate faster?
        // do not use ObservableCollection for performance reasons
        
        public DataPoints DataPoints { get; }
        
        public Graph2D()
        {
            DataPoints = new DataPoints();
        }
    }
}