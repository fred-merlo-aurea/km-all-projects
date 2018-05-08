// ActiveInput
// Copyright (c) 2005 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Data;
using System.IO;
using System.Xml; 
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// ASP.NET control that allows to create styled listbox with client-side and server-side ordering, enumeration, move items to and from others listboxes, data bind and more...
	/// </summary>
	[
    ToolboxBitmap(typeof(OrderedListBox), "ToolBoxBitmap.Listbox.bmp"),
	ParseChildren(true, "Items"),
	Serializable 
	]
	public class OrderedListBox : CoreWebControl, IPostBackDataHandler, IPostBackEventHandler
	{
		private const string TxtLeft = "Left";
		private const string TxtRight = "Right";
		private const string TxtUp = "Up";
		private const string TxtDown = "Down";
		private const string UpImageFileName = "arrow_up_blue.gif";
		private const string DownImageFileName = "arrow_down_blue.gif";
		private const string LeftImageFileName = "arrow_left_blue.gif";
		private const string RightImageFileName = "arrow_right_blue.gif";

		private OrderedListBoxItemCollection _items = null;
		//private string _externalScript, _scriptDirectory;
		private static string CLIENTSIDE_API;
		private static string SCRIPTKEY = "ACTIVEINPUT";
		private bool _clientSideEnabled = true;
		/// <summary>
		/// Data source used for data binding.
		/// </summary>
		private object _dataSource;

		/*public OrderedListBox()
		{

		}*/

#if (LICENSE)

		/// <summary>
		/// Used for the license count number.
		/// </summary>
		private static int _useCounter = 0;

/*		/// <summary>
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
				object local = ViewState["License"];
				if (local != null)
					return (string)local;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["License"] = value;
			}
		}*/
#endif

		/// <summary>
		/// Gets or sets the collection containing the items.
		/// </summary>
		[
		DefaultValue(null),
		MergableProperty(false),
		PersistenceMode(PersistenceMode.InnerDefaultProperty),
		Description("Items of the contol.")
		]
		public OrderedListBoxItemCollection Items 
		{
			get 
			{
				if (_items == null) 
				{
					_items = new OrderedListBoxItemCollection();
					if (IsTrackingViewState) 
					{
						((IStateManager)_items).TrackViewState();
					}
				}
				return _items;
			}
		}

		/// <summary>
		/// Gets or sets the specific table in the DataSource to bind to the <see cref="OrderedListBox"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The specific table in the DataSource to bind to the OrderedListBox.")
		]
		public virtual string DataMember
		{
			get
			{
				object dataMember = ViewState["_dataMember"];
				if (dataMember != null)
					return (string)dataMember;
				else return string.Empty;
			}

			set
			{
				ViewState["_dataMember"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the data source for this <see cref="OrderedListBox"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Description("The data source for this OrderedListBox.")
		]
		public virtual object DataSource
		{
			get
			{
				return _dataSource;
			}

			set
			{
				/*if (value != null & !(value is IListSource) &&!(value is IEnumerable))
					throw new ArgumentException("Invalue datasource type","value");*/
				_dataSource = value;
			}
		}

		/// <summary>
		/// Gets or sets the data source that provides the text content of <see cref="OrderedListBoxItem"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),	
		Description("The data source that provides the text content of OrderedListBoxItem.")
		]
		public virtual string DataTextField
		{
			get
			{
				object dataTextField = ViewState["_dataTextField"];
				if (dataTextField != null)
					return (string)dataTextField;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_dataTextField"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the data source that provides the selected state of <see cref="OrderedListBoxItem"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The data source that provides the selected state of ToolItem.")
		]
		public virtual string DataSelectedField
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(DataSelectedField), string.Empty);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(DataSelectedField), value);
			}
		}

		/// <summary>
		/// Gets or sets the data source that provides the lock state of <see cref="OrderedListBoxItem"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),	
		Description("The data source that provides the lock state of ToolItem.")
		]
		public virtual string DataLockedField
		{
			get
			{
				object dataLockedField = ViewState["_dataLockedField"];
				if (dataLockedField != null)
					return (string)dataLockedField;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_dataLockedField"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the formatting string used to control how data bound to the <see cref="OrderedListBox"/> control is displayed.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The formatting string used to control how data bound to the ToolDropDownList control is displayed.")
		]
		public virtual string DataTextFormatString
		{
			get
			{
				object dataTextFormatString = ViewState["_dataTextFormatString"];
				if (dataTextFormatString != null)
					return (string)dataTextFormatString;
				else return string.Empty;
			}

			set
			{
				ViewState["_dataTextFormatString"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each <see cref="OrderedListBoxItem"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the value of each OrderedListBoxItem.")
		]
		public virtual string DataValueField
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(DataValueField), string.Empty);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(DataValueField), value);
			}
		}

		/// <summary>
		/// Gets or sets the number of rows in height of the listbox.
		/// </summary>
		[DefaultValue(4), Bindable(true), Category("Appearance"), Description("the number of rows in height of the listbox.")]
		public virtual int Rows
		{
			get
			{
				object rows = this.ViewState["Rows"];
				if (rows != null)
				{
					return (int) rows;
				}
				return 4;
			}
			set
			{
				if ((value < 1) || (value > 2000))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Rows"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width of the ordered list box.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ArgumentException">The specified border width is a negative value.</exception>
		[Browsable(false)]
		public override Unit BorderWidth
		{
			get
			{
				return base.BorderWidth;
			}
			set
			{
				base.BorderWidth = value;
			}
		}
 
		/// <summary>
		/// Gets or sets the border style of the ordered list box.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		public override BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}
 
		/// <summary>
		/// Gets or sets the border color of ordered list box.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		public override Color BorderColor
		{
			get
			{
				return base.BorderColor;
			}
			set
			{
				base.BorderColor = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not the control render the move buttons. 
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(false),
		Browsable(true),
		Description("Indicates whether or not the control render the move buttons.")
		]
		public virtual bool MoveButtonsDisabled
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(MoveButtonsDisabled), false);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(MoveButtonsDisabled), value);
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not the control enumerate the items.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("Indicates whether or not the control render enumerate the items.")
		] 
		public virtual bool Enumerate
		{
			get
			{
				object obj = ViewState["_enumerate"];
				if (obj != null)
					return ((bool) obj); 
				return true; 
			}
			set
			{
				ViewState["_enumerate"] = value;
			}
		} 

		/// <summary>
		/// Gets or sets a value that indicates whether or not the control allows multiple selection. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("Indicates whether or not the control allows multiple selection.")
		] 
		public virtual bool MultipleSelectionEnabled
		{
			get
			{
				object obj = ViewState["_multipleSelection"];
				if (obj != null)
					return ((bool) obj); 
				return false; 
			}
			set
			{
				ViewState["_multipleSelection"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the left ordered list box.
		/// </summary>
		/// <remarks>Actually, only controls listed in the design-time property windows are supported.</remarks>
		[DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(ActiveUp.WebControls.OrderedListBoxConverter))]
		public string LeftListBox
		{
			get
			{
				object local = ViewState["LeftListBox"];
				if (local != null)
				{
					return (string)local;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				ViewState["LeftListBox"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the left ordered list box.
		/// </summary>
		/// <remarks>Actually, only controls listed in the design-time property windows are supported.</remarks>
		[DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(ActiveUp.WebControls.OrderedListBoxConverter))]
		public string RightListBox
		{
			get
			{
				Page.Trace.Write("Right");
				object local = ViewState["RightListBox"];
				if (local != null)
				{
						Page.Trace.Write("isnull");

					return (string)local;
				}
				else
				{
					return string.Empty;
				}
			}

			set
			{
				ViewState["RightListBox"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the navigation type.
		/// </summary>
		[DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(ActiveUp.WebControls.OrderedListBoxConverter))]
		public OrderedListBoxNavigation Navigation
		{
			get
			{
				object local = ViewState["Navigation"];
				if (local != null)
					return (OrderedListBoxNavigation)local;
				else
					return OrderedListBoxNavigation.NotSet;
			}

			set
			{
				ViewState["Navigation"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the css class to use with the buttons.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("The css class to use with the buttons.")
		] 
		public string ButtonsCssClass
		{
			get
			{
				object local = ViewState["_buttonsCssClass"];
				if (local != null)
					return (string)local;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_buttonsCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to use with the up button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(true),
		Browsable(true),
		Description("The text to use with the up button.")
		]
		public string ButtonUpText
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonUpText), TxtUp);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonUpText), value);
			}
		}

		/// <summary>
		/// Gets or sets the image to use with the up button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Browsable(true),
		Description("The image to use with the up button."),
#if (!FX1_1)
	DefaultValue("")
#else
	DefaultValue("arrow_up_blue.gif")
#endif
		]
		public string ButtonUpImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, UpImageFileName);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonUpImage), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonUpImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the button is enabled or not.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(false),
		Browsable(true),
		Description("The value indicating if the button is enabled or not.")
		]
		public bool ButtonUpDisabled
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonUpDisabled), false);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonUpDisabled), value);
			}
		}

		/// <summary>
		/// Gets or sets the text to use with the down button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(true),
		Browsable(true),
		Description("The text to use with the down button.")
		]
		public string ButtonDownText
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonDownText), TxtDown);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonDownText), value);
			}
		}

		/// <summary>
		/// Gets or sets the image to use with the down button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Browsable(true),
		Description("The image to use with the down button."),
