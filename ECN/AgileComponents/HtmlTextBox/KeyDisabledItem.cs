using System;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="KeyDisabledItem"/> object.
	/// </summary>
	[
		TypeConverter(typeof(ExpandableObjectConverter))
	]
	public class KeyDisabledItem 
	{
		private KeyCode _code;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyDisabledItem"/> class.
		/// </summary>
		public KeyDisabledItem() 
		{
			
		}

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>The code.</value>
		[
		Bindable(true),
		]
		public KeyCode Code
		{
			get {return _code;}
			set {	_code = value;}
		}

	}
}
