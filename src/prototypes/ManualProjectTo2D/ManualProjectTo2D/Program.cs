using System;
using Medja;
using Medja.Controls;
using Medja.Input;
using Medja.OpenTk;
using Medja.OpenTk.Components3D;
using Medja.OpenTk.Controls;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Primitives;
using Medja.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ManualProjectTo2D
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private readonly MedjaApplication _application;
        private DarkBlueTheme _controlFactory;
        private GLScene _scene;
        private GLCuboid _cube;
        private Control _control2D;
        private Canvas _canvas;
        private ProjectionHelper _projectionHelper;

        public Program()
        {
            var settings = new MedjaOpenTKWindowSettings();
            _controlFactory = new DarkBlueTheme(settings);
            settings.ControlFactory = _controlFactory;

            var library = new MedjaOpenTkLibrary();
            _application = MedjaApplication.Create(library);

            CreateMainWindow();
        }

        private void CreateMainWindow()
        {
            var window = _controlFactory.Create<Window>();
            window.CenterOnScreen(800, 600);
            window.Background = DarkBlueThemeValues.WindowBackground;
            window.Content = CreateContent();

            _application.MainWindow = window;
        }

        private Control CreateContent()
        {
            var openGLControl = new OpenGLEventControl();
            openGLControl.Renderer = new OpenGLEventControlRenderer(openGLControl);
            openGLControl.Load += OnLoad;
            openGLControl.RenderControl += OnRender;
            openGLControl.InputState.KeyPressed += OnKeyPressed;
            openGLControl.InputState.OwnsMouseEvents = true;
            
            _control2D = _controlFactory.Create<Control>();
            _control2D.Background = Colors.Blue;
            _control2D.Position.Width = 50;
            _control2D.Position.Height = 50;

            Canvas.SetX(_control2D, 100);
            Canvas.SetY(_control2D, 100);

            _canvas = _controlFactory.Create<Canvas>();
            _canvas.IsTopMost = true;
            _canvas.Add(_control2D);
            _canvas.Margin.SetAll(50);

            var result = _controlFactory.Create<LayerPanel>();
            result.Add(openGLControl);
            result.Add(_canvas);
            
            return result;
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Down)
                _cube.ModelMatrix.AddTranslationY(-0.05f);
            else if (e.Key == Keys.Up)
                _cube.ModelMatrix.AddTranslationY(0.05f);
            else if (e.Key == Keys.Left)
                _cube.ModelMatrix.AddTranslationX(-0.05f);
            else if (e.Key == Keys.Right)
                _cube.ModelMatrix.AddTranslationX(0.05f);
        }

        
        private void OnLoad(object sender, EventArgs e)
        {
            _scene = new GLScene();
            // _scene.Camera = new GLPerspectiveCamera()
            // {
            //     Position = new Vector3(0, 5, 5)
            // };
            _scene.Camera = new GLOrthographicCamera
            {
                Width = 10,
                Height = 10,
                Position = new Vector3(0, 0, 5),
                ZFar = 100
            };

            _cube = new GLCuboid();
            _cube.ModelMatrix.PropertyMatrix.PropertyChanged += OnModelMatrixChanged;

            _scene.Add(_cube);

            _projectionHelper =
                new ProjectionHelper(_application.MainWindow.Position, _canvas.Position, _cube.ModelMatrix);
        }

        private void OnModelMatrixChanged(object sender, PropertyChangedEventArgs e)
        {
            var point = _projectionHelper.ProjectTo2D(_scene.Camera.ViewProjectionMatrix, new Vector4(_cube.ModelMatrix.Translation, 1));
            
            Canvas.SetX(_control2D, point.X);
            Canvas.SetY(_control2D, point.Y);
        }

        private void OnRender(object sender, EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //_cube.ModelMatrix.AddRotationY((float) MedjaMath.Radians(1));

            _scene.Render();
        }

        public void Run()
        {
            _application.Run();
        }
    }
}