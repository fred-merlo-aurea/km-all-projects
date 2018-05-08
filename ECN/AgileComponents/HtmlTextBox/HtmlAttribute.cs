using System;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// The HtmlAttribute object represents a named value associated with an HtmlElement.
	/// </summary>
	internal class HtmlAttribute
	{
		private string _name;
		private string _value;

		public HtmlAttribute()
		{
			_name = "Unnamed";
			_value = "";
		}

		/// <summary>
		/// This constructs an HtmlAttribute object with the given name and value. For wierd
		/// HTML attributes that don't have a value (e.g. "NOWRAP"), specify null as the value.
		/// </summary>
		/// <param name="name">The name of the attribute</param>
		/// <param name="value">The value of the attribute</param>
		public HtmlAttribute(string name,string value)
		{
			_name = name;
			_value = value;
		}

		/// <summary>
		/// The name of the attribute. e.g. WIDTH
		/// </summary>
		[
		Category("General"),
		Description("The name of the attribute")
		]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		/// <summary>
		/// The value of the attribute. e.g. 100%
		/// </summary>
		[
		Category("General"),
		Description("The value of the attribute")
		]
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		/// <summary>
		/// This will return an HTML-formatted version of this attribute. NB. This is
		/// not SGML or XHTML safe, as it caters for null-value attributes such as "NOWRAP".
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if( _value == null )
			{
				return _name;
			}
			else
			{
				return _name + "=\"" + _value + "\"";
			}
		}

		[
		Category("Output"),
		Description("The HTML to represent this attribute")
		]
		public string HTML
		{
			get
			{
				if( _value == null )
				{
					return _name;
				}
				else
				{
					return _name + "=\"" + HtmlElementEncoder.EncodeValue( _value ) + "\"";
				}
			}
		}

		[
		Category("Output"),
		Description("The XHTML to represent this attribute")
		]
		public string XHTML
		{
			get
			{
				if( _value == null )
				{
					return _name.ToLower();
				}
				else
				{
					return _name + "=\"" + HtmlElementEncoder.EncodeValue( _value.ToLower() ) + "\"";
				}
			}
		}
	}
}
