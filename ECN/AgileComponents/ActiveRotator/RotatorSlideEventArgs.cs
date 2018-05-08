using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// The rotator slide event arguments
	/// </summary>
	public sealed class RotatorSlideEventArgs : EventArgs
	{
		/// <summary>
		/// Create an event arguments object
		/// </summary>
		/// <param name="slide">The slide</param>
		public RotatorSlideEventArgs(Slide slide)
		{
			this._slide = slide;
		}

		/// <summary>
		/// Gets or sets the slide
		/// </summary>
		public Slide Slide
		{
			get
			{
				return _slide;
			}
			set
			{
				_slide = value;
			}
		}

		/// <summary>
		/// The slide
		/// </summary>
		private Slide _slide;
	}

}
