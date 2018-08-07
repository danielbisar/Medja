using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using System;

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

			var x = controlFactory.Create<TouchButtonList<string>>();
			x.PageSize = 10;
			x.InitializeButtonFromItem = (item, button) =>
			{
				button.Text = item;
			};
			x.ButtonClicked += (s, e) => Console.WriteLine("Button " + e.Item + " clicked");

			for (int i = 0; i < 100; i++)
				x.AddItem("Item " + i);

			//var stackPanel = controlFactory.Create<VerticalStackPanel>();
			//stackPanel.Children.Add(controlFactory.Create<Button>(p => p.Text = "test"));

			window.Content = x;

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
