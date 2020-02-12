using System;
using Medja.Controls;
using Medja.Primitives;
using Medja.Properties.Binding;
using Medja.Theming;

namespace Medja.Demo
{
    public class SizeToContentTab : ContentControl
    {
        private readonly TextEditor _textEditor;
        private readonly TextBlock _textBlock;
        
        public SizeToContentTab(IControlFactory controlFactory)
        {
            _textEditor = controlFactory.Create<TextEditor>();
            _textEditor.Position.Height = 100;
            _textEditor.Margin.Bottom = 10;
            _textEditor.TextChanged += OnEditorTextChanged;

            _textBlock = controlFactory.Create<TextBlock>();
            _textBlock.TextMeasured += OnTextMeasured;
            _textBlock.TextWrapping = TextWrapping.Auto;

            var stackPanel = controlFactory.Create<VerticalStackPanel>();
            stackPanel.Add(_textEditor);
            stackPanel.Add(_textBlock);
            
            Content = stackPanel;
        }

        private void OnEditorTextChanged(object sender, EventArgs e)
        {
            _textBlock.Text = _textEditor.GetText();
        }

        private void OnTextMeasured(object sender, EventArgs e)
        {
            _textBlock.SetHeightToContent();
        }
    }
}