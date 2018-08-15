using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;
using System;

namespace MedjaOpenGlTestApp
{
	class MainClass
	{
		private static MedjaWindow _window;
		private static DialogParentControl _dialogParentControl;

		public static void Main(string[] args)
		{
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
			var result = controlFactory.Create<InputBoxDialog>();
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
						e.Button.Text = "XXX: " + ((InputBoxDialog)_dialogParentControl.DialogControl).InputText;
						_dialogParentControl.PropertyIsDialogVisible.PropertyChanged -= onDialogClosed;
					}
				};

				_dialogParentControl.PropertyIsDialogVisible.PropertyChanged += onDialogClosed;

				var inputBox = ((InputBoxDialog)_dialogParentControl.DialogControl);
				inputBox.Message = (string)e.Item;
				inputBox.InputText = "type something";

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
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.DepthTest);

			var color = _window.Background;
			GL.ClearColor(color.Red, color.Green, color.Blue, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
		}
	}
}
