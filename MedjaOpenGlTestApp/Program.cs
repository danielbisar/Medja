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

		public static void Main(string[] args)
		{
			var library = new MedjaOpenTkLibrary(new TestAppTheme());
			library.RendererFactory = CreateRenderer;

			var controlFactory = library.ControlFactory;
			var application = MedjaApplication.Create(library);

			var textBlock = controlFactory.Create<TextBlock>();
			textBlock.Text = "My label:";
			textBlock.Position.Width = 50;

			var textBlock2 = controlFactory.Create<TextBlock>();
			textBlock2.Text = "Text2";
			textBlock2.Position.Width = 50;

			var control = controlFactory.Create<Control>();
			control.Background = ColorMap.SecondaryLight;
			
			var dockPanel = controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Right, textBlock);
			dockPanel.Add(Dock.Right, textBlock2);
			dockPanel.Add(Dock.Fill, control);
			
			_window = application.CreateWindow();
			_window.CenterOnScreen(800, 600);
			_window.Background = Colors.Black;
			_window.Content = dockPanel;

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
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.DepthTest);

			var color = _window.Background;
			GL.ClearColor(color.Red, color.Green, color.Blue, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
		}
	}
}
