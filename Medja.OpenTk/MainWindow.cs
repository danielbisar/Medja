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
                    Control = new DynamicItemsControl<VerticalStackLayout, MenuController>
                    {
                        ItemsHost = new VerticalStackLayout(),
                        Clear = p => { },
                        DataItem = _menuController,
                        Apply = (h, d) => d.CurrentMenu.Items.Select(p => new Button { Text = p.Text })
                    }
                }
            });

            _layoutLayer = new LayoutLayer();
            _layoutLayer.SetLayoutRoot(dockLayout);

            _layerPipeline = new LayerPipeline();
            _layerPipeline.AddLayer(_layoutLayer);
            //_layerPipeline.AddLayer(inputLayer);

            _renderPipeline = new RenderPipeline();
            //_renderPipeline.AddLayer(new FilterInvisibleLayer());
            _renderPipeline.AddLayer(new OpenTkRenderLayer());
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

            // sets the 0,0 to the upper left corner
            //GL.Viewport(-(ClientRectangle.Width / 2), ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            _layoutLayer.Size = new Size(ClientRectangle.Width, ClientRectangle.Height);

            /*var projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);*/

            //GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();            
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Ortho(0, ClientRectangle.Width, -ClientRectangle.Height, 0, 1, -1);

            //GL.MatrixMode(MatrixMode.Projection); // Tell opengl that we are doing project matrix work
            //GL.LoadIdentity(); // Clear the matrix
            //GL.Ortho(-9.0, 9.0, -9.0, 9.0, 0.0, 30.0); // Setup an Ortho view
            //GL.MatrixMode(MatrixMode.Modelview); // Tell opengl that we are doing model matrix work. (drawing)
            //GL.LoadIdentity(); // Clear the model matrix

            /*
             glMatrixMode(GL_PROJECTION);
glLoadIdentity();
glViewport(0, 0, screenWidth, screenHeight);
glMatrixMode(GL_MODELVIEW);
glLoadIdentity();
glOrtho(0, screenWidth, 0, screenHeight, 1, -1); // Origin in lower-left corner
glOrtho(0, screenWidth, screenHeight, 0, 1, -1); // Origin in upper-left corner
             */

            /*
             * void display(void)
{
glClear (GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT); //Clear the screen
// Set perspective view
glMatrixMode (GL_PROJECTION);
glLoadIdentity();
gluPerspective(60, 1, 1, 30);
glMatrixMode(GL_MODELVIEW);
glLoadIdentity();

Draw_3d();

//Set ortho view
glMatrixMode (GL_PROJECTION); // Tell opengl that we are doing project matrix work
glLoadIdentity(); // Clear the matrix
glOrtho(-9.0, 9.0, -9.0, 9.0, 0.0, 30.0); // Setup an Ortho view
glMatrixMode(GL_MODELVIEW); // Tell opengl that we are doing model matrix work. (drawing)
glLoadIdentity(); // Clear the model matrix

Draw_2d();

}
             */

            // TODO smooth resizing of content

            //_needsRedraw = true;
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

            GL.ClearColor(Color.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwitchTo2DMode();
            _renderPipeline.Execute(_controlStates);

            SwapBuffers();
        }

        private void SwitchTo2DMode()
        {
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrc1Alpha);
            //GL.Enable(EnableCap.Blend); --> somehow nothing is drawn then
            //GL.Disable(EnableCap.CullFace);
            //GL.Disable(EnableCap.DepthTest);
        }

        private void SwitchTo3DMode()
        {
            /*glCullFace(GL_BACK);
            glEnable(GL_CULL_FACE);
            glEnable(GL_DEPTH_TEST);
            glDisable(GL_BLEND);*/
        }
    }
}
