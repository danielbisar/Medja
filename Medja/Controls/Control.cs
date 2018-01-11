using System.ComponentModel;
using Medja.Rendering;

namespace Medja.Controls
{
    /// <summary>
    /// Any control should inherit from this class.
    /// </summary>
    /// <remarks>
    /// 
    /// ? Background/Foreground colors should be designed via renderers?
    /// 
    /// </remarks>
    public class Control
    {
        private readonly IControlRenderer _renderer;

        /// <summary>
        /// Indicates if the current state of the object was rendered. If IsRendered = false, the current state
        /// might not reflect the state that was last rendered.
        /// 
        /// Not implemented as Property for performance reasons.
        /// </summary>
        public bool IsRendered { get; set; }

        public Control()
        {
            _renderer = RendererFactory.Get(GetType());
        }

        protected IProperty<T> AddProperty<T>()
        {
            var result = new Property<T>();
            result.PropertyChanged += OnPropertyChanged;

            return result;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // todo thread-safety? 
            // todo group properties: some might not effect the ui

            IsRendered = false;
        }

        public void Render(RenderContext renderContext)
        {
            if(!IsRendered || renderContext.ForceRender)
                _renderer.Render(this);
        }
    }
}
