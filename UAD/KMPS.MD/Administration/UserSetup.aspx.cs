//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using KMPS.MD.Objects;
//using System.Configuration;
//using System.Web.Security;
//using System.Data.SqlClient;
//using System.Data;

//namespace KMPS.MD.Administration
//{
//    public partial class UserSetup : KMPS.MD.Main.WebPageHelper
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            Master.Menu = "User Setup";

//            if (!(Page.User.IsInRole("admin") || Page.User.IsInRole("clientadmin")))
//                Response.Redirect("../Default.aspx");

//            divError.Visible = false;
//            lblErrorMessage.Text = "";
//            gvUsers.PageSize = 10;

//            if (!IsPostBack)
//            {
//                List<ECNUsers> Eusers = ECNUsers.GetECNUsers();
//                List<Users> users = Users.GetAllUsers();

//                var query = Eusers.Where(p => !users.Any(p2 => p2.UserID == p.UserID)); 

//                drpUsers.DataSource = query.ToList();
//                drpUsers.DataBind();
//                drpUsers.Items.Insert(0, new ListItem("--Select User-", "0"));

//                drpBaseChannels.DataSource = Users.GetBaseChannels();
//                drpBaseChannels.DataBind();

//                List<Customers> lCustomers = new List<Customers>();
//                lCustomers = Customers.GetCustomersByBaseChannelID(Convert.ToInt32(drpBaseChannels.SelectedItem.Value));

//                lstSourceFields.DataSource = lCustomers.ToList();
//                lstSourceFields.DataBind();

//                var AllUsers = from a in users
//                               join b in Eusers
//                               on a.UserID equals b.UserID
//                               select new {a.UserID, a.Permission, b.UserName, a.ShowSalesView};

//                gvUsers.DataSource = AllUsers.ToList();
//                gvUsers.DataBind();

//                int LicenseCount = Convert.ToInt32(Config.getLicenseCount().Value);
//                int LicenseUsed = Users.GetSalesViewAccessCount();
//                if (LicenseUsed == LicenseCount)
//                    chkShowSaleView.Enabled = false;

//                if (LicenseCount > 0)
//                    lblLicenseCount.Text = "Remaining License : " + (LicenseCount - LicenseUsed).ToString();
//            }
//        }

//        protected void drpPermission_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (drpPermission.SelectedItem.Value == "admin" || drpPermission.SelectedItem.Value == "fullpermission" || drpPermission.SelectedItem.Value == "no edit" || drpPermission.SelectedItem.Value == "clientadmin")
//            {
//                pnlExportPermission.Visible = true;

//                drpBaseChannels.DataSource = DataFunctions.getDataTable("select BaseChannelID, BaseChannelName from basechannel order by BaseChannelName", new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString));
//                drpBaseChannels.DataBind();
//            }
//            else
//            {
//                lstDestFields.Items.Clear();
//                pnlExportPermission.Visible = false;
//            }

//            if (drpPermission.SelectedItem.Value.Equals("readonly", StringComparison.OrdinalIgnoreCase))
//            {
//                lblSalesViewText.Text = "Sales View Only";
//            }
//            else
//            {
//                lblSalesViewText.Text = "Show Sales View";
//            }
//        }

//        protected void btnAdd_Click(object sender, EventArgs e)
//        {

//            for (int i = 0; i < lstSourceFields.Items.Count; i++)
//            {
//                if (lstSourceFields.Items[i].Selected)
//                {
//                    lstDestFields.Items.Add(lstSourceFields.Items[i]);
//                }
//            }
//        }

//        protected void btnremove_Click(object sender, EventArgs e)
//        {
//            for (int i = 0; i < lstDestFields.Items.Count; i++)
//            {
//                if (lstDestFields.Items[i].Selected)
//                {
//                    lstDestFields.Items.RemoveAt(i);
//                    i--;
//                }
//            }
//        }

//        protected void btnSave_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                string ExportPermissionIDs = "";
//                foreach (ListItem li in lstDestFields.Items)
//                {
//                    if (!ExportPermissionIDs.Contains(li.Value))
//                    {
//                        ExportPermissionIDs += (ExportPermissionIDs == string.Empty ? li.Value : "," + li.Value);
//                    }
//                }

//                int UserID = 0;

//                if(drpUsers.Visible == true)
//                {
//                    UserID = Convert.ToInt32(drpUsers.SelectedItem.Value);
//                }
//                else
//                {
//                    UserID = Convert.ToInt32(lblUserID.Text);
//                }


//                Users u = new Users();
//                u.UserID = UserID;
//                u.Permission = drpPermission.SelectedItem.Value;
//                u.ExportPermissionIDs = ExportPermissionIDs;
//                u.ShowSalesView = chkShowSaleView.Checked;

