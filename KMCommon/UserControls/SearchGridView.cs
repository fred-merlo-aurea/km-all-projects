using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Web.UI.Design.WebControls;

[assembly: TagPrefix("KM.Common.UserControls", "SearchGridView")]

namespace KM.Common.UserControls
{
    #region TemplateColumn
    public class NumberColumn : ITemplate
    {
        public void InstantiateIn(Control container)
        {

        }
    }
    #endregion
    [
    ToolboxData("<{0}:SearchGridView runat=server></{0}:SearchGridView>")
    ]
    [ParseChildren(true, "SearchFilters")]
    public class SearchGridView : GridView
    {
        #region Search event and delegate

        public delegate void SearchGridEventHandler(object sender, string _strSearch);
        public event SearchGridEventHandler SearchGrid;

        #endregion

        #region Add event and delegate

        public delegate void AddEventHandler(object sender);
        public event AddEventHandler AddRow;

        #endregion

        #region Controls and constants
        // Controls to implement the search feature
        Panel _pnlSearchFooter;
        protected ImageButton _btnSearch;
        protected ImageButton _btnAdd;
        TextBox _tbSearch;
        DropDownList _ddlFinder;

        //Constants to hold value in view state
        private const string SHOW_EMPTY_FOOTER = "ShowEmptyFooter";
        private const string SHOW_EMPTY_HEADER = "ShowEmptyHeader";

        private const string SHOW_TOTAL_ROWS = "ShowTotalRows";
        private const string NO_OF_ROWS = "NoOfRows";
        private const string SHOW_ROWNUM = "ShowRowNum";
        private const string SHOW_ADD_BUTTON = "ShowAddButton";

        ListItemCollection _lstFilter;
        GridViewRow PagerRow;
        #endregion

        #region Constructor
        // Constructor
        public SearchGridView()
            : base()
        {
            //Initialise controls
            _pnlSearchFooter = new Panel();
            _tbSearch = new TextBox();
            _tbSearch.ID = "_tbSearch";
            _btnSearch = new ImageButton();
            _btnSearch.ID = "_btnSearch";
            _btnAdd = new ImageButton();
            _btnAdd.ID = "_btnAdd";
            _ddlFinder = new DropDownList();
            _ddlFinder.ID = "_ddlFinder";

            //By default turn on the footer shown property
            ShowFooter = true;
        }
        #endregion

