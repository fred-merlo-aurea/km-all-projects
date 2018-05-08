using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Direction of the transition.
	/// </summary>
	/// <remarks>Used by Blinds, CheckerBoard and SmoothScroll. UpLeft, UpRight, DownLeft and DownRight are exclusively used by SmoothScroll transition.</remarks>
	public enum Direction
	{
		/// <summary>
		/// No set, default will be used.
		/// </summary>
		NotSet,
		/// <summary>
		/// Transition will go up.
		/// </summary>
		Up,
		/// <summary>
		/// Transition will go up left.
		/// </summary>
		UpLeft,
		/// <summary>
		/// Transition will go up right.
		/// </summary>
		UpRight,
		/// <summary>
		/// Transition will go down.
		/// </summary>
		Down,
		/// <summary>
		/// Transition will go down left.
		/// </summary>
		DownLeft,
		/// <summary>
		/// Transition will go down right.
		/// </summary>
		DownRight,
		/// <summary>
		/// Transition will go left.
		/// </summary>
		Left,
		/// <summary>
		/// Transition will go right.
		/// </summary>
		Right		
	}
}
