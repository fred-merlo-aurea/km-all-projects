using System;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	#region AjaxPanelEventArgs

	/// <summary>
	/// Class contains event data for callback. 
	/// </summary>
	public class AjaxPanelEventArgs : EventArgs
	{
		#region Fields

		/// <summary>
		/// Callback argument.
		/// </summary>
		private string _argument;

		/// <summary>
		/// Renders HTML contents to the client.
		/// </summary>
		private HtmlTextWriter _output;

		#endregion

		#region Constructors 

		/// <summary>
		/// Creates the event data for the callback.
		/// </summary>
		/// <param name="argument">Callback argument.</param>
		/// <param name="output">HTML contents to the client.</param>
		public AjaxPanelEventArgs(string argument, HtmlTextWriter output) 
		{
			_Init(argument,output);
		}

		/// <summary>
		/// Initializes the event data for the callback.
		/// </summary>
		/// <param name="argument">Callback argument.</param>
		/// <param name="output">HTML contents to the client.</param>
		private void _Init(string argument, HtmlTextWriter output) 
		{
			_argument = argument;
			_output = output;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the callback argument.
		/// </summary>
		public string Argument 
		{
			get
			{
				return _argument;
			}

			set
			{
				_argument = value;
			}
		}

		/// <summary>
		/// Gets or sets the output for rendering HTML contents to the client.
		/// </summary>
		public HtmlTextWriter Output 
		{
			get
			{
				return _output;
			}

			set
			{
				_output = value;
			}
		}

		#endregion
	}

	#endregion
}
