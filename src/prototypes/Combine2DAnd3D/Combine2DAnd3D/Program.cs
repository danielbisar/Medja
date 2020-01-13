using System;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Controls;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Theming;
using OpenTK.Graphics.OpenGL4;

namespace Combine2DAnd3D
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
        }

        private readonly IControlFactory _controlFactory;

        public Program()
        {
            var library = new MedjaOpenTkLibrary();

            var settings = new MedjaOpenTKWindowSettings();
            var theme = new ThemeDarkBlue(settings);
            settings.ControlFactory = theme;
            _controlFactory = theme;

            var application = MedjaApplication.Create(library);
            application.MainWindow = CreateWindow();

            application.Run();
        }

        private Window CreateWindow()
        {
            var window = _controlFactory.Create<Window>();
            window.CenterOnScreen(800, 600);

            var openGLEventControl = _controlFactory.Create<OpenGLEventControl>();
            openGLEventControl.RenderControl += OnRender3DControl;

            var layerPanel = _controlFactory.Create<LayerPanel>();
            layerPanel.Add(openGLEventControl);
            
            window.Content = layerPanel;

            return window;
        }

        private void OnRender3DControl(object sender, EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Flush();
        }
    }
}