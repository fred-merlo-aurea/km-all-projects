using System;

namespace ActiveUp.WebControls
{
	#region StateEventArgs

	/// <summary>
	/// Event argument when the state of the panel changes.
	/// </summary>
	public class StateEventArgs : EventArgs
	{
		#region Fields

		/// <summary>
		/// Panel state.
		/// </summary>
		PanelState _state = PanelState.Expanded;

		#endregion

		#region Constructors 

		/// <summary>
		/// Create a <see cref="StateEventArgs"/> by specifying the state.
		/// </summary>
		/// <param name="state">Panel state.</param>
		public StateEventArgs(PanelState state)
		{
			_state = state;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the panel state.
		/// </summary>
		public PanelState State 
		{
			get
			{
				return _state;
			}

			set
			{
				_state = value;
			}
		}

		#endregion
	}

	#endregion
}
