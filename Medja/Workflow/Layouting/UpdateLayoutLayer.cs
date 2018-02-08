using System;
using System.Collections.Generic;
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
                    var result = lc.Measure(WorkflowState.RenderTargetSize);
                    lc.Arrange(new Point(control.Position.X, control.Position.Y), result);
                }
            }
        }
    }
}
