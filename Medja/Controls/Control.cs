using System.ComponentModel;
using System.Diagnostics;
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

        public bool IsLayoutUpdated { get; set; }

        /// <summary>
        /// Indicates if the current state of the object was rendered. If IsRendered = false, the current state
        /// might not reflect the state that was last rendered.
        /// 
        /// Not implemented as Property for performance reasons.
        /// </summary>
        public bool IsRendered { get; set; }

        private readonly IProperty<float> _x;
        public float X
        {
            get { return _x.Get(); }
            set { _x.Set(value); }
        }

        private readonly IProperty<float> _y;
        public float Y
        {
            get { return _y.Get(); }
            set { _y.Set(value); }
        }

        private readonly IProperty<float> _width;
        public float Width
        {
            get { return _width.Get(); }
            set { _width.Set(value); }
        }

        private readonly IProperty<float> _height;
        public float Height
        {
            get { return _height.Get(); }
            set { _height.Set(value); }
        }

        public Control()
        {
            Debug.WriteLine("Create control " + GetType().Name);
            _renderer = RendererFactory.Get(GetType());

            _x = AddProperty<float>();
            _y = AddProperty<float>();
            _width = AddProperty<float>();
            _height = AddProperty<float>();
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

            IsLayoutUpdated = false;
            IsRendered = false;
        }

        public void UpdateLayout()
        {
            if (IsLayoutUpdated)
                return;

            Layout();
            IsLayoutUpdated = true;
        }

        protected virtual void Layout()
        {
        }

        public void Render(RenderContext renderContext)
        {
            if(!IsRendered || renderContext.ForceRender)
                _renderer.Render(this, renderContext);
        }
    }
}
