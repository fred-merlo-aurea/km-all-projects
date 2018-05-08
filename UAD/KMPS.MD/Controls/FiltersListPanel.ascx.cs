using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using KMPS.MD.Objects;
using Telerik.Web.UI;
using System.Data;

namespace KMPS.MD.Controls
{
    public partial class FiltersListPanel : BaseControl
    {
        public Delegate hideShowFilterPopup;
        public Delegate LoadSelectedFilterData;
        public Delegate LoadSelectedFilterSegmentationData;
        public event EventHandler CausePostBack;
        
        public string Mode
        {
            get
            {
                try
                {
                    return (string)ViewState["Mode"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["Mode"] = value;
            }
        }
        public bool AllowMultiRowSelection
        {
            get
            {
                try
                {
                    return (bool)ViewState["AllowMultiRowSelection"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["AllowMultiRowSelection"] = value;
            }
        }

        public bool ShowFilterSegmentation
        {
            get
            {
                try
                {
                    return (bool)ViewState["ShowFilterSegmentation"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState["ShowFilterSegmentation"] = value;
            }
        }

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        public Enums.LoadFilterOptions FilterRadioButtonOptions
        {
            get
            {
                try
                {
                    return (Enums.LoadFilterOptions)Enum.Parse(typeof(Enums.LoadFilterOptions), ViewState["FilterRadioButtonOPtions"].ToString());
                }
                catch
                {
                    if (ShowFilterSegmentation)
                    {
                        return Enums.LoadFilterOptions.Both;
                    }
                    return Enums.LoadFilterOptions.Filters;
                }
            }
            set
            {
                ViewState["FilterRadioButtonOPtions"] = value;
            }
        }

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;

            this.mdlPopShowFilter.Show();
        }
        private void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSelectFilter_Click(object sender, EventArgs e)
        {

            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                if (Convert.ToInt32(rgFilters.SelectedValue) <= 0)
                {
                    lblErrorMessage.Text = "Please select filter.";
                    divError.Visible = true;
                    return;
                }

                drpSearch.ClearSelection();
                txtSearch.Text = string.Empty;

                List<int> filterIDs = new List<int>();

                foreach (GridDataItem item in rgFilters.SelectedItems)
                {
                    filterIDs.Add(Convert.ToInt32(item.GetDataKeyValue("FilterID")));
                }

                LoadSelectedFilterData.DynamicInvoke(filterIDs);
            }
            else
            {
                if (Convert.ToInt32(rgFilterSegmentations.SelectedValue) <= 0)
                {
                    lblErrorMessage.Text = "Please select filter segmentation.";
                    divError.Visible = true;
                    return;
                }

                drpSearch.ClearSelection();
                txtSearch.Text = string.Empty;

                int filtersegmentationID = 0;

                foreach (GridDataItem item in rgFilterSegmentations.SelectedItems)
                {
                    filtersegmentationID = Convert.ToInt32(item.GetDataKeyValue("FilterSegmentationID"));
                }

                LoadSelectedFilterSegmentationData.DynamicInvoke(filtersegmentationID);
            }

            hideShowFilterPopup.DynamicInvoke();
            if (Mode.Equals("Load", StringComparison.OrdinalIgnoreCase))
                CausePostBack(sender, e);
            mdlPopShowFilter.Hide();
        }

        protected void cbAllFilters_OnCheckedChanged(object sender, EventArgs e)
        {
            rtvFilterCategory.UnselectAllNodes();
            rgFilters.DataSource = null;
            rgFilters.DataBind();
            drpSearch.ClearSelection();
            txtSearch.Text = string.Empty;

            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                rgFilters.DataSource = LoadFilters();
                rgFilters.DataBind();
                phFilters.Visible = true;
                phFilterSegmentations.Visible = false;
            }
            else
            {
                rgFilterSegmentations.DataSource = LoadFilterSegmentations();
                rgFilterSegmentations.DataBind();
                phFilters.Visible = false;
                phFilterSegmentations.Visible = true;
            }
        }

        public void LoadControls()
        {
            // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
            if (rblListType.Items.FindByValue("FilterSegmentations") == null)
            {
                rblListType.Items.Insert(1, new ListItem("Filter Segmentations", "FilterSegmentations"));
            }
            foreach (ListItem item in rblListType.Items)
            {
                item.Enabled = false;
            }

            switch (FilterRadioButtonOptions)
            {
                case Enums.LoadFilterOptions.Filters:
                    rblListType.Items[0].Enabled = true;
                    rblListType.SelectedValue = Enums.ListType.Filters.ToString();
                    break;
                case Enums.LoadFilterOptions.FilterSegmentations:
                    rblListType.Items[1].Enabled = true;
                    rblListType.SelectedValue = Enums.ListType.FilterSegmentations.ToString();
                    break;
                case Enums.LoadFilterOptions.Both:
                    foreach (ListItem item in rblListType.Items)
                    {
                        item.Enabled = true;
                    }
                    rblListType.SelectedValue = Enums.ListType.Filters.ToString();
                    break;
                default:
                    rblListType.Items[0].Enabled = true;
                    rblListType.SelectedValue = Enums.ListType.Filters.ToString();
                    break;
            }

            if (ViewType == Enums.ViewType.ConsensusView || ViewType == Enums.ViewType.RecencyView)
            {
                pnlIsRecentData.Visible = true;

                if (ViewType == Enums.ViewType.RecencyView)
                    cbIsRecentData.Checked = true;
                else
                    cbIsRecentData.Checked = false;
            }
            else
            {
                pnlIsRecentData.Visible = false;
                cbIsRecentData.Checked = false;
            }

            rblListType_SelectedIndexChanged(null, null);

            rtvFilterCategory.DataSource = KMPS.MD.Objects.FilterCategory.GetAll(clientconnections);
            rtvFilterCategory.DataBind();

            RadTreeNode root = new RadTreeNode("No Category", "0");
            rtvFilterCategory.Nodes.Insert(0, root);
            rtvFilterCategory.ExpandAllNodes();
            rtvFilterCategory.Nodes[0].Selected = true;

            if (Mode.Equals("AddExisting", StringComparison.OrdinalIgnoreCase))
            {
                btnSelectFilter.Text = "Select Filter";
                cbAllFilters.Visible = false;
            }
            else if (Mode.Equals("Load", StringComparison.OrdinalIgnoreCase))
            {
                btnSelectFilter.Text = "Load Filter";
                cbAllFilters.Visible = true;
            }
        }

        private IEnumerable LoadFilters()
        {
            List<MDFilter> lmf = new List<MDFilter>();
            lmf = MDFilter.GetFilterByUserIDType(clientconnections, (cbAllFilters.Checked ? 0 : UserID), ViewType, PubID, BrandID, KM.Platform.User.IsAdministrator(UserSession.CurrentUser) ? true : false, Mode.Equals("Load", StringComparison.OrdinalIgnoreCase) ? true : false);

            if (rtvFilterCategory.SelectedValue != "")
            {
                lmf = lmf.FindAll(x => (x.FilterCategoryID ?? 0) == Convert.ToInt32(rtvFilterCategory.SelectedValue));
            }

            if (lmf != null)
            {
                if (txtSearch.Text != string.Empty)
                {
                    switch (drpSearch.SelectedValue.ToUpper())
                    {
                        case "EQUAL":
                            lmf = lmf.FindAll(x => x.Name.ToLower().Equals(txtSearch.Text.ToLower()) || (x.QuestionName ?? "").ToLower().Equals(txtSearch.Text.ToLower()));
                            break;
                        case "START WITH":
                            lmf = lmf.FindAll(x => x.Name.ToLower().StartsWith(txtSearch.Text.ToLower()) || (x.QuestionName ?? "").ToLower().StartsWith(txtSearch.Text.ToLower()));
                            break;
                        case "END WITH":
                            lmf = lmf.FindAll(x => x.Name.ToLower().EndsWith(txtSearch.Text.ToLower()) || (x.QuestionName ?? "").ToLower().EndsWith(txtSearch.Text.ToLower()));
                            break;
                        case "CONTAINS":
                            lmf = lmf.FindAll(x => x.Name.ToLower().Contains(txtSearch.Text.ToLower()) || (x.QuestionName ?? "").ToLower().Contains(txtSearch.Text.ToLower()));
                            break;
                    }
                }
            }

            List<KMPlatform.Entity.User> lusers = new KMPlatform.BusinessLogic.User().Select();

            var query = (from m in lmf
                         join u in lusers on m.CreatedUserID equals u.UserID into mu
                         from f in mu.DefaultIfEmpty()
                         select new { m.FilterId, m.Name, m.QuestionName, m.CreatedDate, CreatedName = f == null ? "" : f.UserName, m.Notes }).ToList();

            return query;
        }

        private IEnumerable LoadFilterSegmentations()
        {
            //Bug #47636 IsAdmin is hardcoded to false here because stored procedure returns all the records for Admin and it is used elswhere with expected behavior.
            DataTable dt = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectViewTypeUserID((cbAllFilters.Checked ? 0 : UserID), PubID, BrandID, ViewType.ToString(), false, rtvFilterCategory.SelectedValue == "" ? 0 : Convert.ToInt32(rtvFilterCategory.SelectedValue), txtSearch.Text, drpSearch.SelectedValue, clientconnections);

            List<DataRow> lmf = dt.AsEnumerable().ToList();

            List<KMPlatform.Entity.User> lusers = new KMPlatform.BusinessLogic.User().Select();

            var query = (from m in lmf
                         join u in lusers on m["CreatedUserID"] equals u.UserID into mu
                         from f in mu.DefaultIfEmpty()
                         select new { FilterSegmentationID = m["FilterSegmentationID"], Name =  m["Name"], FilterSegmentationName = m["FilterSegmentationName"], Notes = m["Notes"], CreatedDate =  m["CreatedDate"], CreatedName = f == null ? "" : f.UserName}).ToList();

            return query;
        }

        private void ResetControls()
        {
            cbAllFilters.Checked = false;
            cbAllFilters.Text = "Show filters created by all users";
            rtvFilterCategory.UnselectAllNodes();
            rtvFilterCategory.CollapseAllNodes();
            rgFilters.DataSource = null;
            rgFilters.DataBind();
            rgFilterSegmentations.DataSource = null;
            rgFilterSegmentations.DataBind();
            drpSearch.ClearSelection();
            txtSearch.Text = string.Empty;
            lblSearch.Text = "Filter Name or Question Name";
            btnSelectFilter.Text = "Load Filter";
        }

        protected void btnCloseFilter_Click(object sender, EventArgs e)
        {
            ResetControls();
            hideShowFilterPopup.DynamicInvoke();
            mdlPopShowFilter.Hide();
        }

        protected void rtvFilterCategory_OnNodeClick(object sender, RadTreeNodeEventArgs e)
        {
            //cbAllFilters.Checked = false;
            rgFilters.DataSource = null;
            rgFilters.DataBind();
            drpSearch.ClearSelection();
            txtSearch.Text = string.Empty;

            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                rgFilters.DataSource = LoadFilters();
                rgFilters.DataBind();
                phFilters.Visible = true;
                phFilterSegmentations.Visible = false;
            }
            else
            {
                rgFilterSegmentations.DataSource = LoadFilterSegmentations();
                rgFilterSegmentations.DataBind();
                phFilters.Visible = false;
                phFilterSegmentations.Visible = true;
            }
        }

        protected void rgFilters_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            rgFilters.DataSource = LoadFilters();
        }

        protected void rgFilterSegmentations_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            rgFilterSegmentations.DataSource = LoadFilterSegmentations();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            rtvFilterCategory.UnselectAllNodes();

            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                rgFilters.DataSource = LoadFilters();
                rgFilters.DataBind();
                phFilters.Visible = true;
                phFilterSegmentations.Visible = false;
            }
            else
            {
                rgFilterSegmentations.DataSource = LoadFilterSegmentations();
                rgFilterSegmentations.DataBind();
                phFilters.Visible = false;
                phFilterSegmentations.Visible = true;
            }
        }

        protected void rblListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();

            if (rblListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                lblSearch.Text = "Filter Name or Question Name";
                cbAllFilters.Text = "Show filters created by all users";
                rgFilters.DataSource = LoadFilters();
                rgFilters.DataBind();
                phFilters.Visible = true;
                phFilterSegmentations.Visible = false;
                btnSelectFilter.Text = "Load Filter";
            }
            else
            {
                lblSearch.Text = "Filter Name or Filter Segmentation";
                cbAllFilters.Text = "Show filter segmentation created by all users";
                rgFilterSegmentations.DataSource = LoadFilterSegmentations();
                rgFilterSegmentations.DataBind();
                phFilters.Visible = false;
                phFilterSegmentations.Visible = true;
                btnSelectFilter.Text = "Load Segmentation";
            }
        }
    }
}