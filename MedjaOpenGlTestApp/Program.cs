using System;
using System.Drawing;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK;

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

			window.Position.Width = 1280;
			window.Position.Height = 1024;

			var button = controlFactory.Create<Button>();
			button.Text = "MyButton";
			button.InputState.MouseClicked += (s, e) => window.Close();
			button.InputState.MouseWheelDeltaProperty.PropertyChanged += p => button.Text = "MouseWheelDelta: " + button.InputState.MouseWheelDelta;

			var openGlTestControl = controlFactory.Create<OpenGlTestControl>();

			var stackPanel = new VerticalStackPanel();
			stackPanel.Position.Width = 170;
			stackPanel.ChildrenHeight = 50;
			stackPanel.Children.Add(button);
			stackPanel.Children.Add(openGlTestControl);

			window.Content = stackPanel;

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
