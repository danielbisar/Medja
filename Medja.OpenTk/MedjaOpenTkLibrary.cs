using System;
using System.Collections.Generic;
using System.Linq;
using Medja.Controls;
using Medja.Debug;
using Medja.OpenTk.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Medja.OpenTk.Input;
using Medja.Theming;

namespace Medja.OpenTk
{
	/// <summary>
	/// Implements the connection between OpenTk and Medja.
	/// </summary>
	public class MedjaOpenTkLibrary : IMedjaLibrary
	{
		private readonly FocusManager _focusManager;
		private readonly List<Control> _controls;
		
		private MedjaWindow _medjaWindow;
		private GameWindow _gameWindow;
		private IRenderer _renderer;
		private OpenTkMouseHandler _mouseHandler;
		private OpenTKKeyboardHandler _keyboardHandler;
		private ControlHierarchy _controlHierarchy;

		public ControlFactory ControlFactory { get; }

		/// <summary>
		/// Gets or sets the function that is used to create the renderer. 
		/// The method is called after the window is displayed.
		/// </summary>
		/// <value>The renderer factory.</value>
		public Func<IRenderer> RendererFactory { get; set; }

		public MedjaOpenTkLibrary(ControlFactory factory = null)
		{
			ControlFactory = factory ?? new OpenTkTheme();
			RendererFactory = () => new OpenTkRenderer();
			_focusManager = new FocusManager();
			_controls = new List<Control>();
		}

		/// <inheritdoc />
		public MedjaWindow CreateWindow()
		{
			return new OpenTkWindow();
		}

		/// <inheritdoc />
		public void Run(MedjaApplication application)
		{
			_medjaWindow = application.MainWindow;
			_gameWindow = ((OpenTkWindow)_medjaWindow).GameWindow;
			_controlHierarchy = new ControlHierarchy(_medjaWindow);

			using (_gameWindow)
			{
				_gameWindow.Resize += OnResize;
				_gameWindow.UpdateFrame += OnUpdateFrame;
				_gameWindow.RenderFrame += OnRenderFrame;
				_gameWindow.Closed += OnWindowClosed;

				_mouseHandler = new OpenTkMouseHandler(_medjaWindow, _gameWindow, _focusManager);
				_mouseHandler.Controls = _controls;
				
				_keyboardHandler = new OpenTKKeyboardHandler(_medjaWindow, _gameWindow, _focusManager);

				_gameWindow.Run(1 / 30.0);
			}
		}

		private void OnWindowClosed(object sender, EventArgs e)
		{
			if (_renderer != null)
			{
				_renderer.Dispose();
				_renderer = null;
			}
		}

		private void OnResize(object sender, EventArgs e)
		{
			var clientRect = _gameWindow.ClientRectangle;
			GL.Viewport(0, 0, clientRect.Width, clientRect.Height);

			AssureRenderer();
			_renderer.SetSize(clientRect);
		}

		private void OnUpdateFrame(object sender, FrameEventArgs e)
		{
			_controls.Clear();

			// call this every time to update the list of controls and eventual ZIndex changes
			_controls.AddRange(_controlHierarchy.GetControls());
			_controlHierarchy.UpdateLayout(); // trigger the layouting pass

			Console.WriteLine(_controls.Select(p => p.ToString()).Aggregate((p1, p2) => { return p1 + ", " + p2; }));
		}

		private void OnRenderFrame(object sender, FrameEventArgs e)
		{
			AssureRenderer();
			
			_renderer.Render(_controls);

			// display what was just drawn
			_gameWindow.SwapBuffers();
		}

		private void AssureRenderer()
		{
			if (_renderer == null)
				_renderer = RendererFactory(); // TODO cannot be initialized without the window, but should also not have the null check here every call, for now seems fine
		}
	}
}
