using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KMPS.MD.Objects;
using System.Linq;
using System.Collections.Specialized;

namespace KMPS.MDAdmin
{
    public partial class WebForm1 : KMPS.MD.Main.WebPageHelper
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

        //public static string connStr = DataFunctions.GetClientSqlConnection(Master.clientconnections);
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.Menu = "Products";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            divMessage.Visible = false;
            lblMessage.Text = "";
            SqlDataSourcePubTypes.ConnectionString = DataFunctions.GetClientSqlConnection(Master.clientconnections).ConnectionString;
            //Trace.Write(DataFunctions.GetClientSqlConnection(Master.clientconnections));

            if (!IsPostBack)
            {
                displayClients();
                displayGroups();
                displayBrands();

                drpFrequency.DataSource = Frequency.GetAll(Master.clientconnections);
                drpFrequency.DataBind();
                drpFrequency.Items.Insert(0, new ListItem("", ""));

                SortField = "PubName";
                SortDirection = "ASC";
                LoadGrid();
            }
        }

        protected void Page_OnPreInit(EventArgs e)
        {
            //SqlDataSourcePub.ConnectionString = connStr;
            //SqlDataSourcePubConnect.ConnectionString = connStr;
            //SqlDataSourcePubTypes.ConnectionString = connStr;
        }

        protected void gvPub_Sorting(object sender, GridViewSortEventArgs e)
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
            List<Pubs> pubs = Pubs.GetAll(Master.clientconnections);

            if (pubs != null && pubs.Count > 0)
            {
                List<Pubs> lst = null;

                switch (SortField.ToUpper())
                {
                    case "PUBNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.PubName).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.PubName).ToList();
                        break;

                    case "PUBCODE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.PubCode).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.PubCode).ToList();
                        break;

                    case "PUBTYPEDISPLAYNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.PubTypeDisplayName).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.PubTypeDisplayName).ToList();
                        break;

                    case "SCORE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.Score).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.Score).ToList();
                        break;

                    case "ENABLESEARCHING":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.EnableSearching).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.EnableSearching).ToList();
                        break;
                    case "SORTORDER":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.SortOrder).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.SortOrder).ToList();
                        break;
                    case "HASPAIDRECORDS":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pubs.OrderBy(o => o.HasPaidRecords).ToList();
                        else
                            lst = pubs.OrderByDescending(o => o.HasPaidRecords).ToList();
                        break;

                }

                if (lst != null)
                {
                    if (txtSearch.Text != string.Empty)
                    {
                        lst = lst.FindAll(x => x.PubName.ToLower().Contains(txtSearch.Text.ToLower()) || (x.PubCode.ToLower().Contains(txtSearch.Text.ToLower())));
                    }
                }

                gvPub.DataSource = lst;
                gvPub.DataBind();
            }
        }

        protected void gvPub_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPub.PageIndex = e.NewPageIndex;
            LoadGrid();
        }

        protected void drpClients_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                lstDestFields.Items.Clear();
                lstSourceFields.Items.Clear();
                if (drpClients.SelectedItem != null)
                {
                    int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClients.SelectedItem.Value), false).CustomerID;

                    List<Groups> allGroups = Groups.GetGroupsByCustomerID(Convert.ToInt32(CustomerID));

                    if (Convert.ToInt32(hfPubID.Value) > 0)
                    {
                        List<PubGroups> PGroups = PubGroups.Get(Master.clientconnections, Convert.ToInt32(hfPubID.Value));

                        foreach (Groups g in allGroups)
                        {
                            if (!PGroups.Exists(x => x.GroupID == g.GroupID))
                                lstSourceFields.Items.Add(new ListItem(g.GroupName, g.GroupID.ToString()));
                            else
                                lstDestFields.Items.Add(new ListItem(g.GroupName, g.GroupID.ToString()));
                        }
                    }
                    else
                    {
                        lstSourceFields.DataSource = allGroups.ToList();
                        lstSourceFields.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void displayGroups()
        {
            lstDestFields.Items.Clear();
            lstSourceFields.Items.Clear();
            if (drpClients.SelectedItem != null)
            {
                int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClients.SelectedItem.Value), false).CustomerID;

                List<Groups> lGroups = new List<Groups>();
                lGroups = Groups.GetGroupsByCustomerID(Convert.ToInt32(CustomerID));

                lstSourceFields.DataSource = lGroups.ToList();
                lstSourceFields.DataBind();
            }
        }

        protected void displayBrands()
        {
            lstSelectedBrands.Items.Clear();
            lstAvailableBrands.Items.Clear();
            lstAvailableBrands.DataSource = Brand.GetAll(Master.clientconnections);
            lstAvailableBrands.DataBind();
        }

        protected void displayClients()
        {
            drpClients.DataSource = Master.UserSession.CurrentUserClientGroupClients;
            drpClients.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!cbIsActive.Checked && Convert.ToInt32(hfPubID.Value) > 0)
                {
                    NameValueCollection nvc = MD.Objects.Pubs.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(hfPubID.Value));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected pub is being used in the following filters, Brand, DownloadTemplates or CrossTab Reports and cannot be updated as inactive  until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        divError.Visible = true;
                        lblErrorMessage.Text = errorMsg;
                        return;
                    }
                }

                if (Pubs.ExistsByIDName(Master.clientconnections, Convert.ToInt32(hfPubID.Value), txtName.Text) != 0)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = "Name already exists.";
                    return;
                }

                if (Pubs.ExistsByIDCode(Master.clientconnections, Convert.ToInt32(hfPubID.Value), txtCode.Text) != 0)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = "Code already exists.";
                    return;
                }

                Pubs p = new Pubs();
                p.PubID = Convert.ToInt32(hfPubID.Value);
                p.PubName = txtName.Text;
                p.PubCode = txtCode.Text;
                p.PubTypeID = Convert.ToInt32(ddlType.SelectedItem.Value);
                p.EnableSearching = cbSearching.Checked;
                p.Score = Convert.ToInt32(drpScore.SelectedItem.Value);
                p.HasPaidRecords = cbHasPaidRecords.Checked;
                p.IsActive = cbIsActive.Checked;
                p.IsUAD = cbIsUAD.Checked;
                p.IsCirc = cbIsCirc.Checked;
                p.UseSubGen = UseSubGen.Checked;
                p.FrequencyID = drpFrequency.SelectedValue != string.Empty ? Convert.ToInt32(drpFrequency.SelectedValue) : (int?)null;
                p.YearStartDate = txtYearStartDate.Text;
                p.YearEndDate = txtYearEndDate.Text;

                if (p.PubID > 0)
                {
                    p.UpdatedByUserID = Master.LoggedInUser;
                    p.DateUpdated = DateTime.Now;
                }
                else
                {
                    p.CreatedByUserID = Master.LoggedInUser;
                    p.DateCreated = DateTime.Now;
                }

                int PubID = p.Save(Master.clientconnections);

                if (PubID != 0)
                {
                    PubGroups.Delete(Master.clientconnections, PubID);
                    foreach (ListItem li in lstDestFields.Items)
                    {
                        PubGroups pg = new PubGroups();
                        pg.PubID = PubID;
                        pg.GroupID = Convert.ToInt32(li.Value);
                        pg.Save(Master.clientconnections);
                    }

                    BrandDetails.DeleteByPubID(Master.clientconnections, PubID);

                    foreach (ListItem li in lstSelectedBrands.Items)
                    {
                        BrandDetails bd = new BrandDetails();
                        bd.BrandID = Convert.ToInt32(li.Value);
                        bd.PubID = PubID;
                        BrandDetails.Save(Master.clientconnections, bd);
                   }
                }

                Response.Redirect("Publications.aspx");
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void ResetControls()
        {
            hfPubID.Value = "0";
            txtName.Text = "";
            txtCode.Text = "";
            ddlType.ClearSelection();
            cbSearching.Checked = true;
            drpScore.ClearSelection();
            drpClients.ClearSelection();
            displayGroups();
            displayBrands();
            cbHasPaidRecords.Checked = false;
            cbIsActive.Checked = true;
            cbIsUAD.Checked = false;
            cbIsCirc.Checked = false;
            UseSubGen.Checked = false;
            drpFrequency.ClearSelection();
            txtYearStartDate.Text = "";
            txtYearEndDate.Text = "";
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            try
            {
                lstSelectedBrands.Items.Clear();
                lstAvailableBrands.Items.Clear();
                lstDestFields.Items.Clear();
                lstSourceFields.Items.Clear();
                drpScore.ClearSelection();
                drpFrequency.ClearSelection();
                ddlType.ClearSelection();

                int PubID = Convert.ToInt32(e.CommandArgument);
                Pubs pub = Pubs.GetByID(Master.clientconnections, PubID);
                if (pub != null)
                {
                    txtName.Text = pub.PubName;
                    txtCode.Text = pub.PubCode;
                    hfPubID.Value = pub.PubID.ToString();
                    ddlType.Items.FindByValue(pub.PubTypeID.ToString()).Selected = true;
                    cbSearching.Checked = (bool)pub.EnableSearching;
                    drpScore.Items.FindByValue(pub.Score.ToString()).Selected = true;
                    hfCreatedDate.Value = pub.DateCreated.ToString();
                    hfCreatedUserID.Value = pub.CreatedByUserID.ToString();
                    cbHasPaidRecords.Checked = pub.HasPaidRecords;
                    cbIsActive.Checked = pub.IsActive;
                    cbIsUAD.Checked = pub.IsUAD;
                    cbIsCirc.Checked = pub.IsCirc;
                    UseSubGen.Checked = pub.UseSubGen;
                    try
                    {
                        drpFrequency.SelectedValue = pub.FrequencyID.ToString();
                    }
                    catch
                    {

                    }
                    txtYearStartDate.Text = pub.YearStartDate;
                    txtYearEndDate.Text = pub.YearEndDate;
                }

                int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(Convert.ToInt32(drpClients.SelectedItem.Value), false).CustomerID;

                List<Groups> allGroups = Groups.GetGroupsByCustomerID(Convert.ToInt32(CustomerID));
                List<PubGroups> PGroups = PubGroups.Get(Master.clientconnections, PubID);

                foreach (Groups g in allGroups)
                {
                    if (!PGroups.Exists(x => x.GroupID == g.GroupID))
                        lstSourceFields.Items.Add(new ListItem(g.GroupName, g.GroupID.ToString()));
                    else
                        lstDestFields.Items.Add(new ListItem(g.GroupName, g.GroupID.ToString()));
                }

                List<Brand> allBrand = Brand.GetAll(Master.clientconnections);
                List<Brand> lb = Brand.GetByPubID(Master.clientconnections, PubID);
                
                foreach (Brand b in allBrand)
                {
                    if (!lb.Exists(x => x.BrandID == b.BrandID))
                        lstAvailableBrands.Items.Add(new ListItem(b.BrandName, b.BrandID.ToString()));
                    else
                        lstSelectedBrands.Items.Add(new ListItem(b.BrandName, b.BrandID.ToString()));
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    lstDestFields.Items.Add(lstSourceFields.Items[i]);
                }
            }
        }

        protected void btnremove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    lstDestFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnAddBrand_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstAvailableBrands.Items.Count; i++)
            {
                if (lstAvailableBrands.Items[i].Selected)
                {
                    lstSelectedBrands.Items.Add(lstAvailableBrands.Items[i]);
                }
            }
        }

        protected void btnRemoveBrand_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstSelectedBrands.Items.Count; i++)
            {
                if (lstSelectedBrands.Items[i].Selected)
                {
                    lstSelectedBrands.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void gvPub_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvPub_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    NameValueCollection nvc = MD.Objects.Pubs.ValidationForDeleteorInActive(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected pub is being used in the following filters, Brand, DownloadTemplates or CrossTab Reports and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        divMessage.Visible = true;
                        lblMessage.Text = errorMsg;
                        return;
                    }

                    Pubs.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    LoadGrid();
                }
                catch (Exception ex)
                {
                    divMessage.Visible = true;
                    lblMessage.Text = ex.Message;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadGrid();
        }
    }
}