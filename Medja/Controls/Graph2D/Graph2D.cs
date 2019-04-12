using System.Collections.Generic;
using System.Linq;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    /// <summary>
    /// A 2D graph control that allows different usage scenarios
    /// - big data: put in you x,y data, the graph will downsample and display it
    /// </summary>
    public class Graph2D : Control
    {
        public Graph2DAxis AxisX { get; }
        public Graph2DAxis AxisY { get; }
        
        
        
        
        // idea: if necessary reuse point objects
        // idea: use a type of list that adds points ordered by x so that max and min methods could operate faster?
        // do not use ObservableCollection for performance reasons
        
        public DataPoints DataPoints { get; }

        // todo remove or abstract
        public int RenderMode {get; set;}
        public float MinDistance {get;set;}
        public float SlopThreshold {get;set;}
        
        public Graph2D(IControlFactory controlFactory)
        {
            AxisX = controlFactory.Create<Graph2DAxis>();
            AxisY = controlFactory.Create<Graph2DAxis>();
            
            DataPoints = new DataPoints();

            MinDistance = 3.0f;
            SlopThreshold = 0.2f;
        }

        /// <summary>
        /// Sets the data points to display. Replaces all existing points.
        /// </summary>
        /// <param name="points">An enumerable of data points.</param>
        /// <param name="isSorted">If true the function assumes the point are already sorted by their x values
        /// ascending.</param>
        public void SetDataPoints(IEnumerable<Point> points, bool isSorted)
        {
            // todo check performance of order by compared to other possibilities like list.sort
            var sortedDataPoints = isSorted ? points : points.OrderBy(p => p.X);
            
            DataPoints.Clear();
            DataPoints.AddRange(sortedDataPoints);
            
            // todo adjust axis
        }
    }
}