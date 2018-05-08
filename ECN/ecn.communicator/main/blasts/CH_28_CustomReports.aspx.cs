using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Configuration;

namespace ecn.communicator.blastsmanager
{
    public partial class CH_28_CustomReports : ECN_Framework.WebPageHelper
    {
        private const string ColumnDistinctEmailCount = "DistinctEmailCount";
        private const string ColumnEmailCount = "EmailCount";
        private const string ColumnGroupName = "GroupName";
        private const string ColumnFilterName = "FilterName";
        private const string ColumnLayoutName = "LayoutName";
        private const string ColumnEmailSubject = "EmailSubject";
        private const string ColumnEmailFromName = "EmailFromName";
        private const string ColumnSendTime = "SendTime";
        private const string ColumnFinishTime = "FinishTime";
        public static string connStr = ConfigurationManager.AppSettings["connstring"];
        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];
        public static string creatordb = ConfigurationManager.AppSettings["creatordb"];
        public static string activitydb = ConfigurationManager.AppSettings["activity"];

        public CH_28_CustomReports()
        {
            Page.Init += new System.EventHandler(Page_Init);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Blast Reporting";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icoblasts.gif><b>Reports</b><br />Gives a report of the Blast in progress.<br />Click on <i>view log</i> to view the log of the emails that has received the blast.<br /><i>Clicks</i> specify the total number of URL clicks in your email by the recepients who received the email. Click on the '[number]' to see who clicked &amp; what link was clicked<br /><i>Bounces</i> specify the number of bounced emails recepients or the email recepients who did not received the blast. Click on the '[number]' to see who did not receive the blast.";
            Master.HelpTitle = "Blast Manager";	

            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))
            {
                if (Page.IsPostBack == false)
                {
                    int requestBlastID = getBlastID();
                    if (requestBlastID > 0)
                    {
                        ECN_Framework.Common.SecurityAccess.canI("Blasts", requestBlastID.ToString());
                        LoadFormData(requestBlastID);
                        LoadClicksGrid(requestBlastID);
                    }
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        public int getBlastID()
        {
            int theBlastID = 0;
            try
            {
                theBlastID = Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBlastID;
        }

       

        #region Data Load
        private void LoadClicksGrid(int BlastID)
        {
            #region View commented Code
            /*string sqlquery=
					" SELECT TOP 10 Count(ActionValue) AS ClickCount, ActionValue AS FullLink, "+
					" CASE WHEN LEN(ActionValue) > 6 THEN LEFT(RIGHT(ActionValue,LEN(ActionValue)-7),40) ELSE ActionValue END AS SmallLink"+
					" FROM EmailActivityLog "+
					" WHERE ActionTypeCode='click' "+
					" AND BlastID="+BlastID+
					" GROUP BY ActionValue "+
					" ORDER BY ClickCount DESC, ActionValue ";
				DataTable dt = DataFunctions.GetDataTable(sqlquery);

				DataTable newDT = new DataTable();
				newDT.Columns.Add(new DataColumn("<center>Click Count</center>"));
				newDT.Columns.Add(new DataColumn("URL / Link Alias"));
				newDT.Columns.Add("LinkDetail", typeof(String));
			
				DataRow newDR;

				foreach ( DataRow dr in dt.Rows ) {
					string clickCount	= dr["ClickCount"].ToString();

					string linkDetail	= "BlastID=" + BlastID + "&link=" + HttpUtility.UrlEncode(dr["FullLink"].ToString());
					string fullLink		= dr["FullLink"].ToString();
					string smallLink		= dr["SmallLink"].ToString();
					string linkORalias	= "";
				
					newDR			= newDT.NewRow();
					newDR[0]		= "<center><a href='clickslinks.aspx?"+linkDetail+"'>"+clickCount.ToString()+"</a></center>";
					string alias		= getLinkAlias(BlastID, fullLink);
					if(alias.Length > 0){
						linkORalias = alias;
					}else {
						linkORalias = smallLink;
					}
					newDR[1]		= "<a href='"+fullLink.ToString()+"' target='_blank'>"+linkORalias.ToString()+"</a>";

					newDT.Rows.Add(newDR);
				}*/
            #endregion

            SqlConnection dbConn = new SqlConnection(DataFunctions.con_activity.ToString());
            SqlCommand topClicksCmd = new SqlCommand("spClickActivity", dbConn);
            topClicksCmd.CommandTimeout = 100;
            topClicksCmd.CommandType = CommandType.StoredProcedure;

            topClicksCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            topClicksCmd.Parameters["@BlastID"].Value = BlastID;
            topClicksCmd.Parameters.Add(new SqlParameter("@HowMuch", SqlDbType.VarChar));
            topClicksCmd.Parameters["@HowMuch"].Value = ClickSelectionDD.SelectedValue.ToString().Trim();

            SqlDataAdapter da = new SqlDataAdapter(topClicksCmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "spClickActivity");
            dbConn.Close();

            DataTable dt = ds.Tables[0];

            DataTable newDT = new DataTable();
            newDT.Columns.Add(new DataColumn("<center>Click Count</center>"));
            newDT.Columns.Add(new DataColumn("URL / Link Alias"));
            newDT.Columns.Add("LinkDetail", typeof(String));

            DataRow newDR;

            foreach (DataRow dr in dt.Rows)
            {
                string clickCount = dr["ClickCount"].ToString();

                string linkDetail = "BlastID=" + BlastID + "&link=" + HttpUtility.UrlEncode(dr["NewActionValue"].ToString());
                string fullLink = dr["NewActionValue"].ToString();
                string smallLink = dr["SmallLink"].ToString();
                string linkORalias = "";

                newDR = newDT.NewRow();
                newDR[0] = "<center>" + clickCount.ToString() + "</center>";
                string alias = getLinkAlias(BlastID, fullLink);
                if (alias.Length > 0)
                {
                    linkORalias = alias;
                }
                else
                {
                    linkORalias = smallLink;
                }
                newDR[1] = "<a href='" + fullLink.ToString() + "' target='_blank'>" + linkORalias.ToString() + "</a>";

                newDT.Rows.Add(newDR);
            }
            TopGrid.DataSource = new DataView(newDT);
            try
            {
                TopGrid.DataBind();
            }
            catch
            {
                TopGrid.CurrentPageIndex = 0;
                TopGrid.DataBind();
            }
        }

        private void LoadFormData(int setBlastID)
        {
            var customerId = Master.UserSession.CurrentUser.CustomerID.ToString();

            GetGroupNumberTable(setBlastID, customerId);

            var sendTotalSql = $"select count(emailID) from BlastActivitySends where BlastID = {setBlastID}";
            decimal sendTotal = Convert.ToInt32(DataFunctions.ExecuteScalar("activity", sendTotalSql));

            var bounceTotalSql =
                $"select count(distinct emailID) from BlastActivityBounces where BlastID = {setBlastID}";
            var bounce = Convert.ToInt32(DataFunctions.ExecuteScalar("activity", bounceTotalSql));
            var success = sendTotal - bounce;

            var clicksuniquecount = 0.0M;
            var opensuniquecount = 0.0M;
            var bouncesuniquecount = 0.0M;
            var subscribesuniquecount = 0.0M;
            var resendsuniquecount = 0.0M;
            var forwardsuniquecount = 0.0M;

            clicksuniquecount = LoadClicks(setBlastID, clicksuniquecount);

            opensuniquecount = LoadOpens(setBlastID, opensuniquecount);

            bouncesuniquecount = LoadBounces(setBlastID, bouncesuniquecount);

            subscribesuniquecount = LoadUnsubscribes(setBlastID, subscribesuniquecount);

            resendsuniquecount = LoadResends(setBlastID, resendsuniquecount);

            forwardsuniquecount = LoadForwards(setBlastID, forwardsuniquecount);

            Successful.Text = $"{success}/{sendTotal}";

            SuccessfulPercentage.Text = "(" + Decimal.Round(((sendTotal == 0 ? 0 : success / sendTotal) * 100), 0).ToString() + "%)";
            ClicksPercentage.Text = Decimal.Round(((sendTotal == 0 ? 0 : clicksuniquecount / sendTotal) * 100), 0).ToString() + " %";
            BouncesPercentage.Text = Decimal.Round(((sendTotal == 0 ? 0 : bouncesuniquecount / sendTotal) * 100), 0).ToString() + " %";
            OpensPercentage.Text = Decimal.Round(((sendTotal == 0 ? 0 : opensuniquecount / sendTotal) * 100), 0).ToString() + " %";
            SubscribesPercentage.Text = Decimal.Round(((sendTotal == 0 ? 0 : subscribesuniquecount / sendTotal) * 100), 0).ToString() + " %";
            ResendsPercentage.Text = Decimal.Round(((sendTotal == 0 ? 0 : resendsuniquecount / sendTotal) * 100), 0).ToString() + " %";
            ForwardsPercentage.Text = Decimal.Round(((sendTotal == 0 ? 0 : forwardsuniquecount / sendTotal) * 100), 0).ToString() + " %";
        }

        private decimal LoadForwards(int setBlastID, decimal forwardsUniqueCount)
        {
            var sqlForwards =
                " select COUNT(DISTINCT barf.EmailID) as 'DistinctEmailCount', count(barf.EmailID) as 'EmailCount' " +
                " FROM BlastActivityRefer barf JOIN ecn5_communicator..Emails e on e.EmailID = barf.EmailID " +
                $" WHERE barf.BlastID = {setBlastID}";

            using (var forwardTable = DataFunctions.GetDataTable(sqlForwards, activitydb))
            {
                foreach (DataRow row in forwardTable.Rows)
                {
                    forwardsUniqueCount = Convert.ToDecimal(row["DistinctEmailCount"].ToString());
                    ForwardsUnique.Text = row["DistinctEmailCount"].ToString();
                    ForwardsTotal.Text = row["EmailCount"].ToString();
                }
            }

            return forwardsUniqueCount;
        }

        private decimal LoadResends(int setBlastID, decimal resendsUniqueCount)
        {
            var sqlResends =
                " SELECT COUNT(DISTINCT bars.EmailID) as 'DistinctEmailCount', count(bars.EmailID) as 'EmailCount' " +
                " FROM BlastActivityResends bars JOIN ecn5_communicator..Emails e on e.EmailID = bars.EmailID " +
                $" WHERE bars.BlastID = {setBlastID}";

            using (var tableResends = DataFunctions.GetDataTable(sqlResends, activitydb))
            {
                foreach (DataRow dr in tableResends.Rows)
                {
                    resendsUniqueCount = Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
                    ResendsUnique.Text = dr["DistinctEmailCount"].ToString();
                    ResendsTotal.Text = dr["EmailCount"].ToString();
                }
            }

            return resendsUniqueCount;
        }

        private void GetGroupNumberTable(int setBlastId, string customerId)
        {
            var sqlGroupName =
                " SELECT " +
                " 'GroupName' = CASE " +
                " WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN g.GroupName " +
                " ELSE '< GROUP DELETED >' " +
                " END, " +
                " 'GrpNavigateURL' = CASE " +
                " WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN '../lists/groupeditor.aspx?GroupID='+CONVERT(VARCHAR,g.GroupID) " +
                " ELSE '' " +
                " END, " +
                " 'FilterName' = CASE " +
                " WHEN f.FilterID <> 0 THEN f.FilterName " +
                " ELSE '< NO FILTER / FILTER DELETED >' " +
                " END, " +
                " 'FltrNavigateURL' = CASE " +
                " WHEN f.FilterID <> 0 THEN '../lists/filters.aspx?FilterID='+CONVERT(VARCHAR,f.FilterID) " +
                " ELSE '' " +
                " END, " +
                " 'LayoutName' = CASE " +
                " WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName " +
                " ELSE '< CAMPAIGN DELETED >' " +
                " END, " +
                " 'LytNavigateURL' = CASE " +
                " WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN '../content/layouteditor.aspx?LayoutID='+CONVERT(VARCHAR,l.LayoutID)  " +
                " ELSE '' " +
                " END, " +
                " b.EmailSubject, b.EmailFromName, b.EmailFrom, b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal " +
                " FROM Groups g JOIN Blasts b ON b.groupID = g.groupID LEFT OUTER JOIN Filters f ON b.filterID = f.filterID " +
                " JOIN Layouts l on b.LayoutID = l.LayoutID " +
                $" WHERE b.BlastID = {setBlastId}"  +
                $" AND b.CustomerID = {customerId}";

            var table = DataFunctions.GetDataTable(sqlGroupName);
            foreach (DataRow row in table.Rows)
            {
                GroupTo.Text = row[ColumnGroupName].ToString();
                Filter.Text = row[ColumnFilterName].ToString();
                Campaign.Text = row[ColumnLayoutName].ToString();
                EmailSubject.Text = row[ColumnEmailSubject].ToString();
                EmailFrom.Text = $"{row[ColumnEmailFromName]} &lt; {row["EmailFrom"]} &gt;";
                SendTime.Text = row[ColumnSendTime].ToString();
                FinishTime.Text = row[ColumnFinishTime].ToString();
                SendTime.Text = row[ColumnSendTime].ToString();
            }
        }

        private decimal LoadUnsubscribes(int setBlastId, decimal subscribesUniqueCount)
        {
            var sqlUnsubscribes =
                $" SELECT COUNT(DISTINCT baus.EmailID) as \'DistinctEmailCount\', " +
                $"count(baus.EmailID) as \'EmailCount\'  FROM BlastActivityUnSubscribes baus " +
                $"JOIN ecn5_communicator..Emails e on e.EmailID = baus.EmailID  " +
                $"join UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID " +
                $"and usc.UnsubscribeCode = \'subscribe\'  WHERE baus.BlastID = {setBlastId}";

            var unSubsDt = DataFunctions.GetDataTable(sqlUnsubscribes, activitydb);
            foreach (DataRow row in unSubsDt.Rows)
            {
                subscribesUniqueCount = ToDecimalWithThrow(row[ColumnDistinctEmailCount].ToString());
                SubscribesUnique.Text = row[ColumnDistinctEmailCount].ToString();
                SubscribesTotal.Text = row[ColumnEmailCount].ToString();
            }

            return subscribesUniqueCount;
        }

        private decimal LoadBounces(int setBlastId, decimal bouncesUniqueCount)
        {
            var sqlBounces =
                $" SELECT COUNT(DISTINCT babo.EmailID) as \'DistinctEmailCount\', " +
                $"count(babo.EmailID) as \'EmailCount\'  FROM BlastActivityBounces babo " +
                $"JOIN ecn5_communicator..Emails e on e.EmailID = babo.EmailID  WHERE babo.BlastID = {setBlastId}";

            var bouncesDt = DataFunctions.GetDataTable(sqlBounces, activitydb);
            foreach (DataRow row in bouncesDt.Rows)
            {
                bouncesUniqueCount = ToDecimalWithThrow(row[ColumnDistinctEmailCount].ToString());
                BouncesUnique.Text = row[ColumnDistinctEmailCount].ToString();
                BouncesTotal.Text = row[ColumnEmailCount].ToString();
            }

            return bouncesUniqueCount;
        }

        private decimal LoadOpens(int setBlastId, decimal opensuniquecount)
        {
            var sqlOpens =
                $" select COUNT(DISTINCT baop.EmailID) as \'DistinctEmailCount\', " +
                $"count(baop.EmailID) as \'EmailCount\'  FROM BlastActivityOpens baop " +
                $"JOIN ecn5_communicator..Emails e on e.EmailID = baop.EmailID  WHERE baop.BlastID = {setBlastId}";

            var table = DataFunctions.GetDataTable(sqlOpens, activitydb);
            foreach (DataRow row in table.Rows)
            {
                opensuniquecount = ToDecimalWithThrow(row[ColumnDistinctEmailCount].ToString());
                OpensUnique.Text = row[ColumnDistinctEmailCount].ToString();
                OpensTotal.Text = row[ColumnEmailCount].ToString();
            }

            return opensuniquecount;
        }

        private decimal LoadClicks(int setBlastId, decimal clicksUniqueCount)
        {
            var sqlClicks =
                " SELECT COUNT(DISTINCT bacl.EmailID) as \'DistinctEmailCount\', " +
                "count(bacl.EmailID) as \'EmailCount\'  FROM BlastActivityClicks bacl " +
                $"JOIN ecn5_communicator..Emails e on e.EmailID = bacl.EmailID  WHERE bacl.BlastID = {setBlastId}";

            var clicksDt = DataFunctions.GetDataTable(sqlClicks, activitydb);
            foreach (DataRow row in clicksDt.Rows)
            {
                clicksUniqueCount = ToDecimalWithThrow(row[ColumnDistinctEmailCount].ToString());
                ClicksUnique.Text = row[ColumnDistinctEmailCount].ToString();
                ClicksTotal.Text = row[ColumnEmailCount].ToString();
            }

            return clicksUniqueCount;
        }

        private int ToIntWithThrow(string numberStr)
        {
            int result;
            if (int.TryParse(numberStr, out result))
            {
                return result;
            }
            var message = $"Couldn't parse {numberStr} into int";
            throw new InvalidOperationException(message);
        }


        private decimal ToDecimalWithThrow(string numberStr)
        {
            decimal result;
            if (decimal.TryParse(numberStr, out result))
            {
                return result;
            }
            var message = $"Couldn't parse {numberStr} into decimal";
            throw new InvalidOperationException(message);
        }
        #endregion

        private string getLinkAlias(int BlastID, String Link)
        {
            string sqlquery = " SELECT Alias FROM " +
                " Blasts b, Layouts l, Content c, linkAlias la " +
                " WHERE " +
                " b.blastID = " + BlastID + " AND b.layoutID = l.layoutID AND " +
                " (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR " +
                " l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR " +
                " l.ContentSlot9 = c.contentID) AND " +
                " la.ContentID = c.ContentID AND la.Link = '" + Link + "'";
            string alias = "";
            try
            {
                alias = DataFunctions.ExecuteScalar(sqlquery).ToString();
            }
            catch (Exception)
            {
                alias = "";
            }

            return alias;
        }

        public void ClickSelectionDD_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code

        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
        }
        #endregion

        private void lnkConversionTracking_Click(object sender, System.EventArgs e)
        {
            Server.Transfer("conversionTracker.aspx?BlastID=" + getBlastID());
        }
    }
}