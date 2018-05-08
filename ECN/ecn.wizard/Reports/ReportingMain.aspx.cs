using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ecn.common.classes;
using System.Configuration;

namespace ecn.wizard.Reports
{
    /// <summary>
    /// Summary description for ReportingMain.
    /// </summary>
    public partial class ReportingMain : ecn.wizard.MasterPage
    {
        protected string cid = "";
        protected string uid = "";
        protected string bcid = "";

        private void SetUserName()
        {
            string sql = "SELECT UserName FROM ecn5_accounts.dbo.Users WHERE UserID=" + uid;

            //TODO
            //lblEmail.Text = DataFunctions.ExecuteScalar("accounts",sql).ToString();
        }

        private void LoadReporting()
        {
            /* REMINDER: Add following to the query where clause
             *  AND b.StatusCode='sent'
             */
            /*string sql = "SELECT (SELECT LayoutName FROM Layouts WHERE LayoutID=b.LayoutID) as MessageName, "+
                                    "b.EmailSubject as Subject, b.SendTime as SentTime, b.BlastID as BlastID, "+
                                    "(SELECT COUNT(emailID) FROM EmailActivityLog "+
                                        "WHERE BlastID = b.BlastID AND (ActionTypeCode = 'send' OR ActionTypeCode = 'testsend')) AS Sends "+
                                "FROM Blasts b "+
                                "WHERE b.CustomerID="+cid+" AND b.userid="+uid+" ORDER BY b.SendTime DESC";
            */

            //string sql = " SELECT l.LayoutName as MessageName, b.EmailSubject as Subject, b.SendTime as SentTime, b.BlastID as BlastID, " +
            //    " COUNT(eal.emailID) AS Sends " +
            //    " FROM Blasts b " +
            //    " JOIN Layouts l on l.layoutID = b.LayoutID " +
            //    " left outer JOIN EmailActivityLog eal on b.blastID = eal.BlastID and ((ActionTypeCode = 'send' OR ActionTypeCode = 'testsend')) " +
            //    " WHERE b.CustomerID=" + cid + " AND b.StatusCode='sent'" +  //AND b.userid="+uid+"
            //    " GROUP BY l.LayoutName, b.EmailSubject, b.SendTime, b.BlastID " +
            //    " ORDER BY b.SendTime DESC";

            string sql =
                " SELECT l.LayoutName as MessageName, b.EmailSubject as Subject, b.SendTime as SentTime, b.BlastID as BlastID, " +
                " COUNT(bas.emailID) AS Sends FROM ecn5_communicator..Blasts b JOIN ecn5_communicator..Layouts l on l.layoutID = b.LayoutID " +
                " left outer JOIN BlastActivitySends bas on b.blastID = bas.BlastID " +
                " WHERE b.CustomerID = " + cid + " GROUP BY l.LayoutName, b.EmailSubject, b.SendTime, b.BlastID " +
                " ORDER BY b.SendTime DESC";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable("activity", cmd);

            dgReports.DataSource = dt;
            dgReports.DataBind();
        }

        protected void GetInfoFromTicket()
        {
            // the first cookie is SessionID and the second one is the one that we set in the login page. Get that one.
            //			HttpCookie hc = Request.Cookies[1];
            // When we set it up, we ecrypted it, so get a encrypted cookie string
            //			string strToDecrypt = hc.Value;
            // The cookie string is nothing but encrypted FormsAuthenticationTicket. So use Decrypt to get the ticket back
            //			FormsAuthenticationTicket fat = FormsAuthentication.Decrypt(strToDecrypt);
            // Get the UserData, thats where CustomerID, GroupID and UserID is in the comma seperated form
            //			string []allData = fat.UserData.Split(',');
            cid = CustomerID.ToString();//allData[0];							// [0] --> CustomerID
            uid = UserID.ToString();//fat.Name;							// ticket-name --> UserID
            //			gid = allData[allData.Length-1];	// [last-element] --> GroupID [Don't this I need this here]
            bcid = ChannelID.ToString();//allData[1];							// [1] --> BaseChannelID
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            imgSelection.Src = UrlBase + "images/main.gif";
            //Get the cookie and FormsAuthenticationticket
            GetInfoFromTicket();

            // Set the user-name
            SetUserName();

            // Save Reporting so that other Reports.* can use it too
            //			if (!IsPostBack && (Session["reports"] == null)) {				
            //				// Thsi means, either the user is coming here for the first-time or it's a new session
            //				r.CustomerID = CustomerID;//cid;
            //				r.UserID =  UserID ;//uid;
            //				r.Email = lblEmail.Text;
            //				r.ChannelID = bcid =  ChannelID ;
            //				r.ToSession();
            //			}

            // Load the DataGrid with all required information
            LoadReporting();
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion


    }
}
