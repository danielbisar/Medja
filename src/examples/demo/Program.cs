using System;
using Medja.Controls;
using Medja.OpenTk;
using Medja.Primitives;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Theming;
using OpenTK.Graphics.OpenGL4;

namespace Medja.Demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run();
        }

        private readonly MedjaApplication _application;

        public Program()
        {
            var settings = new MedjaOpenTKWindowSettings();
            var controlFactory = new CustomControlsFactory(settings);
            settings.ControlFactory = controlFactory;
            
            var library = new MedjaOpenTkLibrary();
            _application = MedjaApplication.Create(library);
            
            CreateMainWindow(controlFactory);
        }

        public void Run()
        {
            _application.Run();
        }

        private void CreateMainWindow(IControlFactory controlFactory)
        {
            var window = controlFactory.Create<Window>();
            window.CenterOnScreen(800, 800);
//            window.Position.X = 2;
//            window.Position.Y = 2;
//            window.Position.Width = 800;
//            window.Position.Height = 800;
            window.Background = DarkBlueThemeValues.WindowBackground;
            window.Content = CreateContent(window);

            _application.MainWindow = window;
        }

        private Control CreateContent(Window window)
        {
            var controlFactory = window.ControlFactory;
            
            //var simpleFactory = new SimpleFactory();
            //simpleFactory.AddAlias("Button", () => controlFactory.Create<Button>());
            
            //var button = simpleFactory.CreateFromFile<Button>("button.s");

            var button = controlFactory.Create<Button>();
            button.Text = "Normal";

            var touchedButton = controlFactory.Create<Button>();
            touchedButton.Text = "Touched";
            touchedButton.InputState.IsLeftMouseDown = true;

            var disabledButton = controlFactory.Create<Button>();
            disabledButton.Text = "Disabled";
            disabledButton.IsEnabled = false;

            var buttonStackPanel = controlFactory.Create<HorizontalStackPanel>();
            buttonStackPanel.ChildrenWidth = 150;
            buttonStackPanel.SpaceBetweenChildren = 50;
            buttonStackPanel.Add(button);
            buttonStackPanel.Add(touchedButton);
            buttonStackPanel.Add(disabledButton);

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
            progressBar.Margin.SetLeftAndRight(60);

            var slider = controlFactory.Create<Slider>();
            slider.MaxValue = 100;
            slider.Value = 75;
            
            var sliderPanel = controlFactory.Create<DockPanel>();
            sliderPanel.Add(Dock.Left, controlFactory.Create<RepeatButton>(b =>
            {
                b.Text = "-";
                b.Position.Width = 50;
                b.InputState.Clicked += (s, e) => slider.Value--;
                b.Margin.Right = 10;
            }));
            sliderPanel.Add(Dock.Right, controlFactory.Create<RepeatButton>(b =>
            {
                b.Text = "+";
                b.Position.Width = 50;
                b.InputState.Clicked += (s, e) => slider.Value++;
                b.Margin.Left = 10;
            }));
            sliderPanel.Add(Dock.Fill, slider);
            sliderPanel.Position.Height = sliderPanel.Children[0].Position.Height;

            
            progressBar.PropertyValue.BindTo(slider.PropertyValue);

            var textBox = controlFactory.Create<TextBox>();
            textBox.Text = "TextBox";

            var textBoxDisabled = controlFactory.Create<TextBox>();
            textBoxDisabled.IsEnabled = false;
            textBoxDisabled.Text = "disabled";

            var textBoxFocused = controlFactory.Create<TextBox>();
            textBoxFocused.Text = "focused";
            window.FocusManager.SetFocus(textBoxFocused);

            var textBoxStackPanel = controlFactory.Create<HorizontalStackPanel>();
            textBoxStackPanel.ChildrenWidth = 150;
            textBoxStackPanel.SpaceBetweenChildren = 50;
            textBoxStackPanel.Add(textBox);
            textBoxStackPanel.Add(textBoxDisabled);
            textBoxStackPanel.Add(textBoxFocused);

            var textBlock = controlFactory.Create<TextBlock>();
            textBlock.TextWrapping = TextWrapping.Auto;
            textBlock.Text = "Modern UI Design without Android or iOS. A Linux device that is supported by the mono runtime " +
                             "is enough. (ARM, MIPS, x86-64, x86, s390x 64bit, SPARC 32 bit, PowerPC)\n" +
                             ".NET Core support is going to come.";
            textBlock.Position.Height = 75;

            var checkBoxStackPanel = controlFactory.Create<HorizontalStackPanel>();
            checkBoxStackPanel.ChildrenWidth = 150;
            checkBoxStackPanel.SpaceBetweenChildren = 50;
            checkBoxStackPanel.Add(checkBox);
            checkBoxStackPanel.Add(checkedCheckBox);
            checkBoxStackPanel.Add(checkBoxDisabled);

            var comboBox = controlFactory.Create<ComboBox>();
            comboBox.Title = "Select an item";
            comboBox.Add("123");
            comboBox.Add("456");
            comboBox.Add("789");
            
            var disabledComboBox = controlFactory.Create<ComboBox>();
            disabledComboBox.IsEnabled = false;
            disabledComboBox.Title = "disabled";

            var menuItem = controlFactory.Create<MenuItem>();
            menuItem.Title = "MenuItem";
            
            var anotherStackPanel = controlFactory.Create<HorizontalStackPanel>();
            anotherStackPanel.ChildrenWidth = 200;
            anotherStackPanel.SpaceBetweenChildren = 50;
            anotherStackPanel.Add(comboBox);
            anotherStackPanel.Add(disabledComboBox);
            anotherStackPanel.Add(menuItem);
            
            var tabControl = controlFactory.Create<TabControl>();
            tabControl.AddTab(controlFactory.Create<TabItem>(p =>
            {
                p.Header = "Text Editor";
                p.Content = controlFactory.Create<TextEditor>(x => x.SetText("Multiline text editor\ncheck it out"));
            }));
            tabControl.AddTab(controlFactory.Create<TabItem>(p =>
            {
                p.Header = "Left Tabs";
                p.Content = controlFactory.Create<TabControl>(t =>
                {
                    t.TabHeaderPosition = TabHeaderPosition.Left;
                    t.AddTab("SubTab1", controlFactory.Create<HorizontalStackPanel>(x =>
                    {
                        x.ChildrenWidth = 120;
                        x.SpaceBetweenChildren = 50;
                        x.VerticalAlignment = VerticalAlignment.Top;
                        x.Add(controlFactory.CreateTextBlock("ABC def"));
                        x.Add(controlFactory.CreateTextBlock("ghi jkl"));
                    }));
                    t.AddTab("SubTab2", null);
                });
            }));
            tabControl.AddTab(controlFactory.Create<TabItem>(t =>
            {
                t.Header = "Dialogs";
                t.Content = controlFactory.Create<VerticalStackPanel>(s =>
                    {
                        s.HorizontalAlignment = HorizontalAlignment.Left;
                        s.Position.Width = 200;
                        s.Add(controlFactory.Create<Button>(b =>
                        {
                            b.Text = "Message dialog";
                            b.InputState.Clicked += (sender, args) => DialogService.Show(
                                controlFactory.Create<SimpleMessageDialog>(
                                    d =>
                                    {
                                        d.Text = "This is a simple overlay dialog";
                                    }));
                        }));
                        s.Add(controlFactory.Create<Button>(b =>
                        {
                            b.Text = "Question";
                            b.InputState.Clicked += (sender, args) => DialogService.Show(
                                controlFactory.Create<QuestionDialog>(
                                    d =>
                                    {
                                        d.Message = "Are you alright?";
                                    }));
                        }));
                        s.Add(controlFactory.Create<Button>(b =>
                        {
                            b.Text = "Input";
                            b.InputState.Clicked += (sender, args) => DialogService.Show(
                                controlFactory.Create<InputBoxDialog>(
                                    d =>
                                    {
                                        d.Message = "Give me some input...";
                                    }));
                        }));
                    });
            }));
            tabControl.AddTab(controlFactory.Create<TabItem>(h =>
            {
                h.Header = "Touch item list";
                h.Content = controlFactory.Create<TouchItemList<string>>(p =>
                {
                    p.AddItem("Item 1");
                    p.AddItem("Item 2");
                });
            }));
            
            tabControl.Position.Height = 200;
            
            var rootStackPanel = controlFactory.Create<VerticalStackPanel>();
            rootStackPanel.Padding.SetAll(20);
            rootStackPanel.SpaceBetweenChildren = 25;
            rootStackPanel.Add(buttonStackPanel);
            rootStackPanel.Add(checkBoxStackPanel);
            rootStackPanel.Add(progressBar);
            rootStackPanel.Add(sliderPanel);
            rootStackPanel.Add(textBoxStackPanel);
            rootStackPanel.Add(textBlock);
            rootStackPanel.Add(anotherStackPanel);
            rootStackPanel.Add(tabControl);
            
            var rootTabControl = controlFactory.Create<TabControl>();
            rootTabControl.AddTab("Standard", rootStackPanel);
            rootTabControl.AddTab("OpenGL 3D", CreateOpenGLView(controlFactory));
            rootTabControl.AddTab("Slider", 
                controlFactory.Create<Canvas>(canvas =>
                {
                    canvas.Add(controlFactory.Create<Slider>(s =>
                    {
                        s.Orientation = Orientation.Vertical;
                        s.MaxValue = 100;
                        s.Value = 15;
                        s.SetAttachedProperty(Canvas.AttachedXId, 50f);
                        s.SetAttachedProperty(Canvas.AttachedYId, 50f);
                        s.Position.Width = 50;
                        s.Position.Height = 500;
                        s.PropertyValue.PropertyChanged += (o, e) => Console.WriteLine(e.NewValue);
                    }));

                    canvas.Add(controlFactory.Create<Slider>(s =>
                    {
                        s.Orientation = Orientation.Vertical;
                        s.MaxValue = 100;
                        s.Value = 15;
                        s.SetAttachedProperty(Canvas.AttachedXId, 120f);
                        s.SetAttachedProperty(Canvas.AttachedYId, 50f);
                        s.Position.Width = 50;
                        s.Position.Height = 500;
                        s.IsInverted = true;
                        s.PropertyValue.PropertyChanged += (o, e) => Console.WriteLine(e.NewValue); 
                    }));

                    canvas.Add(controlFactory.Create<Slider>(s =>
                    {
                        s.Orientation = Orientation.Horizontal;
                        s.MaxValue = 100;
                        s.Value = 15;
                        s.SetAttachedProperty(Canvas.AttachedXId, 180f);
                        s.SetAttachedProperty(Canvas.AttachedYId, 50f);
                        s.Position.Width = 200;
                        s.Position.Height = 50;
                        s.PropertyValue.PropertyChanged += (o, e) => Console.WriteLine(e.NewValue);
                    }));

                    canvas.Add(controlFactory.Create<Slider>(s =>
                    {
                        s.Orientation = Orientation.Horizontal;
                        s.MaxValue = 100;
                        s.Value = 15;
                        s.SetAttachedProperty(Canvas.AttachedXId, 180f);
                        s.SetAttachedProperty(Canvas.AttachedYId, 120f);
                        s.Position.Width = 200;
                        s.Position.Height = 50;
                        s.IsInverted = true;
                        s.PropertyValue.PropertyChanged += (o, e) => Console.WriteLine(e.NewValue);
                    }));
                }
            ));
            rootTabControl.AddTab("Popups", 
                controlFactory.Create<VerticalStackPanel>(p =>
                {
                    var showPopupButton = controlFactory.Create<Button>();
                    showPopupButton.Text = "show popup";
                    
                    var popup = controlFactory.Create<Popup>();
                    popup.Content = controlFactory.Create<Control>(c => c.Background = Colors.Green);
                    popup.AutoPosRelativeToBottomOf(showPopupButton);
                    
                    popup.Position.Width = 100;
                    popup.Position.Height = 100;
                    
                    var popupContainer = controlFactory.Create<PopupContainer>();
                    popupContainer.Content = showPopupButton;
                    popupContainer.Popup = popup;
                    showPopupButton.InputState.Clicked += (s, e) => popup.ToggleVisibility();
                    
                    p.Add(popupContainer);
                    p.ChildrenHeight = showPopupButton.Position.Height;
                }));

            var dialogContainer = DialogService.CreateContainer(controlFactory, rootTabControl);
            return dialogContainer;
        }

        private Control CreateOpenGLView(IControlFactory cf)
        {
            // test overlay over 3d control
            var comboBox = cf.Create<ComboBox>();
            comboBox.Add("123");
            comboBox.Add("456");
            comboBox.Add("789");
            comboBox.Position.Width = 50;
            
            var textBlock = cf.Create<TextBlock>();
            textBlock.Text = "OpenGL Version: " + GL.GetString(StringName.Version) + ", GLSL Version: " + GL.GetString(StringName.ShadingLanguageVersion);
            textBlock.Position.Width = 300;
            
            var stackPanel = cf.Create<HorizontalStackPanel>();
            stackPanel.Position.Height = 30;
            stackPanel.Add(comboBox);
            stackPanel.Add(textBlock);
            
            var result = cf.Create<DockPanel>();
            result.Add(Dock.Top, stackPanel);
            result.Add(Dock.Fill, cf.Create<OpenGlTestControl>());
            return result;
        }
    }
}