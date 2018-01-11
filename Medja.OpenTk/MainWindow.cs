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

            _menu = new Menu(0.7f);
            _needsRedraw = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            /*var projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);*/

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

            if (_needsRedraw)
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
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
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
