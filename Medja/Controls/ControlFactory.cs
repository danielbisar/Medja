using System;
using System.Collections.Generic;
using System.Text;
using Medja.Primitives;

namespace Medja.Controls
{
	public class ControlFactory
	{
		private readonly Dictionary<Type, Func<object>> _factoryMethods;

		public ControlFactory()
		{
			_factoryMethods = new Dictionary<Type, Func<object>>();
			_factoryMethods.Add(typeof(Button), CreateButton);
			_factoryMethods.Add(typeof(ProgressBar), CreateProgressBar);
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

		protected virtual ProgressBar CreateProgressBar()
		{
			return new ProgressBar();
		}

		public TControl Create<TControl>()
			where TControl : Control
		{
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
