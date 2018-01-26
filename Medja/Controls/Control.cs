using System;
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
    public class Control : MObject
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

        public readonly Property<float> PropertyX;
        public float X
        {
            get { return PropertyX.Get(); }
            set { PropertyX.Set(value); }
        }

        public readonly Property<float> PropertyY;
        public float Y
        {
            get { return PropertyY.Get(); }
            set { PropertyY.Set(value); }
        }

        public readonly Property<float> PropertyWidth;
        public float Width
        {
            get { return PropertyWidth.Get(); }
            set { PropertyWidth.Set(value); }
        }

        public readonly Property<float> PropertyHeight;
        public float Height
        {
            get { return PropertyHeight.Get(); }
            set { PropertyHeight.Set(value); }
        }

        public readonly Property<VerticalAlignment> PropertyVerticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get { return PropertyVerticalAlignment.Get(); }
            set { PropertyVerticalAlignment.Set(value); }
        }

        public Control()
        {
            Debug.WriteLine("Create control " + GetType().Name);
            _renderer = RendererFactory.Get(GetType());

            PropertyX = new Property<float>();
            PropertyX.PropertyChanged += OnNeedsLayoutUpdatedPropertyChanged;

            PropertyY = new Property<float>();
            PropertyY.PropertyChanged += OnNeedsLayoutUpdatedPropertyChanged;

            PropertyWidth = new Property<float>();
            PropertyWidth.PropertyChanged += OnNeedsLayoutUpdatedPropertyChanged;

            PropertyHeight = new Property<float>();
            PropertyHeight.PropertyChanged += OnNeedsLayoutUpdatedPropertyChanged;

            PropertyVerticalAlignment = new Property<VerticalAlignment>();
            PropertyVerticalAlignment.PropertyChanged += OnNeedsLayoutUpdatedPropertyChanged;
        }

        private void OnNeedsLayoutUpdatedPropertyChanged(IProperty property)
        {
            IsLayoutUpdated = false;
            // IsRendered will be set to false if change so that we really need new rendering
        }        

        public void UpdateLayout()
        {
            if (IsLayoutUpdated)
                return;

            Layout(new Size(Width, Height));
            IsLayoutUpdated = true;
        }

        internal virtual void Layout(Size availableSize)
        {
            Width = availableSize.Width;
            //if (VerticalAlignment == VerticalAlignment.Stretch)
            Height = availableSize.Height;            
            IsRendered = false;
        }

        public void Render(RenderContext renderContext)
        {
            if(!IsRendered || renderContext.ForceRender)
                _renderer.Render(this, renderContext);
        }
    }
}
