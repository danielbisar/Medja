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
            get { return _textReader.Peek() != -1; }
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

            return (char)_textReader.Peek();
        }

        public string PeekMax(int length)
        {
            var result = new StringBuilder();
            // IN PROGRESS
            return "";
        }

        public char ReadChar()
        {
            if (!HasMore)
                throw new EndOfStreamException();

            return (char)_textReader.Read();
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

            return true;
        }
    }
}