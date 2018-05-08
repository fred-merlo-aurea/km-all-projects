using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Extension;

namespace ActiveUp.WebControls
{
	#region AutoSuggest 

	/// <summary>
	/// The sort order of the suggest box.
	/// </summary>
	public enum DataSortOrder
	{
		/// <summary>
		/// Ascending order.
		/// </summary>
		Ascending = 0,
		/// <summary>
		/// Descending order.
		/// </summary>
		Descending,
		/// <summary>
		/// The order is not used.
		/// </summary>
		Disabled
	};

	/// <summary>
	/// The filter position of the suggest box.
	/// </summary>
	public enum FilterPosition 
	{
		/// <summary>
		/// Filter the input string at the start of each item.
		/// </summary>
		Start = 0,
		/// <summary>
		/// Filter the input string anywher of each item.
		/// </summary>
		Into
	} ;

	/// <summary>
	/// Represents a <see cref="AutoSuggest"/> object.
	/// </summary>
	[
	ToolboxData("<{0}:AutoSuggest runat=server></{0}:AutoSuggest>"),
	ToolboxBitmap(typeof(AutoSuggest), "ToolBoxBitmap.AutoSuggest.bmp"),
	Serializable,
	]
	public class AutoSuggest : TextBox
	{
		#region Fields

		private DataTable _dataSource = null;

#if (LICENSE)
		//private string _license = string.Empty;
		private int _useCounter = 0;
#endif

		#endregion

		#region Constants

		private string _OK_STATUS_ = "OK";
			
		private string _PARAMETER_IS_CALLBACK_ = "AAS_AutoSuggestIsCallBack";

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string SCRIPTKEY = "ActiveAjaxAutoSuggest";

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoSuggest"/> class.
		/// </summary>
		public AutoSuggest()
		{
			
		}

		#endregion

		#region Properties

		#region Appearance
		
