using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using System;
using Medja.Primitives;
using System.Diagnostics;

namespace MedjaOpenGlTestApp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var library = new MedjaOpenTkLibrary(new TestAppTheme());
			library.RendererFactory = CreateRenderer;

			var controlFactory = library.ControlFactory;
			var application = MedjaApplication.Create(library);

			var window = application.CreateWindow();
			application.MainWindow = window;

			window.CenterOnScreen(800, 600);

			var status = controlFactory.Create<Control>();
			//status.Background = new Color(0, 1, 0);
			status.Position.Height = 50;

			var touchButtonList = controlFactory.Create<TouchButtonList<string>>();
			touchButtonList.PageSize = 5;
			touchButtonList.InitializeButtonFromItem = (item, button) =>
			{
				button.Text = item;
			};
			touchButtonList.ButtonClicked += (s, e) => window.Close(); //Console.WriteLine("Button " + e.Item + " clicked");

			for (int i = 0; i < 100; i++)
				touchButtonList.AddItem("Item " + i);

			touchButtonList.ScrollIntoView("Item 19");

			//var mainDock = controlFactory.Create<DockPanel>();

			//mainDock.Add(Dock.Top, status);
			//mainDock.Add(Dock.Fill, x);

			//var stackPanel = controlFactory.Create<VerticalStackPanel>();
			//stackPanel.Children.Add(controlFactory.Create<Button>(p => p.Text = "test"));

			//var canvas = controlFactory.Create<Canvas>();
			//canvas.Children.Add(touchButtonList);

			//canvas.Position.X = 50;
			//canvas.Position.Y = 50;

			//touchButtonList.AttachedProperties.Add(Canvas.AttachedXId, 10.0f);
			//touchButtonList.AttachedProperties.Add(Canvas.AttachedYId, 10.0f);
			//touchButtonList.Position.Width = 700;
			//touchButtonList.Position.Height = 600;

			var textBlock = controlFactory.Create<TextBlock>();
			textBlock.Text = "This is some text for testing\nwith break";
			textBlock.Position.Width = 800;

			window.Content = textBlock;
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
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.DepthTest);

			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
		}
	}
}
