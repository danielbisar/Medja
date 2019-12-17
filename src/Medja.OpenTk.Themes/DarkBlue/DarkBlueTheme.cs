using Medja.Controls;
using Medja.Primitives;
using Medja.Properties.Binding;
using Medja.Theming;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class DarkBlueTheme : ControlFactory
    {
        public static void SetButtonBackground(Control control)
        {
            control.PropertyBackground.UpdateFrom(control.PropertyIsEnabled, GetButtonBackground);
        }

        public static Color GetButtonBackground(bool isEnabled)
        {
            return isEnabled ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.PrimaryColor.GetDisabled();
        }

        public static Color GetCheckBoxBackground(bool isChecked, bool isEnabled)
        {
            var result = isChecked ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.ControlBackground;

            if (!isEnabled)
                result = result.GetDisabled();

            return result;
        }

        public static Color GetComboBoxBackground(bool isEnabled)
        {
            return isEnabled
                ? Colors.White
                : Colors.White.GetDisabled();
        }

        public static Color GetComboBoxMenuItemBackground(bool isSelected, bool isEnabled)
        {
            var result = isSelected ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.ControlBackground;

            if (!isEnabled)
                result = result.GetDisabled();

            return result;
        }

        public static Color GetMenuItemBackground(bool isSelected, bool isEnabled)
        {
            var result = isSelected ? DarkBlueThemeValues.PrimaryColor : null;

            if (result != null && !isEnabled)
                result = result.GetDisabled();

            return result;
        }

        public static Color GetPopupBackground(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        public static Color GetProgressBarBackground(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        public static Color GetProgressBarProgressColor(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.PrimaryColor
                : DarkBlueThemeValues.PrimaryColor.GetDisabled();
        }

        public static Color GetSliderBackground(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        public static Color GetSliderThumbColor(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.PrimaryTextColor
                : DarkBlueThemeValues.PrimaryTextColor.GetDisabled();
        }

        public static Color GetTabItemBackground(bool isSelected, bool isEnabled)
        {
            var result = isSelected ? DarkBlueThemeValues.PrimaryColor : DarkBlueThemeValues.ControlBackground;

            if (!isEnabled)
                result = result.GetDisabled();

            return result;
        }

        public static Color GetTextBlockBackground(bool isEnabled)
        {
            return isEnabled ? Colors.White : Colors.White.GetDisabled();
        }

        public static Color GetTextBoxBackground(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.ControlBorder
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }

        public static Color GetTextEditorBackground(bool isEnabled)
        {
            return isEnabled
                ? DarkBlueThemeValues.ControlBackground
                : DarkBlueThemeValues.ControlBackground.GetDisabled();
        }
        
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
            SetButtonBackground(button);

            button.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            button.Position.Height = 40;
            button.TextAlignment = TextAlignment.Center;
            button.Padding.Top = 9;

            button.Renderer = new ButtonRenderer(button);
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
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 19;
            result.Padding.Left = 27;
            result.Renderer = new CheckBoxRenderer(result);

            result.PropertyBackground.UpdateFrom(
                result.PropertyIsChecked,
                result.PropertyIsEnabled,
                GetCheckBoxBackground);
            
            return result;
        }

        protected override ComboBox CreateComboBox()
        {
            var result = base.CreateComboBox();

            result.Position.Height = 30;
            result.ItemsPanel.ChildrenHeight = 30;
            result.ItemsPanel.SpaceBetweenChildren = 2;
            result.Renderer = new ComboBoxRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetComboBoxBackground);
            
            return result;
        }

        protected override ComboBoxMenuItem CreateComboBoxMenuItem()
        {
            var result = base.CreateComboBoxMenuItem();

            result.Position.Height = 25;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            result.PropertyBackground.UpdateFrom(
                result.PropertyIsSelected,
                result.PropertyIsEnabled,
                GetComboBoxMenuItemBackground);

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
            result.Position.Height = 25;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            result.PropertyBackground.UpdateFrom(
                result.PropertyIsSelected, 
                result.PropertyIsEnabled,
                GetMenuItemBackground);
            
            return result;
        }

        protected override Popup CreatePopup()
        {
            var result = base.CreatePopup();
            result.Renderer = new PopupRenderer(result);
            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetPopupBackground);
            
            return result;
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
            result.Position.Height = 25;
            result.Renderer = new ProgressBarRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetProgressBarBackground);
            result.PropertyProgressColor.UpdateFrom(result.PropertyIsEnabled, GetProgressBarProgressColor);

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
            result.Position.Height = 20;
            result.Renderer = new SliderRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetSliderBackground);
            result.PropertyThumbColor.UpdateFrom(result.PropertyIsEnabled, GetSliderThumbColor);

            return result;
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
            result.Renderer = new TabItemRenderer(result, DefaultFont.Color);

            result.PropertyBackground.UpdateFrom(
                result.PropertyIsSelected, 
                result.PropertyIsEnabled,
                GetTabItemBackground);

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
            result.Font.Color = Colors.Black;
            result.Padding.SetAll(5);
            result.Position.Height = 30;
            result.Renderer = new TextBlockRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetTextBlockBackground);

            return result;
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 37;
            result.Padding.Top = 9;
            result.Padding.SetLeftAndRight(10);
            result.Renderer = new TextBoxRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetTextBoxBackground);
            
            return result;
        }

        protected override TextEditor CreateTextEditor()
        {
            var result = base.CreateTextEditor();
            result.Renderer = new TextEditorRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetTextEditorBackground);
            
            return result;
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