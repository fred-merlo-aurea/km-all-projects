using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using System.Text;
using System.Collections;
using KMPS.MD.Objects;
using System.Linq;

namespace KMPS.MDAdmin
{
    public partial class CodeSheet : KMPS.MD.Main.WebPageHelper
    {
        //static string prevPage = String.Empty;
        //public string connStr = DataFunctions.GetClientSqlConnection(Master.clientconnections);
        public string groupvalue;
        private ArrayList EntryItems = new ArrayList();

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

        private int PubID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["PubID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int ResponseGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["ResponseGroupID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Code Sheet";
            btnSave.Text = "SAVE";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            lblpnlHeader.Text = "Add Responses";

            if (!IsPostBack)
            {
                SortField = "ResponseValue";
                SortDirection = "ASC";

                drpPubs.DataSource = Pubs.GetActive(Master.clientconnections);
                drpPubs.DataBind();
                drpPubs.Items.Insert(0, new ListItem("Select Product", "0"));

                try
                {
                    drpPubs.Items.FindByValue(PubID().ToString()).Selected = true;
                }
                catch
                {
                    drpPubs.Items.FindByValue("0").Selected = true;
                }

                LoadGroup();

                try
                {
                    drpGroup.Items.FindByValue(ResponseGroupID().ToString()).Selected = true;
                }
                catch
                {
                    drpGroup.Items.FindByValue("0").Selected = true;
                }

                LoadCodeSheetGrid();
                LoadPubTypes();
                LoadAvailablePubs();
                LoadReportGroup();
            }

        }

        protected void LoadGroup()
        {
            drpGroup.DataSource = KMPS.MD.Objects.ResponseGroup.GetActiveByPubID(Master.clientconnections, Convert.ToInt32(drpPubs.SelectedItem.Value));
            drpGroup.DataBind();
            drpGroup.Items.Insert(0, new ListItem("Select Group", "0"));
        }

        protected void LoadReportGroup()
        {
            drpReportGroups.DataSource = new FrameworkUAD.BusinessLogic.ReportGroups().Select(Master.clientconnections).FindAll(x => x.ResponseGroupID == Convert.ToInt32(drpGroup.SelectedValue));
            drpReportGroups.DataBind();
            drpReportGroups.Items.Insert(0, new ListItem("", ""));
        }

        protected void gvCodeSheet_Sorting(object sender, GridViewSortEventArgs e)
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

            LoadCodeSheetGrid();
        }

        protected void LoadCodeSheetGrid()
        {
            List<KMPS.MD.Objects.CodeSheet> lcs = KMPS.MD.Objects.CodeSheet.GetByPubIDResponseGroupID(Master.clientconnections, Convert.ToInt32(drpPubs.SelectedItem.Value), Convert.ToInt32(drpGroup.SelectedItem.Value));
            List<FrameworkUAD.Entity.ReportGroups> lrg = new FrameworkUAD.BusinessLogic.ReportGroups().Select(Master.clientconnections).FindAll(x => x.ResponseGroupID == Convert.ToInt32(drpGroup.SelectedValue));

            var query = (from c in lcs
                         join r in lrg on c.ReportGroupID equals r.ReportGroupID into cr
                         from f in cr.DefaultIfEmpty()
                         select new { c.CodeSheetID, c.ResponseValue, c.ResponseDesc, DisplayName = f == null ? "" : f.DisplayName, c.IsActive, c.IsOther  }).ToList();


            if (query != null && query.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "RESPONSEVALUE":
                        if (SortDirection.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.ResponseValue).ToList();
                        else
                            query = query.OrderByDescending(o => o.ResponseValue).ToList();
                        break;

                    case "RESPONSEDESC":
                        if (SortDirection.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.ResponseDesc).ToList();
                        else
                            query = query.OrderByDescending(o => o.ResponseDesc).ToList();
                        break;

                    case "DISPLAYNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.DisplayName).ToList();
                        else
                            query = query.OrderByDescending(o => o.DisplayName).ToList();
                        break;

                    case "ISACTIVE":
                        if (SortDirection.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.IsActive).ToList();
                        else
                            query = query.OrderByDescending(o => o.IsActive).ToList();
                        break;
                    case "ISOTHER":
                        if (SortDirection.ToUpper() == "ASC")
                            query = query.OrderBy(o => o.IsOther).ToList();
                        else
                            query = query.OrderByDescending(o => o.IsOther).ToList();
                        break;
                }
            }

