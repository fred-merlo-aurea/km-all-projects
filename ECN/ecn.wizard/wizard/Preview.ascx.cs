namespace ecn.wizard.wizard
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Data.SqlClient;
	using ecn.common.classes;
	using ecn.wizard.Component;
	using System.Configuration;

	/// <summary>
	///		Summary description for Preview.
	/// </summary>
	public partial class Preview : ecn.wizard.MasterControl, IWizard
	{

		public void Initialize() 
		{
			string sql = "SELECT TemplateSource FROM Templates WHERE TemplateID=" + WizardSession.TemplateID;
			string body = Convert.ToString(DataFunctions.ExecuteScalar(sql));

			string ContentSource = WizardSession.ContentSource;

			// Before sshowing the preview we need to add code-snippet and footer information to content-source
			if (WizardSession.Salutation.Equals("firstname")) 
			{
				ContentSource = "<P align=left>Dear <strong>%%FirstName%%</strong>,</P>" + ContentSource;
			} 
			else 
			{
				ContentSource = "<P align=left>Dear <strong>%%FirstName%% %%LastName%%</strong>,</P>" + ContentSource;
			}
			// In any case we need to insert Footer-Information in the end
			ContentSource += "<P align=left>"+
				WizardSession.FooterName+"<BR>"+
				WizardSession.FooterTitle+"<BR>"+
				WizardSession.FooterCompany+"<BR>"+
				WizardSession.FooterPhone+"<BR></P>";

			if (WizardSession.IsCustomHeader)
			{
				string headerImage = "";
				if (WizardSession.HeaderImage != string.Empty)
				{
					ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
                    headerImage = "<IMG src='" + System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/Customers/" + CustomerID + "/Images/" + WizardSession.HeaderImage + "' border=0>";
				}
				else
					headerImage = "&nbsp;";

				ContentSource = "<TR><TD style='HEIGHT: 100px;border:0px solid #afc2d5;' valign='middle' ><center>" + headerImage + "</center></TD></TR><TR><TD style='PADDING-RIGHT: 30px; PADDING-LEFT: 30px; FONT-SIZE: 12px; PADDING-BOTTOM: 30px; PADDING-TOP: 30px'>" + ContentSource + "</TD></TR>";
			}

			body = StringFunctions.Replace(body,"%%slot1%%",ContentSource);
			body=StringFunctions.Replace(body,"®","&reg;");
			body=StringFunctions.Replace(body,"©","&copy;");
			body=StringFunctions.Replace(body,"™","&trade;");
			body=StringFunctions.Replace(body,"…","...");
			body=StringFunctions.Replace(body,((char)0).ToString(),"");
			body="<table><tr><td align=left>"+body+"</td></tr></table>";

			if (WizardSession.IsCustomHeader)
			{
				body = "<DIV style='FONT-WEIGHT: bold; FONT-SIZE: 12px; COLOR: red'>If your image is wider than the borders on the screen below, your image is more than 650px(pixels) wide.<BR>For better results, reduce the width of your image to 650px..</DIV><bR>" + body;
			}

			previewLbl.Text = body;
		}


		public bool Save() 
		{

			if (Page.IsValid)
			{
				if (WizardSession.ProcessCC)
					return true;
				else
				{
					try
					{
						SaveWizard sWizard = new SaveWizard(ChannelID,CustomerID, UserID);

						if (PerformChecks())
						{
							sWizard.Save();
							return true;
						}
					}
					catch(Exception ex)
					{
						lblError.Text = ex.Message;
					}
				}
				return false;
			}
			else
				return false;
		}

		private bool PerformChecks() 
		{
			string msg = ConfigurationManager.AppSettings["createerrmsg"];

			// Errors for unknown reasons. Mainly because there is no session contents available to successfully process datastore.
			if (WizardSession.GroupID == -1 || WizardSession.TemplateID == -1) 
			{
				msg = msg.Replace("%%errmsg%%", "Could not find information about selected Group and Template.");
				lblError.Text = msg;
				return false;
			}

			if (WizardSession.Name == string.Empty || WizardSession.ContentSource == string.Empty) 
			{
				msg = msg.Replace("%%errmsg%%", "Could not find information regarding Message.");
				lblError.Text = msg;

				return false;
			}

			return true;
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

		}
		#endregion

	}
}


