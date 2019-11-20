namespace Medja.Controls
{
    public class ControlHierarchy
    {
        private readonly Control _rootControl;
        public ControlLists Lists { get; }
        
        public ControlHierarchy(Control rootControl)
        {
            _rootControl = rootControl;
            Lists = new ControlLists();
        }

        public void UpdateLists()
        {
            Lists.Clear();
            UpdateLists(_rootControl, _rootControl.IsTopMost, false);
        }

        /// <summary>
        /// Fills the lists of <see cref="Lists"/>.
        /// </summary>
        /// <param name="currentControl">The control to iterate.</param>
        /// <param name="isTopMost">If the current or containing control is marked as IsTopMost.</param>
        /// <param name="needsLayoutUpdate">If the parent needs a layout update call.</param>
        private void UpdateLists(Control currentControl, bool isTopMost, bool needsLayoutUpdate)
        {
            if (currentControl.IsVisible)
            {
                if (currentControl.NeedsRendering)
                    Lists.NeedsRendering = true;
                
                // keep the control highest in the hierarchy to trigger a layout pass
                if (!needsLayoutUpdate && !currentControl.IsLayoutUpdated)
                {
                    Lists.UpdateLayout.Add(currentControl);
                    needsLayoutUpdate = true;
                }
                
                // add all subsequent controls inside a topmost to the same list
                // otherwise only containers would be rendered for top most controls
                if (isTopMost || currentControl.IsTopMost)
                    Lists.TopMost.Add(currentControl);
                else if (currentControl is Control3D) // need different processing
                    Lists.Controls3D.Add(currentControl);
                else // default control
                    Lists.Controls2D.Add(currentControl);

                var children = currentControl.GetChildren();
                
                foreach(var child in children)
                    UpdateLists(child, isTopMost || currentControl.IsTopMost, needsLayoutUpdate);
            }
        }

        public void UpdateLayout()
        {
            foreach(var control in Lists.UpdateLayout)
                control.UpdateLayout();
        }
    }
}