using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Mail;

namespace ecn.common.classes {



    public class error : ECN_Framework.WebPageHelper
    {
		protected System.Web.UI.WebControls.Label lblMsgWithSCDReason;
		protected System.Web.UI.WebControls.Label lblMsgWithOutSCDReason;
		protected System.Web.UI.WebControls.Panel errMsgWithSCD_Pnl;
		protected System.Web.UI.WebControls.Panel errMsgWithOutSCD_Pnl;
		protected System.Web.UI.WebControls.TextBox txtErr;
		protected System.Web.UI.WebControls.Image errorMsgImage;
	

		
		/// Displays task message depending on the status code
		
		/// <param name="cd">Status Code</param>
		private void withStatusCode (int cd) {
			string errorMsg = "";
			errorMsgImage.ImageUrl = "/ecn.images/images/with_SCD.jpg";

			switch (cd) {
				case 0: {
					errorMsg = "UNKNOWN ERROR";
					break;
				}
				case 400: {
					errorMsg = "Requested page not found.";
					break;
				}
				case 401: {
					errorMsg = "Not Authorized to view this page.";
					break;
				}
				case 403: {
					errorMsg = "Requested page can not be displayed.";
					break;
				}
				case 404: {
					errorMsg = "Cannot find web site.";
					break;
				}
				case 405: {
					errorMsg = "Page can not be displayed.";
					break;
				}
				default: {
					withoutStatusCode();
					break;
				}
			}
			lblMsgWithSCDReason.Text			= errorMsg;
			errMsgWithSCD_Pnl.Visible			= true;
			lblMsgWithOutSCDReason.Text	= "";
			errMsgWithOutSCD_Pnl.Visible	= false;
			txtErr.Visible = false;
		}

		private void withoutStatusCode() {
			try {
				Exception e = (Exception)Application["err"];
				int customerID = Convert.ToInt32(new ECN_Framework.Common.SecurityCheck().CustomerID().ToString());
				int userID			= Convert.ToInt32(new ECN_Framework.Common.SecurityCheck().UserID().ToString());
				errorMsgImage.ImageUrl = "/ecn.images/images/withOut_SCD.jpg";

				StringBuilder emailBody = new StringBuilder("&body=");
				emailBody.Append(e.StackTrace+e.InnerException);

				lblMsgWithOutSCDReason.Text	= e.Message;
				errMsgWithOutSCD_Pnl.Visible	= true;
				lblMsgWithSCDReason.Text			= "";
				errMsgWithSCD_Pnl.Visible			= false;

				string taskStack = e.Message+"\n"+e.Source+"\n"+e.StackTrace+"\n"+e.InnerException;
				txtErr.Text = taskStack;

				string errorPath = Request.QueryString["aspxerrorpath"].ToString();
				string product = "";
				if(errorPath.IndexOf("ecn.communicator") > 0)
					product = "COMMUNICATOR";
				else if (errorPath.IndexOf("ecn.accounts") > 0)
					product = "ACCOUNTS";
				else if (errorPath.IndexOf("ecn.collector") > 0)
					product = "COLLECTOR";
				else if (errorPath.IndexOf("ecn.creator") > 0)
					product = "CREATOR";


				ECNTasks ecnTask = new ECNTasks(customerID, "COMMUNICATOR", e.Message.ToString(), Request.QueryString["aspxerrorpath"].ToString(), taskStack.ToString(),"low", "pending", "");
				int taskID = ecnTask.insertECNTask();
				if(taskID > 0){
					ecnTask.taskID = taskID;
					string addressbarURL = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString();
					string notifyEmailBody = "";
					notifyEmailBody	= "<table width=100% border=0><tr><td style=\"font-family:'verdana'; font-size:11;\">";
					notifyEmailBody	+= "<b>"+e.Message.ToString()+"</b><br>";
					notifyEmailBody	+= "<br><b>Product:</b>&nbsp;"+product;
					notifyEmailBody	+= "<br><b>Customer ID:</b>&nbsp;"+customerID;
					notifyEmailBody	+= "<br><b>User ID:</b>&nbsp;"+userID+"&nbsp;&nbsp;[<a href='http://www.ecn5.com/ecn.accounts/main/users/userlogin.aspx?UserID="+userID+"'>Login here</a>]";
					notifyEmailBody	+= "<br><b>SPY Info:</b>&nbsp;["+Request.UserHostAddress+"] / ["+Request.UserAgent+"]";
					notifyEmailBody	+= "<br><b>Raw URL Path:</b>&nbsp;"+addressbarURL;
					notifyEmailBody	+= "<br><b>Task Page Path:</b>&nbsp;"+errorPath;
					notifyEmailBody	+= "<br><br><b>Task Stack:</b><br>"+taskStack.Replace("\n","<br>").ToString();
					notifyEmailBody	+= "<br><br><b>Online Task Status:</b>&nbsp;<a href='http://www.ecn5.com/ecn.intranet.reports/taskhandling/ecntaskslist.aspx?tskID="+taskID+"' >Click to Update Status</a>";
					notifyEmailBody	+= "</td></tr></table>";

					ecnTask.notifyTaskstatus(notifyEmailBody);
				}
			}catch{
				Server.ClearError();				
			}
		}



		
		/// Converts object to int
		
		/// <param name="obj">object to convert</param>
		/// <returns>converted int value</returns>
		private int toInt (object obj) {
			if (obj != null) {
				try {
					int i = Convert.ToInt32(obj);
					return i;
				} catch {
					//lblMsg.Text = "There seems to be some kind of mistake. Please contact support@knowledgemarketing.com, with a detailed description";
					return 0;
				}
			} else {
				withoutStatusCode();
				return 0;
			}
		}

		private void Page_Load(object sender, System.EventArgs e) {
			if (Request.QueryString["scd"] != null) {
				int cd = toInt(Request.QueryString["scd"]);
				withStatusCode(cd);
			}
			else {
				withoutStatusCode();
			}
			Server.ClearError();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}