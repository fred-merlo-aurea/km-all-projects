using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolButtonMenuItemControlBuilder"/>.
	/// </summary>
	[Serializable]
	public class ToolButtonMenuItemControlBuilder : ControlBuilder
	{
		/// <summary>
		/// Determines whether the white space literals in the control must be processed or ignored.
		/// </summary>
		/// <returns>True if the white space literals in the control must be processed, otherwise, false.</returns>
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
        
		/// <summary>
		/// Determines whether the literal string of an HTML control must be HTML decoded.
		/// </summary>
		/// <returns>True if the HTML control literal string is to be decoded, otherwise, false.</returns>
		public override bool HtmlDecodeLiterals()
		{
			// ListItem text gets rendered as an encoded attribute value.

			// At parse time text specified as an attribute gets decoded, and so text specified as a
			// literal needs to go through the same process.

			return true;
		}
	}
}
