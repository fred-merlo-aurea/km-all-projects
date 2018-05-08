using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Data.SqlClient;
using System.Configuration;

namespace ecn.communicator.blastsmanager
{
    public partial class blasts_main : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect("../ecnwizard/");
        }
        //THIS PAGE WILL NO LONGER BE USED
        /*
        protected System.Web.UI.WebControls.HyperLinkColumn ActiveGridStatusLink;
        protected System.Web.UI.WebControls.HyperLinkColumn ActiveGridCancelLink;
        protected BWare.UI.Web.WebControls.DataPanel DataPanel1;
        protected BWare.UI.Web.WebControls.DataPanel Datapanel2;
        protected BWare.UI.Web.WebControls.DataPanel Datapanel3;

        int ActiveRecordCount = 0;
        int SentRecordCount = 0;
        int ScheduledRecordCount = 0;
        string action = "";
        string blastID = "0";
        string userID = "0";
        string customerID = "0";
        bool CustomerHasUserDepartments = false;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Master.Menu = "blasts";
            //Master.SubMenu="blast status";
            Master.Heading = "Emails and Reports";

            ErrorLabel.Text = "";
            ErrorLabel.Visible = false;

            if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv))
            {
                userID = Master.UserSession.CurrentUser.UserID.ToString();
                customerID = Master.UserSession.CurrentUser.CustomerID.ToString();

                CustomerHasUserDepartments = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures. "Users Departments");

                try
                {
                    action = Request.QueryString["action"].ToString();
                }
                catch (Exception)
                {
                    action = "";
                }
                try
                {
                    blastID = Request.QueryString["blastID"].ToString();
                }
                catch (Exception)
                {
                    blastID = "0";
                }

                if (Page.IsPostBack == false)
                {
                    txtFrom.Text = DateTime.Now.AddDays(-90).ToShortDateString();
                    txtTo.Text = DateTime.Now.ToShortDateString();
                    if ((action.Equals("delete")) && (Convert.ToInt32(blastID) > 0))
                    {
                        DeleteBlast(blastID);
                    }
                    loadBlastCatsDR(customerID);
                    LoadUserDD("%");

                    ViewState["activeSortField"] = "SendTime";
                    ViewState["activeSortDirection"] = "DESC";

                    ViewState["sentSortField"] = "SendTime";
                    ViewState["sentSortDirection"] = "DESC";

                    ViewState["scheduledSortField"] = "SendTime";
                    ViewState["scheduledSortDirection"] = "DESC";
                    LoadGrids();
                }

            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private void loadBlastCatsDR(String CustomerID)
        {
            BlastCats.DataSource = DataLists.GetCommCodesDR("BlastCategory", CustomerID);
            BlastCats.DataBind();
            BlastCats.Items.Insert(0, new ListItem("All", "-1"));
            if (BlastCats.Items.Count > 0)
            {
                BlastCats.Items.Insert(0, new ListItem("--Generic Type--", "0"));
                BlastCats.Visible = true;
            }
            else
            {
                BlastCats.Visible = false;
            }
        }

        private void loadActiveGrid(String CustomerID)
        {
            string sqlquery =
                //" SELECT BlastID, EmailSubject, SendTime FROM Blasts ";
            " SELECT BlastID, '['+CONVERT(VARCHAR(10),BlastID) +'] '+EmailSubject EmailSubject, SendTime, BlastType, GroupName, LayoutName, (CASE WHEN b.FilterID = 2147483644 THEN 'Suppressed for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483647 THEN 'UnClicked for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483645 THEN 'UnOpened for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483643 THEN 'Opened for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483642 THEN 'Clicked for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN isnull(b.FilterID,0) <= 0 THEN '< NO FILTER >' WHEN f.FilterID <> 0 THEN f.FilterName ELSE '< FILTER DELETED >' END) FilterName FROM Blasts b LEFT OUTER JOIN Filters f ON b.filterID = f.filterID LEFT OUTER JOIN Groups g ON b.GroupID = g.GroupID JOIN Layouts l on b.LayoutiD = l.LayoutID";
            if (CustomerHasUserDepartments)
            {
                sqlquery += " JOIN DeptItemReferences dr ON Blasts.BlastID = dr.ItemID AND dr.Item = 'BLST' ";
            }
            sqlquery += " WHERE b.CustomerID=" + CustomerID +
                " AND StatusCode='active' " +
                " AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser(" + CustomerID + "," + userID + ")) " +
                " ORDER BY SendTime ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery);

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["activeSortField"].ToString() + ' ' + ViewState["activeSortDirection"].ToString();

            ActiveRecordCount = dt.Rows.Count;
            ActiveGrid.DataSource = dv;

            try
            {
                ActiveGrid.DataBind();
            }
            catch
            {
                ActiveGrid.PageIndex = 0;
                ActiveGrid.DataBind();
            }

            ActiveGrid.ShowEmptyTable = true;
            ActiveGrid.EmptyTableRowText = "No active blasts to display";

            //if (dt.Rows.Count > 0)
            //{
            //    ActivePanel.Visible = true;
            //}
        }

        private void loadScheduledGrid(String CustomerID)
        {
            string sqlquery =
                " SELECT BlastID, '['+CONVERT(VARCHAR(10),BlastID) +'] '+EmailSubject EmailSubject, SendTime, BlastType, GroupName, LayoutName, CASE WHEN ISNULL(bs.Period,'') in ('d','w','m') THEN 'y' ELSE 'n' END as IsRecurring, (CASE  WHEN b.FilterID = 2147483644 THEN 'Suppressed for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483647 THEN 'UnClicked for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483645 THEN 'UnOpened for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483643 THEN 'Opened for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN b.FilterID = 2147483642 THEN 'Clicked for BlastID [' + Convert(varchar,b.refblastID) + ']' WHEN isnull(b.FilterID,0) <= 0 THEN '< NO FILTER >' WHEN f.FilterID <> 0 THEN f.FilterName ELSE '< FILTER DELETED >' END) FilterName FROM Blasts b LEFT OUTER JOIN Filters f ON b.filterID = f.filterID LEFT OUTER JOIN Groups g ON b.GroupID = g.GroupID JOIN Layouts l on b.LayoutiD = l.LayoutID LEFT OUTER JOIN BlastSchedule bs on b.BlastScheduleID = bs.BlastScheduleID";
            if (CustomerHasUserDepartments)
            {
                sqlquery += " JOIN DeptItemReferences dr ON Blasts.BlastID = dr.ItemID AND dr.Item = 'BLST' ";
            }
            sqlquery += " WHERE b.CustomerID=" + CustomerID +
                " AND StatusCode='pending' AND (BlastType='html' OR BlastType='sample' OR BlastType='champion')" +
                " AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser(" + CustomerID + "," + userID + ")) " +
                " ORDER BY SendTime ";

            DataTable dt = DataFunctions.GetDataTable(sqlquery);

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["scheduledSortField"].ToString() + ' ' + ViewState["scheduledSortDirection"].ToString();

            ScheduledRecordCount = dt.Rows.Count;
            ScheduledGrid.DataSource = dv;

            try
            {
                ScheduledGrid.DataBind();
            }
            catch
            {
                ScheduledGrid.PageIndex = 0;
                ScheduledGrid.DataBind();
            }

            ScheduledGrid.ShowEmptyTable = true;
            ScheduledGrid.EmptyTableRowText = "No scheduled blasts to display";


        }

        public void SentGrid_ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblIsBlastGroup = (Label)e.Item.FindControl("lblIsBlastGroup");

                if (lblIsBlastGroup.Text.ToLower() == "y")
                {
                    e.Item.BackColor = System.Drawing.Color.Silver;
                    e.Item.Cells[1].Text = "Multi-Group Blast";
                    e.Item.Cells[8].Text = "";
                }
                else
                {
                    LinkButton deleteBtn = e.Item.FindControl("DeleteBlastBtn") as LinkButton;
                    if (e.Item.Cells[9].Text.ToString().ToUpper().Equals("N"))
                    {
                        if ((Convert.ToDateTime(e.Item.Cells[4].Text.ToString())) < DateTime.Now.AddDays(-7))
                        {
                            deleteBtn.Attributes.Add("onclick", "return confirm('Blast titled \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" and its associated Reporting will be DELETED !!" + "\\n" + "\\n" + "Are you sure you want to contine? This process CANNOT be undone.');");
                        }
                        else
                        {
                            deleteBtn.Enabled = false;
                            deleteBtn.Attributes.Add("style", "cursor:hand;padding:0;margin:0;");
                            deleteBtn.Attributes.Add("onclick", "alert('Blast titled \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" CANNOT be deleted because it was sent within 7 Days of current time." + "\\n" + "If you need further assistance please call your Account Manager.');");
                        }
                    }
                    else
                    {
                        deleteBtn.Attributes.Add("onclick", "return confirm('Blast titled \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" and its associated Reporting will be DELETED !!" + "\\n" + "\\n" + "Are you sure you want to contine? This process CANNOT be undone.');");
                    }
                }
            }
            return;
        }

        public void SentGrid_Command(Object sender, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "DELETE")
                {
                    int blastID = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteBlast(blastID.ToString());
                    LoadGrids();
                }
                else if (e.CommandName.ToUpper() == "RESEND")
                {
                    int blastID = Convert.ToInt32(e.CommandArgument.ToString());
                    ResendBlast(blastID.ToString());
                    LoadGrids();
                }
            }
            catch { }
        }

        private void loadSentGrid(String CustomerID)
        {
            bool validRequest = true;
            if (txtFrom.Text.Length > 0 || txtTo.Text.Length > 0)
            {
                try
                {
                    DateTime fromDate = Convert.ToDateTime(txtFrom.Text);
                    DateTime toDate = Convert.ToDateTime(txtTo.Text);
                    TimeSpan ts = toDate - fromDate;
                    if (ts.Days < 0 || ts.Days > 365)
                    {
                        ErrorLabel.Text = "Please enter date range where the range is no greater than 365 days";
                        ErrorLabel.Visible = true;
                        validRequest = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = ex.Message;
                    ErrorLabel.Visible = true;
                    validRequest = false;
                }
            }

            if (validRequest)
            {
                string blastType = BlastTypeDD.SelectedValue.ToString();
                int blastCat = 0;

                try
                {
                    blastCat = Convert.ToInt32(BlastCats.SelectedValue.ToString());
                }
                catch
                {
                    blastCat = 0;
                }

                //string sqlquery=
                //    //" SELECT "+count+" b.EmailSubject,b.SendTime, b.BlastType, b.BlastID, (SELECT COUNT(emailID) FROM EmailActivityLog WHERE BlastID = b.BlastID AND (ActionTypeCode = 'send' OR ActionTypeCode = 'testsend')) AS sendTotal,  f.FilterName, g.GroupName"+
                //    " SELECT "+ //b.EmailSubject,b.SendTime, b.BlastType, b.BlastID, SendTotal,  f.FilterName, g.GroupName, b.TestBlast"+
                //    " CASE WHEN blastgroupID IS NULL THEN blastID ELSE blastgroupID END AS ID,  CASE WHEN blastgroupID IS NULL THEN 'N' ELSE 'Y' END AS IsBlastGroup, " +
                //    "   MAX(CASE WHEN blastgroupID IS NULL THEN '['+CONVERT(VARCHAR(10),BlastID) +'] '+ b.EmailSubject ELSE bg.emailsubject END ) AS emailsubject, " + 
                //    "   MAX(b.SendTime) AS sendtime, " + 
                //    "   MAX(b.BlastType) AS blasttype, " + 
                //    "   Sum(SendTotal) AS sendtotal, " + 
                //    "   MAX(CASE "+
                //    "           WHEN blastgroupID IS NULL THEN ( "+
                //    "           (CASE "+
                //    "               WHEN b.FilterID = 2147483647 THEN 'UnClicked for BlastID [' + Convert(varchar,b.refblastID) + ']' "+
                //    "               WHEN b.FilterID = 2147483645 THEN 'UnOpened for BlastID [' + Convert(varchar,b.refblastID) + ']' "+
                //    "               WHEN ISNULL(b.FilterID,0) <= 0 THEN '< NO FILTER >' WHEN f.FilterID <> 0 THEN f.FilterName ELSE '< FILTER DELETED >' END "+
                //    "           ) "+
                //    "       ) ELSE '' END ) AS Filtername, " +
                //    "   MAX(CASE WHEN blastgroupID IS NULL THEN g.GroupName ELSE '' END) AS groupname,  " +
                //    "   MAX(b.TestBlast) as testblast, max(BlastID) AS bID " +
                //    " FROM Blasts b " +
                //    " LEFT OUTER JOIN blastgrouping bg ON bg.blastIDs LIKE '%' + CONVERT(VARCHAR,b.blastID) + '%'"; 
                //if (SentUserID.SelectedItem.Value != "*")
                //{
                //    sqlquery += " AND bg.userID = " + SentUserID.SelectedItem.Value;
                //}
                //sqlquery += " LEFT OUTER JOIN Filters f ON b.filterID = f.filterID "+
                //    " LEFT OUTER JOIN Groups g ON b.GroupID = g.GroupID ";
                //if(CustomerHasUserDepartments){
                //    sqlquery += " JOIN DeptItemReferences dr ON b.BlastID = dr.ItemID AND dr.Item = 'BLST' ";
                //}
                //sqlquery +=	" WHERE b.CustomerID="+CustomerID+
                //    " AND (b.GroupID in (select GroupID from dbo.fn_getGroupsforUser("+CustomerID+","+ userID +")) OR g.GroupID IS NULL)" + 
                //    " AND b.StatusCode='sent'";
                //if (BlastCats.SelectedItem.Value != "-1")
                //{
                //    sqlquery += " AND CodeID = " + blastCat;
                //}
                //if (!(SentUserID.SelectedItem.Value.Equals("-1")))
                //{
                //    sqlquery += " AND b.UserID="+SentUserID.SelectedItem.Value;
                //}
                //if(CustomerHasUserDepartments){
                //    sqlquery += "AND dr.DepartmentID IN (SELECT DepartmentID FROM ecn5_accounts.dbo.UserDepartments WHERE UserID = "+userID+")";
                //}
                //sqlquery += " AND TestBlast = '"+blastType+"' ";
                //if (txtSubjectSearch.Text.Trim().Length > 0)
                //{
                //    sqlquery += " AND (b.EmailSubject like '%" + txtSubjectSearch.Text.Trim() + "%' OR bg.EmailSubject like '%" + txtSubjectSearch.Text.Trim() + "%') ";
                //}
                //if (txtGroupSearch.Text.Trim().Length > 0)
                //{
                //    sqlquery += " AND (GroupName like '%" + txtGroupSearch.Text.Trim() + "%') ";
                //}
                ////(b.SendTime > '9/8/2010' AND b.SendTime < '9/10/2010')
                //if (txtFrom.Text.Trim().Length > 0 && txtTo.Text.Trim().Length > 0)
                //{
                //    sqlquery += " AND (b.SendTime > '" + txtFrom.Text.Trim() + "' AND b.SendTime < '" + txtTo.Text.Trim() + "') ";
                //}

                //sqlquery += " GROUP BY CASE WHEN blastgroupID IS NULL THEN blastID ELSE blastgroupID END, CASE WHEN blastgroupID IS NULL THEN 'N' ELSE 'Y' END ";
                //sqlquery += " ORDER BY 4 DESC";

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connString"]);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("spGetBlastEmails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@BlastCat", SqlDbType.Int).Value = Convert.ToInt32(BlastCats.SelectedItem.Value);
                cmd.Parameters.Add("@BlastType", SqlDbType.Char).Value = blastType;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(userID);
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = Convert.ToInt32(CustomerID);
                cmd.Parameters.Add("@HasUserDepts", SqlDbType.Bit).Value = CustomerHasUserDepartments;
                cmd.Parameters.Add("@SubjectSearch", SqlDbType.VarChar).Value = txtSubjectSearch.Text.Trim();
                cmd.Parameters.Add("@GroupSearch", SqlDbType.VarChar).Value = txtGroupSearch.Text.Trim().Replace("'", "''");
                cmd.Parameters.Add("@FromDate", SqlDbType.VarChar).Value = Convert.ToDateTime(txtFrom.Text + " 00:00:00");
                cmd.Parameters.Add("@ToDate", SqlDbType.VarChar).Value = Convert.ToDateTime(txtTo.Text + " 23:59:59");
                cmd.Parameters.Add("@SentUserID", SqlDbType.Int).Value = Convert.ToInt32(SentUserID.SelectedItem.Value);
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                conn.Open();
                adapter.Fill(ds, "DataTable");
                conn.Close();
                DataTable dt = ds.Tables["DataTable"];

                DataView dv = dt.DefaultView;
                dv.Sort = ViewState["sentSortField"].ToString() + ' ' + ViewState["sentSortDirection"].ToString();

                SentRecordCount = dt.Rows.Count;
                //SentGrid.DataSource=dt.DefaultView;
                SentGrid.DataSource = dv;

                try
                {
                    SentGrid.DataBind();
                }
                catch
                {
                    SentGrid.PageIndex = 0;
                    SentGrid.DataBind();
                }

                SentGrid.ShowEmptyTable = true;
                SentGrid.EmptyTableRowText = "No sent blasts to display";
            }
        }

        public void loadAbGrid(string customer_id)
        {
            AbGrid.DataSource = ECN_Framework_BusinessLayer.Communicator.Blast.GetCustomerSamples(Convert.ToInt32(customer_id), Convert.ToInt32(SentUserID.SelectedItem.Value), Master.UserSession.CurrentUser);
            AbGrid.CurrentPageIndex = 0;
            AbGrid.DataBind();
        }

        private void LoadUserDD(string usermatch)
        {
            SentUserID.DataSource = DataLists.GetUsersDR(usermatch, customerID);
            SentUserID.DataBind();
            SentUserID.Items.Insert(0, new ListItem("All", "-1"));
            //SentUserID.Items.Insert(0,new ListItem("-- All Users --", "-1"));
            SentUserID.Items.FindByValue("-1").Selected = true;
            //SentUserID.Items.FindByValue(userID).Selected = true;
            if (SentUserID.Items.Count == 1)
            {
                SentUserID.Visible = false;
            }
        }

        private int getTestBlastCount()
        {
            int count = 0;
            try
            {
                count = Convert.ToInt32(ConfigurationManager.AppSettings["CU_" + customerID + "_TestBlastEmails"].ToString());
            }
            catch
            {
                try
                {
                    count = Convert.ToInt32(ConfigurationManager.AppSettings["CH_" + Master.UserSession.CurrentBaseChannel.BaseChannelID + "_TestBlastEmails"].ToString());
                }
                catch
                {
                    count = Convert.ToInt32(ConfigurationManager.AppSettings["BASE_TestBlastEmails"].ToString());
                }
            }

            return count;
        }

        public void ResendBlast(string theBlastID)
        {
            SqlCommand cmd = new SqlCommand("spDeleteEmailActivityByBlastID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@blastID", theBlastID);
            DataFunctions.Execute("activity", cmd);
            ECN_Framework.Communicator.Entity.Blast refBlast = ECN_Framework.Communicator.Entity.Blast.GetByBlastID(Convert.ToInt32(theBlastID));
            LicenseCheck lc = new LicenseCheck();
            int total = Convert.ToInt32(lc.BlastCheck(refBlast.CustomerID, refBlast.GroupID, refBlast.FilterID, refBlast.RefBlastID, refBlast.BlastSuppression));
            int canSend = getTestBlastCount();
            if (total <= canSend)
            {
                string sqlquery = "UPDATE Blasts set SendTime = '" + DateTime.Now.ToString() + "', StatusCode = 'pending', BlastEngineID = NULL, SendTotal = 0, AttemptTotal = 0, SuccessTotal = 0 WHERE StatusCode != 'pending' and BlastID = " + theBlastID;
                DataFunctions.Execute(sqlquery);
            }
            else
            {
                ErrorLabel.Text = "ERROR: The Group list selected for test blast, contains more than the allowed <i>" + canSend + "</i> emails for testing.<br>Use a Filter [or] choose a Group with <i>" + canSend + "</i> or less emails in it.";
                ErrorLabel.Visible = true;
            }

            //SqlCommand cmd = new SqlCommand("spDeleteEmailActivityByBlastID");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@blastID", theBlastID);
            //DataFunctions.Execute("activity", cmd);
            //string sqlquery = "UPDATE Blasts set SendTime = '" + DateTime.Now.ToString() + "', StatusCode = 'pending', BlastEngineID = NULL, SendTotal = 0, AttemptTotal = 0, SuccessTotal = 0 WHERE StatusCode != 'pending' and BlastID = " + theBlastID;
            //DataFunctions.Execute(sqlquery);
        }

        public void DeleteBlast(string theBlastID)
        {
            string sqlquery = "";
            sqlquery = " SELECT StatusCode FROM Blasts WHERE BlastID = " + theBlastID;
            string BlastStatus = DataFunctions.ExecuteScalar(sqlquery).ToString();

            sqlquery = " SELECT TestBlast FROM Blasts WHERE BlastID = " + theBlastID;
            string TestBlast = DataFunctions.ExecuteScalar(sqlquery).ToString();

            if (BlastStatus.ToLower() == "active")
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "showalert", "<script language='javascript'>onload=function(){alert('Email Blast is currently being processed and cannot be deleted!');};</script>");
            }
            else if (BlastStatus.ToLower() == "pending")
            {
                //sqlquery			=	"if exists (SELECT BlastID FROM Blasts WHERE BlastID = " + theBlastID+ " AND StatusCode = 'sent' AND SuccessTotal > 0) UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + theBlastID+ " ELSE delete from blasts where blastID = " + theBlastID;

                //We don't want to delete any blast record physically - ashok 10/05/07 after the incident with F51 customer.
                sqlquery = "if exists (SELECT BlastID FROM Blasts WHERE BlastID = " + theBlastID + " AND StatusCode = 'pending') UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + theBlastID + " ELSE UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + theBlastID;
                DataFunctions.Execute(sqlquery);

                SqlCommand cmd = new SqlCommand("spDeleteEmailActivityByBlastID");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@blastID", theBlastID);
                DataFunctions.Execute("activity", cmd);
                //sqlquery = " DELETE FROM EmailActivityLog  WHERE BlastID=" + theBlastID;
                //DataFunctions.Execute(sqlquery);

                sqlquery =
                    " DELETE FROM DeptItemReferences " +
                    " WHERE Item = 'BLST' AND ItemID=" + theBlastID;
                try
                {
                    DataFunctions.Execute(sqlquery);
                }
                catch { }
            }
            else
            {
                if (TestBlast.ToLower() == "n")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "showalert", "<script language='javascript'>onload=function(){alert('Email Blast has been sent and is not a Test Blast so cannot be deleted!');};</script>");
                }
                else
                {
                    //sqlquery	=	"if exists (SELECT BlastID FROM Blasts WHERE BlastID = " + theBlastID+ " AND StatusCode = 'sent' AND SuccessTotal > 0) UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + theBlastID+ " ELSE delete from blasts where blastID = " + theBlastID;
                    //We don't want to delete any blast record physically - ashok 10/05/07 after the incident with F51 customer.
                    sqlquery = "if exists (SELECT BlastID FROM Blasts WHERE BlastID = " + theBlastID + " AND StatusCode = 'sent' AND SuccessTotal > 0) UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + theBlastID + " ELSE UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + theBlastID;
                    DataFunctions.Execute(sqlquery);

                    SqlCommand cmd = new SqlCommand("spDeleteEmailActivityByBlastID");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@blastID", theBlastID);
                    DataFunctions.Execute("activity", cmd);
                    //sqlquery = " DELETE FROM EmailActivityLog  WHERE BlastID=" + theBlastID;
                    //DataFunctions.Execute(sqlquery);

                    sqlquery =
                        " DELETE FROM DeptItemReferences " +
                        " WHERE Item = 'BLST' AND ItemID=" + theBlastID;
                    try
                    {
                        DataFunctions.Execute(sqlquery);
                    }
                    catch { }
                }
            }
            //Response.Redirect("default.aspx");
        }
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            //SentGrid.RowDataBound +=new GridViewRowEventHandler(SentGrid_RowDataBound);
            //SentGrid.PageIndexChanging +=new GridViewPageEventHandler(SentGrid_PageIndexChanging);
            //SentGrid.RowDeleting +=new GridViewDeleteEventHandler(SentGrid_RowDeleting);
            //ScheduledGrid.RowDataBound +=new GridViewRowEventHandler(ScheduledGrid_RowDataBound);
            //ScheduledGrid.PageIndexChanging +=new GridViewPageEventHandler(ScheduledGrid_PageIndexChanging);
            //ActiveGrid.RowDataBound +=new GridViewRowEventHandler(ActiveGrid_RowDataBound);
            //ActiveGrid.PageIndexChanging +=new GridViewPageEventHandler(ActiveGrid_PageIndexChanging);
        }
        #endregion

        protected void DD_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        public void SentUserID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadGrids()
        {
            loadActiveGrid(customerID);
            loadScheduledGrid(customerID);
            loadSentGrid(customerID);
        }

        protected void SentGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                SentGrid.PageIndex = e.NewPageIndex;
            }
            loadSentGrid(customerID);
        }

        protected void ScheduledGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                ScheduledGrid.PageIndex = e.NewPageIndex;
            }
            loadScheduledGrid(customerID);
        }

        protected void ActiveGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                ActiveGrid.PageIndex = e.NewPageIndex;
            }
            loadActiveGrid(customerID);
        }

        private bool BlastHasEmailPreview(int blastID)
        {
            ECN_Framework.Communicator.Entity.Blast b = ECN_Framework.Communicator.Entity.Blast.GetByBlastID(blastID);
            return b.HasEmailPreview;
        }
        protected void ScheduledGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsScheduled = (Label)e.Row.FindControl("lblTotalRecordsScheduled");
                lblTotalRecordsScheduled.Text = ScheduledRecordCount.ToString();

                Label lblTotalNumberOfPagesScheduled = (Label)e.Row.FindControl("lblTotalNumberOfPagesScheduled");
                lblTotalNumberOfPagesScheduled.Text = ScheduledGrid.PageCount.ToString();

                TextBox txtGoToPageScheduled = (TextBox)e.Row.FindControl("txtGoToPageScheduled");
                txtGoToPageScheduled.Text = (ScheduledGrid.PageIndex + 1).ToString();

                DropDownList ddlPageSizeScheduled = (DropDownList)e.Row.FindControl("ddlPageSizeScheduled");
                ddlPageSizeScheduled.SelectedValue = ScheduledGrid.PageSize.ToString();
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex >= 0)
                {
                    LinkButton deleteButton = e.Row.FindControl("DeleteScheduledBlastBtn") as LinkButton;
                    deleteButton.Attributes.Add("onclick", "return confirm('Are you Sure Selected Blast and Reports associated with this blast will be permanently deleted.');");

                    int blastID = 0;
                    int.TryParse(ScheduledGrid.DataKeys[e.Row.RowIndex].Value.ToString(), out blastID);
                    ImageButton btnEmailPreview = (ImageButton)e.Row.FindControl("btnEmailPreview");
                    btnEmailPreview.CommandArgument = blastID.ToString();
                    if (blastID > 0)
                    {
                        btnEmailPreview.Visible = BlastHasEmailPreview(blastID);
                        btnEmailPreview.OnClientClick = "javascript:window.open('BlastEmailPreview.aspx?blastID=" + blastID.ToString() + "','BlastEmailPreview','resizable=yes,scrollbars=yes,menubar=yes,titlebar=no,toolbar=no'); return false;";
                    }
                    else
                        btnEmailPreview.Visible = false;
                }

            }
        }
        protected void ActiveGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsActive = (Label)e.Row.FindControl("lblTotalRecordsActive");
                lblTotalRecordsActive.Text = ActiveRecordCount.ToString();

                Label lblTotalNumberOfPagesActive = (Label)e.Row.FindControl("lblTotalNumberOfPagesActive");
                lblTotalNumberOfPagesActive.Text = ActiveGrid.PageCount.ToString();

                TextBox txtGoToPageActive = (TextBox)e.Row.FindControl("txtGoToPageActive");
                txtGoToPageActive.Text = (ActiveGrid.PageIndex + 1).ToString();

                DropDownList ddlPageSizeActive = (DropDownList)e.Row.FindControl("ddlPageSizeActive");
                ddlPageSizeActive.SelectedValue = ActiveGrid.PageSize.ToString();
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int blastID = 0;
                int.TryParse(ActiveGrid.DataKeys[e.Row.RowIndex].Value.ToString(), out blastID);
                ImageButton btnEmailPreview = (ImageButton)e.Row.FindControl("btnEmailPreview");
                btnEmailPreview.CommandArgument = blastID.ToString();
                if (blastID > 0)
                {
                    btnEmailPreview.Visible = BlastHasEmailPreview(blastID);
                    btnEmailPreview.OnClientClick = "javascript:window.open('BlastEmailPreview.aspx?blastID=" + blastID.ToString() + "','BlastEmailPreview','resizable=yes,scrollbars=yes,menubar=yes,titlebar=no,toolbar=no'); return false;";
                }
                else
                    btnEmailPreview.Visible = false;
            }
        }
        protected void SentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsSent = (Label)e.Row.FindControl("lblTotalRecordsSent");
                lblTotalRecordsSent.Text = SentRecordCount.ToString();

                Label lblTotalNumberOfPagesSent = (Label)e.Row.FindControl("lblTotalNumberOfPagesSent");
                lblTotalNumberOfPagesSent.Text = SentGrid.PageCount.ToString();

                TextBox txtGoToPageSent = (TextBox)e.Row.FindControl("txtGoToPageSent");
                txtGoToPageSent.Text = (SentGrid.PageIndex + 1).ToString();

                DropDownList ddlPageSizeSent = (DropDownList)e.Row.FindControl("ddlPageSizeSent");
                ddlPageSizeSent.SelectedValue = SentGrid.PageSize.ToString();
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int blastID = 0;
                int.TryParse(SentGrid.DataKeys[e.Row.RowIndex].Value.ToString(), out blastID);
                ImageButton btnEmailPreview = (ImageButton)e.Row.FindControl("btnEmailPreview");
                btnEmailPreview.CommandArgument = blastID.ToString();
                if (blastID > 0 && ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures. "Email Preview"))
                {
                    btnEmailPreview.Visible = BlastHasEmailPreview(blastID);
                    btnEmailPreview.OnClientClick = "javascript:window.open('BlastEmailPreview.aspx?blastID=" + blastID.ToString() + "','BlastEmailPreview','resizable=yes,scrollbars=yes,menubar=yes,titlebar=no,toolbar=no'); return false;";
                }
                else
                    btnEmailPreview.Visible = false;

                Label lblIsBlastGroup = e.Row.FindControl("lblIsBlastGroup") as Label;

                if (lblIsBlastGroup.Text.ToLower() == "y")
                {
                    e.Row.BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[1].Text = "Multi-Group Blast";
                    e.Row.Cells[9].Text = "";
                }
                else
                {
                    LinkButton deleteBtn = e.Row.FindControl("DeleteBlastBtn") as LinkButton;
                    if (e.Row.Cells[9].Text.ToString().ToUpper().Equals("N"))
                    {
                        if ((Convert.ToDateTime(e.Row.Cells[4].Text.ToString())) < DateTime.Now.AddDays(-7))
                        {
                            deleteBtn.Attributes.Add("onclick", "return confirm('Blast titled \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" and its associated Reporting will be DELETED !!" + "\\n" + "\\n" + "Are you sure you want to contine? This process CANNOT be undone.');");
                        }
                        else
                        {
                            deleteBtn.Enabled = false;
                            deleteBtn.Attributes.Add("style", "cursor:hand;padding:0;margin:0;");
                            deleteBtn.Attributes.Add("onclick", "alert('Blast titled \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" CANNOT be deleted because it was sent within 7 Days of current time." + "\\n" + "If you need further assistance please call your Account Manager.');");
                        }
                    }
                    else
                    {
                        deleteBtn.Attributes.Add("onclick", "return confirm('Blast titled \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" and its associated Reporting will be DELETED !!" + "\\n" + "\\n" + "Are you sure you want to contine? This process CANNOT be undone.');");
                    }

                    LinkButton resendBtn = e.Row.FindControl("ResendBlastBtn") as LinkButton;
                    Label isTestBlast = e.Row.FindControl("lblIsTestBlast") as Label;
                    if (isTestBlast.Text.ToString().ToUpper().Equals("Y"))
                    {
                        resendBtn.Attributes.Add("onclick", "return confirm('Blast titled \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" will be resent and will clear all previous activity!" + "\\n" + "\\n" + "Are you sure you want to contine? This process CANNOT be undone.');");
                    }
                    else
                    {
                        resendBtn.Enabled = false;
                        resendBtn.Attributes.Add("style", "cursor:hand;padding:0;margin:0;");
                        resendBtn.Attributes.Add("onclick", "alert('Blast titled \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" CANNOT be resent as it is not a test blast.');");
                    }

                }
                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrids();
            //try
            //{
            //    DateTime fromDate = Convert.ToDateTime(txtFrom.Text);
            //    DateTime toDate = Convert.ToDateTime(txtTo.Text);
            //    TimeSpan ts = toDate - fromDate;
            //    if (ts.Days >= 0 && ts.Days <= 60)
            //    {
            //        LoadGrids();
            //    }
            //    else
            //    {
            //        ErrorLabel.Text = "Please enter date range where the range is no greater than 60 days";
            //        ErrorLabel.Visible = true;
            //    }                

            //}
            //catch (Exception ex)
            //{
            //    ErrorLabel.Text = ex.Message;
            //    ErrorLabel.Visible = true;
            //}
        }

        protected void GoToPageActive_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageActive = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageActive.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.ActiveGrid.PageCount)
            {
                this.ActiveGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                this.ActiveGrid.PageIndex = 0;
            }
            loadActiveGrid(customerID);
        }

        protected void GoToPageSent_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageSent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageSent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.SentGrid.PageCount)
            {
                this.SentGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                this.SentGrid.PageIndex = 0;
            }
            loadSentGrid(customerID);
        }

        protected void GoToPageScheduled_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageScheduled = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageScheduled.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.ScheduledGrid.PageCount)
            {
                this.ScheduledGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                ScheduledGrid.PageIndex = 0;
            }
            loadScheduledGrid(customerID);
        }

        protected void SentGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int blastID = Convert.ToInt32(SentGrid.DataKeys[e.RowIndex].Values[0]);
                DeleteBlast(blastID.ToString());
                LoadGrids();
            }
            catch { }
        }

        protected void ActiveGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void ScheduledGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "DELETE")
                {
                    int blastID = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteBlast(blastID.ToString());
                    LoadGrids();
                }
            }
            catch { }
        }
        public void SentGrid_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "DELETE")
                {
                    int blastID = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteBlast(blastID.ToString());
                    LoadGrids();
                }
                else if (e.CommandName.ToUpper() == "RESEND")
                {
                    int blastID = Convert.ToInt32(e.CommandArgument.ToString());
                    ResendBlast(blastID.ToString());
                    LoadGrids();
                }
            }
            catch { }
        }

        protected void ScheduledGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void ActiveGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.ActiveGrid.PageSize = int.Parse(dropDown.SelectedValue);
            ActiveGrid.PageIndex = 0;
            loadActiveGrid(customerID);
        }

        protected void ScheduledGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.ScheduledGrid.PageSize = int.Parse(dropDown.SelectedValue);
            ScheduledGrid.PageIndex = 0;
            loadScheduledGrid(customerID);
        }

        protected void SentGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.SentGrid.PageSize = int.Parse(dropDown.SelectedValue);
            SentGrid.PageIndex = 0;
            loadSentGrid(customerID);
        }

        protected void SentGrid_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (e.SortExpression.ToString() == ViewState["sentSortField"].ToString())
            {
                switch (ViewState["sentSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["sentSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["sentSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["sentSortField"] = e.SortExpression;
                ViewState["sentSortDirection"] = "DESC";
            }
            loadSentGrid(customerID);
        }

        protected void ScheduledGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["scheduledSortField"].ToString())
            {
                switch (ViewState["scheduledSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["scheduledSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["scheduledSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["scheduledSortField"] = e.SortExpression;
                ViewState["scheduledSortDirection"] = "DESC";
            }
            loadScheduledGrid(customerID);
        }

        protected void ActiveGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["activeSortField"].ToString())
            {
                switch (ViewState["activeSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["activeSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["activeSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["activeSortField"] = e.SortExpression;
                ViewState["activeSortDirection"] = "DESC";
            }
            loadActiveGrid(customerID);
        }
         */
    }
}