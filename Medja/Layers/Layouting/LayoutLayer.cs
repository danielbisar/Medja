using System;
using System.Collections.Generic;

namespace Medja.Layers.Layouting
{
    public class LayoutLayer : ILayer
    {
        private LayoutControl _layoutRoot;

        public Size Size { get; set; }

        public void SetLayoutRoot(LayoutControl layout)
        {
            _layoutRoot = layout;
            Size = Size.Empty;
        }

        public IEnumerable<ControlState> Apply(IEnumerable<ControlState> states)
        {
            // ignore existing states for now - so this layer should be the first
            var rootControlState = new ControlState
            {
                Control = _layoutRoot,
                Position = new PositionInfo
                {
                    X = 0, Y = 0,
                    Width = Size.Width,
                    Height = Size.Height
                }
            };

            return HandleControl(rootControlState);
        }

        private IEnumerable<ControlState> HandleControl(ControlState state)
        {            
            if (state.Control is LayoutControl lc)
            {
                ApplyMargin(lc, state.Position);

                // execute sublayout calls
                foreach(var subState in lc.GetLayout(state))
                {
                    foreach (var item in HandleControl(subState))
                        yield return item;
                }
            }
            else
                yield return state;
        }

        private void ApplyMargin(LayoutControl lc, PositionInfo position)
        {
            var margin = lc.Margin;
            position.X += margin.Left;
            position.Y += margin.Top;
            position.Width -= margin.Left + margin.Right;
            position.Height -= margin.Top + margin.Bottom;
        }
    }
}
