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
		private static DialogParentControl _dialogParentControl;
		private static FramesPerSecondCounter _fpsCounter;

		public static void Main(string[] args)
		{
			_fpsCounter = new FramesPerSecondCounter();
			_fpsCounter.FramesCounted += (s, e) => Console.WriteLine(_fpsCounter.FramesPerSecond);

			var library = new MedjaOpenTkLibrary(new TestAppTheme());
			library.RendererFactory = CreateRenderer;

			var controlFactory = library.ControlFactory;
			var application = MedjaApplication.Create(library);


			_dialogParentControl = controlFactory.Create<DialogParentControl>();
			_dialogParentControl.Content = CreateTouchButtonList(controlFactory);
			_dialogParentControl.DialogControl = CreateDialog(controlFactory);
			_dialogParentControl.IsDialogVisible = true;

			_window = application.CreateWindow();
			_window.CenterOnScreen(800, 600);
			_window.Background = Colors.Black;
			_window.Content = _dialogParentControl;


			application.MainWindow = _window;
			application.Run();
		}

		private static Dialog CreateDialog(ControlFactory controlFactory)
		{
			var result = controlFactory.Create<QuestionDialog>();
			result.Message = "This is a message!";

			return result;
		}

		private static Control CreateTouchButtonList(ControlFactory factory)
		{
			var touchButtonList = factory.Create<TouchButtonList<string>>();
			touchButtonList.PageSize = 5;
			touchButtonList.InitializeButtonFromItem = (item, b) =>
			{
				b.Text = item;
			};
			touchButtonList.ButtonClicked += (s, e) =>
			{
				_dialogParentControl.IsDialogVisible = true;

				PropertyChangedEventHandler onDialogClosed = null;

				onDialogClosed = p =>
				{
					if (_dialogParentControl.IsDialogVisible == false)
					{
						e.Button.Text = "XXX: " + ((QuestionDialog)_dialogParentControl.DialogControl).IsConfirmed;
						_dialogParentControl.PropertyIsDialogVisible.PropertyChanged -= onDialogClosed;
					}
				};

				_dialogParentControl.PropertyIsDialogVisible.PropertyChanged += onDialogClosed;

				var inputBox = ((QuestionDialog)_dialogParentControl.DialogControl);
			};

			for (int i = 0; i < 100; i++)
				touchButtonList.AddItem("Item " + i);

			//touchButtonList.ScrollIntoView("Item 19");
			touchButtonList.RemoveItem("Item 19");

			return touchButtonList;
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
