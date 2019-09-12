using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class DarkBlueTheme : ControlFactory
    {
        private readonly MedjaOpenTKWindowSettings _windowSettings;

        public DarkBlueTheme(MedjaOpenTKWindowSettings windowSettings)
        {
            _windowSettings = windowSettings;
            DefaultFont = new Font();
            DefaultFont.Name = "Roboto";
            DefaultFont.Color = DarkBlueThemeValues.PrimaryTextColor;
        }
        
        protected override Button CreateButton()
        {
            var result = base.CreateButton();
            SetupButton(result);
            
            return result;
        }

        private void SetupButton(Button button)
        {
            button.Bind(p => p.PropertyBackground,
                GetButtonBackground,
                p => p.PropertyIsEnabled);

            button.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            button.Position.Height = 40;
            button.TextAlignment = TextAlignment.Center;
            button.Padding.Top = 9;

            button.Renderer = new ButtonRenderer(button);
        }

        private static Color GetButtonBackground(Button button)
        {
            return button.IsEnabled ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.PrimaryColor.GetDisabled();
        }

        protected override Canvas CreateCanvas()
        {
            var result = base.CreateCanvas();
            
            result.Renderer = new ControlRenderer(result);
            return result;
        }

        protected override CheckBox CreateCheckBox()
        {
            var result = base.CreateCheckBox();

            result.Bind(p => p.PropertyBackground, 
                GetCheckBoxBackground, 
                p => p.PropertyIsChecked, 
                p => p.PropertyIsEnabled);
            
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 19;
            result.Padding.Left = 27;

            result.Renderer = new CheckBoxRenderer(result);

            return result;
        }

        private static Color GetCheckBoxBackground(CheckBox checkBox)
        {
            var result = checkBox.IsChecked ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.ControlBackground;

            if (!checkBox.IsEnabled)
                result = result.GetDisabled();

            return result;
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

        private static Color GetComboBoxBackground(ComboBox comboBox)
        {
            return comboBox.IsEnabled 
                ? Colors.White 
                : Colors.White.GetDisabled();
        }

        protected override ComboBoxMenuItem CreateComboBoxMenuItem()
        {
            var result = base.CreateComboBoxMenuItem();

            result.Bind(p => p.PropertyBackground, 
                GetComboBoxMenuItemBackground, 
                p => p.PropertyIsSelected,
                p => p.PropertyIsEnabled);
            
            result.Position.Height = 25;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            return result;
        }

        private static Color GetComboBoxMenuItemBackground(ComboBoxMenuItem menuItem)
        {
            var result = menuItem.IsSelected ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.ControlBackground;

            if (!menuItem.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override ContentControl CreateContentControl()
        {
            var result = base.CreateContentControl();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override Control CreateControl()
        {
            var result = base.CreateControl();
            result.Renderer = new ControlRenderer(result);
            
            return result;
        }

        protected override DockPanel CreateDockPanel()
        {
            var result = base.CreateDockPanel();

            result.Renderer = new ControlRenderer(result);
            return result;
        }
        
        protected override HorizontalStackPanel CreateHorizontalStackPanel()
        {
            var result = base.CreateHorizontalStackPanel();
            result.Renderer = new ControlRenderer(result);

            return result;
        }
        
        protected override Image CreateImage()
        {
            var result = base.CreateImage();
            result.Renderer = new ImageRenderer(result);

            return result;
        }
        
        protected override MenuItem CreateMenuItem()
        {
            var result = base.CreateMenuItem();

            result.Bind(p => p.PropertyBackground, 
                GetMenuItemBackground, 
                p => p.PropertyIsSelected,
                p => p.PropertyIsEnabled);
            
            result.Position.Height = 25;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            return result;
        }

        private static Color GetMenuItemBackground(MenuItem menuItem)
        {
            var result = menuItem.IsSelected ? DarkBlueThemeValues.PrimaryColor : null;

            if (result != null && !menuItem.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override Popup CreatePopup()
        {
            var result = base.CreatePopup();
            result.Bind(p => p.PropertyBackground, GetPopupBackground, p => p.PropertyIsEnabled);

            result.Renderer = new PopupRenderer(result);
			return result;
		}

        private Color GetPopupBackground(Popup popup)
        {
            return popup.IsEnabled ? DarkBlueThemeValues.ControlBackground : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        protected override RepeatButton CreateRepeatButton()
        {
            var result = base.CreateRepeatButton();
            SetupButton(result);
            
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
            
            result.Position.Height = 25;

            result.Renderer = new ProgressBarRenderer(result);

            return result;
        }

        private Color GetProgressBarProgressColor(ProgressBar progressBar)
        {
            return progressBar.IsEnabled 
                ? DarkBlueThemeValues.PrimaryColor 
                : DarkBlueThemeValues.PrimaryColor.GetDisabled();
        }

        private Color GetProgressBarBackground(ProgressBar progressBar)
        {
            return progressBar.IsEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
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
            result.Background = DarkBlueThemeValues.ControlBackground;
            
            return result;
        }
        
        protected override NumericKeypadDialog CreateNumericKeypadDialog()
        {
            var result = base.CreateNumericKeypadDialog();
            result.Background = DarkBlueThemeValues.ControlBackground;
            result.Renderer = new ControlRenderer(result);

            return result;
        }
        
        protected override InputBoxDialog CreateInputBoxDialog()
        {
            var result = base.CreateInputBoxDialog();
            result.Renderer = new ControlRenderer(result);
            result.Background = DarkBlueThemeValues.ControlBackground;

            return result;
        }
        
        protected override QuestionDialog CreateQuestionDialog()
        {
            var result = base.CreateQuestionDialog();
            result.Renderer = new ControlRenderer(result);
            result.Background = DarkBlueThemeValues.ControlBackground;

            return result;
        }
        
        protected override DialogButtonsControl CreateDialogButtonsControl()
        {
            var result = base.CreateDialogButtonsControl();
            result.ButtonWidth = 150;
            result.ButtonHeight = 40;
            result.Margin.Top = 20;
            
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

            result.Position.Height = 20;
            
            result.Renderer = new SliderRenderer(result);

            return result;
        }

        private Color GetSliderBackground(Slider slider)
        {
            return slider.IsEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        private Color GetSliderThumbColor(Slider slider)
        {
            return slider.IsEnabled
                ? DarkBlueThemeValues.PrimaryTextColor
                : DarkBlueThemeValues.PrimaryTextColor.GetDisabled();
        }

        protected override TabControl CreateTabControl()
        {
            var result = base.CreateTabControl();
            
            result.TabHeaderSpacing = 5;
            result.TabHeaderHeight = 30;
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
        
        private static Color GetTabItemBackground(TabItem tabItem)
        {
            var result = tabItem.IsSelected ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.ControlBackground;

            if (!tabItem.IsEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override TablePanel CreateTablePanel()
        {
            var result = base.CreateTablePanel();
            
            result.Renderer = new ControlRenderer(result);
            return result;
        }
        
        protected override TextBlock CreateTextBlock()
        {
            var result = base.CreateTextBlock();

            result.Bind(p => p.PropertyBackground,
                GetTextBlockBackground,
                p => p.PropertyIsEnabled);
            
            result.Font.Color = Colors.Black;
            result.Padding.SetAll(5);
            result.Position.Height = 30;

            result.Renderer = new TextBlockRenderer(result);

            return result;
        }

        private Color GetTextBlockBackground(TextBlock textBlock)
        {
            return textBlock.IsEnabled ? Colors.White : Colors.White.GetDisabled();
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();

            result.Bind(p => p.PropertyBackground,
                GetTextBoxBackground, 
                p => p.PropertyIsEnabled);
            
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 37;
            result.Padding.Top = 9;
            result.Padding.SetLeftAndRight(10);

            result.Renderer = new TextBoxRenderer(result);

            return result;
        }

        private Color GetTextBoxBackground(TextBox textBox)
        {
            return textBox.IsEnabled
                ? DarkBlueThemeValues.ControlBorder
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        protected override TextEditor CreateTextEditor()
        {
            var result = base.CreateTextEditor();
            result.Bind(p => p.PropertyBackground,
                GetTextEditorBackground, 
                p => p.PropertyIsEnabled);
            result.Renderer = new TextEditorRenderer(result);

            return result;
        }

        private Color GetTextEditorBackground(TextEditor textEditor)
        {
            return textEditor.IsEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        protected override VerticalStackPanel CreateVerticalStackPanel()
        {
            var result = base.CreateVerticalStackPanel();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override Window CreateWindow()
        {
            var result = new OpenTkWindow(_windowSettings);
            result.Background = DarkBlueThemeValues.WindowBackground;
            result.Renderer = new WindowRenderer(result);

            return result;
        }
    }
}