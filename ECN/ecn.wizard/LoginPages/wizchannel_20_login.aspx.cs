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

namespace ecn.wizard.LoginPages
{
	/// <summary>
	/// Summary description for wizchannel_20_login.
	/// </summary>
	public partial class wizchannel_20_login : System.Web.UI.Page
	{
		public string msgValue
		{
			get
			{ 
				string theMsgValue= "";
				try 
				{
					theMsgValue	= Request.QueryString["msg"].ToString();
				}
				catch(Exception E) 
				{
					string devnull=E.ToString();
				}
				return theMsgValue;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e) 
		{
			// Put user code to initialize the page here
			switch (msgValue)
			{
				case "InvalidLogin":
					MsgLabel.Visible = true;
					MsgLabel.Text = "Login Invalid. Please Retry";
					break;
				case "InActiveLogin":
					MsgLabel.Visible = true;
					MsgLabel.Text = "Login Inactive. Please Contact Admin";
					break;
				default:
					MsgLabel.Visible = false;
					MsgLabel.Text = "";
					break;
			}
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
