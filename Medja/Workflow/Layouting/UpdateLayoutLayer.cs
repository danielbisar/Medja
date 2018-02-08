using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Medja.Layers.Layouting;
using Medja.Primitives;

namespace Medja.Layouting
{
    public class UpdateLayoutLayer : ILayer
    {
        public WorkflowState WorkflowState { get; set; }
        
        public void Execute()
        {
            foreach(var control in WorkflowState.Controls)
            {
                if(control.Control is LayoutControl lc)
                {
                    // TODO handle empty size and use WorkflowState.TargetRenderSize
                    var availableSize = new Size(control.Position.Width, control.Position.Height);
                    var result = lc.Measure(availableSize);
                    lc.Arrange(new Point(control.Position.X, control.Position.Y), result);

                    Debug.WriteLine("Found layout control: " + lc.GetType().Name);
                    Debug.WriteLine("AvailableSize: " + availableSize);
                    Debug.WriteLine("After measure: " + result);
                }
            }
        }
    }
}
