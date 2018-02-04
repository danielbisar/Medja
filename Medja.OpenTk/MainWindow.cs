using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Medja.Controls;
using Medja.Layers;
using Medja.Layers.Layouting;
using Medja.Layers.Rendering;
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
        private readonly LayerPipeline _layerPipeline;
        private readonly OpenTkRenderLayer _renderLayer;
        private readonly RenderPipeline _renderPipeline;
        private readonly LayoutLayer _layoutLayer;
        private IEnumerable<ControlState> _controlStates;

        public MainWindow()
        {
            Title = "TestApp";

            _menuController = new MenuController();
            CreateMenu();

            var dockLayout = new DockLayout(new DockedControl[]
            {
                new DockedControl
                {
                    Dock = Dock.Right,
                    MinWidth = 200,
                    Control = new VerticalStackLayout(new Control[]
                    {
                        new Button { Text = "Test1" },
                        new Button { Text = "Test2" }
                    })
                    {
                        Margin = new Thickness(0,10)
                    }

                    /*new DynamicItemsControl<VerticalStackLayout, MenuController>
                    {
                        ItemsHost = new VerticalStackLayout(),
                        Clear = p => { },
                        DataItem = _menuController,
                        Apply = (h, d) => d.CurrentMenu.Items.Select(p => new Button { Text = p.Text })
                    }*/
                }
            });

            _layoutLayer = new LayoutLayer();
            _layoutLayer.SetLayoutRoot(dockLayout);

            _layerPipeline = new LayerPipeline();
            _layerPipeline.AddLayer(_layoutLayer);
            //_layerPipeline.AddLayer(inputLayer);

            _renderLayer = new OpenTkRenderLayer();
            _renderPipeline = new RenderPipeline();
            //_renderPipeline.AddLayer(new FilterInvisibleLayer());
            _renderPipeline.AddLayer(_renderLayer);
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
            _layoutLayer.Size = new Size(ClientRectangle.Width, ClientRectangle.Height);
            _renderLayer.Resize(ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            // TODO check position and forward to relevant ui controls
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _controlStates = _layerPipeline.Execute();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //GL.ClearColor(Color.Gray);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwitchTo2DMode();
            _renderPipeline.Execute(_controlStates);

            //SwitchTo3DMode();

            ////GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            ////GL.LoadIdentity();
            //GL.Rotate(2, 1, 1, 0);
            //GL.Begin(BeginMode.LineLoop);
            //GL.Vertex3(0, 0, 0);
            //GL.Vertex3(0, 0.5, 0);
            //GL.Vertex3(0.5, 0.5, 0.5);
            //GL.Vertex3(0.5, 0, 0.5);
            //GL.End();

            SwapBuffers();
        }

        private void SwitchTo2DMode()
        {
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrc1Alpha);
            ////GL.Enable(EnableCap.Blend); --> somehow nothing is drawn then
            //GL.Disable(EnableCap.CullFace);
            //GL.Disable(EnableCap.DepthTest);
        }

        private void SwitchTo3DMode()
        {
            GL.UseProgram(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
        }
    }
}
