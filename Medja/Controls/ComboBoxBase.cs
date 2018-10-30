using System;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class ComboBoxBase : ContentControl
    {
        protected readonly ControlFactory ControlFactory;

        private VerticalStackPanel _itemContainer;
        
        public readonly Property<bool> PropertyIsDropDownOpen;
        public bool IsDropDownOpen
        {
            get { return PropertyIsDropDownOpen.Get(); }
            set { PropertyIsDropDownOpen.Set(value); }
        }

        public ComboBoxBase(ControlFactory controlFactory)
        {
            PropertyIsDropDownOpen = new Property<bool>();
            ControlFactory = controlFactory;
            Content = CreateContent();
        }
        
        private Control CreateContent()
        {
            var button = ControlFactory.Create<Button>();
            button.Text = "ˇ";
            button.Position.Width = 50;
            button.InputState.MouseClicked += OnDropDownButtonClicked;

            var textBlock = ControlFactory.Create<TextBlock>();
            textBlock.Text = "BLA";
            
            var dockPanel = ControlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Right, button);
            dockPanel.Add(Dock.Fill, textBlock);
            dockPanel.VerticalAlignment = VerticalAlignment.Stretch;
            dockPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            _itemContainer = ControlFactory.Create<VerticalStackPanel>();

            return dockPanel;
        }

        private void OnDropDownButtonClicked(object sender, EventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }
    }
}