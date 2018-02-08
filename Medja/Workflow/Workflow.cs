using System;
using System.Collections.Generic;
using Medja.Controls;
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
    }
}
