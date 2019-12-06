using System;
using System.Collections.Generic;
using System.Linq;

namespace Medja.OpenTk.Components3D
{
    public class GLScene : IDisposable
    {
        public GLCamera Camera { get; set; }
        public List<GLComponent> Components { get; }

        public GLScene()
        {
            Components = new List<GLComponent>();
        }

        public void Add(GLComponent component)
        {
            Components.Add(component);
        }

        public void Render()
        {
            // possible optimizations:
            // - only updated viewProjectionMatrix for the controls if it changed
            // - since Matrix4 is a struct the values are copied and not referenced; this could be changed too (via ref?)
            var viewProjectionMatrix = Camera.ViewProjectionMatrix;
            
            foreach (var model in Components.OfType<IViewProjectionMatrix>())
                model.ViewProjectionMatrix = viewProjectionMatrix; 
            
            foreach(var component in Components)
                component.Render();
        }

        public void Dispose()
        {
            Camera?.Dispose();
            
            foreach(var component in Components)
                component.Dispose();
        }

        public void ApplyToAllModels(Action<GLModel> action)
        {
            foreach (var model in Components.OfType<GLModel>())
                action(model);
        }
    }
}
