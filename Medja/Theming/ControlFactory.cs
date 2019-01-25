using System;
using System.Collections.Generic;
using System.Reflection;
using Medja.Controls;
using Medja.Utils.Reflection;

namespace Medja.Theming
{
	/// <summary>
	/// A factory to create controls. The main prupose is to allow you to modify default settings, when creating any
	/// medja control and to setup the renderer of each control.
	/// </summary>
	/// <remarks>There are some controls that use generics. The naming of the method is the same as for other controls.
	/// If you have a class MyControl<T> the method would be named 'CreateMyControl<T>'. You cannot register generic
	/// methods via AddFactoryMethod but the <see cref="Create{TControl}()"/> method will search via reflection.</remarks>
	public class ControlFactory
	{
		private readonly Dictionary<Type, Func<object>> _factoryMethods;

		public ControlFactory()
		{
			_factoryMethods = new Dictionary<Type, Func<object>>();

			_factoryMethods.Add(typeof(Button), CreateButton);
			_factoryMethods.Add(typeof(Canvas), CreateCanvas);
			_factoryMethods.Add(typeof(CheckBox), CreateCheckBox);
			_factoryMethods.Add(typeof(Control), CreateControl);
			_factoryMethods.Add(typeof(ContentControl), CreateContentControl);
			_factoryMethods.Add(typeof(Dialog), CreateDialog);
			_factoryMethods.Add(typeof(DialogButtonsControl), CreateDialogButtonsControl);
			_factoryMethods.Add(typeof(DialogParentControl), CreateDialogParentControl);
			_factoryMethods.Add(typeof(DockPanel), CreateDockPanel);
			_factoryMethods.Add(typeof(HorizontalStackPanel), CreateHorizontalStackPanel);
			_factoryMethods.Add(typeof(InputBoxDialog), CreateInputBoxDialog);
			_factoryMethods.Add(typeof(ScrollableContainer), CreateScrollableContainer);
			_factoryMethods.Add(typeof(ScrollingGrid), CreateScrollingGrid);
			_factoryMethods.Add(typeof(SideControlsContainer), CreateSideControlContainer);
			_factoryMethods.Add(typeof(SimpleMessageDialog), CreateSimpleMessageDialog);
			_factoryMethods.Add(typeof(Slider), CreateSlider);
			_factoryMethods.Add(typeof(TabControl), CreateTabControl);
			_factoryMethods.Add(typeof(TextBox), CreateTextBox);
			_factoryMethods.Add(typeof(TextBlock), CreateTextBlock);
			_factoryMethods.Add(typeof(ProgressBar), CreateProgressBar);
			_factoryMethods.Add(typeof(QuestionDialog), CreateQuestionDialog);
			_factoryMethods.Add(typeof(TablePanel), CreateTablePanel);
			_factoryMethods.Add(typeof(VerticalStackPanel), CreateVerticalStackPanel);
			_factoryMethods.Add(typeof(VerticalScrollBar), CreateVerticalScrollBar);
			
			// generic methods are not added here
			// TouchButtonList<T>
			// ComboBox<T>
		}

		protected void AddFactoryMethod<TControl>(Func<TControl> factory)
			where TControl : Control
		{
			_factoryMethods.Add(typeof(TControl), factory);
		}
		
		protected virtual TablePanel CreateTablePanel()
		{
			return new TablePanel();
		}

		protected virtual Button CreateButton()
		{
			return new Button();
		}

		protected virtual Canvas CreateCanvas()
		{
			return new Canvas();
		}

		protected virtual CheckBox CreateCheckBox()
		{
			return new CheckBox();
		}

		protected virtual Control CreateControl()
		{
			return new Control();
		}

		protected virtual ContentControl CreateContentControl()
		{
			return new ContentControl();
		}

		protected virtual Dialog CreateDialog()
		{
			return new Dialog();
		}

		protected virtual DialogButtonsControl CreateDialogButtonsControl()
		{
			return new DialogButtonsControl(this);
		}

		protected virtual DialogParentControl CreateDialogParentControl()
		{
			return new DialogParentControl();
		}

		protected virtual DockPanel CreateDockPanel()
		{
			return new DockPanel();
		}

		protected virtual HorizontalStackPanel CreateHorizontalStackPanel()
		{
			return new HorizontalStackPanel();
		}

		protected virtual InputBoxDialog CreateInputBoxDialog()
		{
			return new InputBoxDialog(this);
		}

		protected virtual ScrollableContainer CreateScrollableContainer()
		{
			return new ScrollableContainer(this);
		}
		
		protected virtual ScrollingGrid CreateScrollingGrid()
		{
			return new ScrollingGrid();
		}

		protected virtual SideControlsContainer CreateSideControlContainer()
		{
			return new SideControlsContainer(this);
		}

		protected virtual SimpleMessageDialog CreateSimpleMessageDialog()
		{
			return new SimpleMessageDialog(this);
		}

		protected virtual Slider CreateSlider()
		{
			return new Slider();
		}

		protected virtual TabControl CreateTabControl()
		{
			return new TabControl();
		}

		protected virtual TextBox CreateTextBox()
		{
			return new TextBox();
		}

		protected virtual TextBlock CreateTextBlock()
		{
			return new TextBlock();
		}

		protected virtual ProgressBar CreateProgressBar()
		{
			return new ProgressBar();
		}

		protected virtual TouchButtonList<T> CreateTouchButtonList<T>() where T : class
		{
			return new TouchButtonList<T>(this);
		}

		protected virtual ComboBox<T> CreateComboBox<T>() where T : class
		{
			return new ComboBox<T>(this);
		}

		protected virtual QuestionDialog CreateQuestionDialog()
		{
			return new QuestionDialog(this);
		}

		protected virtual VerticalStackPanel CreateVerticalStackPanel()
		{
			return new VerticalStackPanel();
		}

		protected virtual VerticalScrollBar CreateVerticalScrollBar()
		{
			return new VerticalScrollBar();
		}

		public TControl Create<TControl>()
			where TControl : Control
		{
			var type = typeof(TControl);

			if (type.IsGenericType)
			{
				var genericTypeArguments = type.GenericTypeArguments;

				if (genericTypeArguments.Length != 1)
					throw new ArgumentException("only generic types with one generic parameter are supported");

				var typeName = type.GetNameWithoutGenericArity();
				var method = GetType().GetMethod("Create" + typeName, BindingFlags.NonPublic | BindingFlags.Instance);
				var genericMethod = method.MakeGenericMethod(type.GenericTypeArguments);

				return (TControl)genericMethod.Invoke(this, null);
			}

			return (TControl)_factoryMethods[typeof(TControl)]();
		}

		public TControl Create<TControl>(Action<TControl> applyCustomStyle)
			where TControl : Control
		{
			var result = Create<TControl>();
			applyCustomStyle(result);

			return result;
		}
		
		/// <summary>
		/// Creates a text block with the given text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public TextBlock CreateTextBlock(string text)
		{
			return Create<TextBlock>(p => p.Text = text);
		}

		/// <summary>
		/// Same as <see cref="CreateTextBlock()"/> but adds ": " to the text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public TextBlock CreateLabel(string text)
		{
			return CreateTextBlock(text + ": ");
		}
	}
}
