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
                case TextWrapping.None:
                    return new []{text};
                default:
                    throw new ArgumentOutOfRangeException(nameof(TextWrapping));
            }
        }

        private string[] WrapToEllipses(string text, float maxWidth)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            var lastCharWasNl = false;

            for (int i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];

                if (currentChar == '\n')
                {
                    lastCharWasNl = true;
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();

                    if (i + 1 < text.Length && text[i + 1] == '\r')
                        i++; // ignore the \r after an \n
                }
                else
                {
                    lastCharWasNl = false;
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

                        // skip until the next line
                        for (; i < text.Length && text[i] != '\n'; i++) ;

                        // if we still have text to process, the abort condition
                        // was the new line, so set the flag, just in case we have
                        // an empty line
                        if (i < text.Length) 
                            lastCharWasNl = true;
                    }
                }
            }
            
            if(currentLine.Length > 0 || lastCharWasNl)
                lines.Add(currentLine.ToString());

            return lines.ToArray();
        }

        private string[] AutoWrap(string text, float maxWidth)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            var lastCharWasNl = false;

            for (int i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                
                if (currentChar == '\n')
                {
                    lastCharWasNl = true;
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();

                    if (i + 1 < text.Length && text[i + 1] == '\r')
                        i++; // ignore the \r after an \n
                }
                else
                {
                    lastCharWasNl = false;
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
                            
                            // edge case, where the word is longer or as long as the line
                            if (currentLine.Length == 0)
                            {
                                lines.Add(wordChars.ToString(0, wordChars.Length - 1));
                                currentLine.Append(wordChars[wordChars.Length - 1]);
                            }
                            else
                            {
                                lines.Add(currentLine.ToString());
                                currentLine.Clear();
                                
                                // reappend the word chars but place them in the next line
                                currentLine.Append(wordChars);
                            }
                        }
                    }
                }
            }
            
            if(currentLine.Length > 0 || lastCharWasNl)
                lines.Add(currentLine.ToString());

            return lines.ToArray();
        }
    }
}