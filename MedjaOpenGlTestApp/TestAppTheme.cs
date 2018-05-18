using System;
using Medja.OpenTk.Rendering;

namespace MedjaOpenGlTestApp
{
	public class TestAppTheme : OpenTkTheme
	{
		public TestAppTheme()
		{
			AddFactoryMethod<OpenGlTestControl>(CreateOpenGlTestControl);
		}

		protected virtual object CreateOpenGlTestControl()
		{
			var result = new OpenGlTestControl();
			result.Renderer = new OpenGlTestControlRenderer();

			return result;
		}
	}
}
