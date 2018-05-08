using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Web.Security;

namespace ecn.showcare.wizard.includes {
	/// <summary>
	///		Summary description for LogWizardActivity.
	/// </summary>
	public class LogWizardActivity : System.Web.UI.UserControl {
		
		private static string ecn_connString {
			get {return (ConfigurationManager.AppSettings["connString"].ToString());}
		}
		protected string customerID	= "";
		protected string userID			= "";

		protected void GetInfoFromTicket () 
		{

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
				customerID	= allData[0];							// [0] --> CustomerID
				userID		= fat.Name;							// ticket-name --> UserID
			}
		}

		private void Page_Load(object sender, System.EventArgs e) {
			GetInfoFromTicket();
			// Put user code to initialize the page here
			string strUrl = HttpContext.Current.Request.Url.ToString(); 
			//Response.Write("userID: "+ userID+"<BR>");
			//Response.Write("customerID: "+ customerID+"<BR>");
			//Response.Write("strUrl: "+ strUrl+"<BR>");
		
			try{
				SqlConnection ecnConnection = new SqlConnection(ecn_connString);
				SqlCommand activityLogInsertCommand = new SqlCommand(null, ecnConnection);
			
				activityLogInsertCommand.CommandText = "INSERT INTO WebsiteActivityLog " 
					+ "(CustomerID,UserID,WebPageURL)" 
					+ " VALUES " 
					+ "(@CustomerID,@UserID,@WebPageURL) "; 

				activityLogInsertCommand.Parameters.Add ("@CustomerID", SqlDbType.Int,4).Value = Convert.ToInt32(customerID.ToString());
				activityLogInsertCommand.Parameters.Add ("@UserID", SqlDbType.Int,4).Value = Convert.ToInt32(userID.ToString());;  
				activityLogInsertCommand.Parameters.Add ("@WebPageURL",SqlDbType.VarChar,500).Value = strUrl;

				activityLogInsertCommand.Connection.Open();
				activityLogInsertCommand.ExecuteScalar();
				activityLogInsertCommand.Connection.Close();
			}catch(Exception){}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
