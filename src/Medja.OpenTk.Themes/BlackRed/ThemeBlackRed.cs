using Medja.Controls;
using Medja.Controls.Buttons;
using Medja.Controls.Container;
using Medja.Controls.Dialogs;
using Medja.Controls.Graph2D;
using Medja.Controls.Menu;
using Medja.Controls.Panels;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.OpenTk.Window;
using Medja.Primitives;
using Medja.Properties.Binding;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class ThemeBlackRed : ThemeOpenTKBase
    {
        private readonly MedjaOpenTKWindowSettings _windowSettings;

        public ThemeBlackRed(MedjaOpenTKWindowSettings windowSettings)
        {
            _windowSettings = windowSettings;
            DefaultFont = new Font();
            DefaultFont.Name = "Monospace";
            DefaultFont.Color = ThemeBlackRedValues.PrimaryTextColor;
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
            button.Renderer = new ButtonRenderer(button);

            button.PropertyBackground.UpdateFrom(
                button.PropertyIsEnabled,
                button.InputState.PropertyIsLeftMouseDown,
                GetButtonBackground);

            button.Font.PropertyColor.UpdateFrom(button.PropertyIsEnabled, GetButtonFontColor);
        }

        private Color GetButtonBackground(bool isEnabled, bool isLeftMouseDown)
        {
            if (!isEnabled)
                return null;

            return isLeftMouseDown
                ? ThemeBlackRedValues.PrimaryColor
                : ThemeBlackRedValues.SecondaryColor;
        }

        private Color GetButtonFontColor(bool isEnabled)
        {
            return isEnabled
                ? ThemeBlackRedValues.PrimaryTextColor
                : ThemeBlackRedValues.PrimaryTextColor.GetDisabled();
        }

        protected override CheckBox CreateCheckBox()
        {
            var result = base.CreateCheckBox();
            result.Renderer = new CheckBoxRenderer(result);
            result.Position.Height = 26;
            result.Padding.Left = 35;
            result.Padding.Top = 2;

            result.PropertyBackground.UpdateFrom(result.PropertyIsChecked, GetCheckBoxBackground);
            result.Font.PropertyColor.UpdateFrom(result.PropertyIsChecked, GetCheckBoxFontColor);

            return result;
        }

        private Color GetCheckBoxBackground(bool isChecked)
        {
            return isChecked
                ? ThemeBlackRedValues.SecondaryColor
                : ThemeBlackRedValues.PrimaryColor;
        }

        private Color GetCheckBoxFontColor(bool isChecked)
        {
            return isChecked
                ? ThemeDarkBlueValues.PrimaryTextColor
                : ThemeDarkBlueValues.PrimaryTextColor.GetDisabled();
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

        private Color GetComboBoxBackground(bool isEnabled)
        {
            return isEnabled
                ? ThemeBlackRedValues.PrimaryColor
                : ThemeBlackRedValues.PrimaryColor.GetDisabled();
        }

        protected override void SetupComboBoxMenuItem(ComboBoxMenuItem item)
        {
            base.SetupComboBoxMenuItem(item);

            item.Position.Height = 30;
            item.Renderer = new MenuItemRenderer(item, DefaultFont.Name, DefaultFont.Color);

            item.PropertyBackground.UpdateFrom(
                item.PropertyIsSelected,
                item.PropertyIsEnabled,
                GetComboBoxMenuItemBackground);
        }

        private Color GetComboBoxMenuItemBackground(bool isSelected, bool isEnabled)
        {
            var result = isSelected
                ? ThemeBlackRedValues.SecondaryColor
                : ThemeBlackRedValues.PrimaryColor;

            if (!isEnabled)
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
            result.Background = ThemeBlackRedValues.PrimaryTextColor;
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
            result.Background = ThemeBlackRedValues.PrimaryColor;

            return result;
        }

        protected override MenuItem CreateMenuItem()
        {
            var result = base.CreateMenuItem();
            result.Position.Height = 40;
            result.Renderer = new MenuItemRenderer(result, DefaultFont.Name, DefaultFont.Color);

            result.PropertyBackground.UpdateFrom(
                result.PropertyIsSelected,
                result.PropertyIsEnabled,
                GetMenuItemBackground);

            return result;
        }

        private Color GetMenuItemBackground(bool isSelected, bool isEnabled)
        {
            var result = isSelected ? ThemeBlackRedValues.SecondaryColor : null;

            if (result != null && !isEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override NumericKeypad CreateNumericKeypad()
        {
            var result = base.CreateNumericKeypad();
            result.Renderer = new ControlRenderer(result);
            result.Background = ThemeBlackRedValues.PrimaryColor;

            return result;
        }

        protected override NumericKeypadDialog CreateNumericKeypadDialog()
        {
            var result = base.CreateNumericKeypadDialog();
            result.Background = ThemeBlackRedValues.PrimaryColor;
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
            result.Background = ThemeBlackRedValues.PrimaryColor;

            return result;
        }

        protected override Slider CreateSlider()
        {
            var result = base.CreateSlider();
            result.Position.Height = 30;
            result.Renderer = new SliderRenderer(result);

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetSliderBackground);
            result.PropertyThumbColor.UpdateFrom(result.PropertyIsEnabled, GetSliderThumbColor);

            return result;
        }

        private Color GetSliderBackground(bool isEnabled)
        {
            return isEnabled
                ? ThemeBlackRedValues.PrimaryColor
                : ThemeBlackRedValues.PrimaryColor.GetDisabled();
        }

        private Color GetSliderThumbColor(bool isEnabled)
        {
            return isEnabled
                ? ThemeBlackRedValues.PrimaryTextColor
                : ThemeBlackRedValues.PrimaryTextColor.GetDisabled();
        }

        protected override TabControl CreateTabControl()
        {
            var result = base.CreateTabControl();
            result.TabHeaderSpacing = 5;
            result.TabHeaderHeight = 40;
            result.TabHeaderMaxWidth = 200;
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
                GetTabItemBackground
            );

            return result;
        }

        private Color GetTabItemBackground(bool isSelected, bool isEnabled)
        {
            var result = isSelected
                ? ThemeBlackRedValues.SecondaryColor
                : ThemeBlackRedValues.PrimaryColor;

            if (!isEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();
            result.Renderer = new TextBoxRenderer(result);
            result.Font.Color = ThemeBlackRedValues.PrimaryTextColor;
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 30;

            result.PropertyBackground.UpdateFrom(
                result.PropertyIsFocused,
                result.PropertyIsEnabled,
                GetTextBoxBackground);

            return result;
        }

        private Color GetTextBoxBackground(bool isFocused, bool isEnabled)
        {
            var result = isFocused
                ? ThemeBlackRedValues.SecondaryColor
                : ThemeBlackRedValues.SecondaryLightColor;

            if (!isEnabled)
                result = result.GetDisabled();

            return result;
        }

        protected override TextBlock CreateTextBlock()
        {
            var result = base.CreateTextBlock();
            result.Renderer = new TextBlockRenderer(result);
            result.Font.Color = ThemeBlackRedValues.PrimaryTextColor;
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 30;

            return result;
        }

        protected override ProgressBar CreateProgressBar()
        {
            var result = base.CreateProgressBar();
            result.Renderer = new ProgressBarRenderer(result);
            result.Position.Height = 25;

            result.PropertyBackground.UpdateFrom(result.PropertyIsEnabled, GetProgressBarBackground);
            result.PropertyProgressColor.UpdateFrom(result.PropertyIsEnabled, GetProgressBarProgressColor);

            return result;
        }

        private Color GetProgressBarProgressColor(bool isEnabled)
        {
            return isEnabled
                ? ThemeBlackRedValues.SecondaryColor
                : ThemeBlackRedValues.SecondaryColor.GetDisabled();
        }

        private Color GetProgressBarBackground(bool isEnabled)
        {
            return isEnabled ?
                ThemeBlackRedValues.PrimaryColor :
                ThemeBlackRedValues.PrimaryColor.GetDisabled();
        }

        protected override QuestionDialog CreateQuestionDialog()
        {
            var result = base.CreateQuestionDialog();
            result.Renderer = new ControlRenderer(result);
            result.Background = ThemeBlackRedValues.PrimaryColor;

            return result;
        }

        protected override VerticalScrollBar CreateVerticalScrollBar()
        {
            var result = base.CreateVerticalScrollBar();
            result.Renderer = new BlackRed.VerticalScrollBarRenderer(result);
            result.Position.Width = 20;
            result.Background = ThemeBlackRedValues.PrimaryColor;

            return result;
        }

        protected override ScrollableContainer CreateScrollableContainer()
        {
            var result = base.CreateScrollableContainer();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override Medja.Controls.Window CreateWindow()
        {
            var result = new OpenTkWindow(_windowSettings);
            return result;
        }
    }
}
