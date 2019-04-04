using System;
using System.IO;
using System.Text;

namespace Medja.Utils.Text
{
    public class PeekBuffer
    {
        private readonly StringBuilder _buffer;
        private int _pos;

        // todo check with stack performance

        public bool HasMore
        {
            get { return _pos < _buffer.Length; }
        }

        public PeekBuffer()
        {
            _buffer = new StringBuilder();
            _pos = 0;
        }

        public void Add(char c)
        {
            _buffer.Append(c);
        }

        public char ReadChar()
        {
            if (!HasMore)
                throw new EndOfStreamException();

            return _buffer[_pos++];
        }

        public string PeekMax(int length)
        {
            // IN PROGRESS
        }
    }
}