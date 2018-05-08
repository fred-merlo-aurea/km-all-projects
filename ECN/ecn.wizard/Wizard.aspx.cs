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
//using ecn.wizard.wizard;
using ecn.common.classes;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for Wizard.
	/// </summary>
	public partial class Wizard : ecn.wizard.MasterPage
	{
	
		ArrayList WizardSteps = new ArrayList();
		protected System.Web.UI.WebControls.PlaceHolder wizContent;
		Control ctlWizard;
		string ChargeCard;

		private void InitializeGroup()
		{
			
			//Page.RegisterHiddenField( "__EVENTTARGET", btnNext.ClientID);

			if (WizardSession.GroupID == -1)
				if (Request.QueryString["gid"] != null && Request.QueryString["gid"].ToString() != string.Empty )
					WizardSession.GroupID = Convert.ToInt32(Request.QueryString["gid"]);

			if (StepIndex == (WizardSteps.Count - 1))
			{
				btnBack.Visible=false;
				btnNext.Visible=false;
			}
		}


		private int StepIndex 
		{
			get 
			{
				if (ViewState["StepIndex"] == null)
					return 0;
				else
					return (int)ViewState["StepIndex"];
			}

			set { ViewState["StepIndex"] = value; }
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			ChargeCard = Convert.ToString(DataFunctions.ExecuteScalar("accounts","select ChargeCrCard from wizard_BaseFee where BaseChannelID=" + ChannelID));
			
			if (ChargeCard == string.Empty || ChargeCard.ToUpper() == "Y")
				WizardSession.ProcessCC = true;


			WizardSteps.Add("wizard/ChooseTemplate.ascx");
			WizardSteps.Add("wizard/CreateMessage.ascx");
			WizardSteps.Add("wizard/Preview.ascx");
			
			if (WizardSession.ProcessCC)
				WizardSteps.Add("wizard/CreditCardSend.ascx");

			WizardSteps.Add("wizard/Confirmation.ascx");

			InitializeGroup();
			LoadWizardStep();
			
		}

		protected void Page_PreRender(object sender, System.EventArgs e) 
		{

			if (StepIndex == 0 || StepIndex == (WizardSteps.Count - 1))
				btnBack.Visible = false;
			else
				btnBack.Visible = true;

			if (StepIndex == (WizardSteps.Count - 2))
				if (ChannelID == 2)
				{
					btnNext.Visible=false;
				}
				else
				{
					btnNext.ImageUrl="~/images/btn_send_email.gif";
				}
				else if (StepIndex == (WizardSteps.Count - 1))
					btnNext.ImageUrl="~/images/btn_back_to_main_menu.gif";
				else
					btnNext.ImageUrl="~/images/btn_continue.gif";


		}

		void LoadWizardStep() 
		{
			ctlWizard = Page.LoadControl((string)WizardSteps[StepIndex]);


			if (StepIndex == (WizardSteps.Count - 1))
			{
				if (WizardSession.ProcessCC)
					wizImage.ImageUrl = "~/images/img_step5.1.gif";
				else
					wizImage.ImageUrl = "~/images/img_step5.gif";
			}
			else
				wizImage.ImageUrl = "~/images/img_step" + (StepIndex + 1) + ".gif";

			ctlWizard.ID = "ECNWizard";

			phwizContent.Controls.Clear();
			phwizContent.Controls.Add(ctlWizard);

			((IWizard)ctlWizard).Initialize();
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
			this.btnNext.Click += new System.Web.UI.ImageClickEventHandler(this.btnNext_Click);

		}
		#endregion

		private void btnBack_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			StepIndex --;
			LoadWizardStep();
		}

		private void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			if (((IWizard)ctlWizard).Save()) 
			{
				StepIndex ++;
				if (StepIndex == WizardSteps.Count)
					Response.Redirect( "default.aspx" );
				else
					LoadWizardStep();
			}
		}
	}
}
