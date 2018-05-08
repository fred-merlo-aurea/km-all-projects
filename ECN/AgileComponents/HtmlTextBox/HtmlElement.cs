using System;
using System.Text;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// The HtmlElement object represents any HTML element. An element has a name
	/// and zero or more attributes.
	/// </summary>
	internal class HtmlElement: HtmlNode
	{
		private string _name;
		private HtmlNodeCollection _nodes;
		private HtmlAttributeCollection _attributes;
		private bool _isTerminated;
		private bool _isExplicitlyTerminated;

		/// <summary>
		/// This constructs a new HTML element with the specified tag name.
		/// </summary>
		/// <param name="name">The name of this element</param>
		public HtmlElement(string name)
		{
			_nodes = new HtmlNodeCollection( this );
			_attributes = new HtmlAttributeCollection(this);
			_name = name;
			_isTerminated = false;
		}

		/// <summary>
		/// This is the tag name of the element. e.g. BR, BODY, TABLE etc.
		/// </summary>
		[
		Category("General"),
		Description("The name of the tag/element")
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
		/// This is the collection of all child nodes of this one. If this node is actually
		/// a text node, this will throw an InvalidOperationException exception.
		/// </summary>
		[
		Category("General"),
		Description("The set of child nodes")
		]
		public HtmlNodeCollection Nodes
		{
			get
			{
				if( IsText() )
				{
					throw new InvalidOperationException("An HtmlText node does not have child nodes");
				}
				return _nodes;
			}
		}

		/// <summary>
		/// This is the collection of attributes associated with this element.
		/// </summary>
		[
		Category("General"),
		Description("The set of attributes associated with this element")
		]
		public HtmlAttributeCollection Attributes
		{
			get
			{
				return _attributes;
			}
		}

		/// <summary>
		/// This flag indicates that the element is explicitly closed using the "<name/>" method.
		/// </summary>
		internal bool IsTerminated
		{
			get
			{
				if( Nodes.Count > 0 )
				{
					return false;
				}
				else
				{
					return _isTerminated | _isExplicitlyTerminated;
				}
			}
			set
			{
				_isTerminated = value;
			}
		}

		/// <summary>
		/// This flag indicates that the element is explicitly closed using the name method.
		/// </summary>
		internal bool IsExplicitlyTerminated
		{
			get
			{
				return _isExplicitlyTerminated;
			}
			set
			{
				_isExplicitlyTerminated = value;
			}
		}

		internal bool NoEscaping
		{
			get
			{
				return "script".Equals( Name.ToLower() ) || "style".Equals( Name.ToLower() );
			}
		}

		/// <summary>
		/// This will return the HTML representation of this element.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string value = "<" + _name;
			foreach( HtmlAttribute attribute in Attributes )
			{
				value += " " + attribute.ToString();
			}
			value += ">";
			return value;
		}

		[
		Category("General"),
		Description("A concatination of all the text associated with this element")
		]
		public string Text
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach( HtmlNode node in Nodes )
				{
					if( node is HtmlText )
					{
						stringBuilder.Append( ((HtmlText)node).Text );
					}
				}
				return stringBuilder.ToString();
			}
		}

		/// <summary>
		/// This will return the HTML for this element and all subnodes.
		/// </summary>
		[
		Category("Output")
		]
		public override string HTML
		{
			get
			{
				StringBuilder html = new StringBuilder();
				html.Append( "<" + _name );
				foreach( HtmlAttribute attribute in Attributes )
				{
					html.Append( " " + attribute.HTML );
				}
				if( Nodes.Count > 0 )
				{
					html.Append( ">" );
					foreach( HtmlNode node in Nodes )
					{
						html.Append( node.HTML );
					}
					html.Append( "</" + _name + ">" );
				}
				else
				{
					if( IsExplicitlyTerminated )
					{
						html.Append( "></" + _name + ">" );
					}
					else if( IsTerminated )
					{
						html.Append( "/>" );
					}
					else
					{
						html.Append( ">" );
					}
				}
				return html.ToString();
			}
		}

		/// <summary>
		/// This will return the XHTML for this element and all subnodes.
		/// </summary>
		[
		Category("Output")
		]
		public override string XHTML
		{
			get
			{
				if( "html".Equals( _name ) && this.Attributes[ "xmlns" ] == null )
				{
					Attributes.Add( new HtmlAttribute( "xmlns" , "http://www.w3.org/2000/xhtml" ) );
				}
				StringBuilder html = new StringBuilder();
				html.Append( "<" + _name.ToLower() );
				foreach( HtmlAttribute attribute in Attributes )
				{
					html.Append( " " + attribute.XHTML );
				}
				if( IsTerminated )
				{
					html.Append( "/>" );
				}
				else
				{
					if( Nodes.Count > 0 )
					{
						html.Append( ">" );
						foreach( HtmlNode node in Nodes )
						{
							html.Append( node.XHTML );
						}
						html.Append( "</" + _name.ToLower() + ">" );
					}
					else
					{
						html.Append( "/>" );
					}
				}
				return html.ToString();
			}
		}
	}
}
