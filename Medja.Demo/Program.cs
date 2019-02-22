using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using Medja.Primitives;

namespace Medja.Demo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run();
        }
        
        private readonly MedjaApplication _application;

        public Program()
        {
            var library = new MedjaOpenTkLibrary(new DemoTheme());
            library.RendererFactory = () => new OpenTkRenderer("Roboto");
            _application = MedjaApplication.Create(library);
        }

        public void Run()
        {
            CreateMainWindow();
            _application.Run();
        }

        private void CreateMainWindow()
        {
            var window = _application.CreateWindow();
            window.CenterOnScreen(800, 600);
            //window.Background = DemoThemeColors.Background;
            window.Content = CreateContent();

            _application.MainWindow = window;
        }

        private Control CreateContent()
        {
            var controlFactory = _application.Library.ControlFactory;
            
            var button = controlFactory.Create<Button>();
            button.Text = "Normal";

            var touchedButton = controlFactory.Create<Button>();
            touchedButton.Text = "Touched";
            touchedButton.InputState.IsLeftMouseDown = true;

            var disabledButton = controlFactory.Create<Button>();
            disabledButton.Text = "Disabled";
            disabledButton.IsEnabled = false;

            var buttonStackPanel = controlFactory.Create<HorizontalStackPanel>();
            buttonStackPanel.ChildrenWidth = 100;
            buttonStackPanel.SpaceBetweenChildren = 50;
            buttonStackPanel.Children.Add(button);
            buttonStackPanel.Children.Add(touchedButton);
            buttonStackPanel.Children.Add(disabledButton);

            var checkBox = controlFactory.Create<CheckBox>();
            checkBox.Text = "CheckBox";
            
            var checkedCheckBox = controlFactory.Create<CheckBox>();
            checkedCheckBox.IsChecked = true;
            checkedCheckBox.Text = "checked";

            var checkBoxDisabled = controlFactory.Create<CheckBox>();
            checkBoxDisabled.IsEnabled = false;
            checkBoxDisabled.Text = "disabled";
            
            var progressBar = controlFactory.Create<ProgressBar>();
            progressBar.MaxValue = 100;
            progressBar.Value = 75;

            var slider = controlFactory.Create<Slider>();
            slider.MaxValue = 100;
            slider.Value = 75;

            var textBox = controlFactory.Create<TextBox>();
            textBox.Text = "TextBox";

            var textBoxDisabled = controlFactory.Create<TextBox>();
            textBoxDisabled.IsEnabled = false;
            textBoxDisabled.Text = "disabled";

            var textBoxFocused = controlFactory.Create<TextBox>();
            textBoxFocused.Text = "focused";
            FocusManager.Default.SetFocus(textBoxFocused);

            var textBoxStackPanel = controlFactory.Create<HorizontalStackPanel>();
            textBoxStackPanel.ChildrenWidth = 150;
            textBoxStackPanel.SpaceBetweenChildren = 50;
            textBoxStackPanel.Children.Add(textBox);
            textBoxStackPanel.Children.Add(textBoxDisabled);
            textBoxStackPanel.Children.Add(textBoxFocused);
            
            var textBlock = controlFactory.Create<TextBlock>();
            textBlock.TextWrapping = TextWrapping.Auto;
            textBlock.Text = "Modern UI Design without Android or iOS. A Linux device that is supported by the mono runtime " +
                             "is enough. (ARM, MIPS, x86-64, x86, s390x 64bit, SPARC 32 bit, PowerPC)\n" +
                             ".NET Core support is going to come.";
            textBlock.Position.Height = 75;

            var checkBoxStackPanel = controlFactory.Create<HorizontalStackPanel>();
            checkBoxStackPanel.ChildrenWidth = 100;
            checkBoxStackPanel.SpaceBetweenChildren = 50;
            checkBoxStackPanel.Children.Add(checkBox);
            checkBoxStackPanel.Children.Add(checkedCheckBox);
            checkBoxStackPanel.Children.Add(checkBoxDisabled);
            
            var result = controlFactory.Create<VerticalStackPanel>();
            result.Padding.SetAll(20);
            result.Background = DemoThemeValues.Background;
            result.SpaceBetweenChildren = 25;
            result.Children.Add(buttonStackPanel);
            result.Children.Add(checkBoxStackPanel);
            result.Children.Add(progressBar);
            result.Children.Add(slider);
            result.Children.Add(textBoxStackPanel);
            result.Children.Add(textBlock);
            
            return result;
        }
    }
}