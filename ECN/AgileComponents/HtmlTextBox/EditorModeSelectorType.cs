using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Edition modes selection types.
	/// </summary>
	public enum EditorModeSelectorType
	{
		/// <summary>
		/// The view source option will not be shown.  Changing view source mode can not be accomplished by the end-user; however, the Client Side API functions do provide scripting access to switch modes.
		/// </summary>
		None,
		/// <summary>
		/// Tab Selection used to switch between Edit and Source modes.  Similar to the bottom tabs used in the Visual Studio Web Form Designer.
		/// </summary>
		Tabs,
		/// <summary>
		/// Edit and Source modes are switched using a checkbox.  This option was also available in version 1.
		/// </summary>
		CheckBox
	}
}
