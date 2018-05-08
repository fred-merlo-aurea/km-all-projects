using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using Telerik.Web.UI;
using System.Data;
using DiagnosticsTrace = System.Diagnostics.Trace;
using KM.Common;
using Enums = KMPS.MD.Objects.Enums;
using KMP = KM.Platform;
using KMBusiness = KMPlatform.BusinessLogic;
using KMPEnums = KMPlatform.Enums;

namespace KMPS.MD.Main
{
    public partial class FilterList : FilterBase
    {
        private const string SearchEqual = "EQUAL";
        private const string SearchStartsWith = "START WITH";
        private const string SearchEndsWith = "END WITH";
        private const string SearchContains = "CONTAINS";
        private const string SortAsceding = "ASC";
        private const string FieldBrandName = "BRANDNAME";
        private const string FieldName = "NAME";
        private const string FieldFilterType = "FILTERTYPE";
        private const string FieldPubName = "PUBNAME";
        private const string FieldFilterCategoryName = "FILTERCATEGORYNAME";
        private const string FieldQuestionCategoryName = "QUESTIONCATEGORYNAME";
        private const string FieldCreatedDate = "CREATEDDATE";
        private const string FieldCreatedName = "CREATEDNAME";
        private const string FieldNotes = "NOTES";
        private const int BrandAll = -1;

        delegate void HidePanel();

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

        protected override string DefaultSearchLabel { get { return "Filter Name or Question Name"; } }

        protected override Label SearchLabel { get { return lblSearch; } }

        protected override RadioButtonList ListType { get { return rblListType; } }

        protected override PlaceHolder FiltersPlaceHolder { get { return phFilters; } }

        protected override PlaceHolder FilterSegmentationsPlaceHolder { get { return phFilterSegmentations; } }

        private void FilterSavePopupHide()
        {
            FilterSave.Visible = false;
            LoadGrid();
        }

        private void FilterSegmentationSavePopupHide()
        {
            FilterSegmentationSave.Visible = false;
            LoadFilterSegmentationGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Filters";
            Master.SubMenu = "View Filters/Filter Segmentations";

            HidePanel delFilterSave = new HidePanel(FilterSavePopupHide);
            this.FilterSave.hideFilterSavePopup = delFilterSave;

            HidePanel deFSNoParam = new HidePanel(FilterSegmentationSavePopupHide);
            this.FilterSegmentationSave.hideFilterSegmentationPopup = deFSNoParam;

            lblErrorMessage.Text = "";
            divError.Visible = false; 

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                gvFilters.PageSize = 25;
                SortField = "Name";
                SortDirection = "ASC";
                SortFieldFS = "FilterSegmentationName";
                SortDirectionFS = "ASC";

                FilterSave.Mode = "Edit";
                rtvFilterCategory.DataSource = KMPS.MD.Objects.FilterCategory.GetAll(Master.clientconnections);
                rtvFilterCategory.DataBind();

                RadTreeNode root = new RadTreeNode("No Category", "0");
                rtvFilterCategory.Nodes.Insert(0, root);
                rtvFilterCategory.ExpandAllNodes();
                rtvFilterCategory.Nodes[0].Expanded = true;
                rtvFilterCategory.Nodes[0].Selected = true;

                LoadBrands();
                LoadGrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfBrandID.Value = drpBrand.SelectedValue;

            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                LoadGrid();
            }
            else
            {
                LoadFilterSegmentationGrid();
            }
        }

        protected void rtvFilterCategory_OnNodeClick(object sender, RadTreeNodeEventArgs e)
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

