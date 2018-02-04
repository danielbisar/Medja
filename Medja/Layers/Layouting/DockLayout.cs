using System;
using System.Collections.Generic;
using System.Text;
using Medja.Controls;

namespace Medja.Layers.Layouting
{
    public class DockLayout : LayoutControl
    {
        private List<DockedControl> _dockedControls;
        
        public DockLayout(IEnumerable<DockedControl> dockedControls)
        {
            _dockedControls = new List<DockedControl>(dockedControls);
        }

        public DockLayout()
            : this(new DockedControl[0])
        {
        }

        public void Add(DockedControl dockedControl)
        {
            _dockedControls.Add(dockedControl);
        }

        public override IEnumerable<ControlState> GetLayout(ControlState state)
        {
            // TODO finish implementation, by no means done now, for our use case this is enough
            foreach(var dockedControl in _dockedControls)
            {
                switch (dockedControl.Dock)
                {
                    case Dock.Top:
                        break;
                    case Dock.Left:
                        break;
                    case Dock.Right:
                        yield return new ControlState
                        {
                            Control = dockedControl.Control,
                            Position = new PositionInfo
                            {
                                // TODO handle: MinWidth unset or > availableWidth
                                X = state.Position.Width - dockedControl.MinWidth,
                                Y = 0, // TODO track already added controls
                                Width = dockedControl.MinWidth,
                                Height = state.Position.Height
                            }
                        };
                        break;
                    case Dock.Bottom:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Dock");
                }
            }
        }
    }
}
