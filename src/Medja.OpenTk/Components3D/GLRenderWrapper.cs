using System;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    public class GLRenderWrapper<TModel> : GLComponent, IViewProjectionMatrix
    where TModel: GLModel
    {
        public TModel Model { get; }

        public Matrix4 ViewProjectionMatrix
        {
            get { return Model.ViewProjectionMatrix; } 
            set { Model.ViewProjectionMatrix = value; }
        }
        
        public Action<TModel> RenderModel { get; set; }

        public GLRenderWrapper(TModel model, Action<TModel> renderModel)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            RenderModel = renderModel ?? throw new ArgumentNullException(nameof(renderModel));
        }

        public override void Render()
        {
            RenderModel(Model);
        }

        protected override void Dispose(bool disposing)
        {
            Model.Dispose();
            RenderModel = null;
            
            base.Dispose(disposing);
        }
    }

    public static class GLRenderWrapper
    {
        public static GLRenderWrapper<TModel> AddRenderWrapper<TModel>(this GLScene scene, TModel model, Action<TModel> renderModel) 
            where TModel : GLModel
        {
            var wrapper = new GLRenderWrapper<TModel>(model, renderModel);
            scene.Components.Add(wrapper);
            
            return wrapper;
        }
    }
}