using System;
using Medja.Controls;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TextEditorRenderer : SkiaControlRendererBase<TextEditor>
    {
        public TextEditorRenderer(TextEditor control)
            : base(control)
        {
        }

        protected override void InternalRender()
        {
            var lines = _control.Lines;

            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
        }
    }
}