using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Medja.Input;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls
{
    /// <summary>
    /// A ComboBox or so called drop down control.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="ControlFactory.ComboBoxMenuItemStyle"/> for generated <see cref="MenuItem"/>s.
    /// </remarks>
    /// <example>
    /// <code>
    /// var comboBox = controlFactory.Create&lt;ComboBox&gt;();
    /// var item = comboBox.Add("item 1");
    /// comboBox.SelectedItem = item;
    /// </code>
    /// </example>
    public class ComboBox : Control
    {
        private readonly Popup _popup;
        private readonly IControlFactory _controlFactory;

        private readonly VerticalStackPanel _itemsPanel;
        /// <summary>
        /// The panel that is responsible for displaying the items inside the popup.
        /// </summary>
        /// <remarks>You can use this instead of the <see cref="Add"/> method to add other controls instead of the
        /// default generated <see cref="MenuItem"/>.</remarks>
        public VerticalStackPanel ItemsPanel
        {
            get { return _itemsPanel; }
        }
        
        /// <summary>
        /// Gets or sets the method used to get the display text from an item added to the ItemsPanel.
        /// </summary>
        public Func<Control, string> GetDisplayTextFromItem { get; set; }

        public readonly Property<string> PropertyTitle;
        /// <summary>
        /// The title that is currently displayed.
        /// </summary>
        public string Title
        {
            get { return PropertyTitle.Get(); }
            set { PropertyTitle.Set(value); }
        }
        
        public readonly Property<bool> PropertyIsDropDownOpen;
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

        public readonly Property<Control> PropertySelectedItem;
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public Control SelectedItem
        {
            get { return PropertySelectedItem.Get(); }
            set { PropertySelectedItem.Set(value); }
        }

        public readonly Property<string> PropertyDisplayText;
        /// <summary>
        /// Gets the text that should currently be displayed.
        /// </summary>
        /// <remarks>Is null if no item is selected.</remarks>
        public string DisplayText
        {
            get { return PropertyDisplayText.Get(); }
            private set { PropertyDisplayText.Set(value); }
        }

        public readonly Property<int> PropertyItemsCount;
        /// <summary>
        /// Bindable count of items.
        /// </summary>
        public int ItemsCount
        {
            get => PropertyItemsCount.Get();
            private set => PropertyItemsCount.Set(value);
        }
        
        public ComboBox(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            
            PropertyDisplayText = new Property<string>();
            PropertyItemsCount = new Property<int>();
            PropertyIsDropDownOpen = new Property<bool>();
            PropertyMaxDropDownHeight = new Property<float>();
            PropertySelectedItem = new Property<Control>();
            PropertyTitle = new Property<string>();

            PropertyIsFocused.PropertyChanged += OnIsFocusedChanged;
            PropertyMaxDropDownHeight.AffectsLayout(this);
            PropertyDisplayText.BindTo(PropertySelectedItem, ApplyGetDisplayTextFromItem);
            DisplayText = "";
            
            MaxDropDownHeight = 400;
            
            InputState.Clicked += OnClicked;
            InputState.OwnsMouseEvents = true;
            
            _itemsPanel = controlFactory.Create<VerticalStackPanel>();
            _itemsPanel.Children.CollectionChanged += OnItemsPanelCollectionChanged;
            
            _popup = CreatePopup(controlFactory);
            _popup.PropertyBackground.BindTo(PropertyBackground);
        }

        private string ApplyGetDisplayTextFromItem(Control item)
        {
            if (GetDisplayTextFromItem == null)
            {
                if (item is MenuItem menuItem)
                    return menuItem.Title;
                
                return item != null ? item.ToString() : "";
            }
                
            return GetDisplayTextFromItem(item);
        }

        private void OnItemsPanelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<Control>())
                    UnregisterOnClicked(item);
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<Control>())
                    RegisterOnClicked(item);
            }

            ItemsCount = _itemsPanel.Children.Count;
            IsLayoutUpdated = false;
        }

        private void RegisterOnClicked(Control item)
        {
            item.InputState.Clicked += OnItemClicked;
        }

        private void UnregisterOnClicked(Control item)
        {
            item.InputState.Clicked -= OnItemClicked;
        }

        /// <summary>
        /// Adds a new item to the ComboBox (creates a <see cref="MenuItem"/> and sets its
        /// <see cref="MenuItem.Title"/>).
        /// </summary>
        /// <param name="title">The title to use.</param>
        /// <returns>The created <see cref="MenuItem"/>.</returns>
        public MenuItem Add(string title)
        {
            var menuItem = _controlFactory.Create<ComboBoxMenuItem>();
            menuItem.Title = title;
            
            ItemsPanel.Add(menuItem);

            return menuItem;
        }

        public void Clear()
        {
            ItemsPanel.Clear();
        }

        private void OnItemClicked(object sender, EventArgs e)
        {
            var inputState = (InputState) sender;
            SelectedItem = inputState.Control;
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

        /// <summary>
        /// Positions this control and the popup (even when it is not visible).
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <remarks>The <see cref="Popup"/> placement depends on the position inside the <see cref="Window"/>
        /// If the control is to far at the bottom (the popup would not fit inside the window) it is placed above.
        /// </remarks>
        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);

            var isDropDownAboveControl = false;

            _popup.Position.X = Position.X;
            _popup.Position.Width = Position.Width;
            _popup.Position.Height = GetDropDownHeight();

            var root = GetRootControl();
            
            if (root is Window)
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

        /// <summary>
        /// If <see cref="IsDropDownOpen"/> is true this will return the <see cref="Popup"/>, else nothing.
        /// </summary>
        /// <returns>If <see cref="IsDropDownOpen"/> is true this will return the <see cref="Popup"/>, else nothing.
        /// </returns>
        public override IEnumerable<Control> GetChildren()
        {
            if (IsDropDownOpen)
                yield return _popup;
        }

        /// <summary>
        /// Selects an item based on its <see cref="MenuItem.Title"/>.
        /// </summary>
        /// <param name="title">The items title.</param>
        public void SelectItem(string title)
        {
            var item = _itemsPanel.Children.OfType<MenuItem>().FirstOrDefault(p => p.Title == title);
            SelectedItem = item;
        }

        /// <summary>
        /// Removes the currently selected item.
        /// </summary>
        /// <param name="getNewSelectedItem">Gets the new selected item.</param>
        /// <returns>true if any item was selected, else false.</returns>
        public bool RemoveSelected(Func<ComboBox, Control> getNewSelectedItem = null)
        {
            if (SelectedItem == null)
                return false;

            _itemsPanel.Remove(SelectedItem);
            SelectedItem = getNewSelectedItem?.Invoke(this);
            
            return true;
        }
    }
}
