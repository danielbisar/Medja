using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class SideControlsContainer : ContentControl
    {
        private readonly Button _showMenuButton;
        
        public readonly Property<Control> PropertySideContent;
        public Control SideContent
        {
            get { return PropertySideContent.Get(); }
            set { PropertySideContent.Set(value); }
        }
        
        public readonly Property<float> PropertySideContentWidth;
        public float SideContentWidth
        {
            get { return PropertySideContentWidth.Get(); }
            set { PropertySideContentWidth.Set(value); }
        }
        
        public readonly Property<bool> PropertyIsSideContentVisible;
        public bool IsSideContentVisible
        {
            get { return PropertyIsSideContentVisible.Get(); }
            set { PropertyIsSideContentVisible.Set(value); }
        }
        
        public SideControlsContainer(ControlFactory controlFactory)
        {
            PropertySideContent = new Property<Control>();
            PropertySideContentWidth = new Property<float>();
            PropertyIsSideContentVisible = new Property<bool>();

            SideContentWidth = 200;
            
            _showMenuButton = controlFactory.Create<Button>();
            _showMenuButton.Text = "=";
            _showMenuButton.InputState.MouseClicked += OnMenuButtonClicked;
            _showMenuButton.Position.Width = 50;
            _showMenuButton.Position.Height = 50;
        }

        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
            IsSideContentVisible = !IsSideContentVisible;
            IsLayoutUpdated = false;
        }

        public override IEnumerable<Control> GetChildren()
        {
            if (Content != null)
                yield return Content;
            
            if (SideContent != null && IsSideContentVisible)
                yield return SideContent;

            yield return _showMenuButton;
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);

            _showMenuButton.Position.X = Position.X + availableSize.Width - (_showMenuButton.Position.Width + Padding.TopAndBottom + Margin.TopAndBottom);
            _showMenuButton.Position.Y = Position.Y + Padding.Top + Margin.Top;
                
            _showMenuButton.Arrange(new Size(_showMenuButton.Position.Width, _showMenuButton.Position.Height));
           
            if (IsSideContentVisible && SideContent != null)
            {
                SideContent.Position.Width = SideContentWidth;
                SideContent.Position.Height = Position.Height - Padding.TopAndBottom - Margin.TopAndBottom;
                
                SideContent.Position.X = Position.X + availableSize.Width - (SideContent.Position.Width + Padding.TopAndBottom + Margin.TopAndBottom);
                SideContent.Position.Y =  Position.Y + Padding.Top + Margin.Top;
                
                SideContent.Arrange(new Size(SideContent.Position.Width, SideContent.Position.Height));
            }
        }
    }
}