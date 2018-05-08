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
using System.IO;
using ecn.common.classes;
using ecn.showcare.wizard.main;
using System.Text.RegularExpressions;
using System.Configuration;

namespace ecn.showcare.wizard
{
	/// <summary>
	/// Summary description for UpdateMessage.
	/// </summary>
	public class UpdateMessage : System.Web.UI.Page
	{
		protected CKEditor.NET.CKEditorControl contents;
		protected System.Web.UI.HtmlControls.HtmlInputText msgTitle;
		protected System.Web.UI.HtmlControls.HtmlInputText email;
		protected System.Web.UI.HtmlControls.HtmlInputText name;
		protected System.Web.UI.HtmlControls.HtmlInputText emailSubject;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton Radio1;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton Radio2;
		protected System.Web.UI.HtmlControls.HtmlInputText footerName;
		protected System.Web.UI.HtmlControls.HtmlInputText footerTitle;
		protected System.Web.UI.HtmlControls.HtmlInputText footerCompany;
		protected System.Web.UI.HtmlControls.HtmlInputText footerPhone;
		protected System.Web.UI.WebControls.ImageButton btnBack;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvMessageTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvHeaderFromName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmailSubject;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvFooterName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvFooterTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCompany;
		protected Steps steps = new Steps();
		protected Step2 step2;
		protected System.Web.UI.WebControls.ImageButton btnNext;
		protected System.Web.UI.WebControls.Label lblMsg;
		protected System.Web.UI.WebControls.RegularExpressionValidator valEmailAddress;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.Panel pnlCustomHeader;
		protected System.Web.UI.HtmlControls.HtmlInputFile fHeaderImg;
		protected System.Web.UI.HtmlControls.HtmlInputText HeaderImg;
		protected string contenttext="";
		/* ================================ FLOW ================================
		 * Initial Condition:	
		 *		Populate editor with default contents of the template selected.
		 *		Editor is "readonly"
		 * Required Fields:
		 *		msgTitle, email, name, emailSubject, footerName, footerTitle, footerCompany
		 *		at least one salutation
		 * Optional Fields:
		 *		footerPhone
		 * Upon Submit:
		 *		Check if all required fields are entered [javascript]
		 *		Transfer the control to code-behind
		 *			[imp] Check to see if user has not messed-up the code-snippets [imp]	
		 *				// including; check existing code snippets 
		 *					They can have only Salutation [FirstName/First-LastName, FromName, Title, CompanyName, PhoneNumber]
		 *				// check to see if they haven't entered mallicious code-snippet
		 *		if all is well SUBMIT the form
		 *			Save details in the SESSION object [think about creating object for each page, may be a structure!]
		 *				MsgName, FromEmail, FromName, EmailSubject, Salutation, FooterFromName, 
		 *				FooterTitle, FooterCompanyName, FooterPhone, ContentID.
		 *		otherwise pop-up the appropriate message
		 * 
		 *  ============================== FLOW END ==============================
		 */

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Hide error message label
			lblMsg.Visible = false;

			HeaderImg.Attributes.Add("Readonly","true");
			// Set Last visited step to current
			if (!IsPostBack && (steps.Current > steps.LastVisited)) {
				steps.nextStep();
			}
			// You want to create step2 object one and only one time
			if ((steps.Current == steps.LastVisited) && (Session["step2"] == null)) {	// That means this is the first time user is visitng this page
				step2 = new Step2();
			} else {
				step2 = (Step2) Session["step2"];
			}



