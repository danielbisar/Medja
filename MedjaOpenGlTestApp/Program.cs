using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;
using System;
using Medja.Performance;

namespace MedjaOpenGlTestApp
{
	class MainClass
	{
		private static MedjaWindow _window;
		private static FramesPerSecondCounter _fpsCounter;

		public static void Main(string[] args)
		{
			_fpsCounter = new FramesPerSecondCounter();
			_fpsCounter.FramesCounted += (s, e) => Console.WriteLine(_fpsCounter.FramesPerSecond);

			var library = new MedjaOpenTkLibrary(new TestAppTheme());
			library.RendererFactory = CreateRenderer;

			var controlFactory = library.ControlFactory;
			var application = MedjaApplication.Create(library);

			var textBlock1 = controlFactory.Create<TextBlock>();
			textBlock1.Text = "tab 1 content";

			var textBlock2 = controlFactory.Create<TextBlock>();
			textBlock2.Text = "tab 2 content";

			var textBlock3 = controlFactory.Create<TextBlock>();
			textBlock3.Text = "tab 3 content";

			var tabControl = controlFactory.Create<TabControl>();
			tabControl.AddTab(new TabItem("Tab1", textBlock1));
			tabControl.AddTab(new TabItem("Tab2", textBlock2));
			tabControl.AddTab(new TabItem("Tab3", textBlock3));

			_window = application.CreateWindow();
			_window.CenterOnScreen(800, 600);
			_window.Background = Colors.Black;
			_window.Content = tabControl;

			application.MainWindow = _window;
			application.Run();
		}

		private static IRenderer CreateRenderer()
		{
			var openTkRenderer = new OpenTkRenderer();
			openTkRenderer.Before3D = SetupOpenGl;

			return openTkRenderer;
		}

		private static void SetupOpenGl()
		{
			_fpsCounter.Tick();

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.DepthTest);

			var color = _window.Background;
			GL.ClearColor(color.Red, color.Green, color.Blue, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
		}
	}
}