        #region properties
        [Category("Appearance")]
        [DefaultValue(true)]
        [Bindable(BindableSupport.No)]
        public bool ShowEmptyFooter
        {
            get
            {
                if (this.ViewState[SHOW_EMPTY_FOOTER] == null)
                {
                    this.ViewState[SHOW_EMPTY_FOOTER] = true;
                }

                return (bool)this.ViewState[SHOW_EMPTY_FOOTER];
            }
            set
            {
                this.ViewState[SHOW_EMPTY_FOOTER] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowEmptyHeader
        {
            get
            {
                if (this.ViewState[SHOW_EMPTY_HEADER] == null)
                {
                    this.ViewState[SHOW_EMPTY_HEADER] = true;
                }

                return (bool)this.ViewState[SHOW_EMPTY_HEADER];
            }
            set
            {
                this.ViewState[SHOW_EMPTY_HEADER] = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [Bindable(BindableSupport.No)]
        public bool ShowAddButton
        {
            get
            {
                if (this.ViewState[SHOW_ADD_BUTTON] == null)
                {
                    this.ViewState[SHOW_ADD_BUTTON] = false;
                }

                return (bool)this.ViewState[SHOW_ADD_BUTTON];
            }
            set
            {
                this.ViewState[SHOW_ADD_BUTTON] = value;
            }
        }

        [Category("Behavior")]
        [Bindable(BindableSupport.No)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        [Editor(typeof(ListItemsCollectionEditor), typeof(UITypeEditor))]
        public virtual ListItemCollection SearchFilters
        {
            get
            {
                if (_lstFilter == null)
                {
                    _lstFilter = new ListItemCollection();
                    ((IStateManager)_lstFilter).TrackViewState();
                }
                return _lstFilter;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowTotalRows
        {
            get
            {
                if (this.ViewState[SHOW_TOTAL_ROWS] == null)
                {
                    this.ViewState[SHOW_TOTAL_ROWS] = true;
                }

                return (bool)this.ViewState[SHOW_TOTAL_ROWS];
            }
            set
            {
                this.ViewState[SHOW_TOTAL_ROWS] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowRowNumber
        {
            get
            {
                if (this.ViewState[SHOW_ROWNUM] == null)
                {
                    this.ViewState[SHOW_ROWNUM] = true;
                }

                return (bool)this.ViewState[SHOW_ROWNUM];
            }
            set
            {
                this.ViewState[SHOW_ROWNUM] = value;
            }
        }
        #endregion

        #region overridden functions
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int count = base.CreateChildControls(dataSource, dataBinding);

            //  no rows in grid. create header and footer in this case
            if (count == 0 && (ShowEmptyFooter || ShowEmptyHeader))
            {
                //  create the table
                Table table = this.CreateChildTable();

                DataControlField[] fields;
                if (this.AutoGenerateColumns)
                {
                    PagedDataSource source = new PagedDataSource();
                    source.DataSource = dataSource;

                    System.Collections.ICollection autoGeneratedColumns = this.CreateColumns(source, true);
                    fields = new DataControlField[autoGeneratedColumns.Count];
                    autoGeneratedColumns.CopyTo(fields, 0);
                }
                else
                {
                    fields = new DataControlField[this.Columns.Count];
                    this.Columns.CopyTo(fields, 0);
                }

                if (ShowEmptyHeader)
                {
                    //  create a new header row
                    GridViewRow headerRow = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    this.InitializeRow(headerRow, fields);
                    headerRow.ApplyStyle(HeaderStyle);
                    // Fire the OnRowCreated event to handle showing row numbers
                    OnRowCreated(new GridViewRowEventArgs(headerRow));
                    //  add the header row to the table
                    table.Rows.Add(headerRow);
                }

                //  create the empty row
                GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                TableCell cell = new TableCell();
                cell.ColumnSpan = fields.Length;
                cell.Width = Unit.Percentage(100);

                //  respect the precedence order if both EmptyDataTemplate
                //  and EmptyDataText are both supplied ...
                if (this.EmptyDataTemplate != null)
                {
                    this.EmptyDataTemplate.InstantiateIn(cell);
                }
                else if (!string.IsNullOrEmpty(this.EmptyDataText))
                {
                    cell.Controls.Add(new LiteralControl(EmptyDataText));
                }

                emptyRow.Cells.Add(cell);
                table.Rows.Add(emptyRow);

                if (ShowEmptyFooter)
                {
                    //  create footer row
                    GridViewRow footerRow = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                    this.InitializeRow(footerRow, fields);
                    // Fire the OnRowCreated event to handle showing search tool
                    OnRowCreated(new GridViewRowEventArgs(footerRow));
                    //  add the footer to the table
                    table.Rows.Add(footerRow);
                }

                this.Controls.Clear();
                this.Controls.Add(table);
            }

            return count;
        }

        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            if (dataSource != null)
                ViewState[NO_OF_ROWS] = dataSource.DataSourceCount;
            return base.CreateColumns(dataSource, useDataSource);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //If showrownumber option is turned on then add 
            //the template column as the first column.
            if (!DesignMode && ShowRowNumber)
            {
                TemplateField tmpCol = new TemplateField();
                NumberColumn numCol = new NumberColumn();
                tmpCol.ItemTemplate = numCol;
                // Insert this as the first column
                this.Columns.Insert(0, tmpCol);
            }
        }

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            if (!DesignMode) //During Runtime
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //If ShowFooter is set to true
                    if (ShowFooter && e.Row.Cells.Count > 0)
                    {
                        //If TotalRows has to be shown
                        if (ShowTotalRows)
                        {
                            e.Row.Cells[0].Text = ViewState[NO_OF_ROWS] + " Rows.";
                        }
                        if (e.Row.Cells[e.Row.Cells.Count - 1].Controls.Count == 0)
                        {
                            //Create the search control
                            Table table = new Table();
                            table.Style.Add("width", "100%");
                            table.Style.Add("align", "right");
                            TableRow tr = new TableRow();
                            TableCell tc = new TableCell();
                            tc.Style.Add("align", "right");
                            tc.Style.Add("width", "100%");

                            //Populate the dropdownlist with the Ids of the columns to be filtered
                            if (_ddlFinder.Items.Count == 0)
                                SetFilter();

                            _btnSearch.Width = 20;
                            _btnSearch.Height = 20;
                            _btnSearch.ImageAlign = ImageAlign.AbsMiddle;
                            _btnSearch.AlternateText = "Search";
                            //Assign the function that is called when search button is clicked
                            _btnSearch.Click += new ImageClickEventHandler(_btnSearch_Click);

                            if (ShowAddButton)
                            {
                                _btnAdd.Width = 20;
                                _btnAdd.Height = 20;
                                _btnAdd.ImageAlign = ImageAlign.AbsMiddle;
                                _btnAdd.AlternateText = "Add";
                                //Assign the function that is called when Add button is clicked
                                _btnAdd.Click += new ImageClickEventHandler(_btnAdd_Click);
                            }

                            tc.Controls.Add(_ddlFinder);
                            tc.Controls.Add(_tbSearch);
                            tc.Controls.Add(_btnSearch);
                            if (ShowAddButton)
                                tc.Controls.Add(_btnAdd);
                            tr.Cells.Add(tc);
                            table.Rows.Add(tr);

                            _pnlSearchFooter.Controls.Add(table);
                            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(_pnlSearchFooter);

                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    // If ShowHeader is set to true and 
                    // If Row number has to be shown

                    if (ShowRowNumber && ShowHeader)
                    {
                        e.Row.Cells[0].Text = "Sno";
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (ShowRowNumber)
                    {
                        //Set the row number in every row
                        e.Row.Cells[0].Text = (e.Row.RowIndex + (this.PageSize * this.PageIndex) + 1).ToString();
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Pager)
                {
                    PagerRow = e.Row;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //Assign the image url for search button
            if (_btnSearch.ImageUrl == null || _btnSearch.ImageUrl == "")
            {
                _btnSearch.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "KM.WebControls.search.bmp");
            }
            if (_btnAdd.ImageUrl == null || _btnAdd.ImageUrl == "")
            {
                _btnAdd.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "KM.WebControls.new.gif");
            }
        }
        #endregion

        #region Search Functions
        public void SetFilter()
        {
            _ddlFinder.Items.Clear();
            //Copy the items to the dropdownlist
            foreach (ListItem li in SearchFilters)
                _ddlFinder.Items.Add(li);
        }

        protected string ConstructSearchString()
        {
            string _strText = _tbSearch.Text.Trim();

            if (_strText == string.Empty)
                return string.Empty;

            return _ddlFinder.SelectedValue + " like '" + _strText + "%'";
        }

        void _btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string sSearchText = ConstructSearchString();
            OnSearchGrid(sSearchText);
        }

        protected void OnSearchGrid(string _strSearch)
        {
            if (SearchGrid != null)
            {
                SearchGrid(this, _strSearch);
            }
        }
        #endregion

        #region Add event
        void _btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (AddRow != null)
                AddRow(this);
        }

        #endregion
    }
}