//                Users.SaveUsers(u);

//                Response.Redirect("UserSetup.aspx");
//            }
//            catch (Exception ex)
//            {
//                divError.Visible = true;
//                lblErrorMessage.Text = ex.Message;
//            }
//        }

//        protected void btnCancel_Click(object sender, EventArgs e)
//        {
//            Response.Redirect("UserSetup.aspx");
//        }

//        protected void drpBaseChannel_SelectedIndexChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                List<Customers> lCustomers = new List<Customers>();
//                lCustomers = Customers.GetCustomersByBaseChannelID(Convert.ToInt32(drpBaseChannels.SelectedItem.Value));

//                lstSourceFields.DataSource = lCustomers.ToList();
//                lstSourceFields.DataBind();
//            }
//            catch (Exception ex)
//            {
//                divError.Visible = true;
//                lblErrorMessage.Text = ex.Message;
//            }
//        }

//        protected void lnkEdit_Command(object sender, CommandEventArgs e)
//        {
//            int LicenseCount = Convert.ToInt32(Config.getLicenseCount().Value);
//            if (Users.GetSalesViewAccessCount() == LicenseCount)
//            {
//                chkShowSaleView.Enabled = false;
//            }


//            drpUsers.ClearSelection();
//            drpBaseChannels.ClearSelection();
//            drpPermission.ClearSelection();

//            string[] args = e.CommandArgument.ToString().Split('/');
//            String userID = args[0];

//            lblUsername.Text = args[1];
//            lblUsername.Visible = true;
//            lblUserID.Text = args[0];

//            drpUsers.Visible = false;

//            Users user = Users.GetUserByID(Convert.ToInt32(userID));

//            drpPermission.Items.FindByValue(user.Permission).Selected = true;

//            chkShowSaleView.Checked = user.ShowSalesView;
//            if (chkShowSaleView.Checked)
//                chkShowSaleView.Enabled = true;


//            if (user.Permission == "admin" || user.Permission == "fullpermission" || user.Permission == "no edit" || drpPermission.SelectedItem.Value == "clientadmin")
//            {
//                pnlExportPermission.Visible = true;
//            }
//            else
//            {
//                lstDestFields.Items.Clear();
//                pnlExportPermission.Visible = false;
//            }

//            if (drpPermission.SelectedItem.Value.Equals("readonly", StringComparison.OrdinalIgnoreCase))
//            {
//                lblSalesViewText.Text = "Sales View Only";
//            }
//            else
//            {
//                lblSalesViewText.Text = "Show Sales View";
//            }

//            drpBaseChannels.DataSource = Users.GetBaseChannels();
//            drpBaseChannels.DataBind();

//            List<Customers> lCustomers = new List<Customers>();
//            lCustomers = Customers.GetCustomersByBaseChannelID(Convert.ToInt32(drpBaseChannels.SelectedItem.Value));

//            List<int> PermissionIDslist = new List<int>();

//            string ExportPemissions = user.ExportPermissionIDs;

//            if (ExportPemissions.Length > 0)
//            {
//                string[] ExportPermissionIDs = ExportPemissions.Split(',');

//                foreach (string ExportPermissionID in ExportPermissionIDs)
//                {
//                    PermissionIDslist.Add(Convert.ToInt32(ExportPermissionID));
//                }

//            }

//            lstSourceFields.DataSource = lCustomers.ToList();
//            lstSourceFields.DataBind();

//            List<Customers> AllCustomers = new List<Customers>();
//            AllCustomers = Customers.GetAllCustomers();

//            var destquery = from item1 in AllCustomers
//                            where (PermissionIDslist.Any(item2 => item2 == item1.CustomerID))
//                            orderby item1.CustomerName ascending
//                            select item1;

//            lstDestFields.DataSource = destquery.ToList();
//            lstDestFields.DataBind();
//        }

//        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
//        {
//        }

//        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
//        {
//            if (e.CommandName == "Delete")
//            {
//                Users.DeleteUser(Convert.ToInt32(e.CommandArgument.ToString()));
//                loadgrid();
//            }
//        }
//        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            gvUsers.PageIndex = e.NewPageIndex;
//            loadgrid();
//        }

//        protected void loadgrid()
//        {
//            List<ECNUsers> Eusers = ECNUsers.GetECNUsers();
//            List<Users> users = Users.GetAllUsers();
//            var AllUsers = from a in users
//                           join b in Eusers
//                           on a.UserID equals b.UserID
//                           select new { a.UserID, a.Permission, b.UserName, a.ShowSalesView };

//            gvUsers.DataSource = AllUsers.ToList();
//            gvUsers.DataBind();
//        }

//    }
//}