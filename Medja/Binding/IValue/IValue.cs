namespace Medja.Binding
{
    /// <summary>
    /// Similar like lazy loaded property but allows exchange of implementation. When and how the value actually 
    /// gets created depends on the implementation. (async, lazy, etc.)
    /// What should be garanteed: value will return a result after some time.
    /// </summary>
    public interface IValue<T>
    {
        T Value { get; }
    }
}
