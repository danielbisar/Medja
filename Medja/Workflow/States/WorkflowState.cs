using System.Collections.Generic;
using Medja.Primitives;

namespace Medja
{
    public class WorkflowState
    {
        public List<ControlState> Controls { get; }
        public Size RenderTargetSize { get; set; }

        public WorkflowState()
        {
            Controls = new List<ControlState>();
        }
    }
}