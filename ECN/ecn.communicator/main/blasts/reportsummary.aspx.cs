using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.communicator.includes;
using ecn.common.classes;

namespace ecn.communicator.blastsmanager
{
    public partial class reportsummary : ECN_Framework.WebPageHelper
    {
        //PAGE NO LONGER USED
        /*
        DateTime StartDate = DateTime.Now.AddDays(-30);
        DateTime EndDate = DateTime.Now;

        public reportsummary()
        {
            Page.Init += new System.EventHandler(Page_Init);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Summary Report";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icoblasts.gif><b>Summary Reports</b><br />Gives a summary of Clicks, Bounces and Subscription reports.<br /><br /><b>Latest Clicks</b><br />Lists 10 recent URL clicks in the recent blasts sent.<br /><br /><b>Latest Bounces</b><br />Lists 10 latest email address bounced in the rencet Blasts. <i>Blast</i> column lists the email blast for which the bounced email address was assigned.<br /><i>Bounce Type</i> lists the type of bounce, which would be a <i>softBounce</i> (for instance: email Inbox full) or a <i>hardBounce</i> (for instance: email address doesnot exist).<br /><br /><b>Latest Subscription Changes</b><br />Gives a report of 15 recent email addresses subscribed or unsubscribed.";
            Master.HelpTitle = "Blast Manager";	

            if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv))
            {
                if (Page.IsPostBack == false)
                {
                    loadClickGrid();
                    loadBounceGrid();
                    loadSubscribeGrid();
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private void loadClickGrid()
        {
            //string sqlquery =
            //    " SELECT TOP 10 eal.EMailID, e.EmailAddress, eal.BlastID, b.EmailSubject, eal.ActionDate, eal.ActionValue " +
            //    " FROM EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID JOIN Blasts b ON eal.BlastID = b.BlastID" +
            //    " WHERE ActionTypeCode='click' " +
            //    "   AND e.CustomerID=" + sc.CustomerID() +
            //    "   AND ActionDate >= '" + StartDate.ToString() + "' " +
            //    "   AND ActionDate <= '" + EndDate.ToString() + "' " +
            //    " ORDER BY ActionDate DESC";

            string sqlquery =
                " SELECT TOP 10 bacl.EMailID, e.EmailAddress, bacl.BlastID, b.EmailSubject, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue' " +
                " FROM BlastActivityClicks bacl JOIN ecn5_communicator..Emails e ON bacl.EMailID = e.EMailID JOIN ecn5_communicator..Blasts b " +
                " ON bacl.BlastID = b.BlastID WHERE e.CustomerID = " + Master.UserSession.CurrentUser.CustomerID + " AND bacl.ClickTime >= '" + StartDate.ToString() + "' AND bacl.ClickTime <= '" + EndDate.ToString() + "' ORDER BY bacl.ClickTime DESC ";

            DataTable dt = DataFunctions.GetDataTable(sqlquery,DataFunctions.con_activity);
            ClickGrid.DataSource = dt.DefaultView;
            ClickGrid.DataBind();

            ClickChart.LineColor = Color.Green;
            ClickChart.CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
            ClickChart.ChartType = "bar";
            ClickChart.ActionTypeCode = "click";
        }

        private void loadBounceGrid()
        {
            //string sqlquery =
            //    " SELECT TOP 10 eal.EMailID, e.EmailAddress, eal.BlastID, b.EmailSubject, eal.ActionDate, eal.ActionValue " +
            //    " FROM EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID JOIN Blasts b ON eal.BlastID = b.BlastID" +
            //    " WHERE ActionTypeCode='bounce' " +
            //    " AND e.CustomerID=" + sc.CustomerID() +
            //    " AND ActionDate >= '" + StartDate.ToString() + "' " +
            //    " AND ActionDate <= '" + EndDate.ToString() + "' " +
            //    " ORDER BY ActionDate DESC";

            string sqlquery =
                " SELECT TOP 10 babo.EMailID, e.EmailAddress, babo.BlastID, b.EmailSubject, babo.BounceTime as ActionDate, bc.BounceCode as ActionValue " +
                " FROM BlastActivityBounces babo JOIN ecn5_communicator..Emails e ON babo.EMailID = e.EMailID JOIN " +
                " ecn5_communicator..Blasts b ON babo.BlastID = b.BlastID " +
                " join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID " +
                " WHERE e.CustomerID = " + Master.UserSession.CurrentUser.CustomerID + " AND babo.BounceTime >= '" + StartDate.ToString() + "' AND babo.BounceTime <= ' " + EndDate.ToString() + "' ORDER BY ActionDate DESC ";

            DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.con_activity);
            BounceGrid.DataSource = dt.DefaultView;
            BounceGrid.DataBind();

            BounceChart.LineColor = Color.Red;
            BounceChart.CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
            BounceChart.ChartType = "bar";
            BounceChart.ActionTypeCode = "bounce";
        }

        private void loadSubscribeGrid()
        {
            //string sqlquery =
            //    " SELECT TOP 10 eal.EMailID, e.EmailAddress, eal.BlastID, b.EmailSubject, eal.ActionDate, eal.ActionValue " +
            //    " FROM EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID JOIN Blasts b ON eal.BlastID = b.BlastID" +
            //    " WHERE ActionTypeCode = 'subscribe' " +
            //    " AND e.CustomerID=" + sc.CustomerID() +
            //    " AND ActionDate >= '" + StartDate.ToString() + "' " +
            //    " AND ActionDate <= '" + EndDate.ToString() + "' " +
            //    " ORDER BY ActionDate DESC";

            string sqlquery =
                " SELECT TOP 10 baus.EMailID, e.EmailAddress, baus.BlastID, b.EmailSubject, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as ActionValue " +
                " FROM BlastActivityUnSubscribes baus JOIN ecn5_communicator..Emails e ON baus.EMailID = e.EMailID " +
                " JOIN ecn5_communicator..Blasts b ON baus.BlastID = b.BlastID " +
                " JOIN UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID " +
                " WHERE e.CustomerID =  " + Master.UserSession.CurrentUser.CustomerID +
                " AND baus.UnsubscribeTime >= '" + StartDate.ToString() + "' AND baus.UnsubscribeTime <= '" + EndDate.ToString() + "'" +
                " ORDER BY ActionDate DESC ";

            DataTable dt = DataFunctions.GetDataTable(sqlquery,DataFunctions.con_activity);
            SubscribeGrid.DataSource = dt.DefaultView;
            SubscribeGrid.DataBind();

            SubscribeChart.LineColor = Color.Blue;
            SubscribeChart.CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
            SubscribeChart.ChartType = "bar";
            SubscribeChart.ActionTypeCode = "subscribe";
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
         */
    }
}
