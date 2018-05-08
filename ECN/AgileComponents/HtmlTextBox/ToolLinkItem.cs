using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolLinkItem"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolLinkItem
	{
		private string _address;
		private string _text;
		private string _anchor;
		private string _target;
		private string _tooltip;
		private ToolLinkItemCollection _links;

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLinkItem"/> class.
		/// </summary>
		public ToolLinkItem()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_Init(string.Empty,string.Empty,string.Empty,string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLinkItem"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		public ToolLinkItem(string address)
		{
			_Init(address,string.Empty,string.Empty,string.Empty,string.Empty);
		}
	
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLinkItem"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="text">The text.</param>
		public ToolLinkItem(string address, string text)
		{
			_Init(address,text,string.Empty,string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLinkItem"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="text">The text.</param>
		/// <param name="anchor">The anchor.</param>
		public ToolLinkItem(string address, string text, string anchor)
		{
			_Init(address,text, anchor,string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLinkItem"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="text">The text.</param>
		/// <param name="anchor">The anchor.</param>
		/// <param name="target">The target.</param>
		public ToolLinkItem(string address, string text, string anchor, string target)
		{
			_Init(address,text,anchor,target, string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLinkItem"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="text">The text.</param>
		/// <param name="anchor">The anchor.</param>
		/// <param name="target">The target.</param>
		/// <param name="tooltip">The tooltip.</param>
		public ToolLinkItem(string address, string text, string anchor, string target, string tooltip)
		{
			_Init(address,text,anchor,target, tooltip);
		}

		private void _Init(string address, string text, string anchor, string target, string tooltip)
		{
			_address = address;
			_text = text;
			_anchor = anchor;
			_target = target;
			_tooltip = tooltip;
			_links = new ToolLinkItemCollection();
		}

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get {return _address;}
			set {_address = value;}
		}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get {return _text;}
			set {_text = value;}
		}

		/// <summary>
		/// Gets or sets the anchor.
		/// </summary>
		/// <value>The anchor.</value>
		public string Anchor
		{
			get {return _anchor;}
			set {_anchor = value;}
		}

		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public string Target
		{
			get {return _target;}
			set {_target = value;}
		}

		/// <summary>
		/// Gets or sets the tooltip.
		/// </summary>
		/// <value>The tooltip.</value>
		public string Tooltip
		{
			get {return _tooltip;}
			set {_tooltip = value;}
		}

		/// <summary>
		/// Gets or sets the links.
		/// </summary>
		/// <value>The links.</value>
		public ToolLinkItemCollection Links
		{
			get {return _links;}
			set {_links = value;}
		}

/*		public string Address
		{
			get 
			{
				object linkaddress = ViewState["_linkaddress"];					
				if (linkaddress != null)
					return (string)linkaddress;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_linkaddress"] = value;
			}
		}

		public string Title
		{
			get 
			{
				object linktitle = ViewState["_linktitle"];					
				if (linktitle != null)
					return (string)linktitle;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_linktitle"] = value;
			}
		}

		public string Anchor
		{
			get 
			{
				object linkanchor = ViewState["_linkanchor"];					
				if (linkanchor != null)
					return (string)linkanchor;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_linkanchor"] = value;
			}
		}

		public string Target
		{
			get 
			{
				object linktarget = ViewState["_linktarget"];					
				if (linktarget != null)
					return (string)linktarget;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_linktarget"] = value;
			}
		}*/
	}
}
