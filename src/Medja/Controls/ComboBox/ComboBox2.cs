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
            _popup = controlFactory.Create<Popup>();
            _popup.Parent = this;
            
            PropertyTitle = new Property<string>();
            PropertyIsDropDownOpen = new Property<bool>();
            PropertyDropDownWidth = new Property<float?>();
            PropertyMaxDropDownHeight = new Property<float>();

            PropertyDropDownWidth.AffectsLayout(this);
            PropertyMaxDropDownHeight.AffectsLayout(this);

            MaxDropDownHeight = 200;
            
            InputState.Clicked += OnClicked;

            _popup.PropertyBackground.BindTo(PropertyBackground);
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

            var root = GetRootControl();
            var isDropDownAboveControl = false;

            if (root is MedjaWindow)
                isDropDownAboveControl = Position.Y + Position.Height + MaxDropDownHeight > root.Position.Height;

            _popup.Position.X = Position.X;
            _popup.Position.Width = DropDownWidth ?? Position.Width;
            _popup.Position.Height = MaxDropDownHeight;

            if (!isDropDownAboveControl)
                _popup.Position.Y = Position.Y + Position.Height;
            else
                _popup.Position.Y = Position.Y - _popup.Position.Height;
        }

        public override IEnumerable<Control> GetChildren()
        {
            if (IsDropDownOpen)
                yield return _popup;
        }
    }
}
