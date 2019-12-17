using Medja.Controls;
using Medja.Properties;
using Medja.Properties.Binding;

namespace Medja
{
    /// <summary>
    /// Represents a binding that sets <see cref="Control.IsLayoutUpdated"/> to false on change of the a property.
    /// </summary>
    /// <typeparam name="T">The properties type.</typeparam>
    public class MarkLayoutDirtyOnPropertyChanged<T> : IBinding
    {
        private readonly Control _control;
        private readonly Property<T> _property;
        private bool _isDisposed;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="control">The control to mark for layout updates.</param>
        /// <param name="property">The property that should be watched for changes.</param>
        public MarkLayoutDirtyOnPropertyChanged(Control control, Property<T> property)
        {
            _control = control;
            _property = property;
            _property.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _control.IsLayoutUpdated = false;
        }

        public void Update()
        {
        }

        /// <summary>
        /// Unregisters the event handler.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed) 
                return;
            
            _isDisposed = true;
            _property.PropertyChanged -= OnPropertyChanged;
        }
    }
}