using Medja.Primitives;

namespace Medja.Controls
{
    /// <summary>
    /// Helper class to arrange a control inside another one.
    /// </summary>
    public class ContentArranger
    {
        public Control Control { get; set; }
        
        public ContentArranger()
        {
        }

        public void Position(Rect area)
        {
            if (Control == null)
                return;
            
            var pos = Control.Position;
            
            if (Control.HorizontalAlignment == HorizontalAlignment.Right)
                pos.X = area.X + pos.Width;
            else
                pos.X = area.X;

            if (Control.VerticalAlignment == VerticalAlignment.Bottom)
                pos.Y = area.Y + area.Height - pos.Height;
            else
                pos.Y = area.Y;
        }

        public void Stretch(Rect area)
        {
            if (Control == null) 
                return;
			
            var pos = Control.Position;

            if(Control.HorizontalAlignment == HorizontalAlignment.Stretch)
                pos.Width = area.Width;

            if (Control.VerticalAlignment == VerticalAlignment.Stretch)
                pos.Height = area.Height;

            Control.Arrange(new Size(pos.Width, pos.Height));
        }
    }
}