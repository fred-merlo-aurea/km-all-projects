using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Configuration;

namespace ecn.communicator.Customers
{
    public partial class MTAEditor : ECN_Framework.WebPageHelper
    {
        int userID = 0;

        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string connString = ConfigurationManager.AppSettings["com"];

        #region Variables
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        string gvSortExpr = String.Empty;
        private string gvSortDir
        {

            get { return ViewState["SortDirection"] as string ?? "ASC"; }

            set { ViewState["SortDirection"] = value; }

        }
        #endregion

        //This procedure returns the Sort Direction
        

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.INDEX;;
            Master.SubMenu = "";
            Master.Heading = "Customer MTAs";
            Master.HelpContent = "";
            Master.HelpTitle = "Customer MTA Setup";	

            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                userID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
                if(!IsPostBack)
                {
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                }
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        #region GridView1 Event Handlers
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }

        //This event occurs for each row
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            
            // Make sure we aren't in header/footer rows
            if (row.DataItem == null)
            {
                return;
            }

            //Find Child GridView control

            //ip
            GridView gv = new GridView();
            gv = (GridView)row.FindControl("gvIP");

            //Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
            if (gv.UniqueID == gvUniqueID)
            {
                gv.PageIndex = gvNewPageIndex;
                gv.EditIndex = gvEditIndex;
                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["MTAID"].ToString() + "','one');</script>");
            }

            //Prepare the query for Child GridView by passing the Customer ID of the parent row
            gv.DataSource = GetIPDT(((DataRowView)e.Row.DataItem)["MTAID"].ToString());
            gv.DataBind();

            //customer
            gv = new GridView();
            gv = (GridView)row.FindControl("gvCustomer");

            //Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
            if (gv.UniqueID == gvUniqueID)
            {
                gv.PageIndex = gvNewPageIndex;
                gv.EditIndex = gvEditIndex;
                //Check if Sorting used
                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["MTAID"].ToString() + "','one');</script>");
            }

            //Prepare the query for Child GridView by passing the Customer ID of the parent row
            gv.DataSource = GetCustomerDT(((DataRowView)e.Row.DataItem)["MTAID"].ToString());
            gv.DataBind();

        }

        //This event occurs for any operation on the row of the grid
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int mtaID = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "Edit")
            {
                SetupAddMTA(mtaID);
                pnlEMessage.Visible = false;
                lblEMessage.Text = "";
                mpeMTA.Show();
            }
            else if (e.CommandName == "Delete")
            {
                DeleteMTA(mtaID);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                GridView1.PageIndex = e.NewPageIndex;
            }
            GridView1.DataSource = GetParentDT();
            GridView1.DataBind();
        }
        #endregion        

        #region gvIP Event Handlers
        protected void gvIP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ipID = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "Edit")
            {
                SetupAddMTAIP(ipID);
                pnlEMessage2.Visible = false;
                lblEMessage2.Text = "";
                mpeMTAIP.Show();
            }
            else if (e.CommandName == "Delete")
            {
                DeleteMTAIP(ipID);
            }
        }

        protected void gvIP_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void gvIP_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
        #endregion

        #region gvCustomer Event Handlers
        protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] argsArray = e.CommandArgument.ToString().Split(','); 
            int mtaID = Convert.ToInt32(argsArray[0]);
            int customerID = Convert.ToInt32(argsArray[1]);
            if (e.CommandName == "Edit")
            {
                SetupAddMTACustomer(mtaID, customerID);
                pnlEMessage3.Visible = false;
                lblEMessage3.Text = "";
                mpeMTACustomer.Show();
            }
            else if (e.CommandName == "Delete")
            {
                DeleteMTACustomer(mtaID, customerID);
            }
        }

        protected void gvCustomer_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void gvCustomer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
        #endregion

        #region private methods
        private string GetSortDirection()
        {
            switch (gvSortDir)
            {
                case "ASC":
                    gvSortDir = "DESC";
                    break;

                case "DESC":
                    gvSortDir = "ASC";
                    break;
            }
            return gvSortDir;
        }

        private DataTable GetIPDT(string mtaID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlSelect = "SELECT MTAID, IPID, IPAddress, HostName FROM MTAIP" +
                                    " WHERE MTAID = " + mtaID;

            cmd.CommandText = sqlSelect;
            return DataFunctions.GetDataTable(cmd);
        }

        private DataTable GetCustomerDT(string mtaID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlSelect = "SELECT m.MTAID, m.CustomerID, c.CustomerName, m.IsDefault FROM MTACustomer m JOIN ecn5_accounts..Customer c on m.CustomerID = c.CustomerID " +
                                    " WHERE m.MTAID = " + mtaID;

            cmd.CommandText = sqlSelect;
            return DataFunctions.GetDataTable(cmd);
        }

        private DataTable GetParentDT()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlSelect = "SELECT distinct m.MTAID, m.MTAName, m.DomainName, bc.MTAName as ConfigName FROM MTA m join BlastConfig bc on m.BlastConfigID = bc.BlastConfigID";// +
            //    "LEFT OUTER JOIN MTACustomer mc on m.MTAID = mc.MTAID " +
            //    "LEFT OUTER JOIN MTAIP ip on m.MTAID = ip.MTAID " +
            //    "JOIN ecn5_accounts..Customer c on mc.CustomerID = c.CustomerID " +
            //"WHERE " +
            //    "c.CustomerName like '%@CustomerName%' AND " +
            //    "ip.IPAddress like '%@IPAddress%' AND " +
            //    "ip.HostName like '%@HostName%' AND " +
            //    "m.MTAName like '%@MTAName%' AND " +
            //    "m.DomainName like '%@DomainName%' " +
            //"ORDER BY  " +
            //    "m.MTAName";

            string customerName = txtSearchCustomer.Text.Trim();
            string mtaName = txtSearchMTA.Text.Trim();
            string domainName = txtSearchDomain.Text.Trim();
            string ip = txtSearchIP.Text.Trim();
            string hostName = txtSearchHost.Text.Trim();

            string where = "";
            if (customerName.Length > 0)
            {
                sqlSelect += " LEFT OUTER JOIN MTACustomer mc on m.MTAID = mc.MTAID JOIN ecn5_accounts..Customer c on mc.CustomerID = c.CustomerID";
                cmd.Parameters.AddWithValue("@CustomerName", "%" + customerName + "%");
                where = " c.CustomerName like @CustomerName";
            }
            if (ip.Length > 0 || hostName.Length > 0)
            {
                sqlSelect += " LEFT OUTER JOIN MTAIP ip on m.MTAID = ip.MTAID";
                if (ip.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@IPAddress", "%" + ip + "%");
                    if (where.Length > 0)
                    {
                        where += " and";
                    }
                    where += " ip.IPAddress like @IPAddress";
                }
                if (hostName.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@HostName", "%" + hostName + "%");
                    if (where.Length > 0)
                    {
                        where += " and";
                    }
                    where += " ip.HostName like @HostName";
                }
            }
            if (mtaName.Length > 0)
            {
                cmd.Parameters.AddWithValue("@MTAName", "%" + mtaName + "%");
                if (where.Length > 0)
                {
                    where += " and";
                }
                where += " m.MTAName like @MTAName";
            }
            if (domainName.Length > 0)
            {
                cmd.Parameters.AddWithValue("@DomainName", "%" + domainName + "%");
                if (where.Length > 0)
                {
                    where += " and";
                }
                where += " m.DomainName like @DomainName";
            }
            if (where.Length > 0)
            {
                sqlSelect += " where" + where + " ORDER BY  m.MTAName";
            }
            else
            {
                sqlSelect += " ORDER BY  m.MTAName";
            }
            cmd.CommandText = sqlSelect;
            return DataFunctions.GetDataTable(cmd);
        }

        private int CheckForDefaultMTA(int customerID)
        {
            int defaultMTAID = 0;
            try
            {
                string selectSQL = "select mtaid from mtacustomer where customerid = @CustomerID and IsDefault = 1";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = selectSQL;
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        defaultMTAID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
            }
            catch (Exception)
            {
            }

            return defaultMTAID;
        }

        private DataTable GetCustomersForMTA(int mtaID)
        {
            DataTable dt = null;
            try
            {
                string selectSQL = "select customerID from MTACustomer where mtaid = @MTAID";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = selectSQL;
                cmd.Parameters.AddWithValue("@MTAID", mtaID);
                dt = DataFunctions.GetDataTable(cmd);
            }
            catch (Exception)
            {
            }
            return dt;
        }

        private void SetupAddMTA(int? mtaID)
        {

            ddlBlastConfig.Items.Clear();
            ddlBlastConfig.DataSource = GetConfigList();
            ddlBlastConfig.DataTextField = "ConfigName";
            ddlBlastConfig.DataValueField = "ConfigID";
            ddlBlastConfig.DataBind();

            if (mtaID == null)
            {
                btnSaveMTA.Text = "Add";

                lblMTAID.Text = "";
                txtMTAName.Text = "";
                txtDomainName.Text = "";

                ddlBlastConfig.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlBlastConfig.Items.FindByValue("").Selected = true;
                ddlBlastConfig.Enabled = true;
            }
            else
            {
                btnSaveMTA.Text = "Update";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from MTA where MTAID = @MTAID";
                cmd.Parameters.AddWithValue("@MTAID", mtaID);
                DataTable dtMTA = DataFunctions.GetDataTable(cmd);
                lblMTAID.Text = Convert.ToInt32(mtaID).ToString();
                txtMTAName.Text = dtMTA.Rows[0]["MTAName"].ToString();
                txtDomainName.Text = dtMTA.Rows[0]["DomainName"].ToString();
                ddlBlastConfig.Items.FindByValue(dtMTA.Rows[0]["BlastConfigID"].ToString()).Selected = true;
                ddlBlastConfig.Enabled = false;
            }

        }

        private void DeleteMTAIP(int ipID)
        {            
            string sqlquery = "delete MTAIP where IPID = " + ipID;
            DataFunctions.Execute(sqlquery);
            GridView1.DataSource = GetParentDT();
            GridView1.DataBind();
        }

        private void DeleteMTACustomer(int mtaID, int customerID)
        {
            string sqlquery = "delete MTACustomer where MTAID = " + mtaID + " and CustomerID = " + customerID;
            DataFunctions.Execute(sqlquery);
            
            //make sure there is still a default for the customer if any customer-mta still exists
            DataTable dt = GetMTAListForCustomer(customerID);
            if (dt != null && dt.Rows.Count > 0)
            {
                bool found = false;
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToBoolean(row["IsDefault"].ToString()))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    string updateSQL = "update MTACustomer set IsDefault = @IsDefault where MTAID = @MTAID and CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = updateSQL;
                    cmd.Parameters.AddWithValue("@MTAID", Convert.ToInt32(dt.Rows[0]["MTAID"].ToString()));
                    cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dt.Rows[0]["CustomerID"].ToString()));
                    cmd.Parameters.AddWithValue("@IsDefault", "true");
                    DataFunctions.Execute(cmd);
                }
            }
            GridView1.DataSource = GetParentDT();
            GridView1.DataBind();
        }

        private void DeleteMTA(int mtaID)
        {
            DataTable dtCustomers = GetCustomersForMTA(mtaID);
            foreach (DataRow row in dtCustomers.Rows)
            {
                DeleteMTACustomer(mtaID, Convert.ToInt32(row["CustomerID"].ToString()));
            }
            string sqlquery = "delete MTAIP where MTAID = " + mtaID + ";delete MTA where MTAID = " + mtaID;
            DataFunctions.Execute(sqlquery);
            GridView1.DataSource = GetParentDT();
            GridView1.DataBind();
        }

        private void SetupAddMTAIP(int? ipID)
        {
            ddlMTA.Items.Clear();
            ddlMTA.DataSource = GetMTAList();
            ddlMTA.DataTextField = "MTAName";
            ddlMTA.DataValueField = "MTAID";
            ddlMTA.DataBind();

            if (ipID == null)
            {
                btnSaveMTAIP.Text = "Add";

                ddlMTA.Items.Insert(0, new ListItem(String.Empty, String.Empty));            
                ddlMTA.Items.FindByValue("").Selected = true;
                txtIPAddress.Text = "";
                txtHostName.Text = "";
            }
            else
            {
                btnSaveMTAIP.Text = "Update";                

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from MTAIP where IPID = @IPID";
                cmd.Parameters.AddWithValue("@ipID", ipID);
                DataTable ddlMTAIP = DataFunctions.GetDataTable(cmd);
                lblIPID.Text = Convert.ToInt32(ipID).ToString();
                ddlMTA.Items.FindByValue(ddlMTAIP.Rows[0]["MTAID"].ToString()).Selected = true;
                txtIPAddress.Text = ddlMTAIP.Rows[0]["IPAddress"].ToString();
                txtHostName.Text = ddlMTAIP.Rows[0]["HostName"].ToString();
            }

        }

        private void SetupAddMTACustomer(int? mtaID, int? customerID)
        {
            ddlMTACustomer.Items.Clear();
            ddlMTACustomer.DataSource = GetMTAList();
            ddlMTACustomer.DataTextField = "MTAName";
            ddlMTACustomer.DataValueField = "MTAID";
            ddlMTACustomer.DataBind();

            ddlCustomer.Items.Clear();
            ddlCustomer.DataSource = GetCustomerList();
            ddlCustomer.DataTextField = "CustomerName";
            ddlCustomer.DataValueField = "CustomerID";
            ddlCustomer.DataBind();

            ddlIsDefault.Items.Clear();
            ddlIsDefault.Items.Add(new ListItem("YES", "1"));
            ddlIsDefault.Items.Add(new ListItem("NO", "0"));

            if (mtaID == null || customerID == null)
            {
                btnSaveMTACustomer.Text = "Add";

                ddlMTACustomer.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlMTACustomer.Items.FindByValue("").Selected = true;
                ddlCustomer.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlCustomer.Items.FindByValue("").Selected = true;
                ddlIsDefault.Items.FindByValue("1").Selected = true;
                ddlMTACustomer.Enabled = true;
                ddlCustomer.Enabled = true;
            }
            else
            {
                btnSaveMTACustomer.Text = "Update";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from MTACustomer where MTAID = @MTAID and CustomerID = @CustomerID";
                cmd.Parameters.AddWithValue("@MTAID", mtaID);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                DataTable dtMTACustomer = DataFunctions.GetDataTable(cmd);
                lblCustomerMTAID.Text = Convert.ToInt32(mtaID).ToString();
                lblCustomerID.Text = Convert.ToInt32(customerID).ToString();
                ddlMTACustomer.Items.FindByValue(dtMTACustomer.Rows[0]["MTAID"].ToString()).Selected = true;
                ddlCustomer.Items.FindByValue(dtMTACustomer.Rows[0]["CustomerID"].ToString()).Selected = true;
                if (Convert.ToBoolean(dtMTACustomer.Rows[0]["IsDefault"].ToString()))
                {
                    ddlIsDefault.Items.FindByValue("1").Selected = true;
                }
                else
                {
                    ddlIsDefault.Items.FindByValue("0").Selected = true;
                }
                ddlMTACustomer.Enabled = false;
                ddlCustomer.Enabled = false;
            }

        }

        private DataTable GetCustomerList()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select CustomerID, CustomerName from ecn5_Accounts..Customer order by CustomerName ASC";
            return DataFunctions.GetDataTable(cmd);
        }

        private DataTable GetConfigList()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select BlastConfigID as ConfigID, MTAName as ConfigName from BlastConfig where BlastConfigID in (1,3) order by ConfigName ASC";
            return DataFunctions.GetDataTable(cmd);
        }

        private DataTable GetMTAList()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select MTAID, MTAName from MTA order by MTAName ASC";
            return DataFunctions.GetDataTable(cmd);
        }

        private DataTable GetMTAListForCustomer(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from MTACustomer where CustomerID = @CustomerID order by IsDefault ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd);
        }

        private void AddMTA()
        {
            if (btnSaveMTA.Text == "Add")
            {
                string insertSQL = "insert into MTA (MTAName, DomainName, BlastConfigID) values (@MTAName, @DomainName, @BlastConfigID)";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertSQL;
                cmd.Parameters.AddWithValue("@MTAName", txtMTAName.Text);
                cmd.Parameters.AddWithValue("@DomainName", txtDomainName.Text);
                cmd.Parameters.AddWithValue("@BlastConfigID", ddlBlastConfig.SelectedValue);

                try
                {
                    DataFunctions.Execute(cmd);
                    btnSaveMTA.Text = "Add";
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                    mpeMTA.Hide();
                }
                catch (Exception ex)
                {
                    lblEMessage.Text = ex.ToString();
                    pnlEMessage.Visible = true;
                    mpeMTA.Show();
                }
            }
            else
            {
                string updateSQL = "update MTA set MTAName = @MTAName, DomainName = @DomainName, BlastConfigID = @BlastConfigID where MTAID = @MTAID";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = updateSQL;
                cmd.Parameters.AddWithValue("@MTAID", Convert.ToInt32(lblMTAID.Text));
                cmd.Parameters.AddWithValue("@MTAName", txtMTAName.Text);
                cmd.Parameters.AddWithValue("@DomainName", txtDomainName.Text);
                cmd.Parameters.AddWithValue("@BlastConfigID", ddlBlastConfig.SelectedValue);

                try
                {
                    DataFunctions.Execute(cmd);
                    btnSaveMTA.Text = "Add";
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                    mpeMTA.Hide();
                }
                catch (Exception ex)
                {
                    lblEMessage.Text = ex.ToString();
                    pnlEMessage.Visible = true;
                    mpeMTA.Show();
                }
            }
        }

        private void AddMTAIP()
        {
            if (btnSaveMTAIP.Text == "Add")
            {
                string insertSQL = "insert into MTAIP (MTAID, IPAddress, HostName) values (@MTAID, @IPAddress, @HostName)";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertSQL;
                cmd.Parameters.AddWithValue("@MTAID", ddlMTA.SelectedValue);
                cmd.Parameters.AddWithValue("@IPAddress", txtIPAddress.Text);
                cmd.Parameters.AddWithValue("@HostName", txtHostName.Text);

                try
                {
                    DataFunctions.Execute(cmd);
                    btnSaveMTAIP.Text = "Add";
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                    mpeMTAIP.Hide();
                }
                catch (Exception ex)
                {
                    lblEMessage2.Text = ex.ToString();
                    pnlEMessage2.Visible = true;
                    mpeMTAIP.Show();
                }
            }
            else
            {
                string updateSQL = "update MTAIP set MTAID = @MTAID, IPAddress = @IPAddress, HostName = @HostName where IPID = @IPID";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = updateSQL;
                cmd.Parameters.AddWithValue("@IPID", Convert.ToInt32(lblIPID.Text));
                cmd.Parameters.AddWithValue("@MTAID", ddlMTA.SelectedValue);
                cmd.Parameters.AddWithValue("@IPAddress", txtIPAddress.Text);
                cmd.Parameters.AddWithValue("@HostName", txtHostName.Text);

                try
                {
                    DataFunctions.Execute(cmd);
                    btnSaveMTAIP.Text = "Add";
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                    mpeMTAIP.Hide();
                }
                catch (Exception ex)
                {
                    lblEMessage2.Text = ex.ToString();
                    pnlEMessage2.Visible = true;
                    mpeMTAIP.Show();
                }
            }
        }

        private void UpdateDefault(int customerID, int mtaID, string isDefault)
        {
            string updateSQL = "update MTACustomer set IsDefault = @IsDefault where MTAID = @MTAID and CustomerID = @CustomerID";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = updateSQL;
            cmd.Parameters.AddWithValue("@MTAID", mtaID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@IsDefault", isDefault);
            DataFunctions.Execute(cmd);
        }

        private void AddMTACustomer()
        {
            int currDefaultMTAID = CheckForDefaultMTA(Convert.ToInt32(ddlCustomer.SelectedValue));
            bool resetDefault = false;
            if (btnSaveMTACustomer.Text == "Add")
            {
                string insertSQL = "insert into MTACustomer (MTAID, CustomerID, IsDefault) values (@MTAID, @CustomerID, @IsDefault)";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertSQL;
                cmd.Parameters.AddWithValue("@MTAID", ddlMTACustomer.SelectedValue);
                cmd.Parameters.AddWithValue("@CustomerID", ddlCustomer.SelectedValue);
                if (ddlIsDefault.SelectedValue == "1" || currDefaultMTAID == 0 || (lblCustomerMTAID.Text.Length > 0 && (currDefaultMTAID == Convert.ToInt32(lblCustomerMTAID.Text))))
                {
                    cmd.Parameters.AddWithValue("@IsDefault", "true");
                    if (currDefaultMTAID > 0)
                    {
                        resetDefault = true;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IsDefault", "false");
                }

                try
                {
                    DataFunctions.Execute(cmd);
                    if (resetDefault)
                    {
                        UpdateDefault(Convert.ToInt32(ddlCustomer.SelectedValue), currDefaultMTAID, "false");
                    }
                    btnSaveMTACustomer.Text = "Add";
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                    mpeMTACustomer.Hide();
                }
                catch (Exception ex)
                {
                    lblEMessage3.Text = ex.ToString();
                    pnlEMessage3.Visible = true;
                    mpeMTACustomer.Show();
                }
            }
            else
            {
                try
                {
                    if (ddlIsDefault.SelectedValue == "1" || currDefaultMTAID == 0 || (lblCustomerMTAID.Text.Length > 0 && (currDefaultMTAID == Convert.ToInt32(lblCustomerMTAID.Text))))
                    {
                        UpdateDefault(Convert.ToInt32(lblCustomerID.Text), Convert.ToInt32(lblCustomerMTAID.Text), "true");
                        if (currDefaultMTAID > 0 && currDefaultMTAID != Convert.ToInt32(lblCustomerMTAID.Text))
                        {
                            resetDefault = true;
                        }
                    }
                    else
                    {
                        UpdateDefault(Convert.ToInt32(lblCustomerID.Text), Convert.ToInt32(lblCustomerMTAID.Text), "false");
                    }
                    if (resetDefault)
                    {
                        UpdateDefault(Convert.ToInt32(lblCustomerID.Text), currDefaultMTAID, "false");
                    }
                    btnSaveMTACustomer.Text = "Add";
                    GridView1.DataSource = GetParentDT();
                    GridView1.DataBind();
                    mpeMTACustomer.Hide();
                }
                catch (Exception ex)
                {
                    lblEMessage3.Text = ex.ToString();
                    pnlEMessage3.Visible = true;
                    mpeMTACustomer.Show();
                }
            }
        }
        #endregion

        #region page events
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = GetParentDT();
            GridView1.PageIndex = 0;
            GridView1.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchCustomer.Text = "";
            txtSearchMTA.Text = "";
            txtSearchDomain.Text = "";
            txtSearchIP.Text = "";
            txtSearchHost.Text = "";
            GridView1.DataSource = GetParentDT();
            GridView1.PageIndex = 0;
            GridView1.DataBind();
        }

        protected void btnSaveMTA_Click(object sender, EventArgs e)
        {
            AddMTA();            
        }

        protected void btnSaveMTAIP_Click(object sender, EventArgs e)
        {
            AddMTAIP();
        }

        protected void btnSaveMTACustomer_Click(object sender, EventArgs e)
        {
            AddMTACustomer();
        }

        protected void btnAddMTA_Click(object sender, EventArgs e)
        {
            SetupAddMTA(null);
            pnlEMessage.Visible = false;
            lblEMessage.Text = "";
            mpeMTA.Show();
        }

        protected void btnAddMTAIP_Click(object sender, EventArgs e)
        {
            SetupAddMTAIP(null);
            pnlEMessage2.Visible = false;
            lblEMessage2.Text = "";
            mpeMTAIP.Show();
        }

        protected void btnAddMTACustomer_Click(object sender, EventArgs e)
        {
            SetupAddMTACustomer(null, null);
            pnlEMessage3.Visible = false;
            lblEMessage3.Text = "";
            mpeMTACustomer.Show();
        }
        #endregion
    }
}
