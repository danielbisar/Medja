using System;
using Medja.Binding;
using Medja.Controls;
using Medja.Properties;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class ComboBoxEnumBindingTest
    {
        private enum MyEnum
        {
            None,
            Full,
            Half
        }

        [Fact]
        public void FailsWithoutCallToCreateItems()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            Assert.Throws<InvalidOperationException>(() => binding.Bind());
        }
        
        [Fact]
        public void SelectItemBasedOnValue()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            binding.CreateItems();
            binding.Bind();
            
            enumProperty.Set(MyEnum.Full);
            
            Assert.Equal("Full", ((MenuItem) comboBox.SelectedItem).Title);
        }

        [Fact]
        public void DoesNotUpdatePropertyAfterDispose()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            binding.CreateItems();
            binding.Bind();

            var title = binding.GetTitle(MyEnum.Half);
            
            binding.Dispose();
            comboBox.SelectItem(title);

            Assert.Equal(MyEnum.None, enumProperty.Get());
        }

        [Fact]
        public void DoesNotUpdateSelectedItemAfterDispose()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            binding.CreateItems();
            binding.Bind();

            binding.Dispose();
            enumProperty.Set(MyEnum.Full);

            Assert.Equal("None", ((MenuItem) comboBox.SelectedItem).Title);
        }

        [Fact]
        public void BindSelectsFirstItem()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            binding.CreateItems();
            binding.Bind();

            Assert.Equal("None", ((MenuItem) comboBox.SelectedItem).Title);
        }

        [Fact]
        public void CanSelectTranslatedItem()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            binding.Translate(p =>
            {
                switch (p)
                {
                    case MyEnum.None:
                        return "n";
                    case MyEnum.Full:
                        return "f";
                    case MyEnum.Half:
                        return "h";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(p), p, null);
                }
            });
            binding.CreateItems();
            binding.Bind();

            enumProperty.Set(MyEnum.Full);
            Assert.Equal("f", ((MenuItem) comboBox.SelectedItem).Title);
            
            comboBox.SelectItem("h");
            Assert.Equal(MyEnum.Half, enumProperty.Get());            
        }

        [Fact]
        public void ComboBoxHasCorrectItemCount()
        {
            var comboBox = new ControlFactory().Create<ComboBox>();
            var enumProperty = new Property<MyEnum>();
            var binding = new ComboBoxEnumBinding<MyEnum>(comboBox, enumProperty);

            binding.CreateItems();
            binding.Bind();
            
            Assert.Equal(comboBox.ItemsPanel.Children.Count, Enum.GetValues(typeof(MyEnum)).Length);
        }
    }
}