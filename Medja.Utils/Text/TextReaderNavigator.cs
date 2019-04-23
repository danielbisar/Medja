using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Medja.Utils.Text
{
    // todo what about encoding
    public class TextReaderNavigator
    {
        private readonly TextReader _textReader;
        private readonly PeekBuffer _peekBuffer;

        public bool HasMore
        {
            get
            {
                return _peekBuffer.HasMore
                    || _textReader.Peek() != -1;
            }
        }

        public TextReaderNavigator(TextReader reader)
        {
            _textReader = reader ?? throw new ArgumentNullException(nameof(reader));
            _peekBuffer = new PeekBuffer();
        }

        public TextReaderNavigator(string str)
            : this(new StringReader(str))
        {
        }

        public char PeekChar()
        {
            if (!HasMore)
                throw new EndOfStreamException();

            if (_peekBuffer.HasMore)
                return _peekBuffer.PeekChar();

            return (char)_textReader.Peek();
        }

        public string PeekMax(int length)
        {
            var sb = new StringBuilder(_peekBuffer.PeekMax(length));
            var diff = length - sb.Length;

            for (int i = 0; i < diff; i++)
            {
                if (_textReader.Peek() != -1)
                {
                    var c = (char)_textReader.Read();
                    sb.Append(c);
                    _peekBuffer.Add(c);
                }
                else
                    break;
            }

            return sb.ToString();
        }

        public char ReadChar()
        {
            if (!HasMore)
                throw new EndOfStreamException();

            if (_peekBuffer.HasMore)
                return _peekBuffer.ReadChar();

            return (char)_textReader.Read();
        }

        public void Skip(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ReadChar();
            }
        }

        /// <summary>
        /// Skips exactly the given string or throws an exception.
        /// </summary>
        public bool SkipExpected(string str)
        {
            int i = 0;
            var peeked = PeekMax(str.Length);

            if (peeked.Length != str.Length)
                return false;

            for (; i < str.Length; i++)
            {
                if (str[i] != peeked[i])
                    return false;
            }

            Skip(str.Length);

            return true;
        }

        public bool IsAtNewLine()
        {
            if (!HasMore)
                return false;

            return IsNewLine(PeekChar());
        }

        public static bool IsNewLine(char c)
        {
            return c == '\n' || c == '\r';
        }

        public void SkipLine()
        {
            while (HasMore && !IsNewLine(PeekChar()))
            {
                ReadChar();
            }

            ReadChar(); // skip the first newline char

            // if \r\n, we skipped \r
            if (HasMore && PeekChar() == '\n')
                ReadChar();
        }
    }
}
