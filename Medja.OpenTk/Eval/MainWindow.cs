using System;
using Medja.Primitives;
using Medja.OpenTk.Eval;
using Medja.OpenTk.Rendering;
using OpenTK;

// defines the user opengl implementation, just swap this to
// target another platform
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Medja
{
    public class MainWindow : GameWindow
    {
        private readonly MenuController _menuController;
        //private readonly Workflow _workflow;
        //private readonly ControlState _rootControl;
        //private readonly ControlState _stack;

        public MainWindow()
        {
            //Title = "TestApp";
            //Width = 800;
            //Height = 600;

            //_workflow = new Workflow();
            //_workflow.AddUpdateLayer(new UpdateLayoutLayer());
            //_workflow.AddRenderLayer(new OpenTkRenderLayer());
            //_workflow.SetRenderTargetSize(new Primitives.Size(Width, Height));

            //_menuController = new MenuController();            

            //var dockPanel = new DockLayout();
            //_rootControl = _workflow.AddControl(dockPanel);

            //_rootControl.Position = new Position
            //{
            //    X = 0,
            //    Y = 0,
            //    Width = Width, // todo could be update via binding, currently we do it manually (see below - resize)
            //    Height = Height
            //};
            
            //var stackLayout = new VerticalStackLayout();
            //_stack = _workflow.AddControl(stackLayout);

            //dockPanel.Add(_stack, Dock.Right);

            //_stack.Position = new Position
            //{                
            //    Width = 150,
            //};

            //CreateMenu();
        }

        //private void CreateMenu()
        //{
        //    var mainMenu = new OpenTk.Eval.Menu("MainMenu");
        //    mainMenu.Items.Add(new MenuEntry("Settings"));
        //    mainMenu.Items.Add(new MenuEntry("Quit"));

        //    var settingsMenu = new OpenTk.Eval.Menu("Settings");
        //    settingsMenu.Items.Add(new MenuEntry("< Back")); // , mainMenu.BackCommand
        //    settingsMenu.Items.Add(new MenuEntry("Enable Option 1")); // TODO toggle, databinding or just via click?
        //    settingsMenu.Items.Add(new MenuEntry("Option 2"));
                        
        //    _menuController.PropertyCurrentMenu.PropertyChanged += p => 
        //    {
        //        var verticalStackLayout = (VerticalStackLayout)_stack.Control;

        //        foreach (var child in verticalStackLayout.Children)
        //            _workflow.RemoveControl(child);

        //        verticalStackLayout.Children.Clear();

        //        foreach (var menuEntry in _menuController.CurrentMenu.Items)
        //        {
        //            var controlState = _workflow.AddControl(new Button() { Text = menuEntry.Text });

        //            // TODO remove memory leak; implement the call for Command inside the button
        //            controlState.InputState.MouseClicked += (s, e) => { _menuController.NavigateTo("Settings"); };
        //            verticalStackLayout.Children.Add(controlState);
        //        }
        //    };

        //    _menuController.Add(mainMenu);
        //    _menuController.Add(settingsMenu);
        //}
        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /*
         * public void UpdateInput(InputDeviceState inputDeviceState)
        {
            foreach (var child in _state.Controls.ToArray()) // copy the whole items because input events might modify the current state
            {
                var inputState = child.InputState;
                inputState.IsMouseOver = IsMouseOver(child, inputDeviceState.Pos);
                inputState.IsMouseDown = inputState.IsMouseOver && inputDeviceState.LeftButtonDown;
            }
        }

        private bool IsMouseOver(ControlState child, Point pos)
        {
            var childPos = child.Position;
            return pos.X >= childPos.X
                && pos.Y >= childPos.Y
                && pos.X <= (childPos.X + childPos.Width)
                && pos.Y <= (childPos.Y + childPos.Height);
        }
         *
         *
         */

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            //_workflow.SetRenderTargetSize(new Primitives.Size(ClientRectangle.Width, ClientRectangle.Height));
            //_rootControl.Position.Width = ClientRectangle.Width;
            //_rootControl.Position.Height = ClientRectangle.Height;
        }

        

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            //_workflow.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);            
            //_workflow.Render();
            SwapBuffers();
        }
    }
}
