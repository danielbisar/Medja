using System.Collections.Generic;
using System.Text;

namespace Medja.Controls
{
    public class TextEditor : Control
    {
        // maybe we will use another class instead of string later on
        // on every typing a line must be updated, since strings in c#
        // are read only this copies the whole line and adds the char
        // at the given position...

        public List<string> Lines { get; } 

        public TextEditor()
        {
            Lines = new List<string>();
        }

        ///<summary>Replaces the complete text (content) of the Editor.</summary>
        ///<remarks>Function because the text will be parsed into lines, so we don't want to support binding as it 
        /// would look like this operation is very fast and does not use a lot resources.</remarks>
        public void SetText(string text)
        {
            Lines.Clear();

            var line = new StringBuilder();

            for(int i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if(c == '\n' || c == '\r')
                {
                    Lines.Add(line.ToString());
                    line.Clear();

                    if(c == '\r' && i + 1 < text.Length && text[i+1] == '\n')
                        i++;
                }
                else
                    line.Append(c);
            }
            
            Lines.Add(line.ToString());

            // todo reduce capacity of lines if they used a large amount before and now just a few...
        }
    }
}