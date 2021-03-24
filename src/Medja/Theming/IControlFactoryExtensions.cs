using Medja.Controls;
using Medja.Controls.Container;
using Medja.Utils;

namespace Medja.Theming
{
    /// <summary>
    /// Extensions methods for <see cref="IControlFactory"/>.
    /// </summary>
    public static class IControlFactoryExtensions
    {
        /// <summary>
        /// Same as <see cref="CreateTextBlock"/> but adds ": " to the text.
        /// </summary>
        /// <param name="controlFactory">The instance of <see cref="ControlFactory"/>.</param>
        /// <param name="text">The text for the <see cref="TextBlock"/>.</param>
        /// <returns>The new instance of <see cref="TextBlock"/>.</returns>
        public static TextBlock CreateLabel(this IControlFactory controlFactory, string text)
        {
            return controlFactory.CreateTextBlock(text + ": ");
        }

        /// <summary>
        /// Creates a popup container and a popup
        /// </summary>
        /// <param name="controlFactory"></param>
        /// <param name="content"></param>
        /// <param name="popupContent"></param>
        /// <returns></returns>
        public static PopupContainer CreatePopupContainer(this IControlFactory controlFactory,
            Control content,
            Control popupContent)
        {
            var popup = controlFactory.Create<Popup>();
            popup.Content = popupContent;
            popup.AutoPosRelativeToBottomOf(content);

            var result = controlFactory.Create<PopupContainer>();
            result.Content = content;
            result.Popup = popup;

            return result;
        }

        /// <summary>
        /// Creates a text block with the given text.
        /// </summary>
        /// <param name="controlFactory">The instance of <see cref="ControlFactory"/>.</param>
        /// <param name="text">The text for the <see cref="TextBlock"/>.</param>
        /// <returns>The new instance of <see cref="TextBlock"/>.</returns>
        public static TextBlock CreateTextBlock(this IControlFactory controlFactory, string text)
        {
            var result = controlFactory.Create<TextBlock>();
            result.Text = text;

            return result;
        }
    }
}
