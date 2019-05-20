using System;
using System.Collections.Generic;
using System.Reflection;
using Medja.Controls;
using Medja.Utils.Reflection;

namespace Medja.Theming
{
	/// <summary>
	/// Factory class for all controls in Medja. Connects the Control with its Renderer and allows applying of
	/// default values.
	/// </summary>
	/// <remarks>There are some controls that use generics. The naming of the method is the same as for other controls.
	/// If you have a class MyControl&lt;T&gt; the method would be named 'CreateMyControl&lt;T&gt;'. You cannot register
	/// generic methods via AddFactoryMethod but the <see cref="Create{TControl}()"/> method will search via
	/// reflection.</remarks>
	public class ControlFactory : IControlFactory
	{
		private readonly Dictionary<Type, Func<object>> _factoryMethods;

		public ControlFactory()
		{
			_factoryMethods = new Dictionary<Type, Func<object>>();

			// add in alphabetical order
			_factoryMethods.Add(typeof(Button), CreateButton);
			_factoryMethods.Add(typeof(Canvas), CreateCanvas);
			_factoryMethods.Add(typeof(CheckBox), CreateCheckBox);
			_factoryMethods.Add(typeof(ComboBox), CreateComboBox2);
			_factoryMethods.Add(typeof(ConfirmableDialog), CreateConfirmableDialog);
			_factoryMethods.Add(typeof(Control), CreateControl);
			_factoryMethods.Add(typeof(ContentControl), CreateContentControl);
			_factoryMethods.Add(typeof(Dialog), CreateDialog);
			_factoryMethods.Add(typeof(DialogButtonsControl), CreateDialogButtonsControl);
			_factoryMethods.Add(typeof(DialogParentControl), CreateDialogParentControl);
			_factoryMethods.Add(typeof(DockPanel), CreateDockPanel);
			_factoryMethods.Add(typeof(Graph2D), CreateGraph2D);
			_factoryMethods.Add(typeof(HorizontalStackPanel), CreateHorizontalStackPanel);
			_factoryMethods.Add(typeof(Image), CreateImage);
			_factoryMethods.Add(typeof(ImageButton), CreateImageButton);
			_factoryMethods.Add(typeof(InputBoxDialog), CreateInputBoxDialog);
			_factoryMethods.Add(typeof(MenuItem), CreateMenuItem);
			_factoryMethods.Add(typeof(NumericKeypad), CreateNumericKeypad);
			_factoryMethods.Add(typeof(NumericKeypadDialog), CreateNumericKeypadDialog);
			_factoryMethods.Add(typeof(MedjaWindow), CreateMedjaWindow);
			_factoryMethods.Add(typeof(Popup), CreatePopup);
			_factoryMethods.Add(typeof(ScrollableContainer), CreateScrollableContainer);
			_factoryMethods.Add(typeof(ScrollingGrid), CreateScrollingGrid);
			_factoryMethods.Add(typeof(SideControlsContainer), CreateSideControlContainer);
			_factoryMethods.Add(typeof(SimpleMessageDialog), CreateSimpleMessageDialog);
			_factoryMethods.Add(typeof(Slider), CreateSlider);
			_factoryMethods.Add(typeof(TabControl), CreateTabControl);
			_factoryMethods.Add(typeof(TabItem), CreateTabItem);
			_factoryMethods.Add(typeof(TextBox), CreateTextBox);
			_factoryMethods.Add(typeof(TextBlock), CreateTextBlock);
			_factoryMethods.Add(typeof(TextEditor), CreateTextEditor);
			_factoryMethods.Add(typeof(ProgressBar), CreateProgressBar);
			_factoryMethods.Add(typeof(QuestionDialog), CreateQuestionDialog);
			_factoryMethods.Add(typeof(TablePanel), CreateTablePanel);
			_factoryMethods.Add(typeof(VerticalStackPanel), CreateVerticalStackPanel);
			_factoryMethods.Add(typeof(VerticalScrollBar), CreateVerticalScrollBar);
			
			// generic methods are not added here
			// TouchButtonList<T>
		}

		/// <summary>
		/// Allows you to add factory methods from your sub class.
		/// </summary>
		/// <param name="factory">The factory method.</param>
		/// <typeparam name="TControl">The result type of your factory method.</typeparam>
		protected void AddFactoryMethod<TControl>(Func<TControl> factory)
			where TControl : Control
		{
			_factoryMethods.Add(typeof(TControl), factory);
		}
		
