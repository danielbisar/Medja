using Medja.OpenTk.Rendering;

namespace MedjaOpenGlTestApp
{
	public class TestAppTheme : OpenTkTheme
	{
		public TestAppTheme()
		{
			AddFactoryMethod<OpenGlTestControl>(CreateOpenGlTestControl);
		}

		protected virtual OpenGlTestControl CreateOpenGlTestControl()
		{
			var result = new OpenGlTestControl();
			result.Renderer = new OpenGlTestControlRenderer(result);

			return result;
		}

		//protected override TouchButtonList<T> CreateTouchButtonList<T>()
		//{
		//	var result = base.CreateTouchButtonList<T>();
		//	result.Renderer = new

		//	return result;
		//}
	}
}
