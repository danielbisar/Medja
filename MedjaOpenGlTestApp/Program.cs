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

			var textBox = controlFactory.Create<TextBox>();
			textBox.Text = "ABC";

			var checkBoxDisabled = controlFactory.Create<CheckBox>();
			checkBoxDisabled.Text = "Disabled";
			checkBoxDisabled.VerticalAlignment = VerticalAlignment.Top;
			checkBoxDisabled.IsEnabled = false;
			
			var checkBox = controlFactory.Create<CheckBox>();
			checkBox.Text = "Some text";
			checkBox.VerticalAlignment = VerticalAlignment.Top;
			checkBox.PropertyIsChecked.PropertyChanged += (s, e) => checkBoxDisabled.IsChecked = (bool)e.NewValue;
			
			var tablePanel = controlFactory.Create<TablePanel>();
			tablePanel.Rows.Add(new RowDefinition(50));
			tablePanel.Rows.Add(new RowDefinition(50));
			tablePanel.Columns.Add(new ColumnDefinition());
			tablePanel.Columns.Add(new ColumnDefinition());
			tablePanel.Children.Add(textBlock);
			tablePanel.Children.Add(checkBox);
			tablePanel.Children.Add(textBox);
			tablePanel.Children.Add(checkBoxDisabled);

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
