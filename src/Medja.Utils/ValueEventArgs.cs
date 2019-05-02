using System;

namespace Medja.Utils
{
    /// <summary>
    /// Generic event args for simple value publication.
    /// </summary>
    /// <typeparam name="TValue">The values type.</typeparam>
    public class ValueEventArgs<TValue> : EventArgs
    {
        public TValue Value { get; }

        public ValueEventArgs(TValue value)
        {
            Value = value;
        }
    }
}