#if (!FX1_1)
	DefaultValue("")
#else
	DefaultValue("arrow_down_blue.gif")		
#endif
		]
		public string ButtonDownImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, DownImageFileName);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonDownImage), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonDownImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the button is disabled or not.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(false),
		Browsable(true),
		Description("The value indicating if the button is disabled or not.")
		]
		public bool ButtonDownDisabled
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonDownDisabled), false);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonDownDisabled), value);
			}
		}

		/// <summary>
		/// Gets or sets the text to use with the left button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(true),
		Browsable(true),
		Description("The text to use with the left button.")
		]
		public string ButtonLeftText
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonLeftText), TxtLeft);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonLeftText), value);
			}
		}

		/// <summary>
		/// Gets or sets the image to use with the left button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Browsable(true),
		Description("The image to use with the left button."),
#if (!FX1_1)
	DefaultValue("")
#else
	DefaultValue("arrow_left_blue.gif")
#endif
		]
		public string ButtonLeftImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, LeftImageFileName);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonLeftImage), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonLeftImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the button is disabled or not.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(false),
		Browsable(true),
		Description("The value indicating if the button is disabled or not.")
		]
		public bool ButtonLeftDisabled
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonLeftDisabled), false);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonLeftDisabled), value);
			}
		}

		/// <summary>
		/// Gets or sets the text to use with the right button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(true),
		Browsable(true),
		Description("The text to use with the right button.")
		]
		public string ButtonRightText
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonRightText), TxtRight);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonRightText), value);
			}
		}

		/// <summary>
		/// Gets or sets the image to use with the right button.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Browsable(true),
		Description("The image to use with the right button."),
