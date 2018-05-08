// ActivePager
// Copyright (c) 2002 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Win32;
using System.Text;
//using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// ActivePager is a .NET web control that encapsulate all the rendering and processing logic of a DataGrid, DataList or Repeater paging system.
	/// </summary>
	/// <example>
	/// <b>DataGrid Simple Integration</b><br></br>
	/// <br></br>
	/// Adding ActivePager to a DataGrid web controls is easy. The DataGrid web control includes it's own paging engine and ActivePager takes advantage of this.<br></br>
	/// <br></br>
	/// <u>First (and last) Step: Adding ActivePager</u><br></br>
	/// <br></br>
	/// All you need to do is add ActivePager in your web form (writing code or with the Visual Studio ToolBox) and select the datagrid you want to page. We assume you have previously added the code to bind the data to your DataGrid in the Page_Load or using the appropriate designer. Please note that the styles and column definitions are note displayed in this page to be more cleaner. You can stylize your DataGrid like you wish.<br></br>
	/// <br></br>
	/// <code>
	/// &lt;!--
	/// Here we add the DataGrid that will display the data.
	///
	/// Please note that we don't need to set AllowPaging property to
	/// true or hide the standard pager using it's style visible property.
	/// ActivePager will adjust the needed values himself.
	/// --&gt;
	/// &lt;asp:datagrid id="dtgCustomers" Runat="server" Width="780"&gt;&lt;/asp:datagrid&gt;
	/// &lt;!--
	/// We add now ActivePager. Nothing special except that it's
	/// recommended to set the page size here, but you can also
	/// set it programmaticely.
	/// --&gt;
	/// &lt;AU:PagerBuilder id="dataPager" Runat="server" Width="780" PageSize="5"
	///    ControlToPage="dtgCustomers"&gt;&lt;/AU:PagerBuilder&gt;<br></br>
	/// </code>
	/// <br></br>
	/// <b>DataList or Repeater Integration</b><br></br>
	/// <br></br>
	/// Adding a paging system in a DataList or Repeater can be difficult if you start need to add each time all the methods, web controls and all the logic to make it work. There are a lot of tutorials that explain how to achieve this on the Internet. The problem is that most of them just explain the principle but not exploit all the possibilites. Also, these kind of tutorials don't take in consideration the re-usability. With ActivePager you have a single control you can use in all the page containing paged listings in all your projects.<br></br>
	/// <br></br>
	/// <u>First Step: Adding ActivePager</u><br></br>
	/// <br></br>
	/// We assume your DataList is allready on your web form. Add ActivePager in your web form (writing code or with the Visual Studio ToolBox).<br></br>
	/// <br></br>
	/// <code>
	/// &lt;!--
	///	Here we add the DataList (or Repeater) that will display the data.
	/// --&gt;
	/// &lt;asp:datalist id="dtlCustomers" Runat="server" Width="780"&gt;
	///    &lt;ItemTemplate&gt;
	///       &lt;b&gt;&lt;%# DataBinder.Eval(Container.DataItem, "CustomerID") %&gt;&lt;/b&gt;
	///       - &lt;% DataBinder.Eval(Container.DataItem, "CompanyName") %&gt;
	///    &lt;/ItemTemplate&gt;
	/// &lt;/asp:datalist&gt;
	/// &lt;!--
	/// We add now ActivePager. Nothing special except that it's
	/// recommended to set the page size here, but you can also
	/// set it programmaticely.
	/// --&gt;
	/// &lt;AU:PagerBuilder id="dataPager" Runat="server" Width="780" PageSize="5"&gt;&lt;/AU:PagerBuilder&gt;
	/// </code>
	/// <br></br>
	/// <u>Second Step: Adding The Event Handler</u><br></br>
	/// <br></br>
	/// Unlike DataGrids, DataList, Repeater and other list controls don't implement indexes and paging features. We need to bind our DataList each time we change page.<br></br>
	/// <br></br>
	/// To achieve this we use the IndexChanged delegate of ActivePager. Add the following code to your page. Don't forget to adapt the data binding code to fits your requirements.<br></br>
	/// <br></br>
	/// <code>
	/// protected void Pager_IndexChanged(object sender, System.EventArgs e)
	/// {
	///    // Initialize our record count buffer variable.
	///    int recordCount = 0;
	///
	///    // Get the datasource from your business object
	///    // or a previously defined DataSet.
	///    DataTable dataSource = BusinessObject.GetDataTable(dataPager.CurrentPage, dataPager.PageSize, ref recordCount);
	///
	///    // Tell ActivePager how much records we have in our
	///    // datasource from the buffer that was updated by
	///    // you or your data source provider.
	///    dataPager.RecordCount = recordCount;
	///
	///    // Traditional DataList data binding.
	///    dtlCustomers.DataSource = dataSource;
	///    dtlCustomers.DataBind();
	/// }
	/// </code>
	/// <br></br>
	/// Nothing special to say except you must tell ActivePager the number of total record your data source have. We need to do that since we only bind the data of the selected page.<br></br>
	/// <br></br>
	/// After that we need to assign our event handler to the IndexChanged event of ActivePager. In Design view click on ActivePager the open the Properties windows. Select the Events view then assign the handler we just added ...<br></br>
	/// <br></br>
	/// <u>Third Step: The Final Touch</u><br></br>
	/// <br></br>
	/// We need to populate our DataList (or Repeater) a first time when the web form is first executed. We add the following code :<br></br>
	/// <br></br>
	/// <code>
	/// private void Page_Load(object sender, System.EventArgs e)
	/// {
	///    // Bind our DataList only if it's the first time
	///    // the page is called.
	///    if (!Page.IsPostBack)
	///       Pager_IndexChanged(null, null);
	/// }
	/// </code>
	/// </example>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:PagerBuilder runat=server></{0}:PagerBuilder>")]
	[Designer(typeof(PagedControlDesigner))]
	[ParseChildren(true)]
	[PersistChildren(false)]
	public class PagerBuilder : PagedControl, INamingContainer
	{
		private string _firstText, _lastText;
        bool _firstLastDisabled, _pageGroupDisabled, _previousNextGroupDisabled;

		/// <summary>
		/// The default constructor. Initilize the default settings.
		/// </summary>
		public PagerBuilder()
		{
			_firstLastDisabled = false;
			_pageGroupDisabled = false;
            _previousNextGroupDisabled = false;

			CurrentIndex = 0;
			PageSize = 1;
			RecordCount = 0;

			_firstText = "<b>&lt;&lt;</b>";
			_lastText = "<b>&gt;&gt;</b>";
			InfoTemplate = "Total Records: <b>$RECORDCOUNT$</b> - Page: <b>$CURRENTPAGE$</b> of <b>$PAGECOUNT$</b>";
            NavigationTemplate = "$PREVIOUSPAGE$" + NavSeparator + "$FIRSTPAGE$ $PAGEGROUP$ $LASTPAGE$" + NavSeparator + "$NEXTPAGE$";
		}


		// Properties of the control.
		#region Properties
		/// <summary>
		/// Gets or sets the total number of records contained in the paged list.
		/// </summary>
		/// <remarks>This property is essential if you use a DataList, a Repeater or a DataGrid (with rebinding on index changes), this property needs to be set to let the control calculate all the states.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the total number of records contained in the paged list.")] 
		public int RecordCount
		{
			get
			{
				EnsureChildControls();
				if (ViewState["_recordCount"] == null)
					ViewState["_recordCount"] = 0;
				return (int)ViewState["_recordCount"];
			}

			set
			{
				ViewState["_recordCount"] = value;
				ChildControlsCreated = false;
			}
		}
		
		/// <summary>
		/// Gets or sets the number of items to display per page.
		/// </summary>
		/// <remarks>Only value greater than 0 are allowed. Setting this property to 0 or -1 will apply the default value 1.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the number of items to display per page.")] 
		public int PageSize
		{
			get
			{
				EnsureChildControls();
				return (int)ViewState["_pageSize"];
			}

			set
			{
				if (value < 1)
					ViewState["_pageSize"] = 1;
				else
					ViewState["_pageSize"] = value;
				
				ChildControlsCreated = false;
			}
		}

        [Bindable(true),
        Category("Appearance"),
        Description("Gets or sets the different selectable page sizes.")]
        public string PageSizes
        {
            get
            {
                EnsureChildControls();
                if (ViewState["_pageSizes"] != null)
                    return ViewState["_pageSizes"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                ViewState["_pageSizes"] = value;

                ChildControlsCreated = false;
            }
        }

		/// <summary>
		/// Gets or sets the current index.
		/// </summary>
		[Bindable(true), 
			Category("Appearance"), 
			Description("Gets or sets the current index.")] 
		public int CurrentIndex
		{
			get
			{
				EnsureChildControls();
				return (int)ViewState["_currentIndex"];
			}

			set
			{
				ViewState["_currentIndex"] = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Specify whether the first and last page links are displayed or not.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Specify whether the first and last page links are displayed or not."),
		DefaultValue(false)] 
		public bool FirstLastDisabled
		{
			get
			{
				return _firstLastDisabled;
			}
			set
			{
				_firstLastDisabled = value;
			}
		}

        /// <summary>
        /// Specify whether the previous and next page group button links are displayed or not.
        /// </summary>
        [Bindable(true),
        Category("Appearance"),
        Description("Specify whether the previous and next page group button links are displayed or not."),
        DefaultValue(false)]
        public bool PreviousNextGroupDisabled
        {
            get
            {
                return _previousNextGroupDisabled;
            }
            set
            {
                _previousNextGroupDisabled = value;
            }
        }


		/// <summary>
		/// Specify whether the page group navigation is displayed or not.
		/// </summary>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Specify whether the page group navigation is displayed or not."),
		DefaultValue(false)] 
		public bool PageGroupDisabled
		{
			get
			{
				return _pageGroupDisabled;
			}
			set
			{
				_pageGroupDisabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to display for the first page link. 
		/// </summary>
		/// <remarks>The default value of this property is <c>&quot;&lt;b&gt;&amp;lt;&amp;lt;&lt;/b&gt;&quot;</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("")] 
		public string FirstText
		{
			get
			{
				return _firstText;
			}

			set
			{
				_firstText = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the text to display for the last page link.
		/// </summary>
		/// <remarks>The default value of this property is <c>&quot;&lt;b&gt;&amp;gt;&amp;gt;&lt;/b&gt;&quot;</c>.</remarks>
		[Bindable(true), 
		Category("Appearance"), 
		Description("Gets or sets the text to display for the last page link.")] 
		public string LastText
		{
			get
			{
				return _lastText;
			}

			set
			{
				_lastText = value;
				ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Gets or sets the current page index.
		/// </summary>
		public int CurrentPage
		{
			get
			{
				if (CurrentIndex > 0)
					return ((CurrentIndex) / PageSize) + 1;
				else
					return 1;
			}
			set
			{
				CurrentIndex = (value - 1) * PageSize;
			}
		}

		/// <summary>
		/// Gets the current page group.
		/// </summary>
		public int CurrentGroup
		{
			get
			{
				if (CurrentPage % 10 == 0)
					return ((CurrentPage - 9) / 10) + 1;
				else
					return (CurrentPage / 10) + 1;
			}
		}

		/// <summary>
		/// Gets the total number of pages in the paged list.
		/// </summary>
		public int PageCount
		{
			get
			{
				if (RecordCount % PageSize > 0)
					return (RecordCount / PageSize) + 1;
				else
					return RecordCount / PageSize;
			}
		}

		/// <summary>
		/// Gets or sets the control to page.
		/// </summary>
		/// <remarks>Actually, only controls listed in the design-time property windows are supported.</remarks>
		[DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(ActiveUp.WebControls.PagedControlConverter))]
		public string ControlToPage
		{
			get
			{
				object local = ViewState["ControlToPage"];
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
				ViewState["ControlToPage"] = value;
			}
		}

     	#endregion

		// All the .NET API related methods.
		#region DOTNET API
		/// <summary>
		/// Initialize the pager.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(System.EventArgs e)
		{
			if (!Page.IsPostBack && this.ControlToPage != null && this.ControlToPage.Length > 0)
			{
				InitControlToPage();
			}
		}

		/// <summary>
		/// Gets the total number of records in the specified data source.
		/// </summary>
		/// <param name="dataSource">The datasource to count.</param>
		/// <returns>The total number or records.</returns>
		private int GetRecordCount(object dataSource)
		{
			if (dataSource == null)
				return 0;

			switch (dataSource.GetType().ToString())
			{
				case "System.Data.DataTable":
					return ((System.Data.DataTable)dataSource).Rows.Count;
			}

			return 0;
		}

		/// <summary>
		/// Initialize the control to page.
		/// </summary>
		private void InitControlToPage()
		{
			System.Web.UI.Control controlToPage = this.Parent.FindControl(this.ControlToPage);

			if (controlToPage != null)
			{
				switch (controlToPage.ToString())
				{
					case "System.Web.UI.WebControls.DataGrid":
						System.Web.UI.WebControls.DataGrid dataGrid = (System.Web.UI.WebControls.DataGrid)controlToPage;

						dataGrid.AllowPaging = true;
						dataGrid.AllowCustomPaging = false;
						dataGrid.PagerStyle.Visible = false;
						dataGrid.PageSize = this.PageSize;
						dataGrid.CurrentPageIndex = this.CurrentPage - 1;
						dataGrid.DataBind();

						if (this.RecordCount == 0)
							this.RecordCount = GetRecordCount(dataGrid.DataSource);

						break;
				}					
			}
		}

		/// <summary>
		/// Process all we need before rendering.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPreRender(System.EventArgs e)
		{
 			if (this.ControlToPage != null && this.ControlToPage.Length > 0)
			{
				InitControlToPage();
			}

			/*if (this._useQuery)
			{
				ChildControlsCreated = false;
				this.OnIndexChanged(new EventArgs());
			}*/
		}
		#endregion

		// All the event handlers.
		#region Event Handlers
		/// <summary>
		/// Executed when the previous page link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnPreviousPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnPreviousPage_Click");

			CurrentIndex -= PageSize;
			if (CurrentIndex < 0)
				CurrentIndex = 0;

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when the next page link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnNextPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnNext_Click");

			if (CurrentIndex + 1 < RecordCount)
				CurrentIndex = CurrentIndex + PageSize;

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when the first page link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnFirstPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnFirstPage_Click");

			CurrentIndex = 0;

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when the previous page group link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnPreviousGroup_Click(object sender, System.EventArgs e)
		{
			this.CurrentIndex = (PageSize * 10) * (this.CurrentGroup - 2);

			DebugTrace("In OnPreviousGroup_Click");

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when the next page group link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnNextGroup_Click(object sender, System.EventArgs e)
		{
			this.CurrentIndex = (PageSize * 10) * (this.CurrentGroup);

			DebugTrace("In OnNextGroup_Click");

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when the last page link is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnLastPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnLast_Click");

			int Modulo = RecordCount % PageSize;

			if (Modulo > 0)
				CurrentIndex = RecordCount - Modulo;
			else
				CurrentIndex = RecordCount - PageSize;

			this.OnIndexChanged(new EventArgs());
		}

		/// <summary>
		/// Executed when a specific page is clicked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void OnGotoPage_Click(object sender, System.EventArgs e)
		{
			DebugTrace("In OnGoto_Click");

			int requestedPage = 1;
				
			if (sender.GetType().FullName == "System.Web.UI.WebControls.LinkButton")
				requestedPage = Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).Text);
			else if (sender.GetType().FullName == "System.Web.UI.WebControls.DropDownList")
				requestedPage = Convert.ToInt32(((System.Web.UI.WebControls.DropDownList)sender).SelectedItem.Value);
			
			CurrentIndex = (requestedPage * PageSize) - PageSize;

			this.OnIndexChanged(new EventArgs());
		}


        private void OnPageSize_Changed(object sender, System.EventArgs e)
        {
            DebugTrace("In OnPageSize_Changed");

            if (sender.GetType().FullName == "System.Web.UI.WebControls.DropDownList")
                PageSize = Convert.ToInt32(((System.Web.UI.WebControls.DropDownList)sender).SelectedItem.Value);

            this.OnPageSizeChanged(new EventArgs());
        }
		#endregion

		// All the public methods.
		#region Public Methods
		/// <summary>
		/// Gets the records index.
		/// </summary>
		/// <returns>The current record index.</returns>
		/// <remarks>Please do not use this property.</remarks>
		public int GetRecordIndex()
		{
			return CurrentIndex * PageSize;
		}

        /// <summary>
        /// Copies the state.
        /// </summary>
        /// <param name="target">The target.</param>
        public void CopyState(PagerBuilder target)
        {
            target.CurrentIndex = this.CurrentIndex;
            target.RecordCount = this.RecordCount;
            target.CurrentPage = this.CurrentPage;
            target.PageSize = this.PageSize;
        }
		#endregion

		// All the private methods.
		#region Private Methods
		/// <summary>
		/// Builds the information panel content.
		/// </summary>
		/// <param name="cell">The table cell.</param>
		/// <param name="designTime">Specify if we need the design time string.</param>
		internal override void BuildInfo(TableCell cell, bool designTime)
		{
            string info = (this.RecordCount == 0 && !string.IsNullOrEmpty(InfoTemplateNoRecords) ? InfoTemplateNoRecords : InfoTemplate);

			info = info.Replace("$RECORDCOUNT$", (designTime ? "##" : RecordCount.ToString()));
			info = info.Replace("$CURRENTPAGE$", (designTime ? "##" : (this.RecordCount == 0 ? "0" : CurrentPage.ToString()))); 
			info = info.Replace("$PAGECOUNT$", (designTime ? "##" : PageCount.ToString()));
			info = info.Replace("$CURRENTINDEX$", (designTime ? "##" : CurrentIndex.ToString()));

			System.Web.UI.WebControls.Label infoText = new System.Web.UI.WebControls.Label();
			infoText.Text = info;
			cell.Controls.Add(infoText);
		}

        /// <summary>
        /// Builds the navigation.
        /// </summary>
        /// <param name="cell">The cell.</param>
		internal override void BuildNavigation(TableCell cell)
		{
            string navigationTemplate = (this.RecordCount == 0 && !string.IsNullOrEmpty(NavigationTemplateNoRecords) ? NavigationTemplateNoRecords : NavigationTemplate);

            // Prepare the navigation template for parsing
            string strongSeparator = "{nctrl}";

            navigationTemplate = navigationTemplate.Replace("$PAGESIZES$", "{nctrl}$PAGESIZE${nctrl}");
            navigationTemplate = navigationTemplate.Replace("$PREVIOUSPAGE$", "{nctrl}$PREVIOUSPAGE${nctrl}");
            navigationTemplate = navigationTemplate.Replace("$FIRSTPAGE$", "{nctrl}$FIRSTPAGE${nctrl}");
            navigationTemplate = navigationTemplate.Replace("$PAGESELECTOR$", "{nctrl}$PAGESELECTOR${nctrl}");
            navigationTemplate = navigationTemplate.Replace("$PAGEGROUP$", "{nctrl}$PAGEGROUP${nctrl}");
            navigationTemplate = navigationTemplate.Replace("$LASTPAGE$", "{nctrl}$LASTPAGE${nctrl}");
            navigationTemplate = navigationTemplate.Replace("$NEXTPAGE$", "{nctrl}$NEXTPAGE${nctrl}");

            string[] stringSeparators = new string[] {strongSeparator};

            // Parse
            foreach(string templateItem in navigationTemplate.Split(stringSeparators, StringSplitOptions.None))
            {
                switch (templateItem)
                {
                    case "$PAGESIZES$":
                        if (this.PageSizes != string.Empty)
                        {
                            DropDownList pageSizeSelector = new DropDownList();

                            pageSizeSelector.ApplyStyle(this.SelectorStyle);

                            foreach (string pageSize in this.PageSizes.Split(','))
                            {
                                ListItem item = new ListItem(pageSize, pageSize);
                                if (pageSize == this.PageSize.ToString())
                                    item.Selected = true;

                                pageSizeSelector.Items.Add(item);
                            }

                            pageSizeSelector.AutoPostBack = true;
                            pageSizeSelector.SelectedIndexChanged += new EventHandler(OnPageSize_Changed);
                            cell.Controls.Add(pageSizeSelector);
                        }
                        break;
                    case "$PREVIOUSPAGE$":
                        LinkButton previous = new LinkButton();
			            if (this.CurrentIndex == 0)
				            previous.Enabled = false;
			            previous.CausesValidation = this.CausesValidation;
			            previous.Click += new EventHandler(OnPreviousPage_Click);
			            previous.Text = PreviousText;
			            cell.Controls.Add(previous);
                        break;
                    case "$NEXTPAGE$":
                        // Next
                        LinkButton next = new LinkButton();
                        if (CurrentIndex >= RecordCount - PageSize)
                            next.Enabled = false;
                        next.CausesValidation = CausesValidation;
                        next.Click += new EventHandler(OnNextPage_Click);
                        next.Text = NextText;
                        cell.Controls.Add(next);
                        break;
                    case "$FIRSTPAGE$":
                        if (!this.FirstLastDisabled)
                        {
                            // First
                            System.Web.UI.WebControls.LinkButton first = new System.Web.UI.WebControls.LinkButton();
                            if (this.CurrentIndex == 0)
                                first.Enabled = false;
                            first.CausesValidation = CausesValidation;
                            first.Click += new EventHandler(OnFirstPage_Click);
                            first.Text = _firstText;
                            cell.Controls.Add(first);
                        }
                        break;
                    case "$LASTPAGE$":
                        if (!this.FirstLastDisabled)
                        {
                            // Last
                            System.Web.UI.WebControls.LinkButton last = new System.Web.UI.WebControls.LinkButton();
                            if (CurrentIndex >= RecordCount - PageSize)
                                last.Enabled = false;
                            last.CausesValidation = CausesValidation;
                            last.Click += new EventHandler(OnLastPage_Click);
                            last.Text = _lastText;
                            cell.Controls.Add(last);
                        }
                        break;
                    case "$PAGESELECTOR$":
                        if (this.PageSelectorEnabled)
                        {
                            System.Web.UI.WebControls.DropDownList pageSelector = new System.Web.UI.WebControls.DropDownList();

                            pageSelector.ApplyStyle(this.SelectorStyle);

                            for (int index = 1; index <= this.PageCount; index++)
                            {
                                ListItem item = new ListItem(index.ToString(), index.ToString());
                                if (index == this.CurrentPage)
                                    item.Selected = true;

                                pageSelector.Items.Add(item);
                            }

                            pageSelector.AutoPostBack = true;
                            pageSelector.SelectedIndexChanged += new EventHandler(OnGotoPage_Click);
                            cell.Controls.Add(pageSelector);
                        }
                        break;
                    case "$PAGEGROUP$":
                        if (!this.PageGroupDisabled)
                        {
                            // Calculate start & end positions
                            int start, end, index;

                            start = CurrentPage - 2; // autoshift page groups

                            if (start < 1)
                                start = 1; // but only for page selection over the shift factor

                            end = start + 9; // Setup the end

                            if (end > PageCount)
                                end = PageCount; // But only if page number is lower

                            if (PageCount >= 10 && (end - start) != 9)
                                start = end - 9;

                            // Build the output
                            if (!this.PreviousNextGroupDisabled)
                            {
                                System.Web.UI.WebControls.LinkButton prevPage = new System.Web.UI.WebControls.LinkButton();
                                if (CurrentGroup == 1)
                                    prevPage.Enabled = false;
                                prevPage.CausesValidation = CausesValidation;
                                prevPage.Text = "&lt;";
                                prevPage.Click += new EventHandler(OnPreviousGroup_Click);
                                cell.Controls.Add(prevPage);
                            }

                            if (this.RecordCount != 0)
                                cell.Controls.Add(new LiteralControl(PageGroupLeftSeparator));

                            for (index = start; index <= end; index++)
                            {
                                if (index != CurrentPage)
                                {
                                    System.Web.UI.WebControls.LinkButton gotoPage = new System.Web.UI.WebControls.LinkButton();
                                    gotoPage.CausesValidation = CausesValidation;
                                    gotoPage.Click += new EventHandler(OnGotoPage_Click);
                                    gotoPage.Text = index.ToString();
                                    cell.Controls.Add(gotoPage);
                                }
                                else
                                    cell.Controls.Add(new LiteralControl("<b>" + index.ToString() + "</b>"));

                                if (index != end)
                                    cell.Controls.Add(new LiteralControl("&nbsp;"));
                            }

                            if (this.RecordCount != 0)
                                cell.Controls.Add(new LiteralControl(PageGroupRightSeparator));

                            if (!this.PreviousNextGroupDisabled)
                            {
                                System.Web.UI.WebControls.LinkButton nextPage = new System.Web.UI.WebControls.LinkButton();
                                if (this.RecordCount == 0 || (CurrentGroup == (PageCount + 9) / 10))
                                    nextPage.Enabled = false;
                                nextPage.CausesValidation = CausesValidation;
                                nextPage.Text = "&gt;";
                                nextPage.Click += new EventHandler(OnNextGroup_Click);
                                cell.Controls.Add(nextPage);
                            }
                        }
                        break;
                    case "": break;
                    default: 
                        LiteralControl control = new LiteralControl();
                        control.Text = templateItem;
                        cell.Controls.Add(control);
                        break;
                }
            }
		}
		
		/// <summary>
		/// Prints the property status in the trace log.
		/// </summary>
		/// <remarks>For debugging purposes only.</remarks>
		private void PrintStatus()
		{
			DebugTrace("CurrentIndex=" + CurrentIndex.ToString());
			DebugTrace("PageSize=" + PageSize.ToString());
			DebugTrace("RecordCount=" + RecordCount.ToString());
			DebugTrace("CurrentGroup=" + CurrentGroup.ToString());
		}
		#endregion
	}
}
