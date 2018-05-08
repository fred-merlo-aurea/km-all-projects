using System;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This is the basic HTML document object used to represent a sequence of HTML.
	/// </summary>
	internal class HtmlDocument
	{
		HtmlNodeCollection _nodes = new HtmlNodeCollection(null);
		private string _xtmlHeader = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

		/// <summary>
		/// This will create a new document object by parsing the HTML specified.
		/// </summary>
		/// <param name="html">The HTML to parse.</param>
		/// <param name="wantSpaces">if set to <c>true</c> add spaces.</param>
		internal HtmlDocument(string html,bool wantSpaces)
		{
			HtmlParser parser = new HtmlParser();
			parser.RemoveEmptyElementText = !wantSpaces;
			_nodes = parser.Parse( html );
		}

		[
		Category("General"),
		Description("This is the DOCTYPE for XHTML production")
		]
		public string DocTypeXHTML
		{
			get
			{
				return _xtmlHeader;
			}
			set
			{
				_xtmlHeader = value;
			}
		}

		/// <summary>
		/// This is the collection of nodes used to represent this document.
		/// </summary>
		public HtmlNodeCollection Nodes
		{
			get
			{
				return _nodes;
			}
		}

		/// <summary>
		/// This will create a new document object by parsing the HTML specified.
		/// </summary>
		/// <param name="html">The HTML to parse.</param>
		/// <returns>An instance of the newly created object.</returns>
		public static HtmlDocument Create(string html)
		{
			return new HtmlDocument( html , false );
		}

		/// <summary>
		/// This will create a new document object by parsing the HTML specified.
		/// </summary>
		/// <param name="html">The HTML to parse.</param>
		/// <param name="wantSpaces">Set this to true if you want to preserve all whitespace from the input HTML</param>
		/// <returns>An instance of the newly created object.</returns>
		public static HtmlDocument Create(string html,bool wantSpaces)
		{
			return new HtmlDocument( html , wantSpaces );
		}

		/// <summary>
		/// This will return the HTML used to represent this document.
		/// </summary>
		[
		Category("Output"),
		Description("The HTML version of this document")
		]
		public string HTML
		{
			get
			{
				StringBuilder html = new StringBuilder();
				foreach( HtmlNode node in Nodes )
				{
					html.Append( node.HTML );
				}
				return html.ToString();
			}
		}

		/// <summary>
		/// This will return the XHTML document used to represent this document.
		/// </summary>
		[
		Category("Output"),
		Description("The XHTML version of this document")
		]
		public string XHTML
		{
			get
			{
				StringBuilder html = new StringBuilder();
				if( _xtmlHeader != null )
				{
					html.Append( _xtmlHeader );
					html.Append( "\r\n" );
				}
				foreach( HtmlNode node in Nodes )
				{
					html.Append( node.XHTML );
				}
				return html.ToString();
			}
		}
	}
}