        protected void gvFilters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Delete))
                {
                    try
                    {
                        if (FilterSchedule.ExistsByFilterID(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString())))
                            DisplayError("Filter cannot be deleted. There is an active or inactive scheduled export associated with the filter.  Delete the export and then delete the filter.");
                        else
                        {
                            MDFilter.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Master.LoggedInUser);
                            LoadGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayError(ex.Message);
                    }
                }
            }
            else if (e.CommandName == "FilterEdit")
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
                {
                    try
                    {
                        FilterSave.Mode = "Edit";
                        FilterSave.BrandID = 0;
                        FilterSave.PubID = 0;
                        FilterSave.FilterIDs = e.CommandArgument.ToString();
                        FilterSave.UserID = Master.LoggedInUser;
                        FilterSave.FilterCollections = null;
                        FilterSave.ViewType = Enums.ViewType.None;
                        FilterSave.LoadControls();
                        FilterSave.LoadFilterData();
                        FilterSave.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        DisplayError(ex.Message);
                    }
                }
            }
        }

        protected void gvFilters_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvFilters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFilters.PageIndex = e.NewPageIndex;
            LoadGrid();
        }

        protected void gvFilters_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void gvFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!pnlBrand.Visible)
                {
                    var lblBrandName = (Label)e.Row.FindControl("lblBrandName");
                    lblBrandName.Visible = false;
                }
            }
        }

        protected void grdFilters_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        public override void LoadGrid()
        {
            try
            {
                gvFilters.DataSource = null;
                gvFilters.DataBind();

                List<MDFilter> mdFilters;

                if (ToInt32WithThrow(hfBrandID.Value) > 0)
                {
                    mdFilters = FillFilterOnBrandPresent();
                }
                else
                {
                    mdFilters = FIllFiltersOnNoParticularBrand();
                }

                if (mdFilters?.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(rtvFilterCategory.SelectedValue))
                    {
                        mdFilters = (from filter in mdFilters
                               where filter.FilterCategoryID == ToInt32WithThrow(rtvFilterCategory.SelectedValue)
                               select filter).ToList();
                    }

                    var searchText = txtSearch.Text;
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        mdFilters = FillFilterSearch(mdFilters, searchText);
                    }
                }

                FillFilters(mdFilters);
            }
            catch (Exception ex)
            {
                DiagnosticsTrace.TraceError(ex.ToString());
                DisplayError(ex.Message);
            }
        }

        private List<MDFilter> FillFilterSearch(List<MDFilter> mdFilters, string searchText)
        {
            var searchSelectedValue = drpSearch.SelectedValue;
            if (SearchEqual.Equals(searchSelectedValue, StringComparison.OrdinalIgnoreCase))
            {
                mdFilters = mdFilters.FindAll(
                    x =>
                        x.Name.Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (x.QuestionName ?? string.Empty).Equals(searchText, StringComparison.OrdinalIgnoreCase));
            }
            else if (SearchStartsWith.Equals(searchSelectedValue, StringComparison.OrdinalIgnoreCase))
            {
                mdFilters = mdFilters.FindAll(
                    x => 
                        x.Name.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (x.QuestionName ?? string.Empty).StartsWith(searchText, StringComparison.OrdinalIgnoreCase));
            }
            else if (SearchEndsWith.Equals(searchSelectedValue, StringComparison.OrdinalIgnoreCase))
            {
                mdFilters = mdFilters.FindAll(
                    x =>
                        x.Name.EndsWith(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (x.QuestionName ?? string.Empty).EndsWith(searchText, StringComparison.OrdinalIgnoreCase));
            }
            else if (SearchContains.Equals(searchSelectedValue, StringComparison.OrdinalIgnoreCase))
            {
                mdFilters = mdFilters.FindAll(
                    x =>
                        x.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (x.QuestionName ?? string.Empty).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return mdFilters;
        }

        private List<MDFilter> FIllFiltersOnNoParticularBrand()
        {
            List<MDFilter> mdFilters;
            if (ToInt32WithThrow(hfBrandID.Value) == BrandAll)
            {
                mdFilters = MDFilter.GetInBrandByUserID(Master.clientconnections, Master.LoggedInUser);
            }
            else
            {
                if (KMP.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    mdFilters = MDFilter.GetNotInBrand(Master.clientconnections);
                }
                else
                {
                    mdFilters =
                        MDFilter.GetNotInBrandByUserID(Master.clientconnections, Master.LoggedInUser);
                }
            }

            return mdFilters;
        }

        private List<MDFilter> FillFilterOnBrandPresent()
        {
            List<MDFilter> mdFilters;
            if (KMP.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                mdFilters =
                    MDFilter.GetByBrandID(Master.clientconnections, ToInt32WithThrow(hfBrandID.Value));
            }
            else
            {
                mdFilters = MDFilter.GetByBrandIDUserID(
                    Master.clientconnections,
                    ToInt32WithThrow(hfBrandID.Value),
                    Master.LoggedInUser);
            }

            return mdFilters;
        }

        private void FillFilters(IEnumerable<MDFilter> mdFilters)
        {
            Guard.NotNull(mdFilters, nameof(mdFilters));

            var users = new KMBusiness.User().Select();

            var query = (from mdFilter in mdFilters
                join user in users on mdFilter.CreatedUserID equals user.UserID into filterUsers
                from filterUser in filterUsers.DefaultIfEmpty()
                select new FilterUser
                {
                    FilterId = mdFilter.FilterId,
                    BrandName = mdFilter.BrandName,
                    Name = mdFilter.Name,
                    PubName = mdFilter.PubName,
                    FilterCategoryName = mdFilter.FilterCategoryName,
                    QuestionCategoryName = mdFilter.QuestionCategoryName,
                    QuestionName = mdFilter.QuestionName,
                    FilterType = mdFilter.FilterType,
                    CreatedDate = mdFilter.CreatedDate,
                    CreatedName = filterUser == null ? string.Empty : filterUser.UserName,
                    Notes = mdFilter.Notes
                }).ToList();

            query = SetupSort(query);

            gvFilters.DataSource = query;
            gvFilters.DataBind();

            if (query != null)
            {
                SetGridColumnsVisibility();
            }
        }

        private List<FilterUser> SetupSort(List<FilterUser> query)
        {
            if (string.Equals(SortField, FieldBrandName, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.BrandName).ToList();
            }
            else if (string.Equals(SortField, FieldName, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.Name).ToList();
            }
            else if (string.Equals(SortField, FieldFilterType, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.FilterType).ToList();
            }
            else if (string.Equals(SortField, FieldPubName, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.PubName).ToList();
            }
            else if (string.Equals(SortField, FieldFilterCategoryName, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.FilterCategoryName).ToList();
            }
            else if (string.Equals(SortField, FieldQuestionCategoryName, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.QuestionCategoryName).ToList();
            }
            else if (string.Equals(SortField, FieldCreatedDate, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.CreatedDate).ToList();
            }
            else if (string.Equals(SortField, FieldCreatedName, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.CreatedName).ToList();
            }
            else if (string.Equals(SortField, FieldNotes, StringComparison.OrdinalIgnoreCase))
            {
                query = SortQuery(query, SortDirection, o => o.Notes).ToList();
            }

            return query;
        }

        private static IEnumerable<FilterUser> SortQuery<T>(
            IEnumerable<FilterUser> query, 
            string sortDirection, 
            Func<FilterUser, T> selector)
        {
            if (SortAsceding.Equals(sortDirection, StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(selector);
            }
            else
            {
                query = query.OrderByDescending(selector);
            }
            return query;
        }

        private void SetGridColumnsVisibility()
        {
            if (!KMP.User.HasAccess(
                Master.UserSession.CurrentUser,
                KMPEnums.Services.UAD,
                KMPEnums.ServiceFeatures.ScheduledExport,
                KMPEnums.Access.Edit))
            {
                gvFilters.Columns[9].Visible = false;
            }

            if (!KMP.User.HasAccess(
                Master.UserSession.CurrentUser,
                KMPEnums.Services.UAD,
                KMPEnums.ServiceFeatures.UADFilter,
                KMPEnums.Access.Edit))
            {
                gvFilters.Columns[10].Visible = false;
            }

            if (!KMP.User.HasAccess(
                Master.UserSession.CurrentUser,
                KMPEnums.Services.UAD,
                KMPEnums.ServiceFeatures.UADFilter,
                KMPEnums.Access.Delete))
            {
                gvFilters.Columns[11].Visible = false;
            }
        }

        protected void gvFilterSegmentations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Delete))
                {
                    try
                    {
                        if (FilterSchedule.ExistsByFilterSegmentationID(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString())))
                            DisplayError("Filter Segmentation cannot be deleted. There is an active or inactive scheduled export associated with the filter segmentation.  Delete the export and then delete the filter segmentation.");
                        else
                        {
                            new FrameworkUAD.BusinessLogic.FilterSegmentation().Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.LoggedInUser, Master.clientconnections);
                            LoadFilterSegmentationGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayError(ex.Message);
                    }
                }
            }
            else if (e.CommandName == "FilterSegmentationEdit")
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
                {
                    try
                    {
                        FilterSegmentationSave.Mode = "Edit";
                        FilterSegmentationSave.BrandID = 0;
                        FilterSegmentationSave.PubID = 0;
                        FilterSegmentationSave.FilterSegmentationID = Convert.ToInt32(e.CommandArgument.ToString());
                        FilterSegmentationSave.UserID = Master.LoggedInUser;
                        FilterSegmentationSave.FilterCollection = null;
                        FilterSegmentationSave.FilterViewCollection = null;
                        FilterSegmentationSave.ViewType = Enums.ViewType.None;
                        FilterSegmentationSave.LoadControls();
                        FilterSegmentationSave.LoadFilterSegmentationData();
                        FilterSegmentationSave.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        DisplayError(ex.Message);
                    }
                }
            }
        }

        protected void gvFilterSegmentations_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvFilterSegmentations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFilters.PageIndex = e.NewPageIndex;
            LoadFilterSegmentationGrid();
        }

        protected void gvFilterSegmentations_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void gvFilterSegmentations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!pnlBrand.Visible)
                {
                    var lblBrandName = (Label)e.Row.FindControl("lblBrandName");
                    lblBrandName.Visible = false;
                }
            }
        }

        protected void gvFilterSegmentations_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        public override void LoadFilterSegmentationGrid()
        {
            try
            {
                gvFilterSegmentations.DataSource = null;
                gvFilterSegmentations.DataBind();

                DataTable dt = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectViewTypeUserID(Master.UserSession.CurrentUser.UserID, 0, Convert.ToInt32(hfBrandID.Value), Enums.ViewType.None.ToString(), KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) ? true : false, rtvFilterCategory.SelectedValue == "" ? 0 : Convert.ToInt32(rtvFilterCategory.SelectedValue), txtSearch.Text, drpSearch.SelectedValue, Master.clientconnections);

                List<DataRow> lmf = dt.AsEnumerable().ToList();

                List<KMPlatform.Entity.User> lusers = new KMPlatform.BusinessLogic.User().Select();

                var query = (from m in lmf
                             join u in lusers on m["CreatedUserID"] equals u.UserID into mu
                             from f in mu.DefaultIfEmpty()
                             select new {FilterID = m["FilterId"], BrandName =  m["BrandName"].ToString(), PubName = m["PubName"].ToString(), FilterCategoryName =  m["CategoryName"].ToString(), FilterType = m["FilterType"].ToString(), FilterSegmentationID = m["FilterSegmentationID"], Name = m["Name"], FilterSegmentationName = m["FilterSegmentationName"].ToString(), Notes = m["Notes"].ToString(), CreatedDate = m["CreatedDate"].ToString(), CreatedName = f == null ? "" : f.UserName }).ToList();

                switch (SortFieldFS.ToUpper())
                {
                    case "BRANDNAME":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.BrandName).ToList();
                        else
                            query = query.OrderByDescending(o => o.BrandName).ToList();
                        break;

                    case "NAME":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.Name).ToList();
                        else
                            query = query.OrderByDescending(o => o.Name).ToList();
                        break;

                    case "FILTERSEGMENTATIONNAME":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.FilterSegmentationName).ToList();
                        else
                            query = query.OrderByDescending(o => o.FilterSegmentationName).ToList();
                        break;

                    case "NOTES":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.Notes).ToList();
                        else
                            query = query.OrderByDescending(o => o.Notes).ToList();
                        break;

                    case "FILTERTYPE":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.FilterType).ToList();
                        else
                            query = query.OrderByDescending(o => o.FilterType).ToList();
                        break;

                    case "PUBNAME":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.PubName).ToList();
                        else
                            query = query.OrderByDescending(o => o.PubName).ToList();
                        break;

                    case "FILTERCATEGORYNAME":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.FilterCategoryName).ToList();
                        else
                            query = query.OrderByDescending(o => o.FilterCategoryName).ToList();
                        break;
                    case "CREATEDDATE":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.CreatedDate).ToList();
                        else
                            query = query.OrderByDescending(o => o.CreatedDate).ToList();
                        break;
                    case "CREATEDNAME":
                        if (SortDirectionFS.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.CreatedName).ToList();
                        else
                            query = query.OrderByDescending(o => o.CreatedName).ToList();
                        break;
                }

                gvFilterSegmentations.DataSource = query;
                gvFilterSegmentations.DataBind();

                if (query != null)
                {
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.Edit))
                    {
                        gvFilterSegmentations.Columns[9].Visible = false;
                    }

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
                    {
                        gvFilterSegmentations.Columns[10].Visible = false;
                    }

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Delete))
                    {
                        gvFilterSegmentations.Columns[11].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            rtvFilterCategory.UnselectAllNodes();

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
            rtvFilterCategory.UnselectAllNodes();
            rtvFilterCategory.CollapseAllNodes();
            gvFilters.DataSource = null;
            gvFilters.DataBind();
            gvFilterSegmentations.DataSource = null;
            gvFilterSegmentations.DataBind();
            drpSearch.ClearSelection();
            txtSearch.Text = string.Empty;
            lblSearch.Text = "Filter Name or Question Name";
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            //Nothing to do here, just had to implement abstract method
        }

        private int ToInt32WithThrow(string numberString)
        {
            int result;
            if (!int.TryParse(numberString, out result))
            {
                throw new InvalidOperationException($"Couldn't parse '{numberString}' to int.");
            }
            return result;
        }
    }
}