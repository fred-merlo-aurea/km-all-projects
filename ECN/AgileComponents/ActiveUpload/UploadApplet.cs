// Active Upload v1.0
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Win32;
using System.Web;
using System.Globalization;
using System.IO;
using System.Reflection;
using ActiveUp.WebControls.Common;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="UploadApplet"/> object.
	/// </summary>
	[
		ToolboxBitmap(typeof(UploadApplet), "ToolBoxBitmap.Upload.bmp"),
		Designer(typeof(UploadAppletControlDesigner))
	]
	public class UploadApplet : Control
	{
		//private int _width, _height, _maxFileSize, _maxFileNumber, _maxUploadSize;
		/*private string _scriptURL, _defaultDirectory, _redirectURL, _redirectTarget, _appletLocation, _license,
			_labelButtonAdd, _labelButtonRemove, _labelButtonUpload, _labelTimeRemained, _tableHeaderFilename,
			_tableHeaderSize, _tableHeaderModified;*/
		//private ArrayList _headers;
		//private LabelValueCollection _fileFilters;
		//private UploadSelectionMode _uploadSelectionMode;
		//private ScriptURLType _scriptURLType;

		private string _license = string.Empty;
		private const string DefaultDirectoryValue = "c:\\";

		/*public UploadApplet()
		{
		}*/

		/// <summary>
		/// Sends server control content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			int index = 0;

			string appletCode = @"

<script language=""javascript"">

  document.write('<APPLET CODE = ""activeup.activeupload.UploadApplet.class"" ARCHIVE = ""{0}"" WIDTH = {1} HEIGHT = {2} NAME = ""{3}"" ALIGN = top VSPACE = 0 HSPACE = 0></XMP>\
	    <PARAM NAME = CODE VALUE = ""activeup.activeupload.UploadApplet.class"" >\
    <PARAM NAME = ARCHIVE VALUE = ""{0}"" >\
    <PARAM NAME = NAME VALUE = ""{3}"" >\
    <PARAM NAME=""type"" VALUE=""application/x-java-applet;version=1.5.0"">\
    <PARAM NAME=""scriptable"" VALUE=""false"">\
    <PARAM NAME = server_script VALUE=""{5}"">\
    <PARAM NAME = default_dir VALUE=""{6}"">\
    <PARAM NAME = selection_mode VALUE=""{4}"">\
    {9}<PARAM NAME = max_file_number VALUE=""{24}"">\
    <PARAM NAME = max_file_size VALUE=""{25}"">\
    <PARAM NAME = upload_max_size VALUE=""{26}"">\
    <PARAM NAME = finished_redirect_page VALUE=""{27}"">\
    <PARAM NAME = finished_redirect_target VALUE=""{28}"">\
    {30}{14}{15}{16}{17}{21}{22}{23}{32}{35}{36}{37}{38}{39}{40}{41}{42}{43}{44}{45}{46}<PARAM NAME = serial_number VALUE=""{7}"">\
	</APPLET>\
	');
</SCRIPT>
";
/*
<br>
<APPLET CODE = ""activeup.activeupload.UploadApplet.class"" ARCHIVE = ""{0}"" WIDTH = {1} HEIGHT = {2} NAME = ""{3}"" ALIGN = top VSPACE = 0 HSPACE = 0></XMP>
    <PARAM NAME = CODE VALUE = ""activeup.activeupload.UploadApplet.class"" >
    <PARAM NAME = ARCHIVE VALUE = ""{0}"" >
    <PARAM NAME = NAME VALUE = ""{3}"" >
    <PARAM NAME=""type"" VALUE=""application/x-java-applet;version=1.5.0"">
    <PARAM NAME=""scriptable"" VALUE=""false"">
    <PARAM NAME = server_script VALUE=""{5}"">
    <PARAM NAME = default_dir VALUE=""{6}"">
    <PARAM NAME = selection_mode VALUE=""{4}"">
    {9}<PARAM NAME = max_file_number VALUE=""{24}"">
    <PARAM NAME = max_file_size VALUE=""{25}"">
    <PARAM NAME = upload_max_size VALUE=""{26}"">
    <PARAM NAME = finished_redirect_page VALUE=""{27}"">
    <PARAM NAME = finished_redirect_target VALUE=""{28}"">
    {30}{14}{15}{16}{17}{21}{22}{23}<PARAM NAME = serial_number VALUE=""{7}"">
</APPLET>
</NOEMBED>
</EMBED>
</OBJECT>


<APPLET CODE = ""activeup.activeupload.UploadApplet.class"" ARCHIVE = ""{0}"" WIDTH = {1} HEIGHT = {2} NAME = ""{3}"" ALIGN = top VSPACE = 0 HSPACE = 0>
<PARAM NAME = ""scriptUrl"" VALUE=""{5}"">
</APPLET>

*/

			string selectionMode = string.Empty;
			switch (this.UploadSelectionMode)
			{
				case UploadSelectionMode.FilesAndDirectories:
					selectionMode = "FILES_AND_DIRECTORIES"; break;
				case UploadSelectionMode.FilesOnly:
					selectionMode = "FILES_ONLY"; break;
				case UploadSelectionMode.DirectoriesOnly:
					selectionMode = "DIRECTORIES_ONLY"; break;
			}

			string fileFiltersCode = string.Empty;
			string fileFiltersCode2 = string.Empty;
			for(index=0;index<this.FileFilters.Count;index++)
			{
				fileFiltersCode += "file_filter_" + Convert.ToString(index + 1) + " = " + this.FileFilters[index].Value + "\" \\\n";
				fileFiltersCode += "file_filter_description_" + Convert.ToString(index + 1) + "  = \"" + this.FileFilters[index].Label + "\" \\\n";

				fileFiltersCode2 += "<PARAM NAME = file_filter_" + Convert.ToString(index + 1) + " VALUE=\"" + this.FileFilters[index].Value + "\">\\\n";
				fileFiltersCode2 += "<PARAM NAME = file_filter_description_" + Convert.ToString(index + 1) + " VALUE=\"" + this.FileFilters[index].Label + "\">\\\n";
 
			}

			string headersCode = string.Empty;
			string headersCode2 = string.Empty;

			for (index=0;index<this.Headers.Count;index++)
			{
				headersCode += "header_" + Convert.ToString(index + 1) + " = \"" + this.Headers[index].ToString() + "\" \\\n";
				headersCode2 += "<PARAM NAME=\"HEADER_" + Convert.ToString(index + 1) + "\" VALUE=\"" + this.Headers[index].ToString() + "\">\\\n";
			}
			

			output.Write(string.Format(appletCode, this.AppletLocation, this.Width.ToString(), this.Height.ToString(),
				this.ClientID, selectionMode, this.ScriptURL, this.DefaultDirectory, this.License, fileFiltersCode,
				fileFiltersCode2, GetCode("LABEL_BUTTON_ADD", this.LabelButtonAdd, true, false),
				GetCode("LABEL_BUTTON_REMOVE", this.LabelButtonRemove, true, false),
				GetCode("LABEL_BUTTON_UPLOAD", this.LabelButtonUpload, true, false),
				GetCode("LABEL_TIME_REMAINED", this.LabelTimeRemained, true, false),
				GetCode("LABEL_BUTTON_ADD", this.LabelButtonAdd, true, true),
				GetCode("LABEL_BUTTON_REMOVE", this.LabelButtonRemove, true, true),
				GetCode("LABEL_BUTTON_UPLOAD", this.LabelButtonUpload, true, true),
				GetCode("LABEL_TIME_REMAINED", this.LabelTimeRemained, true, true),
				GetCode("TABLE_HEADER_FILENAME", this.TableHeaderFilename, true, false),
				GetCode("TABLE_HEADER_SIZE", this.TableHeaderSize, true, false),
				GetCode("TABLE_HEADER_MODIFIED", this.TableHeaderModified, true, false),
				GetCode("TABLE_HEADER_FILENAME", this.TableHeaderFilename, true, true),
				GetCode("TABLE_HEADER_SIZE", this.TableHeaderSize, true, true),
				GetCode("TABLE_HEADER_MODIFIED", this.TableHeaderModified, true, true),
				(this.MaxFileNumber == 0 ? string.Empty : this.MaxFileNumber.ToString()),
				(this.MaxFileSize == 0 ? string.Empty : this.MaxFileSize.ToString()),
				(this.MaxUploadSize == 0 ? string.Empty : this.MaxUploadSize.ToString()),
				this.RedirectURL,
				this.RedirectTarget,
				headersCode,
				headersCode2,
				GetCode("IMAGE_PREVIEW", this.ImagePreviewEnabled.ToString(), true, false),
				GetCode("IMAGE_PREVIEW", this.ImagePreviewEnabled.ToString(), true, true),
				GetCode("IMAGE_PREVIEW_WIDTH", (this.ImagePreviewWidth != -1 ? this.ImagePreviewWidth.ToString() : ""), true, false),
				GetCode("IMAGE_PREVIEW_HEIGHT", (this.ImagePreviewHeight != -1 ? this.ImagePreviewHeight.ToString() : ""), true, false),
				GetCode("IMAGE_PREVIEW_WIDTH", (this.ImagePreviewWidth != -1 ? this.ImagePreviewWidth.ToString() : ""), true, true),
				GetCode("IMAGE_PREVIEW_HEIGHT", (this.ImagePreviewHeight != -1 ? this.ImagePreviewHeight.ToString() : ""), true, true),
                GetCode("SHOWTHUMBNAILCOLUMN", this.ShowThumbnailColumn.ToString(), true, false),
                GetCode("SHOWTHUMBNAILCOLUMN", this.ShowThumbnailColumn.ToString(), true, true),
                GetCode("THUMBNAILCOLUMNMAXHEIGHT", (this.ThumbnailColumnMaxHeight != -1 ? this.ThumbnailColumnMaxHeight.ToString() : ""), true, false),
                GetCode("THUMBNAILCOLUMNMAXWIDTH", (this.ThumbnailColumnMaxWidth != -1 ? this.ThumbnailColumnMaxWidth.ToString() : ""), true, false),
                GetCode("THUMBNAILCOLUMNMAXHEIGHT", (this.ThumbnailColumnMaxHeight != -1 ? this.ThumbnailColumnMaxHeight.ToString() : ""), true, true),
                GetCode("THUMBNAILCOLUMNMAXWIDTH", (this.ThumbnailColumnMaxWidth != -1 ? this.ThumbnailColumnMaxWidth.ToString() : ""), true, true),
                GetCode("THUMBNAILCOLUMNCONTRAINTPROPORTIONS", this.ThumbnailColumnContraintProportions.ToString(), true, false),
                GetCode("THUMBNAILCOLUMNCONTRAINTPROPORTIONS", this.ThumbnailColumnContraintProportions.ToString(), true, true),
                GetCode("PROXY_HOST", this.ProxyHost, true, true),
                GetCode("PROXY_HOST", this.ProxyHost, true, false),
                GetCode("PROXY_PORT", this.ProxyPort, true, true),
                GetCode("PROXY_PORT", this.ProxyPort, true, false),
				//GetCode("LABEL_BUTTON_CANCEL", this.LabelButtonCancel, true, false),
				GetCode("LABEL_BUTTON_CANCEL", this.LabelButtonCancel, true, true),
				//GetCode("COLOR_BACKGROUND", this.BackColor,true,false),
				GetCode("COLOR_BACKGROUND", this.BackColor,true,true),
				GetCode("COLOR_BUTTON_ADD", this.ColorButtonAdd,true,false),
				GetCode("COLOR_BUTTON_ADD", this.ColorButtonAdd,true,true),
				GetCode("COLOR_BUTTON_REMOVE", this.ColorButtonRemove,true,false),
				GetCode("COLOR_BUTTON_REMOVE", this.ColorButtonRemove,true,true),
				GetCode("COLOR_BUTTON_UPLOAD", this.ColorButtonUpload,true,false),
				GetCode("COLOR_BUTTON_UPLOAD", this.ColorButtonUpload,true,true),
				GetCode("COLOR_BUTTON_CANCEL", this.ColorButtonCancel,true,false),
				GetCode("COLOR_BUTTON_CANCEL", this.ColorButtonCancel,true,true)
				));


			//output.Write("<APPLET CODE=\"activeup.activeupload.UploadApplet.class\" ARCHIVE = \"" + this.AppletLocation + "\" WIDTH = " + this.Width.ToString() + " HEIGHT = " + this.Height.ToString() + " NAME = testing ALIGN = top VSPACE = 0 HSPACE = 0>\n");
			//output.Write("<PARAM NAME=\"CODE\" VALUE=\"activeup.activeupload.UploadApplet.class\">\n");
			//output.Write("<PARAM NAME=\"ARCHIVE\" VALUE=\"" + this.AppletLocation + "\">\n");
			//output.Write("<PARAM NAME=\"NAME\" VALUE=\"" + this.ClientID + "\">\n");
			//output.Write("<PARAM NAME=\"TYPE\" VALUE=\"application/x-java-applet;version=1.5.0\">\n");
			
			//output.Write("<PARAM NAME=\"scriptable\" VALUE=\"false\">\n");
			//output.Write("<PARAM NAME=\"SERVER_SCRIPT\" VALUE=\"" + this.ScriptURL + "\">\n");
			//output.Write("<PARAM NAME=\"SERVER_SCRIPT_ADRESS_TYPE\" VALUE=\"" + (ScriptURLType == ScriptURLType.Absolute ? "absolute" : "relative") + "\">\n");
			//output.Write("<PARAM NAME=\"DEFAULT_DIR\" VALUE=\"" + this.DefaultDirectory + "\">\n");
			/*for(index=0;index<this.FileFilters.Count;index++)
			{
				output.Write("<PARAM NAME=\"FILE_FILTER_" + Convert.ToString(index + 1) + "\" VALUE=\"" + this.FileFilters[index].Value + "\">\n");
				output.Write("<PARAM NAME=\"FILE_FILTER_DESCRIPTION_" + Convert.ToString(index + 1) + "\" VALUE=\"" + this.FileFilters[index].Label + "\">\n");
			}*/
			/*output.Write("<PARAM NAME=\"FILE_FILTER_1\" VALUE=\"*.jpg,*.gif\">\n");
			output.Write("<PARAM NAME=\"FILE_FILTER_DESCRIPTION_1\" VALUE=\"pictures\">\n");*/
			//output.Write("<PARAM NAME=\"SERIAL_NUMBER\" VALUE=\"" + this.License + "\">\n");
			//output.Write("<PARAM NAME=\"LABEL_BUTTON_ADD\" VALUE=\"" + this.LabelButtonAdd + "\">\n");
			//output.Write("<PARAM NAME=\"LABEL_BUTTON_REMOVE\" VALUE=\"" + this.LabelButtonRemove + "\">\n");
			//output.Write("<PARAM NAME=\"LABEL_BUTTON_UPLOAD\" VALUE=\"" + this.LabelButtonUpload + "\">\n");
			//output.Write("<PARAM NAME=\"LABEL_TIME_REMAINED\" VALUE=\"" + this.LabelTimeRemained + "\">\n");
			//output.Write("<PARAM NAME=\"TABLE_HEADER_FILENAME\" VALUE=\"" + this.TableHeaderFilename + "\">\n");
			//output.Write("<PARAM NAME=\"TABLE_HEADER_SIZE\" VALUE=\"" + this.TableHeaderSize + "\">\n");
			//output.Write("<PARAM NAME=\"TABLE_HEADER_MODIFIED\" VALUE=\"" + this.TableHeaderModified + "\">\n");
			//output.Write("<PARAM NAME=\"MAX_FILE_NUMBER\" VALUE=\"" + (this.MaxFileNumber == 0 ? string.Empty : this.MaxFileNumber.ToString()) + "\">\n");
			//output.Write("<PARAM NAME=\"MAX_FILE_SIZE\" VALUE=\"" + (this.MaxFileSize == 0 ? string.Empty : this.MaxFileSize.ToString()) + "\">\n");
			//output.Write("<PARAM NAME=\"UPLOAD_MAX_SIZE\" VALUE=\"" + (this.MaxUploadSize == 0 ? string.Empty : this.MaxUploadSize.ToString()) + "\">\n");
			//output.Write("<PARAM NAME=\"FINISHED_REDIRECT_PAGE\" VALUE=\"" + this.RedirectURL + "\">\n");
			//output.Write("<PARAM NAME=\"FINISHED_REDIRECT_ADRESS_TYPE\" VALUE=\"relative\">\n");
			//output.Write("<PARAM NAME=\"FINISHED_REDIRECT_TARGET\" VALUE=\"" + this.RedirectTarget + "\">\n");
			//for (index=0;index<this.Headers.Count;index++)
			//	output.Write("<PARAM NAME=\"HEADER_" + Convert.ToString(index + 1) + "\" VALUE=\"" + this.Headers[index].ToString() + "\">\n");
			//output.Write("</APPLET>");
		}

		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="content">The content.</param>
		/// <param name="notEmpty">if set to <c>true</c> not an empty code.</param>
		/// <param name="param">if set to <c>true</c> use parameter.</param>
		/// <returns></returns>
		public string GetCode(string name, Color content, bool notEmpty, bool param)
		{
			if (content != Color.Empty)
			{
				return GetCode(name,Utils.Color2Hex(content),notEmpty,param);
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="content">The content.</param>
		/// <param name="notEmpty">if set to <c>true</c> not empty.</param>
		/// <param name="param">if set to <c>true</c> use parameter.</param>
		/// <returns></returns>
		public string GetCode(string name, string content, bool notEmpty, bool param)
		{
			if (!notEmpty || (notEmpty && content != string.Empty))
			{
				if (param)
					return "<PARAM NAME=\"" + name + "\" VALUE=\"" + content + "\">\\\n";
				else
					return name + " = \"" + content + "\"\\\n";
			}

			return string.Empty;
		}
		/*
				/// <summary>
				/// Determine if we need to register the client side script and render the calendar, selectors with validation or selectors only.
				/// </summary>
				/// <returns>0 if scripting not allowed, 1 if not an uplevel browser but scripting allowed, 2 if all is OK.</returns>
				protected virtual bool IsIE() 
				{
					// Must be on a page.
					// Check whether the client browser has turned off scripting and check
					// browser capabilities. Active Calendar needs the W3C DOM level 1 for
					// control manipulation, Internet Explorer 4+ and at least ECMAScript 1.2.
					Page page = Page;
			
					// Check the browser compatibility.
					System.Web.HttpBrowserCapabilities browser = page.Request.Browser; 

					// Check the browser.
					if (browser.Browser.ToUpper().IndexOf("NETSCAPE") > -1)
						return false;
					else
						return true;
				}*/


		/// <summary>
		/// Gets or sets the license key.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("Lets you specify the license key.")
		]
		public string License
		{
			get
			{
				object obj = ViewState["License"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
                ViewState["License"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the label of the Add button.
		/// </summary>
		public string LabelButtonAdd
		{
			get
			{
				object obj = ViewState["_labelButtonAdd"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["_labelButtonAdd"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the label of the Remove button.
		/// </summary>
		public string LabelButtonRemove
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(LabelButtonRemove), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(LabelButtonRemove), value);
			}
		}
		
		/// <summary>
		/// Gets or sets the label of the Upload button.
		/// </summary>
		public string LabelButtonUpload
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(LabelButtonUpload), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(LabelButtonUpload), value);
			}
		}

		/// <summary>
		/// Gets or sets the label of the Cancel button.
		/// </summary>
		public string LabelButtonCancel
		{
			get
			{
				object obj = ViewState["LabelButtonCancel"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["LabelButtonCancel"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the label of the Time Remained indication.
		/// </summary>
		public string LabelTimeRemained
		{
			get
			{
				object obj = ViewState["LabelTimeRemained"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["LabelTimeRemained"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the label of the table header Filename.
		/// </summary>
		public string TableHeaderFilename
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(TableHeaderFilename), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(TableHeaderFilename), value);
			}
		}

		/// <summary>
		/// Gets or sets the label of the table header Size.
		/// </summary>
		public string TableHeaderSize
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(TableHeaderSize), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(TableHeaderSize), value);
			}
		}
		
		/// <summary>
		/// Gets or sets the label of the table header Modified.
		/// </summary>
		public string TableHeaderModified
		{
			get
			{
				object obj = ViewState["TableHeaderModified"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["TableHeaderModified"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the width in pixels.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the width in pixels."),
		DefaultValue(505)
		]
		public int Width
		{
			get
			{
				object obj = ViewState["Width"];
				if (obj != null)
					return (int)obj;
				else
					return 505;
			}
		
			set
			{
				ViewState["Width"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the width in pixels.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the height in pixels."),
		DefaultValue(200)
		]
		public int Height
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(Height), 200);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(Height), value);
			}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color."),
		DefaultValue("")
		]
		public Color BackColor
		{
			get
			{
				object backColor = ViewState["BackColor"];
				if (backColor != null)
					return (Color)backColor;

				return Color.Empty;
			}

			set
			{
				ViewState["BackColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the add button.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color of the add button."),
		DefaultValue("")
		]
		public Color ColorButtonAdd
		{
			get
			{
				object colorButtonAdd = ViewState["ColorButtonAdd"];
				if (colorButtonAdd != null)
					return (Color)colorButtonAdd;

				return Color.Empty;
			}

			set
			{
				ViewState["ColorButtonAdd"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the remove button.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color of the remove button."),
		DefaultValue("")
		]
		public Color ColorButtonRemove
		{
			get
			{
				object colorButtonRemove = ViewState["ColorButtonRemove"];
				if (colorButtonRemove != null)
					return (Color)colorButtonRemove;

				return Color.Empty;
			}

			set
			{
				ViewState["ColorButtonRemove"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the remove button.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color of the upload button."),
		DefaultValue("")
		]
		public Color ColorButtonUpload
		{
			get
			{
				object colorButtonUpload = ViewState["ColorButtonUpload"];
				if (colorButtonUpload != null)
					return (Color)colorButtonUpload;

				return Color.Empty;
			}

			set
			{
				ViewState["ColorButtonUpload"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the cancel button.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color of the cancel button."),
		DefaultValue("")
		]
		public Color ColorButtonCancel
		{
			get
			{
				object colorButtonCancel = ViewState["ColorButtonCancel"];
				if (colorButtonCancel != null)
					return (Color)colorButtonCancel;

				return Color.Empty;
			}

			set
			{
				ViewState["ColorButtonCancel"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the maximum number of file to be uploaded.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the maximum number of file to be uploaded.")
		]
		public int MaxFileNumber
		{
			get
			{
				object obj = ViewState["MaxFileNumber"];
				if (obj != null)
					return (int)obj;
				else
					return 0;
			}
		
			set
			{
				ViewState["MaxFileNumber"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum file size allowed in bytes.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the maximum file size allowed in bytes.")
		]
		public int MaxFileSize
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(MaxFileSize), 0);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(MaxFileSize), value);
			}
		}

		/// <summary>
		/// Gets or sets the maximum total size in bytes allowed for the uploaded files.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the maximum total size in bytes allowed for the uploaded files.")
		]
		public int MaxUploadSize
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(MaxUploadSize), 0);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(MaxUploadSize), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if we want to enable image preview.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the value indicating if we want to enable image preview.")
		]
		public bool ImagePreviewEnabled
		{
			get
			{
				object obj = ViewState["ImagePreviewEnabled"];
				if (obj != null)
					return (bool)obj;
				else
					return true;
			}
		
			set
			{
				ViewState["ImagePreviewEnabled"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating the width of the preview thumbnail.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the value indicating the width of the preview thumbnail.")
		]
		public int ImagePreviewWidth
		{
			get
			{
				object obj = ViewState["ImagePreviewWidth"];
				if (obj != null)
					return (int)obj;
				else
					return -1;
			}
		
			set
			{
				ViewState["ImagePreviewWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating the height of the preview thumbnail.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the value indicating the height of the preview thumbnail.")
		]
		public int ImagePreviewHeight
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ImagePreviewHeight), -1);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ImagePreviewHeight), value);
			}
		}

        /// <summary>
        /// Gets or sets the value indicating if we want to make thumbnail column visible.
        /// </summary>
		[Bindable(true),
		Category("Behavior"),
        Description("Gets or sets the value indicating if we want to make thumbnail collumn visible.")
		]
        public bool ShowThumbnailColumn
        {

            get
            {
                object obj = ViewState["ShowThumbnailColumn"];
                if (obj != null)
                    return (bool)obj;
                else
                    return false;
            }

            set
            {
                ViewState["ShowThumbnailColumn"] = value;
            }

        }

        /// <summary>
        /// Gets or sets the value indicating the maximum height of the thumbnail column.
        /// </summary>
        [
        Bindable(true),
        Category("Behavior"),
        Description("Gets or sets the value indicating the maximum height of the thumbnail column.")
        ]
        public int ThumbnailColumnMaxHeight
        {
            get
            {
                object obj = ViewState["ThumbnailColumnMaxHeight"];
                if (obj != null)
                    return (int)obj;
                else
                    return -1;
            }

            set
            {
                ViewState["ThumbnailColumnMaxHeight"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating the maximum width of the thumbnail column.
        /// </summary>
        [
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the value indicating the maximum width of the thumbnail column.")
		]
		public int ThumbnailColumnMaxWidth
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ThumbnailColumnMaxWidth), -1);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ThumbnailColumnMaxWidth), value);
			}
		}

        /// <summary>
        /// Gets or sets the value indicating if we want set constraints in thumbnail column proportions.
        /// </summary>
        [Bindable(true),
        Category("Behavior"),
       Description("Gets or sets the value indicating if we want set constraints in thumbnail column proportions.")
        ]
        public bool ThumbnailColumnContraintProportions
        {

            get
            {
                object obj = ViewState["ThumbnailColumnContraintProportions"];
                if (obj != null)
                    return (bool)obj;
                else
                    return true;
            }

            set
            {
                ViewState["ThumbnailColumnContraintProportions"] = value;
            }

        }

        /// <summary>
        /// Gets or sets the proxy host.
        /// </summary>
        [
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the proxy host.")
		]
        public string ProxyHost
        {
            get
            {
                object obj = ViewState["ProxyHost"];
                if (obj != null)
                    return (string)obj;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["ProxyHost"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the proxy port number.
        /// </summary>
        [
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the proxy port number.")
		]
		public string ProxyPort
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ProxyPort), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ProxyPort), value);
			}
		}

		/// <summary>
		/// Gets or sets the URL of the upload script.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the URL of the upload script.")
		]
		public string ScriptURL
		{
			get
			{
				object obj = ViewState["ScriptURL"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["ScriptURL"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the default directory where to pick files client-side.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the default directory where to pick files client-side.")
		]
		public string DefaultDirectory
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(DefaultDirectory),  DefaultDirectoryValue);
			}
		
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(DefaultDirectory), value);
			}
		}

		/// <summary>
		/// Gets or sets the URL of the page where to be redirected after the upload.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the URL of the page where to be redirected after the upload.")
		]
		public string RedirectURL
		{
			get
			{
				object obj = ViewState["RedirectURL"];
				if (obj != null)
					return (string)obj;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["RedirectURL"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the target frame where to redirect after the upload.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the target frame where to redirect after the upload.")
		]
		public string RedirectTarget
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(RedirectTarget), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(RedirectTarget), value);
			}
		}

		/// <summary>
		/// Gets or sets the location of the upload applet.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the location of the upload applet.")
		]
		public string AppletLocation
		{
			get
			{
				object obj = ViewState["AppletLocation"];
				if (obj != null)
					return (string)obj;
				else
					return "ActiveUpload.jar";
			}
			set
			{
				ViewState["AppletLocation"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the files selection mode.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the files selection mode.")
		]
		public UploadSelectionMode UploadSelectionMode
		{
			get
			{
				object obj = ViewState["UploadSelectionMode"];
				if (obj != null)
					return (UploadSelectionMode)obj;
				else
					return UploadSelectionMode.FilesAndDirectories;
			}
		
			set
			{
				ViewState["UploadSelectionMode"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the script URL type.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Gets or sets the script URL type.")
		]
		public ScriptURLType ScriptURLType
		{
			get
			{
				object obj = ViewState["ScriptURLType"];
				if (obj != null)
					return (ScriptURLType)obj;
				else
					return ScriptURLType.Relative;
			}
			set
			{
				ViewState["ScriptURLType"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the headers.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		Description("Gets or sets the headers."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ArrayList Headers
		{
			get
			{
				object obj = ViewState["Headers"];
				if (obj != null)
					return (ArrayList)obj;
				else
				{
					ArrayList list = new ArrayList();
					list.Add("ActiveUpload_Version = 1.2");
					ViewState["Headers"] = list;
					
					return (ArrayList)ViewState["Headers"];
				}
			}
			set
			{
				ViewState["Headers"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the file filters.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		Description("Gets or sets the file filters."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public LabelValueCollection FileFilters
		{
			get
			{
				object obj = ViewState["FileFilters"];
				if (obj != null)
					return (LabelValueCollection)obj;
				else
				{
					ViewState["FileFilters"] = new LabelValueCollection();
					return (LabelValueCollection)ViewState["FileFilters"];
				}
			}
			set
			{
				ViewState["FileFilters"] = value;
			}
		}
		
	}
}

