using System;
using Medja.Layouting;
using Medja.Primitives;
using Medja.OpenTk.Eval;
using Medja.OpenTk.Rendering;
using OpenTK;

// defines the user opengl implementation, just swap this to
// target another platform
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Medja.Controls;

namespace Medja
{
    public class MainWindow : GameWindow
    {
        private readonly MenuController _menuController;
        private readonly Workflow _workflow;
        
        public MainWindow()
        {
            Title = "TestApp";
            Width = 800;
            Height = 600;

            _workflow = new Workflow();
            _workflow.AddUpdateLayer(new UpdateLayoutLayer());
            _workflow.AddRenderLayer(new OpenTkRenderLayer());

            _menuController = new MenuController();
            CreateMenu();

            var x = Width - 155;

            var stackLayout = new VerticalStackLayout();

            _workflow.AddControl(stackLayout).Position = new Position
            {
                X = x,
                Y = 50,
                Width = 150,
                Height = Height - 100
            };

            stackLayout.Children.Add(_workflow.AddControl(new Button()));
            stackLayout.Children.Add(_workflow.AddControl(new Button()));
            stackLayout.Children.Add(_workflow.AddControl(new Button()));
            stackLayout.Children.Add(_workflow.AddControl(new Button()));            
        }

        private void CreateMenu()
        {
            var mainMenu = new OpenTk.Eval.Menu("MainMenu");
            mainMenu.Items.Add(new MenuEntry("Settings"));
            mainMenu.Items.Add(new MenuEntry("Quit"));

            var settingsMenu = new OpenTk.Eval.Menu("Settings");
            settingsMenu.Items.Add(new MenuEntry("< Back")); // , mainMenu.BackCommand
            settingsMenu.Items.Add(new MenuEntry("Enable Option 1")); // TODO toggle, databinding or just via click?
            settingsMenu.Items.Add(new MenuEntry("Option 2"));

            _menuController.Add(mainMenu);
            _menuController.Add(settingsMenu);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            _workflow.SetRenderTargetSize(new Size(ClientRectangle.Width, ClientRectangle.Height));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            // TODO check position and forward to relevant ui controls
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _workflow.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);            
            _workflow.Render();
            SwapBuffers();
        }
    }
}
