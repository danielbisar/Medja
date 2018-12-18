using System;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;
using Medja.Theming;
using MedjaOpenGlTestApp.Tests;
using OpenTK;
using SkiaSharp;

namespace MedjaOpenGlTestApp
{
	public class MainClass
	{
		private static MedjaWindow _window;

		public static void Main(string[] args)
		{
			var library = new MedjaOpenTkLibrary(new TestAppTheme());
			library.RendererFactory = CreateRenderer;

			var controlFactory = library.ControlFactory;
			var application = MedjaApplication.Create(library);

			//var test = new ButtonTest(controlFactory);
			//var test = new ComboBoxTest(controlFactory);
			//var test = new ContentControlTest(controlFactory);
			//var test = new DialogTest(controlFactory);
			//var test = new DockPanelTest(controlFactory);
			//var test = new ScrollableContainerTest(controlFactory);
			//var test = new ScrollingGridTest(controlFactory);
			//var test = new SideControlsContainerTest(controlFactory);
			//var test = new SimpleDockPanelTest(controlFactory);
			//var test = new TabControlTest(controlFactory);
			//var test = new TextBoxTest(controlFactory);
			//var test = new TouchButtonListTest(controlFactory);
			var test = new Control3DTest(controlFactory);
			
			_window = application.CreateWindow();
			_window.CenterOnScreen(800, 600);
			_window.Background = Colors.Black;
			_window.Content = test.Create();
//			
			application.MainWindow = _window;
			application.Run();
		}

		private static IRenderer CreateRenderer()
		{
			var openTkRenderer = new OpenTkRenderer();
			return openTkRenderer;
		}
	}
}
