using System;
using System.Drawing;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using Medja.Rendering;
using OpenTK;

// defines the user opengl implementation, just swap this to
// target another platform
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Medja
{
    public class MainWindow : GameWindow
    {
        public Color Background { get; set; }

        private readonly Menu _menu;
        private bool _needsRedraw;

        public MainWindow()
        {
            MedjaLibrary.Initialize(new MedjaOpenTkRendererMap());

            Background = Color.Gray;
            Title = "TestApp";

            _menu = new Menu(0.5f, 0.75f);
            _needsRedraw = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            /*var projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);*/

            //GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();            
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Ortho(0, ClientRectangle.Width, -ClientRectangle.Height, 0, 1, -1);

            GL.MatrixMode(MatrixMode.Projection); // Tell opengl that we are doing project matrix work
            GL.LoadIdentity(); // Clear the matrix
            GL.Ortho(-9.0, 9.0, -9.0, 9.0, 0.0, 30.0); // Setup an Ortho view
            GL.MatrixMode(MatrixMode.Modelview); // Tell opengl that we are doing model matrix work. (drawing)
            GL.LoadIdentity(); // Clear the model matrix

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

            _needsRedraw = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            // TODO check position and forward to relevant ui controls
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _menu.UpdateLayout();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //_menu.X += 0.001f;
            _menu.Y += 0.001f;

            //if (_needsRedraw)
            {
                _needsRedraw = false;

                // TODO only if redraw is really needed
                GL.ClearColor(Background);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                SwitchTo2DMode();

                // TODO currently we call this only with force = true, but we could also have the case of redraw some controls...
                _menu.Render(new RenderContext() { ForceRender = true });

                SwapBuffers();
            }
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
