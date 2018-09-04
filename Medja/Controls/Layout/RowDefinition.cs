using System.Collections.Generic;

namespace Medja.Controls.Layout
{
	public class RowDefinition
	{
		public float Height { get; set; }

		public RowDefinition()
		{
		}

		public RowDefinition(float height)
		{
			Height = height;
		}
	}
}