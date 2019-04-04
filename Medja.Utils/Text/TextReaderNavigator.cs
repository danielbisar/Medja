using System;
using System.IO;
using System.Collections.Generic;

namespace Medja.Utils.Text
{
    public class TextReaderNavigator
    {
        private readonly TextReader _textReader;

        public bool HasMore
        {
            get { return _textReader.Peek() != -1; }
        }

        public TextReaderNavigator(TextReader reader)
        {
            _textReader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public TextReaderNavigator(string str)
            : this(new StringReader(str))
        {
        }

        public char ReadChar()
        {
            if (!HasMore)
                throw new EndOfStreamException();

            return (char)_textReader.Read();
        }
    }
}