            gvCodeSheet.DataSource = query;
            gvCodeSheet.DataBind();
        }

        private void LoadItemGrid()
        {
            grdItems.DataSource = EntryItems;
            grdItems.DataBind();
        }

        private void LoadPubTypes()
        {
            ddlPubTypes.Items.Clear();
            DataTable MasterGroups = DataFunctions.getDataTable("select  mg.MasterGroupID as 'ID', mg.DisplayName as 'Title' from MasterGroups mg", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            ddlPubTypes.DataSource = MasterGroups;
            ddlPubTypes.DataTextField = "Title";
            ddlPubTypes.DataValueField = "ID";
            ddlPubTypes.DataBind();
        }

        private void LoadPubs()
        {
            LoadAvailablePubs();
            try
            {
                foreach (GridViewRow row in grdItems.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblGroupID = (Label)row.FindControl("lblGroupID");
                        if (lblGroupID.Text == ddlPubTypes.SelectedValue)
                        {
                            Label lblEntryID = (Label)row.FindControl("lblEntryID");
                            foreach (ListItem item in lbAvailablePubs.Items)
                            {
                                if (item.Value == lblEntryID.Text)
                                {
                                    lbSelectedPubs.Items.Add(item);
                                }
                            }
                        }
                    }
                }
                if (lbSelectedPubs.Items.Count > 0)
                {
                    foreach (ListItem item in lbSelectedPubs.Items)
                    {
                        lbAvailablePubs.Items.Remove(item);
                    }
                    btnRemovePub.Enabled = true;
                }
                else
                {
                    btnRemovePub.Enabled = false;
                }
                if (lbAvailablePubs.Items.Count > 0)
                {
                    btnAddPub.Enabled = true;
                }
                else
                {
                    btnAddPub.Enabled = false;
                }
            }
            catch (Exception)
            {
            }

        }

        protected void ddlPubTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectedPubs.Items.Clear();
            LoadPubs();
        }

        protected void btnAddPub_Click(object sender, EventArgs e)
        {
            ListItemCollection itemsToRemove = new ListItemCollection();
            foreach (ListItem item in lbAvailablePubs.Items)
            {
                if (item.Selected)
                {
                    lbSelectedPubs.Items.Add(item);
                    itemsToRemove.Add(item);
                }
            }

            foreach (ListItem item in itemsToRemove)
            {
                lbAvailablePubs.Items.Remove(item);
            }

            if (lbAvailablePubs.Items.Count > 0)
            {
                btnAddPub.Enabled = true;
            }
            else
            {
                btnAddPub.Enabled = false;
            }
            btnRemovePub.Enabled = true;

            UpdateGrid();
        }

        protected void btnRemovePub_Click(object sender, EventArgs e)
        {
            ListBox lbToRemove = new ListBox();
            ListItemCollection items = new ListItemCollection();
            foreach (ListItem item in lbSelectedPubs.Items)
            {
                if (item.Selected)
                {
                    lbAvailablePubs.Items.Add(item);
                    items.Add(item);
                    lbToRemove.Items.Add(item);
                }
            }
            foreach (ListItem item in lbToRemove.Items)
            {
                lbSelectedPubs.Items.Remove(item);
            }

            if (lbSelectedPubs.Items.Count > 0)
            {
                btnRemovePub.Enabled = true;
            }
            else
            {
                btnRemovePub.Enabled = false;
            }
            btnAddPub.Enabled = true;
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            int itemID = 0;
            try
            {
                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblItemType = (Label)row.FindControl("lblItemType");
                    Label lblGroupID = (Label)row.FindControl("lblGroupID");
                    Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                    if (lblGroupID.Text != ddlPubTypes.SelectedValue)
                    {
                        EntryItems.Add(new EntryItem(++itemID, lblItemType.Text, lblGroupID.Text, lblGroupTitle.Text, lblEntryID.Text, lblEntryTitle.Text));
                    }

                }

                grdItems.DataSource = null;
                foreach (ListItem item in lbSelectedPubs.Items)
                {
                    EntryItems.Add(new EntryItem(++itemID, "", ddlPubTypes.SelectedValue, ddlPubTypes.SelectedItem.Text, item.Value, item.Text));
                }
                grdItems.DataSource = EntryItems;
                grdItems.DataBind();
            }
            catch (Exception)
            {
            }

        }

        protected void grdItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int newItemID = 0;
            try
            {
                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblItemType = (Label)row.FindControl("lblItemType");
                    Label lblGroupID = (Label)row.FindControl("lblGroupID");
                    Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                    EntryItems.Add(new EntryItem(++newItemID, lblItemType.Text, lblGroupID.Text, lblGroupTitle.Text, lblEntryID.Text, lblEntryTitle.Text));
                }

                int itemID = Convert.ToInt32(grdItems.DataKeys[Convert.ToInt32(e.RowIndex)].Value);

                for (int i = 0; i <= EntryItems.Count - 1; i++)
                {
                    if (itemID == ((EntryItem)EntryItems[i]).ItemID)
                    {
                        EntryItems.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            LoadItemGrid();
            lbSelectedPubs.Items.Clear();
            LoadPubs();
        }

        private void LoadAvailablePubs()
        {
            lbAvailablePubs.Items.Clear();
            lbAvailablePubs.DataSource = DataFunctions.getDataTable(" Select mcs.MasterID as 'ID', mg.DisplayName + ' - ' + mcs.MasterValue + ' - ' + mcs.MasterDesc as 'Title' " +
                                            "from Mastercodesheet mcs  " +
                                            "inner join MasterGroups mg " +
                                            "on mcs.MasterGroupID = mg.MasterGroupID " +
                                            "where mcs.MasterGroupID='" + ddlPubTypes.SelectedValue + "'" +
                                            "order by Title", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            lbAvailablePubs.DataTextField = "Title";
            lbAvailablePubs.DataValueField = "ID";
            lbAvailablePubs.DataBind();

        }

        protected void dvCodeSheet_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                lblErrorMessage.Text = "Error : " + e.Exception.Message;
                divError.Visible = true;
                e.ExceptionHandled = true;
            }
            else
            {
                gvCodeSheet.DataBind();
                drpGroup.DataBind();
            }
        }

        protected void drpPubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpReportGroups.Items.Clear();
            LoadGroup();
            LoadCodeSheetGrid();
        }

        protected void drpGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpReportGroups.Items.Clear();
            LoadReportGroup();
            LoadCodeSheetGrid();
        }


        protected void lbMasterID_DataBound(object sender, EventArgs e)
        {
            //if (ddlMasterID.Items.Count > 0)
            //{
            //    ddlMasterID.Items[0].Selected = true;
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "SAVE")
            {
                try
                {
                    if (KMPS.MD.Objects.CodeSheet.ExistsByResponseGroupIDResponseValue(Master.clientconnections, Convert.ToInt32(hfCodeSheetID.Value), Convert.ToInt32(drpGroup.SelectedValue), txtResponseValue.Text))
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = " Value already exists";
                        return;
                    }

                    KMPS.MD.Objects.CodeSheet cs = new KMPS.MD.Objects.CodeSheet();
                    cs.CodeSheetID = Convert.ToInt32(hfCodeSheetID.Value);
                    cs.PubID = Convert.ToInt32(drpPubs.SelectedValue);
                    cs.ResponseGroupID = Convert.ToInt32(drpGroup.SelectedValue);
                    cs.ResponseGroup = drpGroup.SelectedItem.ToString();
                    cs.ResponseValue = txtResponseValue.Text;
                    cs.ResponseDesc = txtResponseDesc.Text;
                    cs.IsActive = cbActive.Checked;
                    cs.IsOther = cbIsOther.Checked;
                    cs.ReportGroupID = drpReportGroups.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpReportGroups.SelectedValue);

                    if (cs.CodeSheetID > 0)
                    {
                        cs.UpdatedByUserID = Master.LoggedInUser;
                        cs.DateUpdated = DateTime.Now;
                    }
                    else
                    {
                        cs.CreatedByUserID = Master.LoggedInUser;
                        cs.DateCreated = DateTime.Now;
                    }

                    StringBuilder xmlDocument = new StringBuilder();
                    xmlDocument.Append("<ROOT>");
                    foreach (GridViewRow row in grdItems.Rows)
                    {
                        Label lblEntryID = (Label)row.FindControl("lblEntryID");

                        char[] separator = new char[] { ',' };
                        string[] strSplitArr = lblEntryID.Text.Split(separator);
                        foreach (string arrStr in strSplitArr)
                        {
                            xmlDocument.Append(String.Format("<RECORD MasterID=\"{0}\" />", arrStr));
                        }
                    }
                    xmlDocument.Append("</ROOT>");

                    KMPS.MD.Objects.CodeSheet.Save(Master.clientconnections, cs, xmlDocument);

                    Response.Redirect("CodeSheet.aspx?PubID=" + drpPubs.SelectedItem.Value + "&ResponseGroupID=" + drpGroup.SelectedItem.Value);
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.ToString();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CodeSheet.aspx?PubID=" + drpPubs.SelectedItem.Value + "&ResponseGroupID=" + drpGroup.SelectedItem.Value);
        }

        protected void gvCodeSheet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LoadMasterGrid((GridView)e.Row.FindControl("grdMaster"), Convert.ToInt32(gvCodeSheet.DataKeys[e.Row.RowIndex].Value));

            }
        }

        private void LoadMasterGrid(GridView grd, int CodeSheetID)
        {
            string sql = "select mc.MasterValue, mc.MasterDesc, mg.DisplayName " +
                "from CodeSheet_Mastercodesheet_Bridge cmb " +
                "inner join Mastercodesheet mc on cmb.MasterID = mc.MasterID " +
                "inner join MasterGroups mg on mc.MasterGroupID = mg.MasterGroupID " +
                "where cmb.CodeSheetID = " + CodeSheetID.ToString();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            da.Fill(ds, "Master");
            conn.Close();
            DataTable dataTable = ds.Tables["Master"];
            grd.DataSource = dataTable;
            grd.DataBind();

            ds.Dispose();
            da.Dispose();
            cmd.Dispose();

        }

        protected void gvCodeSheet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int CodeSheetID = Convert.ToInt32(gvCodeSheet.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                int itemID = 0;
                try
                {
                    MD.Objects.CodeSheet cs = MD.Objects.CodeSheet.GetByCodeSheetID(Master.clientconnections, CodeSheetID);
                    hfCodeSheetID.Value = cs.CodeSheetID.ToString();
                    txtPubID.Text = cs.PubID.ToString();
                    txtResponseValue.Text = cs.ResponseValue;
                    txtResponseDesc.Text = cs.ResponseDesc;
                    cbActive.Checked = cs.IsActive;
                    cbIsOther.Checked = cs.IsOther;

                    try
                    {
                        drpReportGroups.SelectedValue = cs.ReportGroupID.ToString();
                    }
                    catch
                    {
                    }

                    DataTable dt = MD.Objects.MasterCodeSheet.GetByCodeSheetID(Master.clientconnections, CodeSheetID);

                    foreach (DataRow dr in dt.Rows)
                    {
                        EntryItems.Add(new EntryItem(++itemID, "", dr["MasterGroupID"].ToString(), dr["DisplayName"].ToString(), dr["MasterID"].ToString(), dr["Display"].ToString()));
                    }

                    grdItems.DataSource = null;
                    grdItems.DataSource = EntryItems;
                    grdItems.DataBind();
                    lbSelectedPubs.Items.Clear();
                    LoadPubs();
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.ToString();
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    KMPS.MD.Objects.CodeSheet.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(drpGroup.SelectedItem.Value));
                    LoadCodeSheetGrid();
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.ToString();
                }
            }
        }

        protected void gvCodeSheet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvCodeSheet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCodeSheet.PageIndex = e.NewPageIndex;
            LoadCodeSheetGrid();
        }
    }
}