using System.Collections.Generic;

namespace Medja.Controls
{
    /// <summary>
    /// Containing different lists as needed for rendering
    /// </summary>
    public class ControlLists
    {
        public List<Control> Controls2D { get; }
        public List<Control> Controls3D { get; }
        public List<Control> TopMost { get; }
        public List<Control> UpdateLayout { get; }
        
        /// <summary>
        /// Gets or sets if any control needs to be rendered.
        /// </summary>
        public bool NeedsRendering { get; set; }
        
        public ControlLists()
        {
            Controls2D = new List<Control>();
            Controls3D = new List<Control>();
            TopMost = new List<Control>();
            UpdateLayout = new List<Control>();
        }

        public void Clear()
        {
            NeedsRendering = false;
            Controls2D.Clear();
            Controls3D.Clear();
            TopMost.Clear();
            UpdateLayout.Clear();
        }

        public List<Control> All()
        {
            var result = new List<Control>(Controls2D.Count + Controls3D.Count + TopMost.Count);
            result.AddRange(Controls2D);
            result.AddRange(Controls3D);
            result.AddRange(TopMost);

            return result;
        }
    }
}