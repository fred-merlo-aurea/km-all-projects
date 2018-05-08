using System;

namespace ActiveUp.WebControls
{

	/// <summary>
	/// Represents a <see cref="StringEventArgs"/> object.
	/// </summary>
	public class StringEventArgs : EventArgs
	{
		private string message;

		/// <summary>
		/// Initializes a new instance of the <see cref="StringEventArgs"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public StringEventArgs(string message)
		{
			if (message==null)
				throw new ArgumentNullException("message");
			this.message = message;
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public String Message
		{
			get
			{
				return this.message;
			}
		}
	}

	
}