		/// <summary>
		/// Creates a new instance of the given type.
		/// </summary>
		/// <typeparam name="TControl">The controls type.</typeparam>
		/// <returns>The new instance of the control.</returns>
		public TControl Create<TControl>()
			where TControl : Control
		{
			var type = typeof(TControl);

			if (type.IsGenericType)
			{
				// todo maybe we can avoid using reflection by using new() keyword as generic parameter constraint  
				var genericTypeArguments = type.GenericTypeArguments;

				if (genericTypeArguments.Length != 1)
					throw new ArgumentException("only generic types with one generic parameter are supported");

				var typeName = type.GetNameWithoutGenericArity();
				var methodName = "Create" + typeName;
				var method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
				
				if(method == null)
					throw new InvalidOperationException($"The method with the name {methodName} was not found.");
				
				var genericMethod = method.MakeGenericMethod(type.GenericTypeArguments);

				return (TControl)genericMethod.Invoke(this, null);
			}

			return (TControl)_factoryMethods[typeof(TControl)]();
		}

		/// <summary>
		/// Creates a new instance of the given type.
		/// </summary>
		/// <param name="applyCustomStyle">Action that is executed after create the new instance.
		/// Allows you to set parameters on the control with a shorter syntax.</param>
		/// <typeparam name="TControl">The controls type.</typeparam>
		/// <returns>The new instance of the control.</returns>
		public TControl Create<TControl>(Action<TControl> applyCustomStyle)
			where TControl : Control
		{
			var result = Create<TControl>();
			applyCustomStyle(result);

			return result;
		}

		/// <summary>
		/// Gets if the given control type can be created from this factory. Doesn't work for generic types yet.
		/// </summary>
		/// <typeparam name="TControl">The controls type.</typeparam>
		/// <returns>true if a factory method is registered for the given type.</returns>
		public bool HasControl<TControl>()
			where TControl: Control
		{
			return HasControl(typeof(TControl));
		}

		/// <summary>
		/// Gets if the given control type can be created from this factory. Doesn't work for generic types yet.
		/// </summary>
		/// <param name="type">The controls type.</param>
		/// <returns>true if a factory method is registered for the given type.</returns>
		public bool HasControl(Type type)
		{
			return _factoryMethods.ContainsKey(type);
		}
		
		// create methods in alphabetical order
		
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

		protected virtual ComboBox CreateComboBox2()
		{
			return new ComboBox(this);
		}

		protected virtual ConfirmableDialog CreateConfirmableDialog()
		{
			return new ConfirmableDialog(this);
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

		protected virtual Graph2D CreateGraph2D()
		{
			return new Graph2D();
		}

		protected virtual HorizontalStackPanel CreateHorizontalStackPanel()
		{
			return new HorizontalStackPanel();
		}

		protected virtual Image CreateImage()
		{
			return new Image();
		}

		protected virtual ImageButton CreateImageButton()
		{
			return new ImageButton(this);
		}
		
		protected virtual InputBoxDialog CreateInputBoxDialog()
		{
			return new InputBoxDialog(this);
		}

		protected virtual MenuItem CreateMenuItem()
		{
			return new MenuItem();
		}
		
		protected virtual NumericKeypad CreateNumericKeypad()
		{
			return new NumericKeypad(this);
		}

		protected virtual NumericKeypadDialog CreateNumericKeypadDialog()
		{
			return new NumericKeypadDialog(this);
		}
		
		protected virtual MedjaWindow CreateMedjaWindow()
		{
			return new MedjaWindow();
		}

		protected virtual Popup CreatePopup()
		{
			return new Popup();
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

		protected virtual TabItem CreateTabItem()
		{
			return new TabItem();
		}

		protected virtual TextBox CreateTextBox()
		{
			return new TextBox();
		}

		protected virtual TextBlock CreateTextBlock()
		{
			return new TextBlock();
		}
		
		protected virtual TextEditor CreateTextEditor()
		{
			return new TextEditor();
		}

		protected virtual ProgressBar CreateProgressBar()
		{
			return new ProgressBar();
		}

		protected virtual TouchButtonList<T> CreateTouchButtonList<T>() where T : class
		{
			return new TouchButtonList<T>(this);
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
		
		
		// style methods - they allow modification of controls that are created by other controls (f.e. the MenuItem
		// used by a ComboBox might look different, than the one used by a Menu)

		public virtual void ComboBoxMenuItemStyle(MenuItem menuItem)
		{
		}
	}
}
