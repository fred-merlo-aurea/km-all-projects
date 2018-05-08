// ActiveToolbar 1.x
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Extension;

namespace ActiveUp.WebControls
{
	#region enum SelectedText

	/// <summary>
	/// Text value of the component when a item is selected.
	/// </summary>
	public enum SelectedText
	{
		/// <summary>
		/// No change.
		/// </summary>
		None = 0,

		/// <summary>
		/// Take the text of the <see cref="ToolItem"/>.
		/// </summary>
		Text,

		/// <summary>
		/// Take the value of the <see cref="ToolItem"/>
		/// </summary>
		Value
	};

	#endregion

	/// <summary>
	/// Represents a <see cref="ToolDropDownList"/>.
	/// </summary>
	[
	ParseChildren(true, "Items"),
	Serializable,
	ToolboxItem(false),
    Designer(typeof(ToolDropDownListDesigner)),
	]
	public class ToolDropDownList : ToolBase, IPostBackEventHandler, IPostBackDataHandler
	{
		#region Variables

		/// <summary>
		/// SelectedIndexChanged event handler.
		/// </summary>
		public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Occurs when the selected index is changed.
		/// </summary>
		public event SelectedIndexChangedEventHandler SelectedIndexChanged;

		/// <summary>
		/// Data source used for data binding.
		/// </summary>
		private object _dataSource;

		/// <summary>
		/// Old selected index before selecting the new one.
		/// </summary>
		private int _oldSelectedIndex = -1;
		private int level = -1;

		/// <summary>
		/// Client side script block.
		/// </summary>
		private string CLIENTSIDE_API = null; 

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string ACTIVETOOLBARSCRIPTKEY = "ActiveToolbar";
		private const string HundredPercent = "100%";
		private const string Zero = "0";
		private const string DownImage = "down.gif";
		private const string CellPaddingCellSpacingZero = " cellpadding=0 cellspacing=0";
		private const string HundredPx = "100px";
		private const string DoubleQuotes = "\"";
		private const string Hidden = "Hidden";
		private const string HiddenSmall = "hidden";
		private const string Ie = "IE";
		private const string Padding = "padding";
		private const string ZeroZeroZeroFourPx = "0px 0px 0px 4px";
		private const string NewLine = "\n";
		private const string StyleAssign = "style=\"";
		private const string TableOpen = "<table";
		private const string Auto = "auto;";
		private const string DivClose = "</div>";
		private const string TableClose = "</table>";
		private const string SourceBlankHtm = "src='blank.htm'";
		private const string Language = "language";
		private const string Javascript = "javascript";
		private const string HandCursor = "cursor:hand;";
		private const string PointerCursor = "cursor:pointer;";
		private const string DisplayNone = "display:none;";
		private const string TagEnd = ">";

		/// <summary>
		/// Indicates if the control can be use dhtml.
		/// </summary>
		private bool _allowDHTML = true;

		private System.Data.DataTable _dataTable;

		#endregion

		#region Constructors

		
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolDropDownList"/> class.
		/// </summary>
		public ToolDropDownList() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolDropDownList"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolDropDownList(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			ID = id;
			BackColor = Color.White;
			BackColorItems = Color.White;
			BorderColor = Color.FromArgb(0xB4,0xB1,0xA3);
			BorderColorRollOver = Color.FromArgb(0x31,0x6A,0xC5);
			BorderStyle = BorderStyle.Solid;
			BorderWidth = 1;
			ItemBackColor = Color.White;
			ItemBackColorRollOver = Color.FromArgb(0x31,0x6A,0xC5);
			ItemsAreaHeight = Unit.Empty;
			WindowBorderColor = Color.FromArgb(0xB4,0xB1,0xA3);
			WindowBorderStyle = BorderStyle.Solid;
			WindowBorderWidth = 1;
			SelectedIndex = -1;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the text displayed.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("The text to displayed.")
		] 
		public override string Text
		{
			get
			{
				return base.TextDefaultIdIfNull;
			}
			set
			{
				base.TextDefaultIdIfNull = value;
			}
		}