#if (!FX1_1)
	DefaultValue("")
#else
	DefaultValue("arrow_right_blue.gif")
#endif
		]
		public string ButtonRightImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, RightImageFileName);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonRightImage), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonRightImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the button is disabled or not.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(false),
		Browsable(true),
		Description("The value indicating if the button is disabled or not.")
		]
		public bool ButtonRightDisabled
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ButtonRightDisabled), false);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ButtonRightDisabled), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if the client side script is disabled or not.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		DefaultValue(true),
		Browsable(true),
		Description("The value indicating if the client side script is disabled or not.")
		]
		public RenderType RenderType
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(RenderType), RenderType.NotSet);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(RenderType), value);
			}
		}

		#region .NET API
		/// <summary>
		/// Registers the API script block.
		/// </summary>
		/// <param name="page">The page.</param>
		public virtual void RegisterAPIScriptBlock(System.Web.UI.Page page) 
		{
			// Register the script block is not allready done.

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveInput.js"));
#else
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveInput.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
#endif					
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveInput.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}


			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_startup"))
			{
				string startupString = string.Empty;
				startupString += "<script>\n";
				startupString += "// Test if the client script is present.\n";
				startupString += "try\n{\n";
				startupString += "AIP_testIfScriptPresent();\n";
				//startupString += "}\ncatch (e) \n{\nalert('Could not find external script file. Please Check the documentation.');\n}\n";
				startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
				startupString += "</script>\n";

				page.RegisterClientScriptBlock(SCRIPTKEY + "_startup",startupString);
			}
		}

		/// <summary>
		/// Determine if we need to register the client side script and render the control.
		/// </summary>
		/// <returns>0 if scripting not allowed, 1 if not an uplevel browser but scripting allowed, 2 if all is OK.</returns>
		private bool IsUpLevel() 
		{
			Page page = Page;
		
			if (this.RenderType == RenderType.NotSet)
			{
				if (page == null || page.Request == null || !page.Request.Browser.JavaScript ||	!(page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0)) 
					return false;

				System.Web.HttpBrowserCapabilities browser = page.Request.Browser; 

				if (((browser.Browser.ToUpper().IndexOf("IE") > -1 && browser.MajorVersion >= 4)
					|| (browser.Browser.ToUpper().IndexOf("NETSCAPE") > -1 && browser.MajorVersion >= 5)))
					return true;

				else if (browser.Browser.ToUpper().IndexOf("OPERA") > -1 && browser.MajorVersion >= 3)
					return true;

				return false;
			}
			else if (this.RenderType == RenderType.UpLevel)
					return true;

			return false;
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary>
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);
			
			_clientSideEnabled = IsUpLevel();

			if (_clientSideEnabled)
			{
				if (((base.Page != null) && base.Enabled))
				{
					RegisterAPIScriptBlock(this.Page);
				}
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AIP, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				RenderOrderedListBox(output);
			}

