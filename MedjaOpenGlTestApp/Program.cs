using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;

namespace MedjaOpenGlTestApp
{
	class MainClass
	{
		private static MedjaWindow _window;

		public static void Main(string[] args)
		{
			var library = new MedjaOpenTkLibrary(new TestAppTheme());
			library.RendererFactory = CreateRenderer;

			var controlFactory = library.ControlFactory;
			var application = MedjaApplication.Create(library);

			_window = application.CreateWindow();
			application.MainWindow = _window;
			_window.Background = Colors.Black;

			_window.CenterOnScreen(800, 600);

			var status = controlFactory.Create<Control>();
			//status.Background = new Color(0, 1, 0);
			status.Position.Height = 50;

			var touchButtonList = controlFactory.Create<TouchButtonList<string>>();
			touchButtonList.PageSize = 5;
			touchButtonList.InitializeButtonFromItem = (item, b) =>
			{
				b.Text = item;
			};
			touchButtonList.ButtonClicked += (s, e) => _window.Close(); //Console.WriteLine("Button " + e.Item + " clicked");

			for (int i = 0; i < 100; i++)
				touchButtonList.AddItem("Item " + i);

			//touchButtonList.ScrollIntoView("Item 19");
			touchButtonList.RemoveItem("Item 19");

			var mainDock = controlFactory.Create<DockPanel>();

			mainDock.Add(Dock.Top, status);
			mainDock.Add(Dock.Fill, touchButtonList);

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

			var textBox = controlFactory.Create<TextBox>();
			textBox.Text = "Enter your text";
			//textBlock.Position.Width = 800;
			//textBlock.Position.Height = 30;

			var textBox2 = controlFactory.Create<TextBox>();
			textBox2.Text = "another one";

			var button = controlFactory.Create<Button>();
			button.Text = "example button";


			var stackPanel = controlFactory.Create<VerticalStackPanel>();
			stackPanel.Padding = new Thickness(10);
			stackPanel.ChildrenHeight = 25;
			stackPanel.Children.Add(textBox);
			stackPanel.Children.Add(textBox2);
			stackPanel.Children.Add(button);

			_window.Content = touchButtonList;
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

			var color = _window.Background;
			GL.ClearColor(color.Red, color.Green, color.Blue, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
		}
	}
}
