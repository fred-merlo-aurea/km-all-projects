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
using ecn.common.classes;
using System.Configuration;
using System.Web.Security;
using ActiveUp.WebControls;


namespace ecn.showcare.wizard.main
{
	/// <summary>
	/// Summary description for list.
	/// </summary>
	public class list : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dgEmailList;
		protected ActiveUp.WebControls.PagerBuilder EmailsPager;
		string groupID = string.Empty;

		protected void GetInfoFromTicket () 
		{
			// the first cookie is SessionID and the second one is the one that we set in the login page. Get that one.
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
				groupID = allData[allData.Length-1];	// [last-element] --> GroupID
			}
		}
		/* ========== FLOW ==========
		 * 1) Populate Templates Radio Button List
		 * 1.5) When user clicks on image popup HtmlTemplate image
		 * 2) Before SUBMIT check if Template is selected
		 *     a) if not display and error message (may be write a javascript)
		 *     b) if passes all the checks, then put the TemplateID in the session and goto next step
		 */
		private void Page_Load(object sender, System.EventArgs e)
		{
			GetInfoFromTicket();

			DataTable dt = DataFunctions.GetDataTable("select e.EmailID, FirstName, LastName, emailaddress from emails e join emailgroups eg on e.emailID = eg.emailID where groupID = " + groupID + " and subscribeTypeCode = 'S' order by firstname, lastname, emailaddress");
			dgEmailList.DataSource = dt;
			dgEmailList.DataBind();
			EmailsPager.RecordCount = dt.Rows.Count;
			// Put user code to initialize the page here
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
	}
}
