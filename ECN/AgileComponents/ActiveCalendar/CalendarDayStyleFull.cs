// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents the full style for client-side manipulator of a <see cref="CalendarDayStyleFull"/>.
	/// </summary>
	[ToolboxItemAttribute(false)]
	public class CalendarDayStyleFull : Style
	{
		/// <summary>
		/// The background image of the element.
		/// </summary>
		private string _backgroundImage = "";

		/// <summary>
		/// The default constructor.
		/// </summary>
		public CalendarDayStyleFull() : base()
		{
			
		}

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered to the specified System.Web.UI.HtmlTextWriter.
		/// </summary>
		/// <param name="writer">A System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
		/// <param name="owner">A WebControl or WebControl derived object that represents the Web server control associated with the Style.</param>
		/// <param name="imageDir">The directory where the images are located.</param>
		public void AddAttributesToRender(HtmlTextWriter writer, WebControl owner, string imageDir) 
		{
			if (this.CssClass == string.Empty)
			{
				AddAttributesToRender(writer,null);
				if (_backgroundImage != null && _backgroundImage != string.Empty)
					writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + Utils.ConvertToImageDir(imageDir,_backgroundImage) + ")");
			}
			else
				writer.AddAttribute(HtmlTextWriterAttribute.Class,this.CssClass);
		}

		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		[
			BindableAttribute(true),
			NotifyParentPropertyAttribute(true)
		]
		public string BackgroundImage
		{
			get
			{
				return _backgroundImage;
			}

			set
			{
				_backgroundImage = value;
			}
		}
	}
}
