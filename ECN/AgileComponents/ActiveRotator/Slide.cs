using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	
	/// <summary>
	/// Represents a <see cref="Slide"/> object.
	/// </summary>
	[ToolboxItem(false)]
	public class Slide : System.Web.UI.Control, INamingContainer
	{
		private string _content;
		private object _dataItem;

		/// <summary>
		/// Initializes a new instance of the <see cref="Slide"/> class.
		/// </summary>
		public Slide()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Slide"/> class.
		/// </summary>
		/// <param name="content">The content.</param>
		public Slide(string content)
		{
			_content = content;
		}

		/// <summary>
		/// Gets or sets the data item.
		/// </summary>
		/// <value>The data item.</value>
		public virtual object DataItem 
		{
			get 
			{
				return _dataItem;
			}
			set 
			{
				_dataItem = value;
			}
		}

		/// <summary>
		/// Gets or sets the contents.
		/// </summary>
		/// <value>The contents.</value>
		public string Content
		{
			get
			{
				return _content;
			}
			set
			{
				_content = value;
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			HtmlTextWriter writer = GetCorrectTagWriter(output);
			writer.AddStyleAttribute("visibility", "hidden");
			writer.AddStyleAttribute("display", "none");
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			writer.AddStyleAttribute("position", "absolute");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			//GetCorrectTagWriter(output).Write(string.Format("<div id=\"{0}\" style=\"visibility:hidden;display:none;\">", this.ClientID));
			writer.Write(_content);
			this.RenderChildren(writer);
			writer.RenderEndTag();
		}

		private HtmlTextWriter tagWriter;
		private HtmlTextWriter GetCorrectTagWriter( HtmlTextWriter writer ) 
		{
			if ( this.tagWriter != null ) return this.tagWriter;

			if ( writer is System.Web.UI.Html32TextWriter ) 
			{
				this.tagWriter =  new HtmlTextWriter( writer.InnerWriter );
			} 
			else 
			{
				this.tagWriter = writer;
			}
			return this.tagWriter;
		}
	}
}
