namespace ecn.wizard.wizard
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text.RegularExpressions;
	using ecn.common.classes;
	using System.IO;
	using System.Configuration;

	public partial class CreateMessage : ecn.wizard.MasterControl, IWizard
	{

        

		

		public void Initialize() 
		{
			lblMsg.Visible = false;
			pEditor.Visible = false;
			
			HeaderImg.Attributes.Add("Readonly","true");

			lblMsg.Text = "";

			ShowValues();

			if (ChannelID == 20)
			{
				email.Text= ConfigurationManager.AppSettings["Channel_" + ChannelID.ToString() + "_Fromaddress"].ToString();
				pnlfromaddress.Visible=false;
			}

			if (WizardSession.ContentSource == string.Empty) 
			{
				PopulateContents(WizardSession.TemplateID);	
			}

			if (WizardSession.IsCustomHeader)
				pnlCustomHeader.Visible=true;				
			else
				pnlCustomHeader.Visible=false;
		}

		public bool Save() 
		{
			if (Page.IsValid)
			{

				if (WizardSession.IsCustomHeader)
				{

					if (fHeaderImg.PostedFile.FileName != string.Empty)
					{

						string ImgPath = System.IO.Path.GetFileName(fHeaderImg.PostedFile.FileName).Replace(" ","_");

						if (!(ImgPath.ToLower().EndsWith(".gif") || ImgPath.ToLower().EndsWith(".jpg"))) 
						{
							lblMsg.Text = "Only gif and jpg images can be uploaded.";
							lblMsg.Visible=true;
							return false;
						} 

						string targetPath = "";
						if (ImgPath != string.Empty)
						{
							ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
				
							//ConfigurationManager.AppSettings["LocalPath"] +

							targetPath =  Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + CustomerID + "/Images/");

							if(!Directory.Exists(targetPath))
								Directory.CreateDirectory(targetPath);

							fHeaderImg.PostedFile.SaveAs(targetPath + ImgPath);
				
							WizardSession.HeaderImage = ImgPath;
						}
					}
					else
					{
						if (HeaderImg.Value == string.Empty)
						{
							lblMsg.Text = "Logo cannot be empty. Please upload the logo.";
							lblMsg.Visible=true;
							return false;
						}
					}

				}

				if (contents.Text == string.Empty)
				{
					lblMsg.Text = "Content text cannot be empty";
					lblMsg.Visible = true;
					return false;
				}

				if (rblFirst.Checked) 
					WizardSession.Salutation = rblFirst.Value;
				else 
					WizardSession.Salutation= rblFirstLast.Value;
				
				// Store All Values into session
				WizardSession.ContentSource = VerifyCodeSnippets(contents.Text);
				WizardSession.ContentText = contents.Text;
				WizardSession.MessageTitle= msgTitle.Text; 
				WizardSession.EmailAddress = email.Text;
				WizardSession.Name = name.Text;
				WizardSession.EmailSubject = emailSubject.Text;
				WizardSession.FooterName = footerName.Text;
				WizardSession.FooterTitle = footerTitle.Text; 
				WizardSession.FooterCompany = footerCompany.Text; 
				WizardSession.FooterPhone = footerPhone.Text; 

				return true;
			}
			else
				return false;
		}


		private void PopulateContents (int TemplateID) 
		{
			try 
			{
				string Content = "";

				string sql = "SELECT ContentSource FROM Content WHERE ContentID = "+
					" (SELECT ContentID FROM TemplateContents WHERE TemplateID=" + TemplateID + ")";

				Content = Convert.ToString(DataFunctions.ExecuteScalar(sql));

				if (Session["ContentID"] != null)
				{
					if (Session["ContentID"].ToString() != string.Empty)
					{
						sql = "SELECT ContentSource FROM Content WHERE ContentID = "+ Session["ContentID"].ToString();

						Content = Content.Replace("%%video%%", Convert.ToString(DataFunctions.ExecuteScalar(sql)));
					}
				}
				else
				{
					Content = Content.Replace("%%video%%", "");
				}


				contents.Text = Content;
				WizardSession.ContentSource = contents.Text;

				if (contents.Text == string.Empty)
				{
					pEditor.Visible = true;
				}
			} 
			catch
			{
				throw;
			}
		}

		private void ShowValues () 
		{
			msgTitle.Text = WizardSession.MessageTitle;
			email.Text = WizardSession.EmailAddress;
			name.Text = WizardSession.Name;
			emailSubject.Text = WizardSession.EmailSubject;

			if (WizardSession.Salutation.EndsWith("last"))
				rblFirstLast.Checked = true;
			else
				rblFirst.Checked = true;

			footerName.Text = WizardSession.FooterName;
			footerTitle.Text = WizardSession.FooterTitle;
			footerCompany.Text = WizardSession.FooterCompany;
			footerPhone.Text = WizardSession.FooterPhone;

			contents.Text = WizardSession.ContentSource;

			if (WizardSession.IsCustomHeader)
			{
				pnlCustomHeader.Visible=true;
				HeaderImg.Value = WizardSession.HeaderImage;
			}
			else
			{
				pnlCustomHeader.Visible=false;
				HeaderImg.Value = "";
			}
		}

		// Just get the contents from the editor and check for [%%EventName%%] [%%BadgeID%%]
		// all these code-snippets and delete any random code-snippets
		private string VerifyCodeSnippets (string csrc) 
		{
			// check for %%firstname%%
			string regexString = @"%{2}[a-zA-Z]+%{2}";		// code-nippets
			Regex regex = new Regex(regexString);
			MatchCollection mc = regex.Matches(csrc);
			//bool flag = false;
			for (int i=0; i<mc.Count; i++) 
			{
				if (mc[i].Value.Equals("%%EventName%%") || mc[i].Value.Equals("%%BadgeID%%")) 
					continue;//flag = true;
				else
					csrc.Replace(mc[i].Value, "");
			}
			
			return csrc;
		}

		/// <summary>
		/// Given an HTML embeded string, this method extracts onyl the Text part from it, ignoring any HTML fromating.
		/// </summary>
		/// <param name="html">HTML embeded string</param>
		/// <returns>Plain string withought HTML formatting</returns>
	

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
			this.ibtnEdit.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnEdit_Click);

		}
		#endregion


		private void ibtnEdit_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
		{
			pEditor.Visible = true;
		}

	}
}
