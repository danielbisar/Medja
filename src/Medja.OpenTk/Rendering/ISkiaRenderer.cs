using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public interface ISkiaRenderer : IControlRenderer
	{
		//TypefaceCache TypefaceCache { get; set; }
		SKTypeface DefaultTypeFace { get; set; }
	}
}
