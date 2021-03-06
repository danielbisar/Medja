using System;
using System.Collections.Generic;
using Medja.Input;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls
{
    /// <summary>
    /// All controls should inherit from this class.
    /// </summary>
    public class Control : IDisposable
    {
        private readonly HashSet<IProperty> _propertiesAffectingRendering;
        private readonly HashSet<IDisposable> _disposables;
        
        public AttachedProperties AttachedProperties { get; }
        public InputState InputState { get; }
        
        /// <summary>
        /// Gets if this control is a 3D or 2D control. Should not change over the lifetime of a control.
        /// </summary>
        public bool Is3D { get; protected set; }
        
        /// <summary>
        /// Gets the position this item should be drawn at.
        /// </summary>
        /// <value>The position.</value>
        public MRect Position { get; }
        
        /// <summary>
        /// Defines the outer border, where no rendering occurs.
        /// </summary>
        public Thickness Margin { get; }

        public readonly Property<Color> PropertyBackground;
        public Color Background
        {
            get { return PropertyBackground.Get(); }
            set { PropertyBackground.Set(value); }
        }

        public readonly OverwritableProperty<bool> PropertyIsEnabled;
        public bool IsEnabled
        {
            get { return PropertyIsEnabled.Get(); }
            set { PropertyIsEnabled.Set(value); }
        }

        public readonly Property<bool> PropertyIsFocused;
        public bool IsFocused
        {
            get { return PropertyIsFocused.Get(); }
            internal set { PropertyIsFocused.Set(value); }
        }

        public IControlRenderer Renderer { get; set; }

        public readonly Property<Visibility> PropertyVisibility;
        public Visibility Visibility
        {
            get { return PropertyVisibility.Get(); }
            set { PropertyVisibility.Set(value); }
        }

        public readonly Property<bool> PropertyIsVisible;
        public bool IsVisible
        {
            get { return PropertyIsVisible.Get(); }
        }

        public readonly Property<Control> PropertyParent;
        public Control Parent
        {
            get { return PropertyParent.Get(); }
            set { PropertyParent.Set(value); }
        }

        public readonly Property<VerticalAlignment> PropertyVerticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get { return PropertyVerticalAlignment.Get(); }
            set { PropertyVerticalAlignment.Set(value); }
        }

        public readonly Property<HorizontalAlignment> PropertyHorizontalAlignment;
        public HorizontalAlignment HorizontalAlignment
        {
            get { return PropertyHorizontalAlignment.Get(); }
            set { PropertyHorizontalAlignment.Set(value); }
        }
        
        public readonly Property<bool> PropertyIsLayoutUpdated;
        /// <summary>
        /// Gets or sets if this control needs to be updated on the next layout run.
        /// </summary>
        public bool IsLayoutUpdated
        {
            get { return PropertyIsLayoutUpdated.Get(); }
            set { PropertyIsLayoutUpdated.Set(value); }
        }

        public readonly Property<bool> PropertyNeedsRendering;
        /// <summary>
        /// Gets or sets if this control should be rendered again.
        /// </summary>
        public bool NeedsRendering
        {
            get { return PropertyNeedsRendering.Get(); }
            set { PropertyNeedsRendering.Set(value); }
        }

        public readonly Property<bool> PropertyIsTopMost;
        /// <summary>
        /// If set to true this control will be rendered above others (later).
        /// </summary>
        public bool IsTopMost
        {
            get { return PropertyIsTopMost.Get(); }
            set { PropertyIsTopMost.Set(value); }
        }
        
        public MRect ClippingArea { get; }

        public readonly Property<bool> PropertyIsDisposed;

        public bool IsDisposed
        {
            get { return PropertyIsDisposed.Get(); }
            private set { PropertyIsDisposed.Set(value); }
        }

        /// <summary>
        /// This event is triggered after the control was arranged.
        /// </summary>
        public event EventHandler Arranged;

        public Control()
        {
            _disposables = new HashSet<IDisposable>();
            _propertiesAffectingRendering = new HashSet<IProperty>();
            
            AttachedProperties = new AttachedProperties();
            InputState = new InputState(this);
            Position = new MRect();
            Position.PropertyHeight.PropertyChanged += OnPositionChanged;
            Position.PropertyWidth.PropertyChanged += OnPositionChanged;
            Position.PropertyX.PropertyChanged += OnPositionChanged;
            Position.PropertyY.PropertyChanged += OnPositionChanged;
            Margin = new Thickness();
            
            PropertyBackground = new Property<Color>();
            PropertyIsEnabled = new OverwritableProperty<bool>();
            PropertyIsEnabled.SetSilent(true);
            PropertyIsFocused = new Property<bool>();
            PropertyIsDisposed = new Property<bool>();
            
            PropertyVisibility = new Property<Visibility>();
            PropertyIsVisible = new Property<bool>();
            PropertyIsVisible.SetSilent(true);
            PropertyVisibility.PropertyChanged += OnVisibilityChanged;

            PropertyParent = new Property<Control>();
            PropertyVerticalAlignment = new Property<VerticalAlignment>();
            PropertyHorizontalAlignment = new Property<HorizontalAlignment>();
            PropertyIsLayoutUpdated = new Property<bool>();
            PropertyIsLayoutUpdated.PropertyChanged += OnIsLayoutUpdatedChanged;
            PropertyNeedsRendering = new Property<bool>();
            PropertyNeedsRendering.PropertyChanged += OnNeedsRenderingChanged;
            
            PropertyIsTopMost = new Property<bool>();
            
            ClippingArea = new MRect();
        }

        private void OnIsLayoutUpdatedChanged(object sender, PropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
                NeedsRendering = true;
        }

        protected virtual void OnNeedsRenderingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Renderer == null)
                NeedsRendering = false;
        }

        protected virtual void OnPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            IsLayoutUpdated = false;
            
            if(Renderer != null)
                NeedsRendering = true;
        }

        private void OnVisibilityChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            PropertyIsVisible.Set(Visibility == Visibility.Visible);
        }

        public virtual void UpdateLayout()
        {
            Arrange(new Size(Position.Width, Position.Height));
            
            IsLayoutUpdated = true;
            NeedsRendering = true; // todo not the best place to set this
        }

        /// <summary>
        /// Arrange places the sub-controls as needed. Default does nothing.
        /// </summary>
        /// <param name="availableSize">Available size.</param>
        public virtual void Arrange(Size availableSize)
        {
            Arranged?.Invoke(this, EventArgs.Empty);
        }

        public virtual IEnumerable<Control> GetChildren()
        {
            return new Control[0];
        }

        /// <summary>
        /// Gets the highest control in the hierarchy.
        /// </summary>
        /// <returns>The highest parent control, or self.</returns>
        public Control GetRootControl()
        {
            var root = this;

            while (root.Parent != null)
                root = root.Parent;

            return root;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // managed objects go here
                
                Renderer?.Dispose();

                foreach (var property in _propertiesAffectingRendering)
                    property.PropertyChanged -= OnRenderingRelevantPropertyChanged;
                
                foreach(var disposable in _disposables)
                    disposable.Dispose();

                IsDisposed = true;
            }
            
            // unmanaged objects go here
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds a property to the listener that affects the rendering.
        /// </summary>
        /// <remarks>This method should be used by themes to define when a control needs to be rerendered.</remarks>
        /// <param name="property">The property that affects rendering.</param>
        public void AffectsRendering(IProperty property)
        {
            if(_propertiesAffectingRendering.Add(property))
                property.PropertyChanged += OnRenderingRelevantPropertyChanged;
        }

        /// <summary>
        /// <see cref="AffectsRendering"/>.
        /// </summary>
        /// <param name="properties">The properties that affect rendering.</param>
        public void AffectRendering(params IProperty[] properties)
        {
            foreach (var property in properties)
                AffectsRendering(property);
        }

        /// <summary>
        /// Removes a property that you previously listened to that affects rendering.
        /// </summary>
        /// <param name="property">The property.</param>
        public void RemoveAffectsRendering(IProperty property)
        {
            if(_propertiesAffectingRendering.Remove(property))
                property.PropertyChanged -= OnRenderingRelevantPropertyChanged;
        }

        public void RemoveAffectRendering(params IProperty[] properties)
        {
            foreach(var property in properties)
                RemoveAffectsRendering(property);
        }

        private void OnRenderingRelevantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedsRendering = true;
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
    }
}
