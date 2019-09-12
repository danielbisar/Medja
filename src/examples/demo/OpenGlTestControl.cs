using Medja.Controls;
using Medja.Properties;
using Font = Medja.Primitives.Font;

namespace Medja.Demo
{
    public class OpenGlTestControl : Control3D
    {
        //public readonly GLPerspectiveCamera Camera;
        //public GLLabel Label;
        
        public Font _font;
        
        public OpenGlTestControl()
        {
            Margin.SetAll(5);
            //_font = new Font();
            
            
            /*Camera = new GLPerspectiveCamera();
            UpdateLabel();
            
            InputState.KeyPressed += OnKeyPressed;
            InputState.OwnsMouseEvents = true;*/
        }


        /*private void UpdateLabel()
        {
            Label?.Dispose();
            Label = new GLLabel(_font);
            Label.Camera = Camera;
            //Label.Position = new Vector3(0, 0, 0);
        }*/
        
        /*int[] fontSize = {8,9,10,11,12,14,16,18,20,22,24,26,28,36,48,72};
        int sizeIndex = 6;

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == null)
            {
                switch (e.KeyChar)
                {
                    case '1':
                        _font.Name = "Source Code Pro";
                        UpdateLabel();
                        break;
                    case '2':
                        _font.Name = "Arial";
                        UpdateLabel();
                        break;
                    case '+':
                        if(sizeIndex + 1 < fontSize.Length)
                            sizeIndex++;
                        
                        _font.Size = fontSize[sizeIndex];
                        UpdateLabel();
                        break;
                    case '-':
                        if(sizeIndex > 0)
                            sizeIndex--;
                        
                        _font.Size = fontSize[sizeIndex];
                        UpdateLabel();
                        break;
                }
            }
            //else if(e.Key == 
        }*/

        // rendering see OpenGlTestControlRenderer
        // force rendering every frame
        protected override void OnNeedsRenderingChanged(object sender, PropertyChangedEventArgs e)
        {
            if(Renderer != null && !NeedsRendering)
                NeedsRendering = true;
        }
    }
}
