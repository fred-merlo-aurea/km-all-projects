using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class ReportGroups : System.Web.UI.Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Report Groups";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                gvReportGroup.PageSize = 10;
                SortField = "DisplayName";
                SortDirection = "ASC";
                LoadPubs(Master.clientconnections);
                LoadResponseGroups(Master.clientconnections);
                LoadGrid();
            }
        }
        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void drpPubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadResponseGroups(Master.clientconnections);
            LoadGrid();
        }

        protected void drpResponseGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void LoadResponseGroups(KMPlatform.Object.ClientConnections clientconnection)
        {
            drpResponseGroup.DataSource = KMPS.MD.Objects.ResponseGroup.GetByPubID(Master.clientconnections, Convert.ToInt32(drpPubs.SelectedItem.Value));
            drpResponseGroup.DataBind();
            drpResponseGroup.Items.Insert(0, new ListItem("Select Response Group", "0"));
        }

        protected void LoadPubs(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Pubs> pubs = Pubs.GetActive(Master.clientconnections);

            var query = (from p in pubs
                    where p.IsCirc == true
                    select p);

            drpPubs.DataSource = query.ToList();
            drpPubs.DataBind();
            drpPubs.Items.Insert(0, new ListItem("Select Product", "0"));
        }


        protected void gvReportGroup_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField)
            {
                SortDirection = (SortDirection.ToUpper() == "ASC" ? "DESC" : "ASC");
            }
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }

        protected void LoadGrid()
        {
            List<FrameworkUAD.Entity.ReportGroups> rg = new FrameworkUAD.BusinessLogic.ReportGroups().Select(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient)).FindAll(x => x.ResponseGroupID == Convert.ToInt32(drpResponseGroup.SelectedValue));

            List<FrameworkUAD.Entity.ReportGroups> lst = null;

            if (rg != null && rg.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "DISPLAYNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.DisplayName).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.DisplayName).ToList();
                        break;
                    case "DISPLAYORDER":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = rg.OrderBy(o => o.DisplayOrder).ToList();
                        else
                            lst = rg.OrderByDescending(o => o.DisplayOrder).ToList();
                        break;
                }
            }

            gvReportGroup.DataSource = lst;
            gvReportGroup.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (new FrameworkUAD.BusinessLogic.ReportGroups().ExistsByIDName(Convert.ToInt32(hfReportGroupID.Value), Convert.ToInt32(drpResponseGroup.SelectedValue), txtDisplayName.Text.Trim(), new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient)))
            {
                DisplayError("Report Group already exists or Report Group contains illegal spaces.");
                return;
            }

            try
            {
                if (Convert.ToInt32(drpResponseGroup.SelectedValue) > 0 && Convert.ToInt32(drpPubs.SelectedValue) > 0)
                {
                    FrameworkUAD.Entity.ReportGroups rg = new FrameworkUAD.Entity.ReportGroups();
                    rg.ReportGroupID = Convert.ToInt32(hfReportGroupID.Value);
                    rg.ResponseGroupID = Convert.ToInt32(drpResponseGroup.SelectedValue);
                    rg.DisplayName = txtDisplayName.Text;

                    if (Convert.ToInt32(hfReportGroupID.Value) > 0)
                    {
                        rg.DisplayOrder = Convert.ToInt32(hfSortOrder.Value);
                    }
                    else
                    {
                        List<FrameworkUAD.Entity.ReportGroups> lrg = new FrameworkUAD.BusinessLogic.ReportGroups().Select(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient)).FindAll(x => x.ResponseGroupID == Convert.ToInt32(drpResponseGroup.SelectedValue));

                        if (lrg.Count == 0)
                            rg.DisplayOrder = 1;
                        else
                            rg.DisplayOrder = lrg.OrderByDescending(x => x.DisplayOrder).FirstOrDefault().DisplayOrder + 1;
                    }

                    new FrameworkUAD.BusinessLogic.ReportGroups().Save(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), rg);

                    LoadGrid();
                    ResetControls();
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void gvReportGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    ResetControls();

                    List<FrameworkUAD.Entity.ReportGroups>  lrg = new FrameworkUAD.BusinessLogic.ReportGroups().Select(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient));

                    FrameworkUAD.Entity.ReportGroups rg = lrg.Find(x=>x.ReportGroupID == Convert.ToInt32(gvReportGroup.DataKeys[Convert.ToInt32(e.CommandArgument)].Value));

                    txtDisplayName.Text = rg.DisplayName;
                    hfSortOrder.Value = rg.DisplayOrder.ToString();
                    hfReportGroupID.Value = rg.ReportGroupID.ToString();
                    btnSave.Text = "UPDATE";
                    lblpnlHeader.Text = "Edit Report Group";

                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        private void ResetControls()
        {
            txtDisplayName.Text = "";
            btnSave.Text = "SAVE";
            lblpnlHeader.Text = "Add Report Group";
            hfSortOrder.Value = "0";
            hfReportGroupID.Value = "0";
        }
        protected void gvReportGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReportGroup.PageIndex = e.NewPageIndex;
            LoadGrid();
        }
    }
}