		/// <summary>
		/// Gets or sets the fore color of the current selected item.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"Black"),
		Description("The fore color of current selected item.")
		]
		public Color SelectedItemForeColor
		{
			get
			{
				object selectedItemForeColor;
				selectedItemForeColor = ViewState["SelectedItemForeColor"];
				if (selectedItemForeColor != null)
					return (Color)selectedItemForeColor;
				else
					return Color.Black;
			}

			set
			{
				ViewState["SelectedItemForeColor"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the background color of the current selected item.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"#C6D8FF"),
		Description("The background color of current selected item.")
		]
		public Color SelectedItemBackColor
		{
			get
			{
				object selectedItemBackColor;
				selectedItemBackColor = ViewState["SelectedItemBackColor"];
				if (selectedItemBackColor != null)
					return (Color)selectedItemBackColor;
				else
					return Color.FromArgb(0xC6, 0xD8, 0xFF);
			}

			set
			{
				ViewState["SelectedItemBackColor"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the background color of the suggest box.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"#E5ECF9"),
		Description("The background color of the suggest box.")
		]
		public Color SuggestBackColor
		{
			get
			{
				object suggestBackColor;
				suggestBackColor = ViewState["SuggestBackColor"];
				if (suggestBackColor != null)
					return (Color)suggestBackColor;
				else
					return Color.FromArgb(0xE5, 0xEC, 0xF9);
			}

			set
			{
				ViewState["SuggestBackColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell padding of the suggest box.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Unit),"0px"),
		Description("The cell padding of the suggest box.")
		]
		public Unit SuggestCellPadding
		{
			get
			{
				object suggestCellPadding;
				suggestCellPadding = ViewState["SuggestCellPadding"];
				if (suggestCellPadding != null)
					return (Unit)suggestCellPadding;
				else
					return Unit.Parse("0px");
			}

			set
			{
				ViewState["SuggestCellPadding"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the cell spacing of the suggest box.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Unit),"2px"),
		Description("The cell spacing of the suggest box.")
		]
		public Unit SuggestCellSpacing
		{
			get
			{
				object suggestCellSpacing;
				suggestCellSpacing = ViewState["SuggestCellSpacing"];
				if (suggestCellSpacing != null)
					return (Unit)suggestCellSpacing;
				else
					return Unit.Parse("2px");
			}

			set
			{
				ViewState["SuggestCellSpacing"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the border style of the suggest box.		
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(BorderStyle),"Solid"),
		Description("The border style of the suggest box.")
		]
		public BorderStyle SuggestBorderStyle
		{
			get
			{
				object suggestBorderStyle;
				suggestBorderStyle = ViewState["SuggestBorderStyle"];
				if (suggestBorderStyle != null)
					return (BorderStyle)suggestBorderStyle;
				else
					return BorderStyle.Solid;
			}

			set
			{
				ViewState["SuggestBorderStyle"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width of the suggest box.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Unit),"1px"),
		Description("The border width of the suggest box.")
		]
		public Unit SuggestBorderWidth
		{
			get
			{
				object suggestBorderWidth;
				suggestBorderWidth = ViewState["SuggestBorderWidth"];
				if (suggestBorderWidth != null)
					return (Unit)suggestBorderWidth;
				else
					return Unit.Parse("1px");
			}
			
			set
			{
				ViewState["SuggestBorderWidth"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the border color of the suggest box.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"#E0E0E0"),
		Description("The border color of suggest box.")
		]
		public Color SuggestBorderColor
		{
			get
			{
				object suggestBorderColor;
				suggestBorderColor = ViewState["SuggestBorderColor"];
				if (suggestBorderColor != null)
					return (Color)suggestBorderColor;
				else
					return Color.FromArgb(0xE0, 0xE0, 0xE0);
			}

			set
			{
				ViewState["SuggestBorderColor"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(typeof(BorderStyle),"Solid"),
		Category("Appearance"),
		Description("Boder style.")
		]
		public override BorderStyle BorderStyle
		{
			get { return base.BorderStyle;}
			set { base.BorderStyle = value;}
		}



		#endregion

		#region Behavior

		/// <summary>
		/// Gets or sets a value indicating if the case must to be ignored.
		/// </summary>
		/// <value><c>true</c> if the case must be ignored; otherwise, <c>false</c>.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicating if the case must to be ignored."),
		DefaultValue(true)
		]
		public bool IgnoreCase
		{
			get
			{
				object ignoreCase = ViewState["IgnoreCase"];

				if (ignoreCase != null)
					return (bool)ignoreCase;

				return true;
			}
			set
			{
				ViewState["IgnoreCase"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to post only the autosuggest elements or all the elements from the whole page.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The value indicates if you want to post only the autosuggest elements or all the elements from the whole page."),
		DefaultValue(true)
		]
		public bool PostAutoSuggestOnly  
		{
			get
			{
				object postAutoSuggestOnly = ViewState["PostAutoSuggestOnly"]; 

				if (postAutoSuggestOnly != null)
					return (bool)postAutoSuggestOnly;

				return true;
			}

			set
			{
				ViewState["PostAutoSuggestOnly"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates the id of the others control you want to post the state.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("If you use the PostAutoSuggestOnly property you can with this property add the id of the others control you want to post the state."),
		DefaultValue(""),
		TypeConverter(typeof(StringArrayConverter))
		]
		public string[] PostId  
		{
			get
			{
				object postId = ViewState["PostId"]; 

				if (postId != null)
					return (string[])postId;

				return new string[] {};
			}

			set
			{
				ViewState["PostId"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the pattern must be in bold.
		/// </summary>
		/// <value><c>true</c> if the pattern must be in bold; otherwise, <c>false</c>.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicating whether the pattern must be in bold"),
		DefaultValue(true)
		]
		public bool BoldSearchPattern
		{
			get
			{
				object boldSearchPattern = ViewState["BoldSearchPattern"];

				if (boldSearchPattern != null)
					return (bool)boldSearchPattern;

				return true;
			}
			set
			{
				ViewState["BoldSearchPattern"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum number of items displayed in the suggest box.
		/// </summary>
		/// <value>The maximum number of items displayed in the suggest box.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The maximum number of items displayed in the suggest box."),
		DefaultValue(10)
		]
		public int MaxDisplayed
		{
			get
			{
				object maxDisplayed = ViewState["MaxDisplayed"];

				if (maxDisplayed != null)
					return (int)maxDisplayed;

				return 10;
			}
			set
			{
				ViewState["MaxDisplayed"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether postback must be occurs after the selection of an item.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if postback must be occurs after the selection of an item; otherwise, <c>false</c>.
		/// </value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicating whether postback must be occurs after the selection of an item."),
		DefaultValue(false)
		]
		public bool DoPostBackAfterSelection
		{
			get
			{
				object doPostBackAfterSelection = ViewState["DoPostBackAfterSelection"];

				if (doPostBackAfterSelection != null)
					return (bool)doPostBackAfterSelection;

				return false;
			}
			set
			{
				ViewState["DoPostBackAfterSelection"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether you have to use it in SSL.
		/// </summary>
		/// <value><c>true</c> if you have to use it in SSL; otherwise, <c>false</c>.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicating whether you have to use it in SSL."),
		DefaultValue(false)
		]
		public bool UseSSL 
		{
			get
			{
				object useSLL = ViewState["UseSLL"];

				if (useSLL != null)
					return (bool)useSLL;

				return false;
			}
			set
			{
				ViewState["UseSLL"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the filter position.
		/// </summary>
		/// <value>The filter position.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description(""),
		DefaultValue(FilterPosition.Into)
		]
		public FilterPosition FilterPosition 
		{
			get
			{
				object filterPosition = ViewState["FilterPosition"];

				if (filterPosition != null)
					return (FilterPosition)filterPosition;

				return FilterPosition.Into;
			}
			set
			{
				ViewState["FilterPosition"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the items don't have to be wrapped.
		/// </summary>
		/// <value><c>true</c> if the items don't have to be wrapped.; otherwise, <c>false</c>.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicating whether the items don't have to be wrapped."),
		DefaultValue(false)
		]
		public bool NoWrap
		{
			get
			{
				object noWrap = ViewState["NoWrap"];

				if (noWrap != null)
					return (bool)noWrap;

				return false;
			}
			set
			{
				ViewState["NoWrap"] = value;
			}
		}

		#endregion

		#region Script

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		[Bindable(false),
		Category("Script"),
		Description("Gets or sets the relative or absolute path to the directory where the API javascript file is."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.SCRIPT_DIRECTORY)
#endif
		]
		public string ScriptDirectory
		{
			get
			{
				object scriptDirectory = ViewState["ScriptDirectory"];

				if (scriptDirectory != null)
					return (string)scriptDirectory;
#if (!FX1_1)
                return string.Empty;
#else
				return Define.SCRIPT_DIRECTORY;
#endif
			}
			set
			{
				ViewState["ScriptDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the filename of the external script file.
		/// </summary>
		/// 
		[
		Bindable(false),
		Category("Script"),
		Description("The filename of the external script file."),
		DefaultValue("")
		]
		public string ExternalScript
		{
			get
			{
				object externalScript = ViewState["ExternalScript"];

				if (externalScript != null)
					return (string)externalScript;

				return string.Empty;
			}
			set
			{
				ViewState["ExternalScript"] = value;
			}
		}

		#endregion

		#region Data
/*
#if (LICENSE)

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
				return _license;
			}
		
			set
			{
				_license = value;
			}
		}

#endif
*/
		/// <summary>
		/// Gets or sets the suggests values.
		/// </summary>
		/// <value>The suggests.</value>
		[Bindable(false),
		Category("Data"),
		Description("The suggests values."),
		DefaultValue(""),
		TypeConverter(typeof(StringArrayConverter))
		]
		public string[] Suggests
		{
			get
			{
				object suggests = ViewState["Suggests"];

				if (suggests != null)
					return (string[])suggests;

				return new string[] {};
			}
			set
			{
				ViewState["Suggests"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the field for sort order.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue("Ascending"),
		Description("Field of the data source for sort order.")
		] 
		public DataSortOrder DataOrder
		{
			get
			{
				object dataOrder = ViewState["DataOrder"];

				if (dataOrder != null)
					return (DataSortOrder)dataOrder;

				return DataSortOrder.Ascending;
			}
			set
			{
				ViewState["DataOrder"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the data source for the sort order.
		/// </summary>
		/// <value>The data source for the sort order.</value>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source for the sort order.")
		] 
		public string DataSort 
		{
			get
			{
				object dataSort = ViewState["DataSort"];

				if (dataSort != null)
					return (string)dataSort;

				return string.Empty;
			}
			set
			{
				ViewState["DataSort"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the date source for the filter.
		/// </summary>
		/// <value>The data source for the filter.</value>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source for the filter.")
		] 
		public string DataFilter 
		{
			get
			{
				object dataFilter = ViewState["DataFilter"];

				if (dataFilter != null)
					return (string)dataFilter;

				return string.Empty;
			}
			set
			{
				ViewState["DataFilter"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the main data source of the suggest.
		/// </summary>
		/// <value>The data source.</value>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Main data source of the suggest."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)

		] 
		public DataTable DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				_dataSource = value;
			}
		}

		#endregion

		#region internal

		private bool IsCallback
		{
			get
			{
				bool isCallBack;
				try
				{
					isCallBack = bool.Parse(HttpRequest.Params[_PARAMETER_IS_CALLBACK_]);
				}
				catch
				{
					isCallBack = false;
				}
				
				return isCallBack;
			}
		}

		/*private string InputText 
		{
			get
			{
				string inputText = string.Empty;
				try
				{
					inputText = HttpRequest.Params[_PARAMETER_INPUT_TEXT_];
				}
				catch
				{
				}
				
				return inputText;
			}
		}*/

		private HttpResponse HttpResponse 
		{
			get
			{
				return System.Web.HttpContext.Current.Response;
			}
		}

		private HttpRequest HttpRequest 
		{
			get
			{
				return System.Web.HttpContext.Current.Request;				
			}
		}

		#endregion

        #endregion

		#region Methods

		#region Script

		/// <summary>
		/// Register the client-side script block in the ASPX page.
		/// </summary>
		public void RegisterAPIScriptBlock() 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
					string CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveAutoSuggest.js");
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveAutoSuggest.js"));
#else
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveAutoSuggest.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
				
			}

		    Page.TestAndRegisterScriptBlock(SCRIPTKEY, ScriptDirectory, "AAS_TestIfScriptPresent()");
		}


		#endregion

		#region Render

		
		/// <summary>
		/// Raises the pre render event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			this.RegisterAPIScriptBlock();

			base.OnPreRender (e);
		}


		/// <summary>
		/// Adds the attributes to render.
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
		}

		
		/// <summary>
		/// Renders the specified output.
		/// </summary>
		/// <param name="output">The output.</param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AAS, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else 
				RenderAutoSuggest(output);


#else
			RenderAutoSuggest(output);

#endif

		}

		private void RenderAutoSuggest(HtmlTextWriter output) 
		{
			
			if (IsCallback) 
			{
				ProcessCallBack();
			}
			else 
			{
				if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_Var_" + ClientID)) 
				{
					string varString = string.Empty;
					varString += "<script>\n";
					varString += "\n// Variable declaration related to the ajax panel '" + ClientID + "'\n";
					varString += string.Format("var ActiveAutoSuggest_{0} = new AAS_AutoSuggest('{0}');\n",this.ClientID);	

					if (BoldSearchPattern)
						varString += string.Format("ActiveAutoSuggest_{0}.BoldSearchPattern = true;\n",ClientID);

					if (DoPostBackAfterSelection)
						varString += string.Format("ActiveAutoSuggest_{0}.DoPostBackAfterSelection = true;\n",ClientID);

					if (NoWrap)
						varString += string.Format("ActiveAutoSuggest_{0}.NoWrap = true;\n",ClientID);

					if (IgnoreCase) 
						varString += string.Format("ActiveAutoSuggest_{0}.IgnoreCase = true;\n",ClientID);

					if (PostAutoSuggestOnly) 
					{
						varString += string.Format("ActiveAutoSuggest_{0}.PostAutoSuggestOnly = '{0}';\n",ClientID);

						if (PostId.Length > 0)
							varString += string.Format("ActiveAutoSuggest_{0}.PostId = '{1}';\n",ClientID,Utils.FormatStringArray(PostId,','));
					}

					varString += string.Format("ActiveAutoSuggest_{0}.SelectedItemBackColor = '{1}';\n",ClientID,Utils.Color2Hex(SelectedItemBackColor));
					varString += string.Format("ActiveAutoSuggest_{0}.SelectedItemForeColor = '{1}';\n",ClientID,Utils.Color2Hex(SelectedItemForeColor));
					varString += string.Format("ActiveAutoSuggest_{0}.SuggestBackColor = '{1}';\n",ClientID,Utils.Color2Hex(SuggestBackColor));
					varString += string.Format("ActiveAutoSuggest_{0}.SuggestCellPadding = {1};\n",ClientID,SuggestCellPadding.Value);
					varString += string.Format("ActiveAutoSuggest_{0}.SuggestCellSpacing = {1};\n",ClientID,SuggestCellSpacing.Value);

					varString += "\n</script>\n";

					Page.RegisterStartupScript(SCRIPTKEY + "_Var_" + ClientID,varString);
				}
                
    			base.Render (output);

				if (this.Context != null)
				{
					output.Write(string.Format("<div id='{0}_Suggest'style='z-index:0;visibility:hidden;left:300px;width:200px;height:200px;background-color:{1};position:absolute;border-color:{2};border-width:{3};border-style:{4};'></div>",ClientID,Utils.Color2Hex(SuggestBackColor),Utils.Color2Hex(SuggestBorderColor),SuggestBorderWidth,SuggestBorderStyle));
					output.Write(string.Format("<iframe style=\"position: absolute; display: none; top: 0px; left: 0px;\" scrolling=\"no\" id=\"{0}\"></iframe>", ClientID + "_mask"));
				}

			}
		}

		#endregion

		#region CallBack

		private void ProcessCallBack() 
		{
			string data = string.Empty;
			string result = string.Empty;

			this.Text = HttpRequest.Params[HttpRequest.Params["AAS_Target"]];
			if (Text != string.Empty) 
			{
				if (DataSource != null) 
				{
					result = ConvertToResponse(SuggestFromDB());
				}

				else if (Suggests.Length > 0) 
				{
					int currentDisplayed = 0;

					foreach(string s in Suggests) 
					{
						if (MaxDisplayed <= 0 || currentDisplayed < MaxDisplayed)
						{
							int ndx = (IgnoreCase == true) ? s.ToUpper().IndexOf(Text.ToUpper()) : s.IndexOf(Text);
							if (((FilterPosition == FilterPosition.Start && ndx == 0) || (FilterPosition == FilterPosition.Into && ndx >= 0)) && result.IndexOf(s+";") == -1)
							{
								result += s;
								result += ";";
								currentDisplayed++;
							}
						}
					}

					result = result.TrimEnd(';');
				}
				
			}
			

			HttpResponse.Clear();
			HttpResponse.StatusCode = 200;
			HttpResponse.StatusDescription = _OK_STATUS_;
			HttpResponse.Write(result);
			HttpResponse.Flush();
			HttpResponse.End();
		}

		private string[] SuggestFromDB() 
		{
			string data = Text;
			string[] result = null;

			if (DataSource != null) 
			{
				string sortOrder = null;

				if (DataOrder == DataSortOrder.Descending)
					sortOrder = " DESC";
				else
					sortOrder = " ASC";

				DataRow[] drTemp = DataSource.Select(DataFilter + " LIKE '%" + data + "%'", (DataSort != string.Empty ? DataSort : DataFilter) + sortOrder);

				result = new string[drTemp.Length];
				int i = 0;

				foreach(DataRow row in drTemp) 
				{
					result[i] = row[DataFilter].ToString();
					i++;
				}

			}

            return result;			
		}

		private string ConvertToResponse(string[] source) 
		{
			string result = string.Empty;

			int currentDisplayed = 0;

			foreach(string s in source) 
			{
				if (MaxDisplayed <= 0 || currentDisplayed < MaxDisplayed)
				{
					result += s;
					result += ";";
					currentDisplayed++;
				}
			}

			result = result.TrimEnd(';');

			return result;
		}

		/// <summary>
		/// Binds a data source the treeview.
		/// </summary>
		public override void DataBind()
		{
			if (this.DataSource != null)
			{
				this.Suggests = new string [DataSource.Rows.Count];
				for (int i = 0 ; i < DataSource.Rows.Count ; i++) 
				{
					Suggests[i] = (string)DataSource.Rows[i][this.DataFilter];
				}
			}
		}

		#endregion

	
		#endregion

	}

	#endregion
}
