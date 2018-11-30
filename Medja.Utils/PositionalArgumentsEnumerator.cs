using System.Collections;
using System.Collections.Generic;

namespace Medja.Utils
{
    public class PositionalArgumentsEnumerator : IEnumerator<CommandLineArgument>
    {
        private readonly IReadOnlyList<CommandLineArgument> _arguments;
        private int _i;
        
        object IEnumerator.Current
        {
            get { return Current; }
        }
        
        public CommandLineArgument Current
        {
            get { return _arguments[_i]; }
        }
        
        public PositionalArgumentsEnumerator(IReadOnlyList<CommandLineArgument> arguments)
        {
            _arguments = arguments;
            _i = -1;
        }

        public bool MoveNext()
        {
            return _i++ < _arguments.Count;
        }

        public void Reset()
        {
            _i = -1;
        }

        public void Dispose()
        {
        }
    }
}