			if (!IsPostBack && (steps.LastVisited > steps.Current)) { // That means user is coming back from the next step. So populate whatever you can
				ShowValues();
			} else if (!IsPostBack) {	// Populate content-editor with the default contents for the Template selected.
				Step1 step1 = (Step1)Session["step1"];
				PopulateContents(step1.TemplateID);	

				if (step1.IsCustomHeader)
					pnlCustomHeader.Visible=true;				
				else
					pnlCustomHeader.Visible=false;
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
			this.btnBack.Click += new System.Web.UI.ImageClickEventHandler(this.btnBack_Click);
			this.btnNext.Click += new System.Web.UI.ImageClickEventHandler(this.btnNext_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void PopulateContents (string TemplateID) {
			string sql = "SELECT ContentSource FROM Content WHERE ContentID = "+
				" (SELECT ContentID FROM TemplateContents WHERE TemplateID="+TemplateID+")";
			try {
				contents.Text = Convert.ToString(DataFunctions.ExecuteScalar(sql));
			} catch {	// Add exception handling here 
			}
		}

		private void ShowValues () {
			msgTitle.Value = step2.MessageName;
			email.Value = step2.EmailAddress;
			name.Value = step2.HeaderFromName;
			emailSubject.Value = step2.EmailSubject;
			
			Step1 step1 = (Step1)Session["step1"];
			if (step1.IsCustomHeader)
			{
				pnlCustomHeader.Visible=true;
				HeaderImg.Value = step2.HeaderImage;
			}
			else
			{
				pnlCustomHeader.Visible=false;
				HeaderImg.Value = "";
			}

			if (step2.Salutation.EndsWith("last"))
				Radio2.Checked = true;
			else
				Radio1.Checked = true;

			footerName.Value = step2.FooterName;
			footerTitle.Value = step2.FooterTitle;
			footerCompany.Value = step2.FooterCompany;
			footerPhone.Value = step2.FooterPhone;

			contents.Text = step2.ContentSource;
		}

		// Just get the contents from the editor and check for [%%EventName%%] [%%BadgeID%%]
		// all these code-snippets and delete any random code-snippets
		private string VerifyCodeSnippets (string csrc) {
			// check for %%firstname%%
			string regexString = @"%{2}[a-zA-Z]+%{2}";		// code-nippets
			Regex regex = new Regex(regexString);
			MatchCollection mc = regex.Matches(csrc);
			//bool flag = false;
			for (int i=0; i<mc.Count; i++) {
				if (mc[i].Value.Equals("%%EventName%%") || mc[i].Value.Equals("%%BadgeID%%")) 
					continue;//flag = true;
				else
					csrc.Replace(mc[i].Value, "");
			}
			/*if (!flag) {
				lblMsg.Text = ConfigurationManager.AppSettings["cserrmsg"];
				lblMsg.Visible = true;
			}*/
			return csrc;
		}

		/// <summary>
		/// Given an HTML embeded string, this method extracts onyl the Text part from it, ignoring any HTML fromating.
		/// </summary>
		/// <param name="html">HTML embeded string</param>
		/// <returns>Plain string withought HTML formatting</returns>
		private string StripTextFromHtml(string html){
			Regex regExp = new Regex("<(.|\n)+?>");
			string strOutput = "";
			//Replace all HTML tag matches with the empty string
			strOutput = regExp.Replace(html, "");
				  
			//Replace all < and > with &lt; and &gt; and &nbsp; with " " space
			strOutput = strOutput.Replace("&lt;", "<");
			strOutput = strOutput.Replace("&gt;", ">");
			strOutput = strOutput.Replace("&nbsp;", " ");
				  
			//ContentText.Text=StringFunctions.Replace(ContentText.Text,"&nbsp;"," ");
			return strOutput;
		}

		private void btnNext_ServerClick(object sender, System.Web.UI.ImageClickEventArgs e) 
		{
			//Hashtable ht = Wizard.ParseUrl(Request.RawUrl);
			string sal = "";
			if (Radio1.Checked) 
			{
				sal = Radio1.Value;
			} 
			else if (Radio2.Checked) 
			{
				sal = Radio2.Value;
			}

			// Before saving everything check to see if code-snippets in the content are fine.
			// !--> There are not going to be any code-snippets in the content or template [from the latest discussion with Ashok]
			// !--> Instead I'll have to instert Dear <salutation> in the beginning and <footer info> at the end
			// Dated: 03/13 Paul said event-id and badge-id are not going to be there always
			//			if (VerifyCodeSnippets (ht["salutation"].ToString())) {					
			// Get the Cotnent Text from the Content Source
			string cnt = VerifyCodeSnippets(contents.Text);
			//			if (!lblMsg.Visible) {
			contenttext = DataFunctions.CleanString(StripTextFromHtml(contents.Text));

			Step1 step1 = (Step1)Session["step1"];
			Step2 step2 = new Step2(msgTitle.Value, email.Value, name.Value, emailSubject.Value, sal, footerName.Value,
				footerTitle.Value, footerCompany.Value, footerPhone.Value, cnt, contenttext, HeaderImg.Value);

			if (step1.IsCustomHeader)
			{

				if (fHeaderImg.PostedFile.FileName != string.Empty)
				{

					string ImgPath = System.IO.Path.GetFileName(fHeaderImg.PostedFile.FileName).Replace(" ","_");

					if (!(ImgPath.ToLower().EndsWith(".gif") || ImgPath.ToLower().EndsWith(".jpg") || ImgPath.ToLower().EndsWith(".jpeg"))) 
					{
						lblMsg.Text = "Only gif and jpg images can be uploaded.";
						lblMsg.Visible=true;
						return;
					} 

					string targetPath = "";
					if (ImgPath != string.Empty)
					{
						ChannelCheck cc = new ChannelCheck();
						SecurityCheck sc= new SecurityCheck();
				
						//ConfigurationManager.AppSettings["LocalPath"] +

						targetPath =  Server.MapPath(cc.getAssetsPath("accounts")+"/"+"channelID_"+ ConfigurationManager.AppSettings["ChannelID"] +"/customers/"+step1.CustomerID +"/images/");

						if(!Directory.Exists(targetPath))
							Directory.CreateDirectory(targetPath);

						fHeaderImg.PostedFile.SaveAs(targetPath + ImgPath);
				
						step2.HeaderImage = ImgPath;
					}
				}
				else
				{
					if (HeaderImg.Value == string.Empty)
					{
						lblMsg.Text = "Logo cannot be empty. Please upload the logo.";
						lblMsg.Visible=true;
						return;
					}
				}

			}

			step2.ToSession();

			steps.increamentCurrentStep();							// Increment current step to next
			Response.Redirect("PreviewEmail.aspx");
			//			}
			//			} else {
			// Display error
			//			}
		}

		private void btnBack_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			// check with Ashok, if we need to clear session is user is going back to previous step. If YES then just uncomments following line
			//step2.ClearAll();
			steps.decreamentCurrentStep();							// Decrement current step to previous one.
			Response.Redirect("ChooseTemplate.aspx");
		}
	}
}
