using Medja.Controls;
using Medja.Utils.Threading.Tasks;

namespace Medja.Utils
{
    /// <summary>
    /// Helps to get the TaskQueue if you need it in a control.
    /// </summary>
    /// <remarks>
    /// This class can be extended if more scenarios are required. Currently it just searches for the task queue
    /// until the control is added inside a control to a window. If you move that control from one window to another
    /// it will not work.
    /// </remarks>
    public class TaskQueueFinder
    {
        private readonly Control _control;
        private TaskQueue<object> _taskQueue;
        
        /// <summary>
        /// Gets the task queue if it could be found.
        /// </summary>
        public TaskQueue<object> TaskQueue
        {
            get
            {
                if(_taskQueue == null)
                    FindTaskQueue();
                
                return _taskQueue;
            }
        }

        public TaskQueueFinder(Control control)
        {
            _control = control;
        }

        private void FindTaskQueue()
        {
            var window = _control.GetRootControl() as Window;
            
            if(window == null)
                return;
            
            _taskQueue = window.TaskQueue;
        }
    }
}