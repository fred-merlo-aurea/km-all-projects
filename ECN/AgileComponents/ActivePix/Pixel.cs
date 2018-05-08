using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="Pixel"/> object.
	/// </summary>
	public class Pixel
	{
		/// <summary>
		/// Red color.
		/// </summary>
		public byte Red;

		/// <summary>
		/// Green color.
		/// </summary>
		public byte Green;

		/// <summary>
		/// Blue color
		/// </summary>
		public byte Blue;

		/// <summary>
		/// Initializes a new instance of the <see cref="Pixel"/> class.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		public Pixel(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pixel"/> class.
		/// </summary>
		public Pixel()
		{
			Red = 0;
			Green = 0;
			Blue = 0;
		}
	}
}
