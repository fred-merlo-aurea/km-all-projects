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

using System.Web.Security;
using ecn.common.classes;
using System.Configuration;

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for ReportingMain.
	/// </summary>
	public class ReportingMain : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dgReports;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected Reporting r = new Reporting();

		protected string cid = "";
		protected string uid = "";
		protected string bcid = "";
	
		protected void GetInfoFromTicket () {

			HttpCookie hc= null;
			for(int i=0;i<Request.Cookies.Count;i++)
			{
				if (Request.Cookies[i].Name.ToUpper() == ".ASPXAUTH")
				{
					hc = Request.Cookies[i];
				}
			}
			if (hc != null)
			{
				// When we set it up, we ecrypted it, so get a encrypted cookie string
				string strToDecrypt = hc.Value;
				// The cookie string is nothing but encrypted FormsAuthenticationTicket. So use Decrypt to get the ticket back
				FormsAuthenticationTicket fat = FormsAuthentication.Decrypt(strToDecrypt);
				// Get the UserData, thats where CustomerID, GroupID and UserID is in the comma seperated form
				string []allData = fat.UserData.Split(',');
				cid = allData[0];	// [0] --> CustomerID
				uid = fat.Name;		// ticket-name --> UserID
				bcid = allData[1];  // [1] --> BaseChannelID
			}
				
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			//Get the cookie and FormsAuthenticationticket
			GetInfoFromTicket();
			
			// Set the user-name
			SetUserName();

			// Save Reporting so that other Reports.* can use it too
			if (!IsPostBack && (Session["reports"] == null)) {				// Thsi means, either the user is coming here for the first-time or it's a new session
				r.CustomerID = cid;
				r.UserID = uid;
				r.Email = lblEmail.Text;
				r.ChannelID = bcid;
				r.ToSession();
			}

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void SetUserName() {
			string sql = "SELECT UserName FROM ecn5_accounts.dbo.Users WHERE UserID="+uid;
			lblEmail.Text = Convert.ToString(DataFunctions.ExecuteScalar("accounts",sql));
		}

		private void LoadReporting() {
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

			string sql = " SELECT l.LayoutName as MessageName, b.EmailSubject as Subject, b.SendTime as SentTime, b.BlastID as BlastID, "+
                            " COUNT(bas.emailID) AS Sends " +
                            " FROM ECN5_COMMUNICATOR..Blasts b with (nolock) " +
                            " JOIN ECN5_COMMUNICATOR..Layouts l with (nolock) on l.layoutID = b.LayoutID " +
                            " JOIN BlastActivitySends bas with (nolock) on b.blastID = bas.BlastID " +
							" WHERE b.CustomerID="+cid+" AND b.userid="+uid+ 
							" GROUP BY l.LayoutName, b.EmailSubject, b.SendTime, b.BlastID "+
							" ORDER BY b.SendTime DESC";

            DataTable dt = DataFunctions.GetDataTable(sql, DataFunctions.GetConnectionString("activity"));
			dgReports.DataSource = dt;
			dgReports.DataBind();
		}
	}
}
