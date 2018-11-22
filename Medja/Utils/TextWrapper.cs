using System;
using System.Collections.Generic;
using System.Text;
using Medja.Primitives;
using Medja.Utils.Text;

namespace Medja.Utils
{
    public class TextWrapper
    {
        public TextWrapping TextWrapping { get; set; }
        public Func<string, float> GetWidth { get; set; }
        
        public string[] Wrap(string text, float maxWidth)
        {
            switch (TextWrapping)
            {
                case TextWrapping.Ellipses:
                    return WrapToEllipses(text, maxWidth);
                case TextWrapping.Auto:
                    return AutoWrap(text, maxWidth);
                default:
                    throw new ArgumentOutOfRangeException(nameof(TextWrapping));
            }
        }

        private string[] WrapToEllipses(string text, float maxWidth)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];

                if (currentChar == '\n')
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();

                    if (i + 1 < text.Length && text[i + 1] == '\r')
                        i++; // ignore the \r after an \n
                }
                else
                {
                    currentLine.Append(currentChar);
                    var textWidth = GetWidth(currentLine.ToString());

                    if (textWidth > maxWidth)
                    {
                        // already 4 chars are too big, so adding "..." doesn't make sense
                        // we will use the string as it is, else:
                        if (currentLine.Length >= 4)
                        {
                            while (currentLine.Length > 1 && GetWidth(currentLine + "...") > maxWidth)
                                currentLine.RemoveLast();

                            currentLine.Append("...");
                        }

                        lines.Add(currentLine.ToString());
                        currentLine.Clear();
                    }
                }
            }
            
            if(currentLine.Length > 0)
                lines.Add(currentLine.ToString());

            return lines.ToArray();
        }

        private string[] AutoWrap(string text, float maxWidth)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                
                if (currentChar == '\n')
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();

                    if (i + 1 < text.Length && text[i + 1] == '\r')
                        i++; // ignore the \r after an \n
                }
                else
                {
                    currentLine.Append(currentChar);
                    var textWidth = GetWidth(currentLine.ToString());

                    if (textWidth > maxWidth)
                    {
                        var hadWhiteSpace = false;
                        
                        // try to find the previous word; white space characters should be ignorable at the end
                        // of a line
                        while (char.IsWhiteSpace(currentChar) && currentLine.Length > 0)
                        {
                            currentLine.RemoveLast();
                            currentChar = currentLine.Last();
                            
                            hadWhiteSpace = true;
                        }
                        
                        // easy case: we just removed some whitespace chars at the end and we should be fine
                        // otherwise we need to find the beginning of the previous word
                        if (hadWhiteSpace)
                        {
                            lines.Add(currentLine.ToString());
                            currentLine.Clear();
                        }
                        else
                        {
                            var wordChars = new StringBuilder();

                            while (!char.IsWhiteSpace(currentChar) && currentLine.Length > 0)
                            {
                                wordChars.Prepend(currentChar);
                                currentLine.RemoveLast();
                                
                                if(currentLine.Length > 0)
                                    currentChar = currentLine.Last();
                            }
                            
                            lines.Add(currentLine.ToString());
                            currentLine.Clear();
                            // reappend the word chars but place them in the next line
                            currentLine.Append(wordChars);
                        }
                    }
                }
            }
            
            if(currentLine.Length > 0)
                lines.Add(currentLine.ToString());

            return lines.ToArray();
        }
    }
}