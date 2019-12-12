using System.Linq;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class ComboBoxTest
    {
        [Fact]
        public void AddSetsTitleOfMenuItem()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var menuItem = comboBox.Add("item");
            
            Assert.Equal("item", menuItem.Title);
        }

        [Fact]
        public void ComboBoxMenuItemIsUsedAsChild()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var menuItem = comboBox.Add("item"); 
            
            Assert.True(menuItem is ComboBoxMenuItem);
        }

        [Fact]
        public void GetChildrenReturnsPopupOnlyIfIsDropDownOpen()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            
            Assert.Empty(comboBox.GetChildren());
            
            comboBox.IsDropDownOpen = true;

            var result = comboBox.GetChildren().ToList();
            
            Assert.Single(result);
            Assert.IsType<Popup>(result[0]);
            
            comboBox.IsDropDownOpen = false;

            Assert.Empty(comboBox.GetChildren());
        }

        [Fact]
        public void OpensPopupAboveComboBoxIfWindowIsTooSmall()
        {
            var factory = new ControlFactory();
            
            var comboBox = factory.Create<ComboBox>();
            comboBox.Add("123");
            comboBox.Add("234");
            comboBox.Add("345");

            var window = factory.Create<Window>();
            window.Position.Width = 100;
            window.Position.Height = 200;

            window.Content = comboBox;
            comboBox.Position.Y = 170;
            comboBox.Position.Height = 30;

            comboBox.ItemsPanel.ChildrenHeight = 30;

            comboBox.IsDropDownOpen = true;
            
            window.Arrange(window.Position.ToSize());

            // cast to make sure it is the popup we are checking
            var popup = (Popup)comboBox.GetChildren().ToList()[0];
            var position = popup.Position;
            
            Assert.NotEqual(0, position.Y);
            Assert.NotEqual(0, position.Height);
            
            Assert.True(position.Y <= comboBox.Position.Y);
            Assert.True(position.Y + position.Height <= comboBox.Position.Y);
        }
        
        [Fact]
        public void OpensPopupUnderComboBox()
        {
            var factory = new ControlFactory();
            
            var comboBox = factory.Create<ComboBox>();
            comboBox.Add("123");
            comboBox.Add("234");
            comboBox.Add("345");
            
            comboBox.Position.Y = 0;
            comboBox.Position.Height = 30;

            comboBox.ItemsPanel.ChildrenHeight = 30;

            comboBox.IsDropDownOpen = true;
            
            comboBox.Arrange(comboBox.Position.ToSize());

            // cast to make sure it is the popup we are checking
            var popup = (Popup)comboBox.GetChildren().First();
            var position = popup.Position;
            
            Assert.True(position.Y > comboBox.Position.Y);
        }

        [Fact]
        public void LimitsPopupToMaxHeightIfSet()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            comboBox.ItemsPanel.ChildrenHeight = 10;
            comboBox.MaxDropDownHeight = 10;
            comboBox.Add("123");
            comboBox.Add("456");

            comboBox.IsDropDownOpen = true;
            comboBox.Arrange(comboBox.Position.ToSize());

            var popup = comboBox.GetChildren().Single();
            
            Assert.Equal(10, popup.Position.Height);
        }

        [Fact]
        public void ChangesSizeOfPopupDependingOnContent()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            comboBox.ItemsPanel.ChildrenHeight = 10;
            comboBox.Add("123");
            comboBox.IsDropDownOpen = true;
            comboBox.Arrange(comboBox.Position.ToSize());

            var popup = comboBox.GetChildren().Single();

            var popupHeight1 = popup.Position.Height;

            comboBox.Add("345");
            
            Assert.False(comboBox.IsLayoutUpdated);
            comboBox.Arrange(comboBox.Position.ToSize());

            var popupHeight2 = popup.Position.Height;
            
            Assert.NotEqual(popupHeight1, popupHeight2);
        }
        
        [Fact]
        public void ItemsPanelAddMarksIsLayoutUpdatedAsFalse()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            comboBox.IsLayoutUpdated = true;
            comboBox.ItemsPanel.Add(new Control());
            
            Assert.False(comboBox.IsLayoutUpdated);
        }
        
        [Fact]
        public void AddMarksIsLayoutUpdatedAsFalse()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            comboBox.IsLayoutUpdated = true;
            comboBox.Add("123");
            
            Assert.False(comboBox.IsLayoutUpdated);
        }

        [Fact]
        public void SelectsItemOnClick()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var menuItem = comboBox.Add("123");

            Assert.Null(comboBox.SelectedItem);
            
            menuItem.InputState.SendClick();
            
            Assert.Equal(menuItem, comboBox.SelectedItem);
        }

        [Fact]
        public void SetDisplayTextWhenSelectedItemIsSet()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var menuItem = comboBox.Add("123");

            comboBox.SelectedItem = menuItem;
            
            Assert.Equal("123", comboBox.DisplayText);

            comboBox.SelectedItem = null;
            
            Assert.Equal("", comboBox.DisplayText);
        }
        
        [Fact]
        public void DisplayTextIsEmptyStringByDefault()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            Assert.Equal("", comboBox.DisplayText);
        }

        [Fact]
        public void UsesCustomGetDisplayTextIfSet()
        {
            var factory = new ControlFactory();
            var comboBox = factory.Create<ComboBox>();
            comboBox.GetDisplayTextFromItem = control => ((TextBlock) control).Text;

            var item = factory.Create<TextBlock>(p => p.Text = "Test");
            
            comboBox.ItemsPanel.Add(item);
            comboBox.SelectedItem = item;
            
            Assert.Equal("Test", comboBox.DisplayText);
        }

        [Fact]
        public void ItemClickSelectsItem()
        {
            var controlFactory = new ControlFactory();
            var comboBox = controlFactory.Create<ComboBox>();

            comboBox.Add("123");
            var item2 = comboBox.Add("456");

            Assert.NotEqual(item2, comboBox.SelectedItem);
            
            item2.InputState.SendClick();
            
            Assert.Equal(item2, comboBox.SelectedItem);
        }

        [Fact]
        public void ClearRemovesClickHandler()
        {
            var controlFactory = new ControlFactory();
            var comboBox = controlFactory.Create<ComboBox>();

            comboBox.Add("123");
            var item2 = comboBox.Add("456");

            comboBox.Clear();
            
            item2.InputState.SendClick();
            
            Assert.NotEqual(item2, comboBox.SelectedItem);
        }
        
        [Fact]
        public void SelectItem()
        {
            var controlFactory = new ControlFactory();
            var comboBox = controlFactory.Create<ComboBox>();
            
            comboBox.Add("123");
            comboBox.Add("234");
            
            Assert.NotEqual("234", ((MenuItem)comboBox.SelectedItem)?.Title);
            
            comboBox.SelectItem("234");
            
            Assert.Equal("234", ((MenuItem)comboBox.SelectedItem)?.Title);
        }

        [Fact]
        public void ItemsCountUpdates()
        {
            var controlFactory = new ControlFactory();
            var comboBox = controlFactory.Create<ComboBox>();

            Assert.Equal(0, comboBox.ItemsCount);

            comboBox.Add("123");

            Assert.Equal(1, comboBox.ItemsCount);

            comboBox.SelectedItem = comboBox.Add("234");

            comboBox.RemoveSelected();

            Assert.Equal(1, comboBox.ItemsCount);
        }

        [Fact]
        public void RemoveSelectedItem()
        {
            var controlFactory = new ControlFactory();
            var comboBox = controlFactory.Create<ComboBox>();

            comboBox.Add("123");
            comboBox.SelectedItem = comboBox.Add("234");

            comboBox.RemoveSelected();

            Assert.Null(comboBox.SelectedItem);
            Assert.Equal(1, comboBox.ItemsCount);
        }

        [Fact]
        public void RemoveSelectedItemUsesCustomSelectionLogic()
        {
            var controlFactory = new ControlFactory();
            var comboBox = controlFactory.Create<ComboBox>();

            comboBox.Add("123");
            comboBox.SelectedItem = comboBox.Add("234");
            comboBox.Add("567");
            comboBox.Add("890");

            comboBox.RemoveSelected(cb => cb.ItemsPanel.Children.FirstOrDefault());

            Assert.Equal(comboBox.ItemsPanel.Children[0],comboBox.SelectedItem);

            comboBox.RemoveSelected(cb => cb.ItemsPanel.Children.LastOrDefault());

            Assert.Equal(comboBox.ItemsPanel.Children.Last(), comboBox.SelectedItem);
        }
    }
}