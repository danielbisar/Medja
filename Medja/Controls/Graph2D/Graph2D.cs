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

        // todo remove or abstract
        public int RenderMode {get;set;}
        public float MinDistance {get;set;}
        public float SlopThreshold {get;set;}
        
        public Graph2D()
        {
            DataPoints = new DataPoints();

            MinDistance = 3.0f;
            SlopThreshold = 0.2f;
        }
    }
}