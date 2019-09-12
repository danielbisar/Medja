using System;
using Medja.Controls;
using Medja.OpenTk.Themes.BlackRed;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Primitives;
using Medja.Theming;
using ButtonRenderer = Medja.OpenTk.Themes.BlackRed.ButtonRenderer;
using CheckBoxRenderer = Medja.OpenTk.Themes.BlackRed.CheckBoxRenderer;
using ComboBoxRenderer = Medja.OpenTk.Themes.BlackRed.ComboBoxRenderer;
using TabControlRenderer = Medja.OpenTk.Themes.BlackRed.TabControlRenderer;
using TextBlockRenderer = Medja.OpenTk.Themes.BlackRed.TextBlockRenderer;
using TextBoxRenderer = Medja.OpenTk.Themes.BlackRed.TextBoxRenderer;

namespace Medja.OpenTk.Themes
{
    public class BlackRedTheme : ControlFactory
    {
        private readonly MedjaOpenTKWindowSettings _windowSettings;

        public BlackRedTheme(MedjaOpenTKWindowSettings windowSettings)
        {
            _windowSettings = windowSettings;
            DefaultFont = new Font();
            DefaultFont.Name = "Monospace";
            DefaultFont.Color = BlackRedThemeValues.PrimaryTextColor;
        }
        
        protected override Button CreateButton()
        {
            var result = base.CreateButton();
            SetupButton(result);
            
            return result;
        }

        private void SetupButton(Button button)
        {
            button.Font.Name = DefaultFont.Name;
            button.Position.Height = 50;
            button.Padding.Top = 14;
            button.TextAlignment = TextAlignment.Center;

            button.Bind(p => p.PropertyBackground,
                GetButtonBackground,
                p => p.PropertyIsEnabled,
                p => p.InputState.PropertyIsLeftMouseDown);

            button.Bind(p => p.Font.PropertyColor,
                GetButtonFontColor,
                p => p.PropertyIsEnabled);

            button.Renderer = new ButtonRenderer(button);
        }

        private Color GetButtonFontColor(Button button)
        {
            return button.IsEnabled 
                ? BlackRedThemeValues.PrimaryTextColor 
                : BlackRedThemeValues.PrimaryTextColor.GetDisabled();
        }

        private Color GetButtonBackground(Button button)
        {
            if (!button.IsEnabled)
                return null;

            return button.InputState.IsLeftMouseDown 
                ? BlackRedThemeValues.PrimaryColor 
                : BlackRedThemeValues.SecondaryColor;
        }

        protected override CheckBox CreateCheckBox()
        {
            var result = base.CreateCheckBox();
            result.Renderer = new CheckBoxRenderer(result);
            result.Position.Height = 26;
            result.Padding.Left = 35;
            result.Padding.Top = 2;

            result.Bind(p => p.PropertyBackground, 
                GetCheckBoxBackground, 
                p => p.PropertyIsChecked);

            result.Bind(p => p.Font.PropertyColor, 
                GetCheckBoxFontColor, 
                p => p.PropertyIsEnabled);
            
            return result;
        }

        private Color GetCheckBoxFontColor(CheckBox checkBox)
        {
            return checkBox.IsEnabled
                ? DarkBlueThemeValues.PrimaryTextColor
                : DarkBlueThemeValues.PrimaryTextColor.GetDisabled();
        }

        private Color GetCheckBoxBackground(CheckBox checkBox)
        {
            return checkBox.IsChecked 
                ? BlackRedThemeValues.SecondaryColor 
                : BlackRedThemeValues.PrimaryColor;
        }

        protected override ComboBox CreateComboBox()
        {
            var result = base.CreateComboBox();
            
            result.Bind(p => p.PropertyBackground, 
                GetComboBoxBackground, 
                p => p.PropertyIsEnabled);
            
            result.Position.Height = 30;
            
            result.ItemsPanel.ChildrenHeight = 30;
            result.ItemsPanel.SpaceBetweenChildren = 2;

            result.Renderer = new ComboBoxRenderer(result);
            
            return result;
        }

        private Color GetComboBoxBackground(ComboBox comboBox)
        {
            return comboBox.IsEnabled 
                ? BlackRedThemeValues.PrimaryColor 
                : BlackRedThemeValues.PrimaryColor.GetDisabled();
        }

