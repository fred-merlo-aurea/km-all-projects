using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Data;
using System.Text;
using System.Globalization;

namespace KMPS.MD.Main
{
    public partial class FilterSchedules : FilterBase
    {
        private const string FilterSegmentation = "FilterSegmentation";
        private const string DownloadPath = "../main/temp/";
        private const string ExportCommandName = "Export";
        private const string DeleteCommandName = "Delete";
        private const string DownloadFileExtension = ".tsv";

        protected override string BrandDefaultEmptyDropDown => "All";

        private string SortField
        {
            get
            {
                return ViewState["SortField"].ToString();
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        private string SortDirection
        {
            get
            {
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        private string Mode
        {
            get
            {
                try
                {
                    return Request.QueryString["Mode"].ToString();
                }
                catch
                {
                    return string.Empty;

                }
            }
        }

        protected override string DefaultSearchLabel { get { return "Filter Name"; } }

        protected override Label SearchLabel { get { return lblSearch; } }

        protected override RadioButtonList ListType { get { return rblListType; } }

        protected override PlaceHolder FiltersPlaceHolder { get { return phFilters; } }

        protected override PlaceHolder FilterSegmentationsPlaceHolder { get { return phFilterSegmentations; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Filters";
            Master.SubMenu = "Scheduled Export";

            lblErrorMessage.Text = "";
            divError.Visible = false;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                gvFilterSchedules.PageSize = 25;
                SortField = "FILTERNAME";
                SortDirection = "ASC";
                SortFieldFS = "FilterSegmentationName";
                SortDirectionFS = "ASC";

                LoadBrands();

                if (Mode == FilterSegmentation)
                {
                    rblListType.SelectedValue = Enums.ListType.FilterSegmentations.ToString();
                    phFilterSegmentations.Visible = true;
                    phFilters.Visible = false;
                    LoadFilterSegmentationGrid();
                }
                else
                {
                    phFilterSegmentations.Visible = false;
                    phFilters.Visible = true;
                    LoadGrid();
                }
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvFilterSchedules.DataSource = null;
            gvFilterSchedules.DataBind();

            hfBrandID.Value = drpBrand.SelectedValue;
            LoadGrid();
        }

        protected void gvFilterSchedules_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lnkExport");
                if (lb != null) ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(lb);
            }

        }

        protected void gvFilterSchedules_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lb = (Label)e.Row.FindControl("lblStartTime");

                DateTime dt = DateTime.Parse(lb.Text, CultureInfo.InvariantCulture);
                lb.Text = dt.ToString("h:mm tt");



                if (!pnlBrand.Visible)
                {
                    var lblBrandName = (Label)e.Row.FindControl("lblBrandName");
                    lblBrandName.Visible = false;
                }
            }
        }

        protected void gvFilterSchedules_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ProcessRowCommand(e, LoadGrid);
        }

        protected void gvFilterSchedules_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvFilterSchedules_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFilterSchedules.PageIndex = e.NewPageIndex;
            LoadGrid();
        }

