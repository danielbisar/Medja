using System;
using System.Collections.Generic;
using Medja.Primitives;
using System.Linq;
using Medja.Properties;
using Medja.Utils.Collections.Generic;

namespace Medja.Controls
{
    /// <summary>
    /// A simple dock panel.
    /// 
    /// To add or remove children use Add, Remove of this class not the Children list directly.
    /// </summary>
    public class DockPanel : Panel
    {
        private readonly Dictionary<Control, Dock> _docks;

        public DockPanel()
        {
            _docks = new Dictionary<Control, Dock>(new ReferenceEqualityComparer<Control>());
        }

        public void Add(Dock dock, Control control)
        {
            _docks.Add(control, dock);
            Children.Add(control);
        }

        public void Insert(Dock dock, Control control, int index)
        {
            _docks.Add(control, dock);
            Children.Insert(index, control);
        }
        
        protected override void OnItemAdded(Control child)
        {
            base.OnItemAdded(child);
            child.PropertyVisibility.AffectsLayoutOf(this);
        }

        protected override void OnItemRemoved(Control child)
        {
            child.PropertyVisibility.RemoveAffectsLayoutOf(this);
            _docks.Remove(child);
            base.OnItemRemoved(child);
        }

        /// <summary>
        /// Removes the item that uses Dock.Fill if any.
        /// </summary>
        public void RemoveFillItem()
        {
            Control fillChild = null;

            foreach (var kvp in _docks)
            {
                if (kvp.Value != Dock.Fill) 
                    continue;
                
                fillChild = kvp.Key;
                break;
            }
            
            if(fillChild != null)
                Children.Remove(fillChild);
        }

        public override void Arrange(Size availableSize)
        {
            var left = Position.X + Padding.Left;
            var top = Position.Y + Padding.Top;

            var height = availableSize.Height - Padding.TopAndBottom;
            var width = availableSize.Width - Padding.LeftAndRight;

            foreach (var child in Children.Where(p => p.Visibility != Visibility.Collapsed))
            {
                var dock = _docks[child];
                var childPos = child.Position;
                
                // width and height
                if (dock == Dock.Top || dock == Dock.Bottom || dock == Dock.Fill)
                {
                    if (child.HorizontalAlignment != HorizontalAlignment.Left
                            && child.HorizontalAlignment != HorizontalAlignment.Right)
                        childPos.Width = width - child.Margin.LeftAndRight;
                        
                    if (child.HorizontalAlignment != HorizontalAlignment.Right)
                        childPos.X = left + child.Margin.Left;
                    else
                        childPos.X = left + width - childPos.Width - child.Margin.Right;
                }
                
                if (dock == Dock.Left || dock == Dock.Right || dock == Dock.Fill)
                {
                    if(child.VerticalAlignment != VerticalAlignment.Bottom
                            && child.VerticalAlignment != VerticalAlignment.Top)
                        childPos.Height = height - child.Margin.TopAndBottom;

                    if (child.VerticalAlignment != VerticalAlignment.Bottom)
                        childPos.Y = top + child.Margin.Top;
                    else
                        childPos.Y = top + height - childPos.Height - child.Margin.Bottom;
                }

                switch (dock)
                {
                    case Dock.Top:
                        childPos.Y = top + child.Margin.Top;
                        top += child.Margin.TopAndBottom + childPos.Height;
                        height -= child.Margin.TopAndBottom + childPos.Height;
                        break;
                    case Dock.Bottom:
                        height -= childPos.Height + child.Margin.TopAndBottom;
                        childPos.Y = top + child.Margin.Top + height;
                        break;
                    case Dock.Left:
                        childPos.X = left + child.Margin.Left;
                        left += child.Margin.LeftAndRight + childPos.Width;
                        width -= child.Margin.LeftAndRight + childPos.Width;
                        break;
                    case Dock.Right:
                        width -= childPos.Width + child.Margin.LeftAndRight;
                        childPos.X = left + child.Margin.Left + width;
                        break;
                    case Dock.Fill:
                        if (Children[Children.Count - 1] != child)
                            throw new InvalidOperationException("Only the last child can be set to Dock.Fill");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dock));
                }

                child.Arrange(new Size(childPos.Width, childPos.Height));
            }
            
            base.Arrange(availableSize);
        }
    }
}
