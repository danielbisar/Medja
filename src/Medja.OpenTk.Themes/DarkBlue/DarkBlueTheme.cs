using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class DarkBlueTheme : ControlFactory
    {
        public DarkBlueTheme()
        {
            DefaultFont = new Font();
            DefaultFont.Name = "Roboto";
        }
        
        protected override Button CreateButton()
        {
            var result = base.CreateButton();
            result.Background = DarkBlueThemeValues.PrimaryColor;
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 40;
            //result.Position.Width = 100;
            result.TextAlignment = TextAlignment.Center;
            result.Padding.Top = 9;
            result.Padding.SetLeftAndRight(0);

            result.Renderer = new ButtonRenderer(result);

            return result;
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
            result.Background = DarkBlueThemeValues.ControlBackground;
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 19;
            result.Padding.Left = 27;

            result.Renderer = new CheckBoxRenderer(result);

            return result;
        }

        protected override ComboBox CreateComboBox()
        {
            var result = base.CreateComboBox();
            result.Background = Colors.White;
            result.Position.Height = 30;
            
            result.ItemsPanel.ChildrenHeight = 30;
            result.ItemsPanel.SpaceBetweenChildren = 2;

            result.Renderer = new ComboBoxRenderer(result);
            
            return result;
        }

        public override void ComboBoxMenuItemStyle(MenuItem menuItem)
        {
            base.ComboBoxMenuItemStyle(menuItem);
            menuItem.Background = DarkBlueThemeValues.ControlBackground;
        }

        protected override ContentControl CreateContentControl()
        {
            var result = base.CreateContentControl();
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

        protected override MedjaWindow CreateMedjaWindow()
        {
            var result = new OpenTkWindow();
            result.Background = Colors.Black;

            return result;
        }

        protected override Popup CreatePopup()
        {
            var result = base.CreatePopup();
            result.Background = DarkBlueThemeValues.ControlBackground;

            result.Renderer = new PopupRenderer(result);
			return result;
		}            

        protected override MenuItem CreateMenuItem()
        {
            var result = base.CreateMenuItem();
            result.Position.Height = 25;
            result.Renderer = new MenuItemRenderer(result);

            return result;
        }

        protected override ProgressBar CreateProgressBar()
        {
            var result = base.CreateProgressBar();
            result.Background = DarkBlueThemeValues.ControlBackground;
            result.Foreground = DarkBlueThemeValues.PrimaryColor;
            result.Position.Height = 25;

            result.Renderer = new ProgressBarRenderer(result);

            return result;
        }

        protected override ScrollingGrid CreateScrollingGrid()
        {
            var result = base.CreateScrollingGrid();

            result.Renderer = new ControlRenderer(result);
            return result;
        }
        
        protected override Slider CreateSlider()
        {
            var result = base.CreateSlider();
            result.Background = DarkBlueThemeValues.ControlBackground;
            result.Foreground = DarkBlueThemeValues.PrimaryTextColor;
            result.Renderer = new SliderRenderer(result);

            return result;
        }

        protected override TablePanel CreateTablePanel()
        {
            var result = base.CreateTablePanel();
            
            result.Renderer = new ControlRenderer(result);
            return result;
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();
            result.Background = DarkBlueThemeValues.ControlBackground;
            result.Font.Color = DarkBlueThemeValues.PrimaryTextColor;
            result.Position.Height = 37;
            result.Padding.Top = 9;
            result.Padding.SetLeftAndRight(10);

            result.Renderer = new TextBoxRenderer(result);

            return result;
        }

        protected override TextBlock CreateTextBlock()
        {
            var result = base.CreateTextBlock();
            result.Background = Colors.White;
            result.Font.Color = Colors.Black;
            result.Padding.SetAll(5);

            result.Renderer = new TextBlockRenderer(result);

            return result;
        }

        protected override TextEditor CreateTextEditor()
        {
            var result = base.CreateTextEditor();
            result.Background = DarkBlueThemeValues.ControlBackground;

            result.Renderer = new TextEditorRenderer(result);

            return result;
        }

        protected override VerticalStackPanel CreateVerticalStackPanel()
        {
            var result = base.CreateVerticalStackPanel();
            result.Renderer = new ControlRenderer(result);

            return result;
        }
        
    }
}