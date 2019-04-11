using System;
using System.IO;
using System.Text;
using Math = System.Math;

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
            if(length < 1)
                throw new ArgumentOutOfRangeException(nameof(length), "Must be >= 1");

            var sb = new StringBuilder(length);
            var pos = _pos;
            var end = Math.Min(_pos + length, sb.Length);

            for(;pos < end; pos++)
                sb.Append(_buffer[pos]);

            return sb.ToString();
        }
    }
}