        protected void gvFilterSchedules_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField)
                SortDirection = (SortDirection.ToUpper() == "ASC" ? "DESC" : "ASC");
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }

        public override void LoadGrid()
        {
            try
            {
                gvFilterSchedules.DataSource = null;
                gvFilterSchedules.DataBind();

                List<FilterSchedule> fs = new List<FilterSchedule>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                {
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                        fs = FilterSchedule.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value), false);
                    else
                        fs = FilterSchedule.GetByBrandIDUserID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value), Master.LoggedInUser, false);
                }
                else
                {
                    // -1 - Data from user assigned all brands
                    if (Convert.ToInt32(hfBrandID.Value) == -1)
                    {
                        fs = FilterSchedule.GetByBrandIDUserID(Master.clientconnections, 0, Master.LoggedInUser, false);
                    }
                    else
                    {
                        //No brand 
                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                            fs = FilterSchedule.GetByBrandID(Master.clientconnections, 0, false);
                        else
                            fs = FilterSchedule.GetByBrandIDUserID(Master.clientconnections, 0, Master.LoggedInUser, false);
                    }
                }

                fs = fs.FindAll(x => x.FilterSegmentationID <= 0 || x.FilterSegmentationID == null).ToList();

                List<FilterSchedule> lst = null;

                if (fs != null && fs.Count > 0)
                {
                    switch (SortField.ToUpper())
                    {
                        case "BRANDNAME":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.BrandName).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.BrandName).ToList();
                            break;
                        case "EXPORTNAME":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.ExportName).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.ExportName).ToList();
                            break;
                        case "EXPORTNOTES":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.ExportNotes).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.ExportNotes).ToList();
                            break;
                        case "FILTERNAME":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.FilterName).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.FilterName).ToList();
                            break;

                        case "RECURRENCETYPE":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.RecurrenceType).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.RecurrenceType).ToList();
                            break;

                        case "STARTDATE":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.StartDate).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.StartDate).ToList();
                            break;
                        case "STARTTIME":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.StartTime.Length).ThenBy(o => o.StartTime).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.StartTime.Length).ThenByDescending(o => o.StartTime).ToList();
                            break;
                        case "ENDDATE":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.EndDate).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.EndDate).ToList();
                            break;
                        case "EXPORTTYPEID":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fs.OrderBy(o => o.ExportTypeID).ToList();
                            else
                                lst = fs.OrderByDescending(o => o.ExportTypeID).ToList();
                            break;
                    }
                }

                if (lst != null)
                {
                    if (txtSearch.Text != string.Empty)
                    {
                        switch (drpSearch.SelectedValue.ToUpper())
                        {
                            case "EQUAL":
                                lst = lst.FindAll(x => x.FilterName.ToLower().Equals(txtSearch.Text.ToLower()));
                                break;
                            case "START WITH":
                                lst = lst.FindAll(x => x.FilterName.ToLower().StartsWith(txtSearch.Text.ToLower()));
                                break;
                            case "END WITH":
                                lst = lst.FindAll(x => x.FilterName.ToLower().EndsWith(txtSearch.Text.ToLower()));
                                break;
                            case "CONTAINS":
                                lst = lst.FindAll(x => x.FilterName.ToLower().Contains(txtSearch.Text.ToLower()));
                                break;
                        }
                    }
                }

                gvFilterSchedules.DataSource = lst;
                gvFilterSchedules.DataBind();

                if (lst != null)
                {
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.Edit))
                    {
                        gvFilterSchedules.Columns[19].Visible = false;
                    }

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.Delete))
                    {
                        gvFilterSchedules.Columns[20].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void gvFilterSegmentationSchedules_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lnkExport");
                if (lb != null) ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(lb);
            }

        }

        protected void gvFilterSegmentationSchedules_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lb = (Label)e.Row.FindControl("lblStartTime");

                DateTime dt = DateTime.Parse(lb.Text, CultureInfo.InvariantCulture);
                lb.Text = dt.ToString("h:mm tt");

                if (!pnlBrand.Visible)
                {
                    var lblBrandName = (Label)e.Row.FindControl("lblBrandName");
                    lblBrandName.Visible = false;
                }
            }
        }

        protected void gvFilterSegmentationSchedules_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ProcessRowCommand(e, LoadFilterSegmentationGrid);
        }       

        protected void gvFilterSegmentationSchedules_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvFilterSegmentationSchedules_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFilterSegmentationSchedules.PageIndex = e.NewPageIndex;
            LoadFilterSegmentationGrid();
        }

        protected void gvFilterSegmentationSchedules_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortFieldFS)
                SortDirectionFS = (SortDirectionFS.ToUpper() == "ASC" ? "DESC" : "ASC");
            else
            {
                SortFieldFS = e.SortExpression;
                SortDirectionFS = "ASC";
            }

            LoadFilterSegmentationGrid();
        }

        public override void LoadFilterSegmentationGrid()
        {
            try
            {
                var filterScheduleList = GetListOfFilterSchedules();
                var orderedFilterScheduleList = SortListOfFilterSchedules(filterScheduleList);
                orderedFilterScheduleList = FilterListOfFiltersSchedules(orderedFilterScheduleList);

                gvFilterSegmentationSchedules.DataSource = orderedFilterScheduleList;
                gvFilterSegmentationSchedules.DataBind();

                if (orderedFilterScheduleList != null)
                {
                    gvFilterSegmentationSchedules.Columns[20].Visible = KM.Platform.User.HasAccess(
                                                Master.UserSession.CurrentUser,
                                                KMPlatform.Enums.Services.UAD,
                                                KMPlatform.Enums.ServiceFeatures.ScheduledExport,
                                                KMPlatform.Enums.Access.Edit);

                    gvFilterSegmentationSchedules.Columns[21].Visible = KM.Platform.User.HasAccess(
                                                Master.UserSession.CurrentUser,
                                                KMPlatform.Enums.Services.UAD,
                                                KMPlatform.Enums.ServiceFeatures.ScheduledExport,
                                                KMPlatform.Enums.Access.Delete);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }    

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                LoadGrid();
            }
            else
            {
                LoadFilterSegmentationGrid();
            }
        }
        
        protected override void ResetControls()
        {
            gvFilterSchedules.DataSource = null;
            gvFilterSchedules.DataBind();
            gvFilterSegmentationSchedules.DataSource = null;
            gvFilterSegmentationSchedules.DataBind();
            drpSearch.ClearSelection();
            txtSearch.Text = string.Empty;
            lblSearch.Text = "Filter Name";
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            //Nothing to do here, had to implement abstract method
        }

        private void ProcessRowCommand(GridViewCommandEventArgs eventArgs, Action loadGridAction)
        {
            if (eventArgs.CommandName.Equals(DeleteCommandName, StringComparison.Ordinal))
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.Delete))
                {
                    try
                    {
                        FilterSchedule.Delete(Master.clientconnections, Convert.ToInt32(eventArgs.CommandArgument), Master.LoggedInUser);
                        loadGridAction();
                    }
                    catch (Exception ex)
                    {
                        DisplayError(ex.Message);
                    }
                }
            }
            else if (eventArgs.CommandName.Equals(ExportCommandName, StringComparison.Ordinal))
            {
                try
                {
                    var exportDataTuple = FilterSchedule.Export(Master.clientconnections, Convert.ToInt32(eventArgs.CommandArgument));
                    var exportData = exportDataTuple.Item1;
                    var outfileName = $"{Server.MapPath(DownloadPath)}{Guid.NewGuid().ToString().Substring(0, 5)}{DownloadFileExtension}";

                    Utilities.Download(exportData, outfileName, exportDataTuple.Item2, 0, 0);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Error: {0}", ex);
                    DisplayError(ex.Message);
                }
            }
        }

        public static List<T> Order<T, TKey>(IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            if (ascending)
            {
                return source.OrderBy(selector).ToList();
            }
            else
            {
                return source.OrderByDescending(selector).ToList();
            }
        }

        private List<FilterSchedule> FilterListOfFiltersSchedules(List<FilterSchedule> orderedFilterScheduleList)
        {
            if (orderedFilterScheduleList != null && txtSearch.Text != string.Empty)
            {
                switch (drpSearch.SelectedValue.ToUpper())
                {
                    case "EQUAL":
                        orderedFilterScheduleList = orderedFilterScheduleList.FindAll(x => x.FilterName.Equals(txtSearch.Text, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "START WITH":
                        orderedFilterScheduleList = orderedFilterScheduleList.FindAll(x => x.FilterName.StartsWith(txtSearch.Text, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "END WITH":
                        orderedFilterScheduleList = orderedFilterScheduleList.FindAll(x => x.FilterName.EndsWith(txtSearch.Text, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "CONTAINS":
                        orderedFilterScheduleList = orderedFilterScheduleList.FindAll(x => x.FilterName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                        break;
                }
            }

            return orderedFilterScheduleList;
        }

        private List<FilterSchedule> SortListOfFilterSchedules(List<FilterSchedule> filterScheduleList)
        {
            List<FilterSchedule> orderedFilterScheduleList = null;

            if (filterScheduleList != null && filterScheduleList.Any())
            {
                var sortAscending = SortDirectionFS.Equals("ASC", StringComparison.OrdinalIgnoreCase);

                switch (SortFieldFS.ToUpper())
                {
                    case "BRANDNAME":
                        return Order(filterScheduleList, o => o.BrandName, sortAscending);

                    case "EXPORTNAME":
                        return Order(filterScheduleList, o => o.ExportName, sortAscending);

                    case "EXPORTNOTES":
                        return Order(filterScheduleList, o => o.ExportNotes, sortAscending);

                    case "FILTERNAME":
                        return Order(filterScheduleList, o => o.FilterName, sortAscending);

                    case "FILTERSEGMENTATIONNAME":
                        return Order(filterScheduleList, o => o.FilterSegmentationName, sortAscending);

                    case "RECURRENCETYPE":
                        return Order(filterScheduleList, o => o.RecurrenceType, sortAscending);

                    case "STARTDATE":
                        return Order(filterScheduleList, o => o.StartDate, sortAscending);

                    case "ENDDATE":
                        return Order(filterScheduleList, o => o.EndDate, sortAscending);

                    case "EXPORTTYPEID":
                        return Order(filterScheduleList, o => o.ExportTypeID, sortAscending);

                    case "STARTTIME":
                        if (sortAscending)
                        {
                            return filterScheduleList.OrderBy(o => o.StartTime.Length).ThenBy(o => o.StartTime).ToList();
                        }
                        else
                        {
                            return filterScheduleList.OrderByDescending(o => o.StartTime.Length).ThenByDescending(o => o.StartTime).ToList();
                        }
                }
            }

            return orderedFilterScheduleList;
        }

        private List<FilterSchedule> GetListOfFilterSchedules()
        {
            var filterScheduleList = new List<FilterSchedule>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    filterScheduleList = FilterSchedule.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value), true);
                }
                else
                {
                    filterScheduleList = FilterSchedule.GetByBrandIDUserID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value), Master.LoggedInUser, true);
                }
            }
            else
            {
                // -1 - Data from user assigned all brands
                if (Convert.ToInt32(hfBrandID.Value) == -1)
                {
                    filterScheduleList = FilterSchedule.GetByBrandIDUserID(Master.clientconnections, 0, Master.LoggedInUser, true);
                }
                else
                {
                    //No brand 
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                    {
                        filterScheduleList = FilterSchedule.GetByBrandID(Master.clientconnections, 0, true);
                    }
                    else
                    {
                        filterScheduleList = FilterSchedule.GetByBrandIDUserID(Master.clientconnections, 0, Master.LoggedInUser, true);
                    }                        
                }
            }

            return filterScheduleList;
        }
    }
}