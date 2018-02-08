using System;
using System.Collections.Generic;
using Medja.Controls;
using Medja.Input;
using Medja.Primitives;

namespace Medja
{
    public class Workflow
    {
        private readonly WorkflowState _state;
        private readonly List<ILayer> _updateWorkflow;
        private readonly List<ILayer> _renderWorkflow;

        public Workflow()
        {
            _state = new WorkflowState();

            _updateWorkflow = new List<ILayer>();
            _renderWorkflow = new List<ILayer>();
        }

        public void AddUpdateLayer(ILayer updateLayer)
        {
            updateLayer.WorkflowState = _state;
            _updateWorkflow.Add(updateLayer);
        }

        public void AddRenderLayer(ILayer renderLayer)
        {
            renderLayer.WorkflowState = _state;
            _renderWorkflow.Add(renderLayer);
        }

        public ControlState AddControl(Control control)
        {
            var result = new ControlState
            {
                Control = control
            };

            _state.Controls.Add(result);

            return result;
        }

        public void Update()
        {
            foreach (var layer in _updateWorkflow)
                layer.Execute();
        }

        public void Render()
        {
            foreach (var layer in _renderWorkflow)
                layer.Execute();
        }

        public void SetRenderTargetSize(Size size)
        {
            _state.RenderTargetSize = size;
        }

        public void RemoveControl(ControlState child)
        {
            _state.Controls.Remove(child);
        }

        public void UpdateInput(InputDeviceState inputDeviceState)
        {
            foreach (var child in _state.Controls.ToArray()) // copy the whole items because input events might modify the current state
            {
                var inputState = child.InputState;
                inputState.IsMouseOver = IsMouseOver(child, inputDeviceState.Pos);
                inputState.IsMouseDown = inputState.IsMouseOver && inputDeviceState.LeftButtonDown;
            }
        }

        private bool IsMouseOver(ControlState child, Point pos)
        {
            var childPos = child.Position;
            return pos.X >= childPos.X
                && pos.Y >= childPos.Y
                && pos.X <= (childPos.X + childPos.Width)
                && pos.Y <= (childPos.Y + childPos.Height);
        }
    }
}
