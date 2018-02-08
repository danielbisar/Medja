using System.Collections.Generic;

namespace Medja
{
    public interface ILayer
    {
        WorkflowState WorkflowState { get; set; }

        void Execute();
    }
}