        protected override ComboBoxMenuItem CreateComboBoxMenuItem()
        {
            var result = base.CreateComboBoxMenuItem();

            result.Bind(p => p.PropertyBackground, 
                GetComboBoxMenuItemBackground, 
                p => p.PropertyIsSelected,
                p => p.PropertyIsEnabled);
            
            result.Position.Height = 30;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            return result;
        }

        private Color GetComboBoxMenuItemBackground(ComboBoxMenuItem menuItem)
        {
            var result = menuItem.IsSelected 
                ? BlackRedThemeValues.SecondaryColor
                : BlackRedThemeValues.PrimaryColor;

            if (!menuItem.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override Control CreateControl()
        {
            var result = base.CreateControl();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override ContentControl CreateContentControl()
        {
            var result = base.CreateContentControl();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override Dialog CreateDialog()
        {
            var result = base.CreateDialog();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override DialogButtonsControl CreateDialogButtonsControl()
        {
            var result = base.CreateDialogButtonsControl();
            result.ButtonWidth = 150;
            result.ButtonHeight = 50;
            result.Margin.Top = 20;
            
            return result;
        }

        protected override Graph2D CreateGraph2D()
        {
            var result = base.CreateGraph2D();

            result.AxisX.Position.Width = 75;
            result.AxisX.Margin.Bottom = 50;
            result.AxisY.Position.Height = 50;
            
            //result.Renderer = new Graph2DRenderer(result);

            return result;
        }

        protected override Graph2DAxis CreateGraph2DAxis()
        {
            var result = base.CreateGraph2DAxis();
            result.Background = BlackRedThemeValues.PrimaryTextColor;
            result.Renderer = new Graph2DAxisRenderer(result);
            result.Font.Color = DefaultFont.Color;
            result.Font.Size = 10;
            result.Font.Name = DefaultFont.Name;
            
            return result;
        }

        protected override Image CreateImage()
        {
            var result = base.CreateImage();
            result.Renderer = new ImageRenderer(result);

            return result;
        }

        protected override InputBoxDialog CreateInputBoxDialog()
        {
            var result = base.CreateInputBoxDialog();
            result.Renderer = new ControlRenderer(result);
            result.Background = BlackRedThemeValues.PrimaryColor;

            return result;
        }

        protected override MenuItem CreateMenuItem()
        {
            var result = base.CreateMenuItem();
            result.Position.Height = 40;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            result.Bind(p => p.PropertyBackground, 
                GetMenuItemBackground, 
                p => p.PropertyIsSelected,
                p => p.PropertyIsEnabled);
            
            return result;
        }

        private Color GetMenuItemBackground(MenuItem menuItem)
        {
            Console.WriteLine("GetMenuItemBackground");
            
            var result = menuItem.IsSelected ? BlackRedThemeValues.SecondaryColor : null;

            if (result != null && !menuItem.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override NumericKeypad CreateNumericKeypad()
        {
            var result = base.CreateNumericKeypad();
            result.Renderer = new ControlRenderer(result);
            result.Background = BlackRedThemeValues.PrimaryColor;

            return result;
        }

        protected override NumericKeypadDialog CreateNumericKeypadDialog()
        {
            var result = base.CreateNumericKeypadDialog();
            result.Background = BlackRedThemeValues.PrimaryColor;
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override Popup CreatePopup()
        {
            var result = base.CreatePopup();
            result.Renderer = new PopupRenderer(result);

            return result;
        }

        protected override RepeatButton CreateRepeatButton()
        {
            var result = base.CreateRepeatButton();
            SetupButton(result);

            return result;
        }

        protected override ScrollingGrid CreateScrollingGrid()
        {
            var result = base.CreateScrollingGrid();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override SimpleMessageDialog CreateSimpleMessageDialog()
        {
            var result = base.CreateSimpleMessageDialog();
            result.Renderer = new ControlRenderer(result);
            result.Background = BlackRedThemeValues.PrimaryColor;

            return result;
        }

        protected override Slider CreateSlider()
        {
            var result = base.CreateSlider();

            result.Bind(p => p.PropertyBackground,
                GetSliderBackground,
                p => p.PropertyIsEnabled);

            result.Bind(p => p.PropertyThumbColor, 
                GetSliderThumbColor, 
                p => p.PropertyIsEnabled);

            result.Position.Height = 30;
            
            result.Renderer = new SliderRenderer(result);

            return result;
        }

        private Color GetSliderThumbColor(Slider slider)
        {
            return slider.IsEnabled
                ? BlackRedThemeValues.PrimaryTextColor
                : BlackRedThemeValues.PrimaryTextColor.GetDisabled();
        }

        private Color GetSliderBackground(Slider slider)
        {
            return slider.IsEnabled
                ? BlackRedThemeValues.PrimaryColor
                : BlackRedThemeValues.PrimaryColor.GetDisabled();
        }

        protected override TabControl CreateTabControl()
        {
            var result = base.CreateTabControl();
            result.TabHeaderSpacing = 5;
            result.TabHeaderHeight = 40;
            result.TabHeaderWidth = 200;
            result.Padding.SetLeftAndRight(5);
            result.Padding.SetTopAndBottom(10);
            result.Renderer = new TabControlRenderer(result);
            
            return result;
        }

        protected override TabItem CreateTabItem()
        {
            var result = base.CreateTabItem();
            
            result.Bind(
                p => p.PropertyBackground, 
                GetTabItemBackground, 
                p => p.PropertyIsSelected, 
                p => p.PropertyIsEnabled);
            
            result.Renderer = new TabItemRenderer(result, DefaultFont.Color);

            return result;
        }

        private Color GetTabItemBackground(TabItem tabItem)
        {
            var result = tabItem.IsSelected 
                ? BlackRedThemeValues.SecondaryColor
                : BlackRedThemeValues.PrimaryColor;

            if (!tabItem.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();
            result.Renderer = new TextBoxRenderer(result);
            result.Font.Color = BlackRedThemeValues.PrimaryTextColor;
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 30;

            result.Bind(p => p.PropertyBackground, 
                GetTextBoxBackground, 
                p => p.PropertyIsFocused, 
                p => p.PropertyIsEnabled);
            
            return result;
        }

        private Color GetTextBoxBackground(TextBox textBox)
        {
            var result = textBox.IsFocused
                ? BlackRedThemeValues.SecondaryColor
                : BlackRedThemeValues.SecondaryLightColor;

            if (!textBox.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override TextBlock CreateTextBlock()
        {
            var result = base.CreateTextBlock();
            result.Renderer = new TextBlockRenderer(result);
            result.Font.Color = BlackRedThemeValues.PrimaryTextColor;
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 30;

            return result;
        }

        protected override ProgressBar CreateProgressBar()
        {
            var result = base.CreateProgressBar();
            
            result.Bind(p => p.PropertyBackground, 
                GetProgressBarBackground, 
                p => p.PropertyIsEnabled);

            result.Bind(p => p.PropertyProgressColor, 
                GetProgressBarProgressColor,
                p => p.PropertyIsEnabled);
            
            result.Renderer = new ProgressBarRenderer(result);
            result.Position.Height = 25;
            
            return result;
        }

        private Color GetProgressBarProgressColor(ProgressBar progressBar)
        {
            return progressBar.IsEnabled
                ? BlackRedThemeValues.SecondaryColor
                : BlackRedThemeValues.SecondaryColor.GetDisabled();
        }

        private Color GetProgressBarBackground(ProgressBar progressBar)
        {
            return progressBar.IsEnabled ? 
                BlackRedThemeValues.PrimaryColor : 
                BlackRedThemeValues.PrimaryColor.GetDisabled();
        }

        protected override QuestionDialog CreateQuestionDialog()
        {
            var result = base.CreateQuestionDialog();
            result.Renderer = new ControlRenderer(result);
            result.Background = BlackRedThemeValues.PrimaryColor;

            return result;
        }

        protected override VerticalScrollBar CreateVerticalScrollBar()
        {
            var result = base.CreateVerticalScrollBar();
            result.Renderer = new VerticalScrollBarRenderer(result);
            result.Position.Width = 20;
            result.Background = BlackRedThemeValues.PrimaryColor;

            return result;
        }

        protected override ScrollableContainer CreateScrollableContainer()
        {
            var result = base.CreateScrollableContainer();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override Window CreateWindow()
        {
            var result = new OpenTkWindow(_windowSettings);
            return result;
        }
    }
}
