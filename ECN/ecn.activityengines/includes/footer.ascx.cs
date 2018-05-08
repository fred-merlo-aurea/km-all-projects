namespace ecn.activityengines.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.communicator.classes;
	using ecn.common.classes;

	
	///		Summary description for footer.
	
	public partial  class footer : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
		
            //ECN_Framework.Common.ECNSession es = ECN_Framework.Common.ECNSession.CurrentSession();

            //string FooterSource = TemplateFunctions.channelTemplate(
            //    es.FooterSource("communicator"), es.UserName, System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/" + es.BaseChannelID,
            //    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, es.BaseChannelName
            //    );
            //footerOutput.Text = FooterSource;
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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
