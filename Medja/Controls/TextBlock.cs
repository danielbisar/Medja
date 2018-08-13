using System;
using Medja.Primitives;

namespace Medja.Controls
{
	public class TextBlock : Control
	{
		public readonly Property<string> PropertyText;
		public string Text
		{
			get { return PropertyText.Get(); }
			set { PropertyText.Set(value); }
		}

		public readonly Property<TextWrapping> PropertyTextWrapping;
		public TextWrapping TextWrapping
		{
			get { return PropertyTextWrapping.Get(); }
			set { PropertyTextWrapping.Set(value); }
		}

		public TextBlock()
		{
			PropertyText = new Property<string>();
			PropertyTextWrapping = new Property<TextWrapping>();
		}
	}
}
