using System;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Components3D;
using Medja.OpenTk.Controls;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Primitives;
using Medja.Theming;
using OpenTK;
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
            openGLEventControl.Load += On3DControlLoad;
            openGLEventControl.RenderControl += OnRender3DControl;

            var stackPanel = _controlFactory.Create<VerticalStackPanel>();
            stackPanel.Position.Width = 200;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            stackPanel.Add(_controlFactory.CreateTextBlock("Test1"));
            stackPanel.Add(_controlFactory.CreateTextBlock("Test2"));
            stackPanel.IsTopMost = true;
            
            var layerPanel = _controlFactory.Create<LayerPanel>();
            layerPanel.Add(openGLEventControl);
            layerPanel.Add(stackPanel);
            
            window.Content = layerPanel;

            return window;
        }

        private GLScene _scene;

        private void On3DControlLoad(object sender, EventArgs e)
        {
            _scene = new GLScene();
            _scene.Camera = new GLOrthographicCamera
            {
                Width = 10,
                Height = 10,
                Position = new Vector3(0, 0, 5)
            };
            _scene.Add(new GLCuboid());
        }

        private void OnRender3DControl(object sender, EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _scene.Render();
            
            GL.Flush();
        }
    }
}