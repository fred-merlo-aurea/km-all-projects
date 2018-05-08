using System;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// The HtmlText node represents a simple piece of text from the document.
	/// </summary>
	internal class HtmlText: HtmlNode
	{
		private string _text;

		/// <summary>
		/// This constructs a new node with the given text content.
		/// </summary>
		/// <param name="text"></param>
		public HtmlText(string text)
		{
			_text = text;
		}

		/// <summary>
		/// This is the text associated with this node.
		/// </summary>
		[
		Category("General"),
		Description("The text located in this text node")
		]
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		/// <summary>
		/// This will return the text for outputting inside an HTML document.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Text;
		}

		internal bool NoEscaping
		{
			get
			{
				if( base.Parent == null )
				{
					return false;
				}
				else
				{
					return ((HtmlElement)base.Parent).NoEscaping;
				}
			}
		}

		/// <summary>
		/// This will return the HTML to represent this text object.
		/// </summary>
		public override string HTML
		{
			get
			{
				if( NoEscaping )
				{
					return Text;
				}
				else
				{
					return HtmlElementEncoder.EncodeValue( Text );
				}
			}
		}

		/// <summary>
		/// This will return the XHTML to represent this text object.
		/// </summary>
		public override string XHTML
		{
			get
			{
				return HtmlElementEncoder.EncodeValue( Text );
			}
		}
	}
}