        [
        Browsable(false),
        Bindable(true),
        Category("Behavior"),
        DefaultValue(""),
        Description("The overflow contents.")
        ]
        protected string Overflow
        {
            get
            {
                string overflow;
                overflow = ((string)base.ViewState["Overflow"]);
                if (overflow != null)
                    return overflow;

                return string.Empty;
            }
            set
            {
                ViewState["Overflow"] = value;
            }
        }


		/// <summary>
		/// Gets or sets the type of rendering.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("The type of rendering.")
		] 
		public ToolDropDownListType DropDownListType
		{
			get
			{
				object type;
				type = base.ViewState["_dropDownListType"];
				if (type != null)
					return (ToolDropDownListType)base.ViewState["_dropDownListType"];
				return ToolDropDownListType.DHTML;
			}
			set
			{
				ViewState["_dropDownListType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the current selected index.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(-1),
		Description("Current selected index.")
		] 
		public virtual int SelectedIndex
		{
			get
			{
				int count;
				for (count=0;count<Items.Count;count++)
					if (Items[count].Selected) 
						return count; 
				return -1; 
			}

			set
			{
				if (value < -1 || value >= Items.Count)
					throw new ArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.","value");

				for (int count=0;count<Items.Count;count++)
					Items[count].Selected = false;
					
				if (value != -1)
				{
					Items[value].Selected = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the current selected item.
		/// </summary>
		public ToolItem SelectedItem
		{
			get
			{
				int count;
				count = this.SelectedIndex;
				if (count >= 0)
					return this.Items[count]; 
				return null; 
			}
		} 

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[
		Bindable(true),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValue(typeof(System.Drawing.Color),"#B4B1A3"),
		Category("Appearance"),
		Description("Border color.")
		]
		public override Color BorderColor
		{
			get {return base.BorderColor;}
			set {base.BorderColor = value;}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"White"),
		Description("The background color.")
		]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the window border color.
		/// </summary>
		/// <value>The window border color.</value>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"#B4B1A3"),
		Description("The window border color.")
		]
		public Color WindowBorderColor
		{
			get
			{
				object winBorderColor;
				winBorderColor = ViewState["_windowBorderColor"];
				if (winBorderColor != null)
					return (Color)winBorderColor;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_windowBorderColor"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the border color of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),""),
		Description("The border color of each ToolItem.")
		]
		public Color ItemBorderColor
		{
		    get
		    {
		        return base.ItemBorderColor;
		    }
		    set
		    {
		        base.ItemBorderColor = value;
		    }
        }

		/// <summary>
		/// Gets or sets the border color rollover of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),""),
		Description("The border color rollover of each ToolItem.")
		]
		public Color ItemBorderColorRollOver
		{
		    get
		    {
		        return base.ItemBorderColorRollOver;
		    }
		    set
		    {
		        base.ItemBorderColorRollOver = value;
		    }
        }

		/// <summary>
		/// Gets or sets the alignment of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Align),"Left"),
		Description("The alignment of each ToolItem.")
		]
		public Align ItemAlign
		{
		    get
		    {
		        return base.ItemAlign;
		    }
			set
			{
			    base.ItemAlign = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"White"),
		Description("The background color of each ToolItem.")
		]
		public Color ItemBackColor
		{
			get
			{
				object itemBackColor;
				itemBackColor = ViewState["_itemBackColor"];
				if (itemBackColor != null)
					return (Color)itemBackColor;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_itemBackColor"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the background color of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"#316AC5"),
		Description("The background color of each ToolItem.")
		]
		public Color ItemBackColorRollOver
		{
			get
			{
				object itemBackColorRollOver;
				itemBackColorRollOver = ViewState["_itemBackColorRollOver"];
				if (itemBackColorRollOver != null)
					return (Color)itemBackColorRollOver;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_itemBackColorRollOver"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border style of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(BorderStyle),"None"),
		Description("The border style of each ToolItem.")
		]
		public BorderStyle ItemBorderStyle
		{
			get
			{
				object itemBorderStyle;
				itemBorderStyle = ViewState["_itemBorderStyle"];
				if (itemBorderStyle != null)
					return (BorderStyle)itemBorderStyle;
				else
					return BorderStyle.None;
			}

			set
			{
				ViewState["_itemBorderStyle"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Unit),""),
		Description("The border width of each ToolItem.")
		]
		public Unit ItemBorderWidth
		{
			get
			{
				object itemBorderWidth;
				itemBorderWidth = ViewState["_itemBorderWidth"];
				if (itemBorderWidth != null)
					return (Unit)itemBorderWidth;
				else
					return Unit.Empty;
			}
			
			set
			{
				ViewState["_itemBorderWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to use for the indentation.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(""),
		Description("The text to use for the indentation.")
		]
		public string IndentText
		{
			get
			{
				return base.IndentText;
			}
			set
			{
			    base.IndentText = value;
			}
		}

		/// <summary>
		/// Gets or sets the value the component have to take when an item is selected.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(typeof(SelectedText),"None"),
		Description("Indicates the values the componenet have to take when an item is selected.")
		]
		public virtual SelectedText ChangeToSelectedText
		{
		    get
		    {
		        return base.ChangeToSelectedText;
		    }
			set
			{
			    base.ChangeToSelectedText = value;
			}
		}

		/// <summary>
		/// Gets or sets the specific table in the DataSource to bind to the <see cref="ToolDropDownList"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The specific table in the DataSource to bind to the ToolDropDownList.")
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
		/// Gets or sets the data source for this <see cref="ToolDropDownList"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Description("The data source for this ToolDropDownList.")
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
		/// Gets or sets the data source that provides the text content of <see cref="ToolItem"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),	
		Description("The data source that provides the text content of ToolItem.")
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
		/// Gets or sets the .
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The data source that provides the text content of ToolItem.")
		]
		public virtual string DataTextOnlyField
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(DataTextOnlyField), string.Empty);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(DataTextOnlyField), value);
			}
		}

		/// <summary>
		/// Gets or sets the formatting string used to control how data bound to the <see cref="ToolDropDownList"/> control is displayed.
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
		/// Gets or sets the field of the data source that provides the value of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the value of each ToolItem.")
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
		/// Gets or sets the field of the data source that provides the parent id.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the parent id.")
		]
		public virtual string DataParentField
		{
			get
			{
				object dataParentField = ViewState["_dataParentField"];
				if (dataParentField != null)
					return (string)dataParentField;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_dataParentField"] = value;
			}
		}

		/*/// <summary>
		/// Gets or sets the field of the data source that provides the foreign data member.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the foreign data member.")
		]
		public virtual string ForeignDataMember
		{
			get
			{
				object foreignDataMember = ViewState["_foreignDataMember"];
				if (foreignDataMember != null)
					return (string)foreignDataMember;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_foreignDataMember"] = value;
			}
		}*/

		/// <summary>
		/// Gets or sets the field of the data source that provides the foreign data key.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the foreign data key.")
		]
		public virtual string DataKeyField
		{
			get
			{
				object dataKeyField = ViewState["_dataKeyField"];
				if (dataKeyField != null)
					return (string)dataKeyField;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_dataKeyField"] = value;
			}
		}

		/*/// <summary>
		/// Gets or sets the field of the data source that provides the foreign data text.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the foreign data text.")
		]
		public virtual string ForeignDataText
		{
			get
			{
				object foreignDataText = ViewState["_foreignDataText"];
				if (foreignDataText != null)
					return (string)foreignDataText;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_foreignDataText"] = value;
			}
		}*/

		/// <summary>
		/// Gets or sets the field of the data source that provides the foreign data master value.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the foreign data master value.")
		]
		public virtual string DataMasterValue
		{
			get
			{
				object dataMasterValue = ViewState["_dataMasterValue"];
				if (dataMasterValue != null)
					return (string)dataMasterValue;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_dataMasterValue"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the foreign data master text.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The field of the data source that provides the foreign data master text.")
		]
		public virtual string DataMasterText
		{
			get
			{
				object dataMasterText = ViewState["_dataMasterText"];
				if (dataMasterText != null)
					return (string)dataMasterText;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_dataMasterText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if we need to translate the foreign data.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("The value indicating if we need to translate the foreign data.")
		]
		public virtual bool TranslateForeignData
		{
			get
			{
				object translateForeignData = ViewState["_translateForeignData"];
				if (translateForeignData != null)
					return (bool)translateForeignData;
				else 
					return false;
			}

			set
			{
				ViewState["_translateForeignData"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the items area height.
		/// </summary>
		/// <value>The items area height.</value>
		public Unit ItemsAreaHeight
		{
			get
			{
				object itemsAreaHeight = ViewState["_itemsAreaHeight"];
				if (itemsAreaHeight != null)
					return (Unit)itemsAreaHeight;
				else
					return Unit.Empty;
			}

			set
			{
				ViewState["_itemsAreaHeight"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether close drop down on click item.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if close drop down on click item; otherwise, <c>false</c>.
		/// </value>
		public bool CloseDropDownOnClickItem
		{
			get
			{
				object closeDropDownOnClickItem = ViewState["_closeDropDownOnClickItem"];
				if (closeDropDownOnClickItem != null)
					return (bool)closeDropDownOnClickItem;
				else
					return true;
			}

			set
			{
				ViewState["_closeDropDownOnClickItem"] = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///	 Register the client-side script block in the ASPX page.
		/// </summary>
		public virtual void RegisterAPIScriptBlock() 
		{
			// Register the script block is not already done.
			Page.TestAndRegisterScriptInclude(ACTIVETOOLBARSCRIPTKEY, CLIENTSIDE_API, GetType());
		}

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			_allowDHTML = AllowDHTML();

			if (_allowDHTML)
			{
				if (((base.Page != null) && base.Enabled))
				{
					RegisterAPIScriptBlock(); 
				}
			}
		}

		private void Orderize(DataTable dataTable, string parent)
		{
			DataRow[] drTemp = dataTable.Select(this.DataParentField + " = " + parent, this.DataTextField + " ASC");			
			int index;
			
			level++;
			foreach(DataRow row in drTemp)
			{
				DataRow newRow = _dataTable.NewRow();
				
				string text = row[this.DataTextField].ToString();

				for(index=0;index<level;index++)
					text = this.IndentText + text;

				newRow[this.DataValueField] = row[this.DataValueField];
				newRow[this.DataKeyField] = row[this.DataKeyField];
				newRow[this.DataTextField] = text;

				this._dataTable.Rows.Add(newRow);

				Orderize(dataTable, row[this.DataKeyField].ToString());
			}
			level--;
		}

		private void DataBindRecursive()
		{
			// Data source
			if (_dataSource == null || _dataSource.GetType().FullName != "System.Data.DataTable")
				throw new Exception("You need to specify a valid DataTable datasource for recursivity feature.");
			
			//string originalValue = Value;

			//Page.Trace.Write(((System.Data.DataTable)_dataSource).Rows.Count.ToString());

			this._dataTable = ((System.Data.DataTable)_dataSource).Clone();

			Orderize((System.Data.DataTable)_dataSource, "0");
			
			if (this.DataMasterText.Length > 0)
			{
				DataRow row = this._dataTable.NewRow();
				row[this.DataValueField] = this.DataMasterValue;
				row[this.DataTextField] = this.DataMasterText;
				this._dataTable.Rows.InsertAt(row, 0);
			}
			this.DataSource = _dataTable;
	

			/*if (_dropDownList.Items.Count > 0 && originalValue != null && _dropDownList.Items.FindByValue(originalValue) != null)
			{
				_dropDownList.Items.FindByValue(originalValue).Selected = true;
			}
			else if (_dropDownList.Items.Count > 0 && DefaultSelection != null && DefaultSelection.Length > 0 && _dropDownList.Items.FindByValue(DefaultSelection) != null)
			{	
				if (_dropDownList.Items.FindByValue(DefaultSelection) != null)
					_dropDownList.Items.FindByValue(DefaultSelection).Selected = true;
			}*/
		}

		/// <summary>
		/// Raises the DataBinding event.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);

			if (this.DataParentField != null && this.DataParentField.Length > 0)
			{
				DataBindRecursive();
			}

			string dataTextField = DataTextField;
			string dataValueField = DataValueField;
			string dataTextOnlyField = DataTextOnlyField;
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
					ToolItem toolItem = new ToolItem();
					if (valuePresent)
					{
						if(dataTextField.Length > 0)
							toolItem.Text = DataBinder.GetPropertyValue(current,dataTextField,dataTextFormat);
						if(dataValueField.Length > 0)
							toolItem.Value = DataBinder.GetPropertyValue(current, dataValueField, null);
						if (dataTextOnlyField.Length > 0)
							toolItem.TextOnly = DataBinder.GetPropertyValue(current, dataTextOnlyField, null);
						else if (dataTextField.Length > 0)
							toolItem.TextOnly = DataBinder.GetPropertyValue(current, dataTextField, dataTextFormat);
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

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			// A workaround before finding a solution
			output.AddAttribute(HtmlTextWriterAttribute.Type, Hidden);
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			var itemsAreaHeightToAdjust = ItemsAreaHeight == Unit.Empty;

			output.AddAttribute(HtmlTextWriterAttribute.Type, HiddenSmall);
			output.AddAttribute(HtmlTextWriterAttribute.Value, itemsAreaHeightToAdjust.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_ItemsAreaHeightToAdjust");
			output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_ItemsAreaHeightToAdjust");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			if (_allowDHTML)
			{
				RenderAllowDhtml(output);
			}
			else
			{
				Page?.VerifyRenderingInServerForm(this);

				output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_drop");
				if (AutoPostBack && Page != null)
				{
					var text = Page.GetPostBackClientEvent(this, string.Empty);
					output.AddAttribute(HtmlTextWriterAttribute.Onchange, text);
					output.AddAttribute(Language, Javascript);
				}

				output.RenderBeginTag(HtmlTextWriterTag.Select);
				RenderItems(output);
				output.RenderEndTag();
			}
		}

		private void RenderAllowDhtml(HtmlTextWriter output)
		{
			RenderAllowDhtmlHeader(output);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.Write(NewLine);

			output.AddAttribute(HtmlTextWriterAttribute.Width, Width.IsEmpty ? HundredPercent : Width.Value.ToString());

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
			output.AddAttribute(HtmlTextWriterAttribute.Border, Zero);
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_text");
			output.AddStyleAttribute(Padding, ZeroZeroZeroFourPx);
			output.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);

			if (ForeColor != Color.Empty)
			{
				output.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(ForeColor));
			}

			Utils.AddStyleFontAttribute(output, Font);
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			if(SelectedIndex == -1)
			{
				output.Write(Text.TrimEnd());
			}
			else if(ChangeToSelectedText == SelectedText.Text)
			{
				output.Write(Items[SelectedIndex].Text);
			}
			else if(ChangeToSelectedText == SelectedText.Value)
			{
				output.Write(Items[SelectedIndex].Value);
			}

			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			RenderDropDownImage(output);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.Write(TableClose);

			RenderAllowDhtmlFooter(output);
		}

		private void RenderDropDownImage(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			// drop down image

			output.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			// Added by PMENGAL

			output.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, Zero);
			if(!string.IsNullOrEmpty(DropDownImage))
			{
				output.AddAttribute(HtmlTextWriterAttribute.Src, Parent is Toolbar ? Utils.ConvertToImageDir(((Toolbar) Parent).ImagesDirectory, DropDownImage, DownImage, Page, GetType()) : DropDownImage);
			}
			else
			{
				if(Parent is Toolbar)
				{
					var imagedir = ((Toolbar) Parent).ImagesDirectory;

					if(Fx1ConditionalHelper<bool>.GetFx1ConditionalValue(true, false))
					{
						if (string.IsNullOrEmpty(DropDownImage))
						{
							imagedir = string.Empty;
						}
					}
					else
					{
						DropDownImage = DownImage;
					}

					output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(imagedir, DropDownImage, DownImage, Page, GetType()));
				}
				else
				{
					output.AddAttribute(HtmlTextWriterAttribute.Src, DownImage);
				}
			}

			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
		}

		private void RenderAllowDhtmlFooter(HtmlTextWriter output)
		{
			var style = new StringBuilder(StyleAssign);
			if (WindowBorderColor != Color.Empty)
			{
				style.Append($"border-color:{Utils.Color2Hex(WindowBorderColor)};");
			}

			style.Append($"border-style:{WindowBorderStyle.ToString()};");
			if (WindowBorderWidth != Unit.Empty)
			{
				style.Append($"border-width:{WindowBorderWidth};");
			}

			if (BackColorItems != Color.Empty)
			{
				style.Append($"background-color:{Utils.Color2Hex(BackColorItems)};");
			}

			var cursor = HandCursor;
			if (Page != null && Page.Request.Browser.Browser.ToUpper() != Ie)
			{
				cursor = PointerCursor;
			}

			style.Append(cursor);
			style.Append(DisplayNone);
			style.Append(DoubleQuotes);

			var table = new StringBuilder();
			table.Append(TableOpen);
			table.Append($" id={ClientID}_items");
			table.Append(CellPaddingCellSpacingZero);
			table.Append(" onmouseenter=\"this.style.display='';\"");
			table.Append($" onmouseleave=ATB_closeDropDownList('{ClientID}')");
			table.Append($" {style}");
			table.Append(TagEnd);

			output.Write(table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			var overflow = Overflow;
			if (string.IsNullOrEmpty(overflow))
			{
				overflow = Page?.Request.Browser.Browser.Equals(Ie, StringComparison.OrdinalIgnoreCase) == true ? Auto : string.Empty;
			}

			if (!string.IsNullOrEmpty(overflow))
			{
				overflow = $"overflow:{overflow};";
			}

			output.Write($"<div id=\"{$"{ClientID}_divItems"}\" style=\"{overflow}height:{(ItemsAreaHeight != Unit.Empty ? ItemsAreaHeight.ToString() : HundredPx)};\">");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Cellpadding != Unit.Empty ? Cellpadding.ToString() : Zero);
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Cellspacing != Unit.Empty ? Cellspacing.ToString() : Zero);

			output.AddAttribute(HtmlTextWriterAttribute.Border, Zero);
			output.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_tableItems");
			output.RenderBeginTag(HtmlTextWriterTag.Table); // Open Table
			RenderItems(output);
			output.RenderEndTag(); // Close Table
			output.Write(DivClose);
			output.RenderEndTag();
			output.RenderEndTag();
			output.Write(TableClose);

			var enableSsl = string.Empty;
			if (EnableSsl)
			{
				enableSsl = SourceBlankHtm;
			}

			output.Write($"<iframe id=\"{$"{ClientID}_mask"}\" scrolling=\"no\" frameborder=\"0\" style=\"position: absolute;display: none;\" {enableSsl}></iframe>");
		}

		private void RenderAllowDhtmlHeader(HtmlTextWriter output)
		{
			Page?.VerifyRenderingInServerForm(this);

			if(AutoPostBack)
			{
				output.Write($"<input type=\"hidden\" name=\"{UniqueID}_doPostBackWhenClick\" id=\"{ClientID}_doPostBackWhenClick\" value=\"True\">");
				output.AddAttribute(HtmlTextWriterAttribute.Type, Hidden);
				output.AddAttribute(HtmlTextWriterAttribute.Name, $"{ClientID}_selectedIndexChanged");
				output.AddAttribute(HtmlTextWriterAttribute.Value, Page.GetPostBackEventReference(this, string.Empty));
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();
			}
			else
			{
				output.Write($"<input type=\"hidden\" name=\"{UniqueID}_doPostBackWhenClick\" id=\"{ClientID}_doPostBackWhenClick\" value=\"False\">");
			}

			output.Write($"<input type=\"hidden\" name=\"{UniqueID}_oldSelectedIndex\" id=\"{ClientID}_oldSelectedIndex\" value=\"{SelectedIndex}\">");
			output.Write($"<input type=\"hidden\" name=\"{UniqueID}_selectedIndex\" id=\"{ClientID}_selectedIndex\" value=\"{SelectedIndex}\">");

			output.Write($"<input type=\"hidden\" name=\"{UniqueID}_changeToSelectedText\" id=\"{ClientID}_changeToSelectedText\" value=\"{ChangeToSelectedText}\">");

			var enumerator = Style.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				output.AddStyleAttribute((string) enumerator.Current, Style[(string) enumerator.Current]);
			}

			ControlStyle.AddAttributesToRender(output);

			var backImage = string.Empty;
			if (!string.IsNullOrEmpty(BackImage))
			{
				backImage = Parent is Toolbar ? $"url({Utils.ConvertToImageDir(((Toolbar) Parent).ImagesDirectory, BackImage)})" : $"url({BackImage})";
			}

			var style = new StringBuilder(StyleAssign);
			if (BackColor != Color.Empty)
			{
				style.Append($"background-color:{Utils.Color2Hex(BackColor)};");
			}

			if (BorderColor != Color.Empty)
			{
				style.Append($"border-color:{Utils.Color2Hex(BorderColor)};");
			}

			style.Append($"border-style:{BorderStyle.ToString()};");
			if (BorderWidth != Unit.Empty)
			{
				style.Append($"border-width:{BorderWidth};");
			}

			if (!string.IsNullOrEmpty(backImage))
			{
				style.Append($"background-image:{backImage};");
			}

			style.Append(DoubleQuotes);

			var table = new StringBuilder(TableOpen);
			table.Append($" id={ClientID}_ddl");
			if (Width != Unit.Empty)
			{
				table.Append($" width={Width}");
			}

			if (Height != Unit.Empty)
			{
				table.Append($" height={Height}");
			}

			table.Append(CellPaddingCellSpacingZero);
			table.Append($" {style}");
			table.Append($" onclick=ATB_openDropDownList('{ClientID}');");
			if (AllowRollOver)
			{
				table.Append($" onmouseover=\"this.style.borderColor='{Utils.Color2Hex(BorderColorRollOver)}';\"");
			}

			table.Append($" onmouseout=\"this.style.borderColor='{Utils.Color2Hex(BorderColor)}';\"");
			table.Append(TagEnd);
			output.Write(table.ToString());
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
					if (_allowDHTML)
					{
						string onclick = string.Format("ATB_changeSelectedIndex('{0}','{1}','{2}');",this.ClientID,i,CloseDropDownOnClickItem);
						if (ItemBorderColor != Color.Empty)
							onclick += string.Format("document.getElementById('{0}_item{1}_parent').style.borderColor='{2}';",ClientID,i.ToString(),Utils.Color2Hex(ItemBorderColor));
						if (ItemBackColor != Color.Empty)
							onclick += string.Format("document.getElementById('{0}_item{1}_parent').style.backgroundColor='{2}';",ClientID,i.ToString(),Utils.Color2Hex(ItemBackColor));
						if (this.ClientSideClick != null && this.ClientSideClick != string.Empty)
							onclick += this.ClientSideClick;

						output.AddAttribute(HtmlTextWriterAttribute.Onclick, onclick);
						output.RenderBeginTag(HtmlTextWriterTag.Tr);
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						/*output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
						output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0"); 
						output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor,Utils.Color2Hex(this.ItemBorderColor));
						output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, this.ItemBorderStyle.ToString());
						output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, this.ItemBorderWidth.ToString());*/
				
						string table = string.Empty;
						table += "<table";
						table += string.Format(" id={0}_item{1}_parent",ClientID,i.ToString());
						table += " cellpadding=0 cellspacing=0";

						if (this.Width.IsEmpty == true)
							table += " width=100%";
						else
							table += " width=100%";

						string mouseover = string.Empty;
						if (ItemBorderColorRollOver != Color.Empty)
							mouseover += string.Format("this.style.borderColor='{0}';",Utils.Color2Hex(ItemBorderColorRollOver));
						if (ItemBackColorRollOver != Color.Empty)
							mouseover += string.Format("this.style.backgroundColor='{0}';",Utils.Color2Hex(ItemBackColorRollOver));
						//output.AddAttribute(string.Format("onmouseover=\"{0}\"",mouseover),null);
						table += string.Format(" onmouseover=\"{0}\"",mouseover);

						string mouseout = string.Empty;
						if (ItemBorderColor != Color.Empty)
							mouseout += string.Format("this.style.borderColor='{0}';",Utils.Color2Hex(ItemBorderColor));
						if (ItemBackColor != Color.Empty)
							mouseout += string.Format("this.style.backgroundColor='{0}';",Utils.Color2Hex(ItemBackColor));
						//output.AddAttribute(string.Format("onmouseout=\"{0}\"",mouseout),null);
						table += string.Format(" onmouseout=\"{0}\"",mouseout);

						string style = "style=\"";
                        if (ItemBorderColor != Color.Empty)
							style+= string.Format("border-color:{0};",Utils.Color2Hex(ItemBorderColor));
						style += string.Format("border-style:{0};",ItemBorderStyle.ToString());
                        if (ItemBorderWidth != Unit.Empty)
							style += string.Format("border-width:{0};",ItemBorderWidth.ToString());
						style += "\"";
						table += string.Format(" {0}",style);

						table += ">";
						output.Write(table);
						//output.RenderBeginTag(HtmlTextWriterTag.Table);
						output.RenderBeginTag(HtmlTextWriterTag.Tr);
						/*output.AddAttribute(HtmlTextWriterAttribute.Nowrap,null);
						output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
						output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_item" + i.ToString());
						output.AddAttribute(HtmlTextWriterAttribute.Value, Items[i].Value);
						output.AddStyleAttribute("padding","0px 0px 0px 4px");*/
						string td = string.Empty;
						td += "<td";
						td += string.Format(" id={0}_item{1}",ClientID,i.ToString());
						td += string.Format(" value=\"{0}\"",Items[i].Value);
						td += " nowrap";
						td += " width=100%";
						td += " padding=\"0px 0px 0px 4px\"";
						td += string.Format(" align={0}",ItemAlign.ToString());
						td += ">";
						//output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write(td);
						output.RenderBeginTag(HtmlTextWriterTag.Span);
						if (Items[i].EmbeddedControl != null)
							Items[i].EmbeddedControl.RenderControl(output);
						else
						{
							output.Write(Items[i].Text);
						}
						output.RenderEndTag();
						//output.RenderEndTag();
						output.Write("</td>");
						output.RenderEndTag();
						//output.RenderEndTag();
						output.Write("</table>");
						output.RenderEndTag();
						output.RenderEndTag();
					}
					else
					{
						output.AddAttribute(HtmlTextWriterAttribute.Value, Items[i].Value);
						if (Items[i].Selected)
							output.AddAttribute(HtmlTextWriterAttribute.Selected, "true");
						output.RenderBeginTag(HtmlTextWriterTag.Option);
						output.Write(Items[i].TextOnly);
						output.RenderEndTag();
					}
				}
		}

		/// <summary>
		/// Determine if we need to register the client side script and render the calendar, selectors with validation or selectors only.
		/// </summary>
		/// <returns>0 if scripting not allowed, 1 if not an uplevel browser but scripting allowed, 2 if all is OK.</returns>
		private bool AllowDHTML() 
		{
			if (this.DropDownListType == ToolDropDownListType.NotSet)
			{
				Page page = Page;
			
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
			else
			{
				if (this.DropDownListType == ToolDropDownListType.DHTML)
					return true;
				else
					return false;
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Raise the <see cref="SelectedIndexChanged"/> of the <see cref="ToolDropDownList"/> control. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnSelectedIndexChanged(EventArgs e) 
		{
			// Check if someone use our event.
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this,e);
		}

		#endregion

		#region IPostBackEventHandler

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			//Page.Trace.Write(this.ID, "RaisePostBackEvent...");
			OnSelectedIndexChanged(EventArgs.Empty);
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
			//Page.Trace.Write(this.ID, "LoadPostData...");

			int currentSelectedIndex = -1;

			if (this.AllowDHTML())
			{
				_oldSelectedIndex = Int32.Parse(postCollection[UniqueID + "_oldSelectedIndex"]);

				currentSelectedIndex = Int32.Parse(postCollection[UniqueID + "_selectedIndex"]);

				SelectedIndex = currentSelectedIndex;
			}
			else
			{
				string _selectedValue = postCollection[UniqueID + "_drop"];

				for(int index=0;index<this.Items.Count;index++)
				{
					if (this.Items[index].Value == _selectedValue)
					{
						this.Items[index].Selected = true;
						SelectedIndex = index;
						break;
					}
				}
			}

			if (_oldSelectedIndex != currentSelectedIndex)
				return true;

			return false;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			//Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
			OnSelectedIndexChanged(EventArgs.Empty);
		}

		#endregion

		/// <summary>
		/// Renders the DropDownList at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
            ToolDropDownListDesigner.DesignDropDownList(ref output, this);
        }
	}
}