#elif (TRIAL)
			_useCounter++;
			if (_useCounter == StaticContainer.UsageCount) 
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
            }
			else
				RenderOrderedListBox(output);
#else
			RenderOrderedListBox(output);
#endif

		}

		/// <summary>
		/// Renders the ordered list box.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderOrderedListBox(HtmlTextWriter output) 
		{
            if (System.Web.HttpContext.Current != null)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                output.RenderBeginTag(HtmlTextWriterTag.Table); // Table Open

                output.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr Open

                output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                output.RenderBeginTag(HtmlTextWriterTag.Td); // Td Open

                // Begin Render ListBox
                output.AddAttribute(HtmlTextWriterAttribute.Size, "8");
                output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_select");
                output.AddAttribute(HtmlTextWriterAttribute.Size, this.Rows.ToString());

                if (MultipleSelectionEnabled)
                    output.AddAttribute(HtmlTextWriterAttribute.Multiple, "multiple");
                output.AddAttribute("onfocus", string.Format("AIP_saveOrder('{0}');", this.ClientID));
                output.AddAttribute("onblur", string.Format("AIP_saveOrder('{0}');", this.ClientID));
                if (this.LeftListBox != string.Empty)
                    output.AddAttribute("leftlistbox", this.LeftListBox);
                if (this.RightListBox != string.Empty)
                    output.AddAttribute("rightlistbox", this.RightListBox);
                if (this.Enumerate)
                    output.AddAttribute("enumerate", "true");
                base.AddAttributesToRender(output);
                output.RenderBeginTag(HtmlTextWriterTag.Select);
                RenderItems(output);
                output.RenderEndTag();
                // End Render ListBox

                output.RenderEndTag(); // Td Close

                if (!this.MoveButtonsDisabled)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                    output.RenderBeginTag(HtmlTextWriterTag.Td); // Td Open

                    if (this._clientSideEnabled)
                    {
                        if (!this.ButtonUpDisabled)
                            RenderButton(output, "AIP_moveItemUp('{0}');", this.ButtonUpText, this.ButtonUpImage, true,"arrow_up_blue.gif");
                        if (!this.ButtonDownDisabled)
                            RenderButton(output, "AIP_moveItemDown('{0}');", this.ButtonDownText, this.ButtonDownImage, true,"arrow_down_blue.gif");
                        if (!this.ButtonLeftDisabled && this.LeftListBox != string.Empty)
                            RenderButton(output, "AIP_moveItemLeft('{0}');", this.ButtonLeftText, this.ButtonLeftImage, true,"arrow_left_blue.gif");
                        if (!this.ButtonRightDisabled && this.RightListBox != string.Empty)
                            RenderButton(output, "AIP_moveItemRight('{0}');", this.ButtonRightText, this.ButtonRightImage, false, "arrow_right_blue.gif");
                    }
                    else
                    {
                        if (!this.ButtonUpDisabled)
                            RenderButton(output, (this.Page != null) ? this.Page.GetPostBackClientEvent(this, "up") : string.Empty, this.ButtonUpText, this.ButtonUpImage, true,"arrow_up_blue.gif");
                        if (!this.ButtonDownDisabled)
                            RenderButton(output, (this.Page != null) ? this.Page.GetPostBackClientEvent(this, "down") : string.Empty, this.ButtonDownText, this.ButtonDownImage, true,"arrow_down_blue.gif");
                        if (!this.ButtonLeftDisabled && this.LeftListBox != string.Empty)
                            RenderButton(output, (this.Page != null) ? this.Page.GetPostBackClientEvent(this, "left") : string.Empty, this.ButtonLeftText, this.ButtonLeftImage, true,"arrow_left_blue.gif");
                        if (!this.ButtonRightDisabled && this.RightListBox != string.Empty)
                            RenderButton(output, (this.Page != null) ? this.Page.GetPostBackClientEvent(this, "right") : string.Empty, this.ButtonRightText, this.ButtonRightImage, false,"arrow_right_blue.gif");
                    }

                    output.RenderEndTag(); // Td Close
                }

                output.RenderEndTag(); // Tr Close

                output.RenderEndTag(); // Table Close

                output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
                output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
                output.AddAttribute(HtmlTextWriterAttribute.Value, this.Items.GetOrderString());
                output.RenderBeginTag(HtmlTextWriterTag.Input);
                output.RenderEndTag();
            }
		}

		private void RenderButton(HtmlTextWriter output, string onClick, string text, string image, bool lineFeed,string imageResource)
		{
			switch (this.Navigation)
			{

				case OrderedListBoxNavigation.NotSet:
				case OrderedListBoxNavigation.Image:
					output.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:" + onClick, this.ClientID));
					output.RenderBeginTag(HtmlTextWriterTag.A);
					output.AddAttribute(HtmlTextWriterAttribute.Value, text);
					output.AddAttribute(HtmlTextWriterAttribute.Type, "image");
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(this.IconsDirectory, image, imageResource, Page, this.GetType()));
					output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
					output.RenderBeginTag(HtmlTextWriterTag.Img);
					output.RenderEndTag();
					output.RenderEndTag();
					break;
				case OrderedListBoxNavigation.Button:
					output.AddAttribute("onclick", string.Format(onClick, this.ClientID));
					output.AddAttribute(HtmlTextWriterAttribute.Value, text);
					output.AddAttribute(HtmlTextWriterAttribute.Type, "button");
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();
					break;
				case OrderedListBoxNavigation.Link:
					output.AddAttribute("onclick", string.Format(onClick, this.ClientID));
					output.AddAttribute(HtmlTextWriterAttribute.Href, "#");
					output.RenderBeginTag(HtmlTextWriterTag.A);
					output.Write(text);
					output.RenderEndTag();
					break;
			}

			if (lineFeed)
				output.Write("<br>");
		}
		/// <summary>
		/// Write the items to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		private void RenderItems(HtmlTextWriter output)
		{
			if (Items != null) 
				for (int i = 0 ; i < Items.Count ; i++)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Value, Items[i].Value);
					if (Items[i].Selected)
						output.AddAttribute(HtmlTextWriterAttribute.Selected, "true");
					if (Items[i].Locked)
						output.AddAttribute("locked", "true");
					Page.Trace.Write(Items[i].Style);
					if (Items[i].Style != string.Empty)
						output.AddAttribute("Style", Items[i].Style);
					output.RenderBeginTag(HtmlTextWriterTag.Option);
					if (this.Enumerate)
						output.Write((i+1).ToString() + ". ");
					output.Write(Items[i].Text);
					output.RenderEndTag();
				}
		}

		#endregion

		#region IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Write(this.ID, "LoadPostData...");

			string _orderValue = postCollection[UniqueID].TrimEnd('|');

			if (_orderValue != this.Items.GetOrderString())
			{
				this.Items.SetOrderString(_orderValue, this.Enumerate);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
			OnOrderIndexChanged(EventArgs.Empty);
		}

		#endregion

		#region Events

		/// <summary>
		/// Raise the SelectedIndexChanged of the <see cref="OrderedListBox"/> control. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnOrderIndexChanged(EventArgs e) 
		{
			// Check if someone use our event.
			if (OrderIndexChanged != null)
				OrderIndexChanged(this,e);
		}

		/// <summary>
		/// Oder index change server event.
		/// </summary>
		public event EventHandler OrderIndexChanged;

		#endregion

		#region IPostBackEventHandler

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			Page.Trace.Write(this.ID, "RaisePostBackEvent..." + eventArgument);
		
			switch (eventArgument)
			{
				case "up": this.Items.MoveSelectedUp(); break;
				case "down": this.Items.MoveSelectedDown(); break;
				case "left": this.Items.MoveSelectedToListBox(this.Page, this.LeftListBox); break;
				case "right": this.Items.MoveSelectedToListBox(this.Page, this.RightListBox); break;
			}

			OnOrderIndexChanged(EventArgs.Empty);
		}

		#endregion

		#region Databind
		/// <summary>
		/// Raises the DataBinding event.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnDataBinding(EventArgs e)
		{
			Page.Trace.Write("OnDataBinding");
			base.OnDataBinding(e);

			string dataTextField = DataTextField;
			string dataValueField = DataValueField;
			string dataSelectedField = DataSelectedField;
			string dataLockedField = DataLockedField;
			string dataTextFormat = DataTextFormatString;
			bool valuePresent = false;
			bool formatPresent = false;

			IEnumerable iEnumerable = Utils.GetResolvedDataSource(DataSource, DataMember);
			if (iEnumerable == null)
			{
				return;
			}

			Items.Clear();

			ICollection iCollection = iEnumerable as ICollection;
			if ((dataTextField.Length != 0) || (dataValueField.Length != 0))
				valuePresent = true;
			if (dataTextFormat.Length != 0)
				formatPresent = true;

			IEnumerator iEnumerator = iEnumerable.GetEnumerator();
			try
			{
				while(iEnumerator.MoveNext())
				{
					object current = iEnumerator.Current;
					OrderedListBoxItem toolItem = new OrderedListBoxItem();
					if (valuePresent)
					{
						if(dataTextField.Length > 0)
							toolItem.Text = DataBinder.GetPropertyValue(current,dataTextField,dataTextFormat);
						if(dataValueField.Length > 0)
							toolItem.Value = DataBinder.GetPropertyValue(current, dataValueField, null);
						if (dataSelectedField.Length > 0)
							toolItem.Selected = (bool)DataBinder.GetPropertyValue(current, dataSelectedField);
						if (dataLockedField.Length > 0)
							toolItem.Locked = (bool)DataBinder.GetPropertyValue(current, dataLockedField);
					}
					else
					{
						if (formatPresent)
							toolItem.Text = string.Format(dataTextFormat, current);
						else
							toolItem.Text = current.ToString();
					}

					Items.Add(toolItem);
				}
			}

			finally
			{
				IDisposable iDisposable = iEnumerator as IDisposable;
				if (iDisposable != null)
					iDisposable.Dispose();
			}

		}
		#endregion
	}
}
