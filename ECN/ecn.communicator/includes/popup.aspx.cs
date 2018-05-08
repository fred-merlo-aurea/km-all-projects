using System;
using System.Web;
using System.Web.SessionState;
using ecn.common.classes;
using ecn.communicator.classes;

namespace ecn.communicator.includes {
	
	
	
	public partial class popup : System.Web.UI.Page	{
	
		protected void Page_Load(object sender, System.EventArgs e) {
			
			string action = "";
			try{
				action= Request.Params["action"].ToString();
			}catch{
				action = "";
			}

			if(action.Equals("") || action.Length == 0 || action.Equals("err")){
				ImgLabel.Text = "<img src='warning2.gif'>";
				MsgLabel.Text = PopUp.PopupMsg.ToString();
				MsgFooterLabel.Visible = true;
				PopUp.PopupMsg = "";
			}else if(action.Equals("msg")){
				ImgLabel.Text = "";
				MsgLabel.Text = PopUp.PopupMsg.ToString();
				PopUp.PopupMsg = "";
				MsgFooterLabel.Visible = false;
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
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
