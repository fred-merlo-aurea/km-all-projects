using System;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ContextMenuItem"/> object.
	/// </summary>
	[
	TypeConverter(typeof(ExpandableObjectConverter))
	]
	public class ContextMenuItem 
	{
		private string _text;
		private string _onclick;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContextMenuItem"/> class.
		/// </summary>
		public ContextMenuItem() 
		{
		}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		[
		Bindable(true)
		]
		public string Text
		{
			get {return _text;}
			set {_text = value;}
		}

		/// <summary>
		/// Gets or sets the onclick client side event.
		/// </summary>
		/// <value>The onclick client side event.</value>
		[
		Bindable(true)
		]
		public string OnClick
		{
			get {return _onclick;}
			set {_onclick = value;}
		}

	}
}
