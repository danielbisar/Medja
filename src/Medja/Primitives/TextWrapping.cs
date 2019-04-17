namespace Medja.Primitives
{
	public enum TextWrapping
	{
		/// <summary>
		/// Does not wrap the text but remove chars that are too much and add ... instead. 
		/// </summary>
		Ellipses,
		/// <summary>
		/// Automatically wraps text, when the controls width is too low.
		/// </summary>
		Auto,
		
		/// <summary>
		/// Does nothing.
		/// </summary>
		None
	}
}
