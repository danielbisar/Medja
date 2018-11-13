using System.Collections.Generic;

namespace Medja
{
	public class Property<T> : IProperty
	{
		// creating a static variable inside this class makes creation 3X as slow as currently
		protected readonly EqualityComparer<T> Comparer;
		protected T _value;

		public event PropertyChangedEventHandler PropertyChanged;

		public Property()
		{
			// register empty handler, see thread safety in wiki
			PropertyChanged += (s, e) => { };
			// see EqualityComparerCache header for info why
			Comparer = EqualityComparerCache<T>.Comparer;
		}
		
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
		/// Sets a value without comparing/throwing an event.
		/// </summary>
		/// <param name="value"></param>
		public virtual void UnnotifiedSet(T value)
		{
			_value = value;
		}

		/// <summary>
		/// Gets the properties value.
		/// </summary>
		/// <returns></returns>
		public virtual T Get()
		{
			return _value;
		}

		/// <summary>
		/// Manually call the property changed event, even though the property has not been changed.
		/// </summary>
		protected void NotifyPropertyChanged(T oldValue, T newValue)
		{
			// ReSharper disable once PossibleNullReferenceException
			// see ctor empty delegate registration
			PropertyChanged(this, new PropertyChangedEventArgs(oldValue, newValue));
		}
		
		public void NotifyPropertyChanged()
		{
			NotifyPropertyChanged(_value, _value);
		}
	}
}
