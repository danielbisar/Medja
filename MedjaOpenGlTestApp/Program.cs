using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;
using System;
using Medja.Controls.Layout;
using Medja.Performance;

namespace MedjaOpenGlTestApp
{
	public class MainClass
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

			var textBlock = controlFactory.Create<TextBlock>();
			textBlock.Text = "My label:";
			textBlock.Position.Height = 50;

			var textBlock2 = controlFactory.Create<TextBlock>();
			textBlock2.Text = "ABC";

			var textBlock3 = controlFactory.Create<TextBlock>();
			textBlock3.Text = "CDE";
			
			var textBox = controlFactory.Create<TextBox>();
			textBox.Text = "Some text";

			var tablePanel = controlFactory.Create<TablePanel>();
			tablePanel.Rows.Add(new RowDefinition(50));
			tablePanel.Rows.Add(new RowDefinition(50));
			tablePanel.Columns.Add(new ColumnDefinition());
			tablePanel.Columns.Add(new ColumnDefinition());
			tablePanel.Children.Add(textBlock);
			tablePanel.Children.Add(textBox);
			tablePanel.Children.Add(textBlock2);
			tablePanel.Children.Add(textBlock3);
			tablePanel.InputState.MouseDragged += (s, e) =>
			{
				Console.WriteLine("Mouse drag: " + e.Vector);
			};

			_window = application.CreateWindow();
			_window.CenterOnScreen(800, 600);
			_window.Background = Colors.Black;
			_window.Content = tablePanel;

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
