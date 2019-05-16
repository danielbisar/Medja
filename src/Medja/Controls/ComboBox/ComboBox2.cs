using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    /// <summary>
    /// A ComboBox or DropDown Control
    /// </summary>
    public class ComboBox2 : Control
    {
        private readonly Popup _popup;
        private readonly VerticalStackPanel _itemsPanel;

        public VerticalStackPanel ItemsPanel
        {
            get { return _itemsPanel; }
        }

        public Property<string> PropertyTitle;
        /// <summary>
        /// The title that is currently displayed.
        /// </summary>
        public string Title
        {
            get { return PropertyTitle.Get(); }
            set { PropertyTitle.Set(value); }
        }
        
        public Property<bool> PropertyIsDropDownOpen;
        /// <summary>
        /// Gets or sets whether the drop down are is open (visible) or not.
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return PropertyIsDropDownOpen.Get(); }
            set { PropertyIsDropDownOpen.Set(value); }
        }
        
        public readonly Property<float> PropertyMaxDropDownHeight;
        public float MaxDropDownHeight
        {
            get { return PropertyMaxDropDownHeight.Get(); }
            set { PropertyMaxDropDownHeight.Set(value); }
        }

        public Property<float?> PropertyDropDownWidth;
        /// <summary>
        /// Gets or sets the width of the popup. If not set, the controls width is used.
        /// </summary>
        public float? DropDownWidth
        {
            get { return PropertyDropDownWidth.Get(); }
            set { PropertyDropDownWidth.Set(value); }
        }
        
        public ComboBox2(IControlFactory controlFactory)
        {
            PropertyTitle = new Property<string>();
            PropertyIsDropDownOpen = new Property<bool>();
            PropertyDropDownWidth = new Property<float?>();
            PropertyMaxDropDownHeight = new Property<float>();

            PropertyDropDownWidth.AffectsLayout(this);
            PropertyMaxDropDownHeight.AffectsLayout(this);

            MaxDropDownHeight = 400;
            
            InputState.Clicked += OnClicked;
            InputState.OwnsMouseEvents = true;
            PropertyIsFocused.PropertyChanged += OnIsFocusedChanged;
            
            _itemsPanel = controlFactory.Create<VerticalStackPanel>();
            
            _popup = CreatePopup(controlFactory);
            _popup.PropertyBackground.BindTo(PropertyBackground);
        }

        private Popup CreatePopup(IControlFactory controlFactory)
        {
            var content = controlFactory.Create<ScrollableContainer>();
            content.Content = _itemsPanel;

            var result = controlFactory.Create<Popup>();
            result.Parent = this;
            result.Content = content;

            return result;
        }

        protected virtual void OnIsFocusedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsFocused == false && !_popup.IsFocused) // lost focus
                IsDropDownOpen = false;
        }

        protected virtual void OnClicked(object sender, EventArgs e)
        {
            var mousePos = InputState.PointerPosition;

            if (Position.IsWithin(mousePos))
                IsDropDownOpen = !IsDropDownOpen;
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);

            var isDropDownAboveControl = false;

            _popup.Position.X = Position.X;
            _popup.Position.Width = DropDownWidth ?? Position.Width;
            _popup.Position.Height = GetDropDownHeight();

            var root = GetRootControl();
            
            if (root is MedjaWindow)
                isDropDownAboveControl = Position.Y + Position.Height + _popup.Position.Height > root.Position.Height;
            
            if (!isDropDownAboveControl)
                _popup.Position.Y = Position.Y + Position.Height;
            else
                _popup.Position.Y = Position.Y - _popup.Position.Height;
        }

        private float GetDropDownHeight()
        {
            return Math.Min(MaxDropDownHeight, GetItemsPanelHeight());
        }

        private float GetItemsPanelHeight()
        {
            return (ItemsPanel.SpaceBetweenChildren + ItemsPanel.ChildrenHeight ?? 0) *
                   ItemsPanel.Children.Count + ItemsPanel.Margin.TopAndBottom +
                   ItemsPanel.Padding.TopAndBottom;
        }

        public override IEnumerable<Control> GetChildren()
        {
            if (IsDropDownOpen)
                yield return _popup;
        }
    }
}
