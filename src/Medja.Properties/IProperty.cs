namespace Medja.Properties
{
    /// <summary>
    /// The interface for values that support change notifications.
    /// </summary>
    public interface IProperty
    {
        event PropertyChangedEventHandler PropertyChanged;
       
        /// <summary>
        /// Calls the event PropertyChanged.
        /// </summary>
        void NotifyPropertyChanged();
    }

    // do not use this interface for performance reasons,
    // if you use the Property Object directly it is a significant performance boost
    // the interface above exists just to be able to use the property for the
    // PropertyChanged delegate

    //public interface IProperty<T> : IProperty
    //{
    //    T Get();
    //    void Set(T value);
    //}
}