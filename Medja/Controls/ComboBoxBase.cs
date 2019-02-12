using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public abstract class ComboBoxBase : ContentControl
    {
        protected readonly IControlFactory ControlFactory;

        protected VerticalStackPanel ItemsPanel;
        protected ScrollableContainer ItemContainer;
        protected TextBlock SelectedItemTextBlock;
        
        public readonly Property<bool> PropertyIsDropDownOpen;
        public bool IsDropDownOpen
        {
            get { return PropertyIsDropDownOpen.Get(); }
            set { PropertyIsDropDownOpen.Set(value); }
        }
        
        public readonly Property<float> PropertyDropDownHeight;
        public float DropDownHeight
        {
            get { return PropertyDropDownHeight.Get(); }
            set { PropertyDropDownHeight.Set(value); }
        }

        protected ComboBoxBase(IControlFactory controlFactory)
        {
            PropertyIsDropDownOpen = new Property<bool>();
            PropertyDropDownHeight = new Property<float>();
            DropDownHeight = 200;
            ControlFactory = controlFactory;
            Content = CreateContent();
            PropertyBackground.PropertyChanged += (s, e) => ItemContainer.Background = (Color) e.NewValue;
            InputState.OwnsMouseEvents = true;
        }
        
        private Control CreateContent()
        {
            var button = ControlFactory.Create<Button>();
            button.Text = "ˇ";
            button.Position.Width = 50;
            button.InputState.MouseClicked += OnDropDownButtonClicked;

            SelectedItemTextBlock = ControlFactory.Create<TextBlock>();
            
            var dockPanel = ControlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Right, button);
            dockPanel.Add(Dock.Fill, SelectedItemTextBlock);
            dockPanel.VerticalAlignment = VerticalAlignment.Stretch;
            dockPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            ItemsPanel = ControlFactory.Create<VerticalStackPanel>();
            ItemsPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            ItemsPanel.VerticalAlignment = VerticalAlignment.Stretch;
            ItemsPanel.ChildrenHeight = 50;
            ItemsPanel.SpaceBetweenChildren = 5;

            UpdateItemsPanel();
            
            ItemContainer = ControlFactory.Create<ScrollableContainer>();
            ItemContainer.Content = ItemsPanel;
            ItemContainer.Visibility = Visibility.Collapsed;
            ItemContainer.IsTopMost = true;

            return dockPanel;
        }

        protected void UpdateItemsPanel()
        {
            ItemsPanel.Position.Height = GetStackPanelHeight();
            IsLayoutUpdated = false;
        }

        private float GetStackPanelHeight()
        {
            return (ItemsPanel.SpaceBetweenChildren + ItemsPanel.ChildrenHeight ?? 0) *
                    ItemsPanel.Children.Count + ItemsPanel.Margin.TopAndBottom + ItemsPanel.Padding.TopAndBottom;
        }

        private void OnDropDownButtonClicked(object sender, EventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;

            if (IsDropDownOpen)
                ItemContainer.Visibility = Visibility.Visible;
        }

        public override IEnumerable<Control> GetChildren()
        {
            foreach (var item in base.GetChildren())
                yield return item;

            if (IsDropDownOpen)
                yield return ItemContainer;
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);

            ItemContainer.Position.X = Position.X;
            ItemContainer.Position.Y = Position.Y + Position.Height;
            ItemContainer.Position.Width = Position.Width;
            ItemContainer.Position.Height = DropDownHeight;
            ItemContainer.Arrange(new Size(ItemContainer.Position.Width, ItemContainer.Position.Height));
        }
    }
}