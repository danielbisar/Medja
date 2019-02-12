using Medja.Controls;

namespace Medja.Theming
{
    /// <summary>
    /// Extensions methods for <see cref="ControlFactory"/>.
    /// </summary>
    public static class ControlFactoryExtensions
    {
        /// <summary>
        /// Creates a text block with the given text.
        /// </summary>
        /// <param name="controlFactory">The instance of <see cref="ControlFactory"/>.</param>
        /// <param name="text">The text for the <see cref="TextBlock"/>.</param>
        /// <returns>The new instance of <see cref="TextBlock"/>.</returns>
        public static TextBlock CreateTextBlock(this ControlFactory controlFactory, string text)
        {
            var result = controlFactory.Create<TextBlock>();
            result.Text = text;

            return result;
        }

        /// <summary>
        /// Same as <see cref="CreateTextBlock"/> but adds ": " to the text.
        /// </summary>
        /// <param name="controlFactory">The instance of <see cref="ControlFactory"/>.</param>
        /// <param name="text">The text for the <see cref="TextBlock"/>.</param>
        /// <returns>The new instance of <see cref="TextBlock"/>.</returns>
        public static TextBlock CreateLabel(this ControlFactory controlFactory, string text)
        {
            return controlFactory.CreateTextBlock(text + ": ");
        }
    }
}