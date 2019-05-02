using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public static class DarkBlueThemeValues
    {
        public static Color Background = Colors.LightGray;
        
        public static Color ControlBackground = Colors.Gray;
        public static Color ControlBorder = Colors.White;
        
        // f.e. used for ProgressBar filled area, button background, checkbox filled area
        public static Color PrimaryColor = Color.FromHexStr("#1565c0");
        public static Color PrimaryTextColor = Color.FromHexStr("#ffffff");
        
        public static SKImageFilter DropShadow = SKImageFilter.CreateDropShadow(2,2,4,4, new SKColor(0,0,0,70), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
        public static SKImageFilter DropShadowDisabled = SKImageFilter.CreateDropShadow(1,1,4,4, new SKColor(0,0,0,50), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
        public static SKImageFilter DropShadowElevated = SKImageFilter.CreateDropShadow(0,10,4,4, new SKColor(0,0,0,70), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);

    }
}