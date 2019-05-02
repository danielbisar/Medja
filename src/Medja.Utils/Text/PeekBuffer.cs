using System;
using System.IO;
using System.Text;

namespace Medja.Utils.Text
{
    public class PeekBuffer
    {
        private readonly StringBuilder _buffer;
        private int _pos;

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

        public void Add(string s)
        {
            _buffer.Append(s);
        }

        public char ReadChar()
        {
            if (!HasMore)
                throw new EndOfStreamException();

            return _buffer[_pos++];
        }

        public char PeekChar()
        {
            return _buffer[_pos];
        }

        public string PeekMax(int length)
        {
            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length), "Must be >= 1");

            var sb = new StringBuilder(length);
            var pos = _pos;
            var end = System.Math.Min(_pos + length, _buffer.Length);

            for (; pos < end; pos++)
                sb.Append(_buffer[pos]);

            return sb.ToString();
        }

        public override string ToString()
        {
            return
            "\nPeekBuffer: " + _buffer.ToString() + "\n" +
            "            " + (_pos < 1 ? "" : new string(' ', _pos)) + "^\n" +
            "_pos = " + _pos;
        }
    }
}