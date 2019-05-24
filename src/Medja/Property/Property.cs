using System.Collections.Generic;
using Medja.Utils.Collections.Generic;

namespace Medja
{
	/// <summary>
	/// Provides change notification for properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks>A fundamental class in Medja. Using this class adds data binding features support. This class is
	/// performance optimized and is almost as fast as using .NET properties. WPF Properties for instance are &gt;
	/// factor 100 slower.</remarks>
	public class Property<T> : IProperty
	{
		// creating a static variable inside this class makes creation 3X as slow as currently
		protected readonly EqualityComparer<T> Comparer;
		protected T _value;

		/// <summary>
		/// The <see cref="PropertyChanged"/> event fires when the value of this property changed or an explicit
		/// notification was requested.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		public Property()
		{
			// register empty handler, see thread safety in wiki
			PropertyChanged += (s, e) => { };
			// see EqualityComparerCache header for info why
			Comparer = EqualityComparerCache<T>.Comparer;
		}
		
		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <remarks>If the value given is different from the value stored the <see cref="PropertyChanged"/> event
		/// will be fired.</remarks>
		public void Set(T value)
		{
			if (Comparer.Equals(_value, value))
				return;

			InternalSet(value);
		}
		
		protected virtual void InternalSet(T value)
		{
			var oldValue = _value;
			_value = value;
			NotifyPropertyChanged(oldValue, value);
		}

		/// <summary>
		/// Sets the value of the property. Does not fire the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <remarks>This method is mainly used for initialization.</remarks>
		public virtual void UnnotifiedSet(T value)
		{
			_value = value;
		}

		/// <summary>
		/// Gets the properties value.
		/// </summary>
		/// <returns>The current value of the property.</returns>
		public virtual T Get()
		{
			return _value;
		}

		protected void NotifyPropertyChanged(T oldValue, T newValue)
		{
			// ReSharper disable once PossibleNullReferenceException
			// see ctor empty delegate registration
			PropertyChanged(this, new PropertyChangedEventArgs(oldValue, newValue));
		}
		
		/// <summary>
		/// Forces the <see cref="PropertyChanged"/> event to fire.
		/// </summary>
		public void NotifyPropertyChanged()
		{
			NotifyPropertyChanged(_value, _value);
		}
	}
}
