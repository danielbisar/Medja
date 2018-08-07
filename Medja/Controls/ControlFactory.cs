using System;
using System.Collections.Generic;
using Medja.Reflection;
using System.Reflection;

namespace Medja.Controls
{
	public class ControlFactory
	{
		private readonly Dictionary<Type, Func<object>> _factoryMethods;

		public ControlFactory()
		{
			_factoryMethods = new Dictionary<Type, Func<object>>();
			_factoryMethods.Add(typeof(Button), CreateButton);
			_factoryMethods.Add(typeof(Control), CreateControl);
			_factoryMethods.Add(typeof(ContentControl), CreateContentControl);
			_factoryMethods.Add(typeof(DockPanel), CreateDockPanel);
			_factoryMethods.Add(typeof(ProgressBar), CreateProgressBar);
			_factoryMethods.Add(typeof(VerticalStackPanel), CreateVerticalStackPanel);
		}

		protected void AddFactoryMethod<TControl>(Func<object> factory)
			where TControl : Control
		{
			_factoryMethods.Add(typeof(TControl), factory);
		}

		protected virtual Button CreateButton()
		{
			return new Button();
		}

		protected virtual Control CreateControl()
		{
			return new Control();
		}

		protected virtual ContentControl CreateContentControl()
		{
			return new ContentControl();
		}

		protected virtual DockPanel CreateDockPanel()
		{
			return new DockPanel();
		}

		protected virtual ProgressBar CreateProgressBar()
		{
			return new ProgressBar();
		}

		protected virtual TouchButtonList<T> CreateTouchButtonList<T>()
		{
			return new TouchButtonList<T>(this);
		}

		protected virtual VerticalStackPanel CreateVerticalStackPanel()
		{
			return new VerticalStackPanel();
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
	}
}
