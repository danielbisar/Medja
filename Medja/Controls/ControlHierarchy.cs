using System.Collections.Generic;

namespace Medja.Controls
{
    public class ControlHierarchy
    {
        private readonly Control _rootControl;
        private readonly Queue<Control> _topMostQueue;
        
        /// <summary>
        /// Top most in the sense of control tree (not rendering)
        /// </summary>
        public Control TopMostControlNeedingLayoutPass { get; private set; } 

        public ControlHierarchy(Control rootControl)
        {
            _rootControl = rootControl;
            _topMostQueue = new Queue<Control>();
        }

        /// <summary>
        /// Get all controls in rendering order.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Control> GetInRenderingOrder()
        {
            TopMostControlNeedingLayoutPass = null;

            foreach (var child in GetAllChildren(_rootControl))
                yield return child;

            while (_topMostQueue.Count > 0)
            {
                yield return _topMostQueue.Dequeue();
            }
        }

        public void UpdateLayout()
        {
            if(TopMostControlNeedingLayoutPass != null)
                TopMostControlNeedingLayoutPass.UpdateLayout();
        }

        /// <summary>
        /// Gets all child controls (deep iteration)
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private IEnumerable<Control> GetAllChildren(Control control, bool isTopMost = false)
        {
            if (TopMostControlNeedingLayoutPass == null && !control.IsLayoutUpdated)
                TopMostControlNeedingLayoutPass = control;

            if (control.IsTopMost || isTopMost)
            {
                _topMostQueue.Enqueue(control);
                isTopMost = true;
            }
            else
            {
                if(control.IsVisible)
                    yield return control;
                else
                    yield break;
            }

            var children = control.GetChildren();

            foreach (var child in children)
            {
                foreach (var subChild in GetAllChildren(child, isTopMost))
                    yield return subChild;
            }
        }
    }
}