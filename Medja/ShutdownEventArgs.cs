using System;
namespace Medja
{
	public class ShutdownEventArgs : EventArgs
    {
		/// <summary>
        /// Gets or sets the value whether the application shutdown should be cancelled.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel { get; set; }

        public ShutdownEventArgs()
        {
        }
    }
}
