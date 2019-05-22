using Medja.Controls;
using Medja.OpenTk.Themes.BlackRed;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.OpenTk.Themes
{
    public class BlackRedTheme : ControlFactory
    {
        public BlackRedTheme()
        {
            DefaultFont = new Font();
            DefaultFont.Name = "Monospace";
        }
        
        protected override Button CreateButton()
        {
            var result = base.CreateButton();
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 50;
            result.Padding.Top = 14;
            result.Font.Color = BlackRedThemeValues.PrimaryTextColor;
            result.TextAlignment = TextAlignment.Center;

            result.Renderer = new ButtonRenderer(result);
            
            return result;
        }

        protected override CheckBox CreateCheckBox()
        {
            var result = base.CreateCheckBox();
            result.Renderer = new CheckBoxRenderer(result);
            result.Position.Height = 26;

            return result;
        }

        protected override ComboBox CreateComboBox()
        {
            var result = base.CreateComboBox();
            result.Background = BlackRedThemeValues.PrimaryColor;
            result.Position.Height = 30;
            
            result.ItemsPanel.ChildrenHeight = 30;
            result.ItemsPanel.SpaceBetweenChildren = 2;

            result.Renderer = new ComboBoxRenderer(result);
            
            return result;
        }

        public override void ComboBoxMenuItemStyle(MenuItem menuItem)
        {
            base.ComboBoxMenuItemStyle(menuItem);
            menuItem.Background = BlackRedThemeValues.PrimaryColor;
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

            return result;
        }

        protected override Graph2D CreateGraph2D()
        {
            var result = base.CreateGraph2D();
            result.Renderer = new Graph2DRenderer(result);

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

        protected override MedjaWindow CreateMedjaWindow()
        {
            // do not use base.CreateMedjaWindow because
            // OpenTkWindow implements some additional features needed
            // by OpenTk
            var result = new OpenTkWindow();
            return result;
        }

        protected override MenuItem CreateMenuItem()
        {
            var result = base.CreateMenuItem();
            result.Position.Height = 40;
            result.Renderer = new MenuItemRenderer(result);

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
            result.Renderer = new SliderRenderer(result);
            result.Position.Height = 30;

            return result;
        }

        protected override TabControl CreateTabControl()
        {
            var result = base.CreateTabControl();
            result.Renderer = new TabControlRenderer(result);

            return result;
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();
            result.Renderer = new TextBoxRenderer(result);
            result.Font.Color = BlackRedThemeValues.PrimaryTextColor;
            result.Background = BlackRedThemeValues.PrimaryColor;
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 26;

            return result;
        }

        protected override TextBlock CreateTextBlock()
        {
            var result = base.CreateTextBlock();
            result.Renderer = new TextBlockRenderer(result);
            result.Font.Color = BlackRedThemeValues.PrimaryTextColor;
            result.Font.Name = DefaultFont.Name;
            result.Position.Height = 23;

            return result;
        }

        protected override ProgressBar CreateProgressBar()
        {
            var result = base.CreateProgressBar();
            result.Renderer = new ProgressBarRenderer(result);

            return result;
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
    }
}
