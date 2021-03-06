using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    /// <summary>
    /// Values used by this theme.
    /// </summary>
    public static class ThemeDarkBlueValues
    {
        public static Color WindowBackground = Colors.LightGray;
        public static Color ControlBackground = Colors.Gray;
        public static Color ControlBorder = Colors.White;
        
        // f.e. used for ProgressBar filled area, button background, checkbox filled area
        public static Color PrimaryColor = Color.FromHexStr("#1565c0");
        public static Color PrimaryTextColor = Colors.White;
        
        public static SKImageFilter DropShadow = SKImageFilter.CreateDropShadow(2,2,4,4, new SKColor(0,0,0,70), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
        public static SKImageFilter DropShadowDisabled = SKImageFilter.CreateDropShadow(1,1,4,4, new SKColor(0,0,0,50), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);
        public static SKImageFilter DropShadowElevated = SKImageFilter.CreateDropShadow(1,10,4,4, new SKColor(0,0,0,70), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);

    }
}