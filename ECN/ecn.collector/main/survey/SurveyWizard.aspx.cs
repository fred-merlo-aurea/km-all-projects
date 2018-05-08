using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.controls;

namespace ecn.collector.main.survey
{
    public partial class SurveyWizard : Page
	{
	    private const string CurrentWizardStepImageUrlTemplate = "/ecn.images/images/sur_step{0}_s.jpg";
	    private const string UncompletedWizardStepImageUrlTemplate = "/ecn.images/images/sur_step{0}_d.jpg";
	    private const string MouseOverImageUrlTemplate = "/ecn.images/images/sur_step{0}_h.jpg";
	    private const string ContentPlaceholderName = "ContentPlaceHolder1";
	    private const string TabName = "btnStep";

	    ArrayList WizardSteps = new ArrayList();
		Control ctlWizard;

		private int StepIndex 
		{
			get 
			{
				if (ViewState["StepIndex"] == null)
					return 1;
				else
					return (int)ViewState["StepIndex"];
			}

			set { ViewState["StepIndex"] = value; }
		}

		private int SurveyID 
		{
			get 
			{
				if (ViewState["SurveyID"] == null)
					return 0;
				else
					return (int)ViewState["SurveyID"];
			}

			set { ViewState["SurveyID"] = value; }
		}

		private int getTabID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["tab"].ToString());
			}
			catch
			{
				return 1;
			}
		}

		private int getSurveyID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["SurveyID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			phError.Visible = false;

			btnHome.Attributes.Add("onclick","javascript:return confirm('All current changes will be lost! Do you want to continue to Survey Home page?')");
			btnCancel1.Attributes.Add("onclick","javascript:return confirm('All current changes will be lost! Do you want to continue to Survey Home page?')");
			btnCancel2.Attributes.Add("onclick","javascript:return confirm('All current changes will be lost! Do you want to continue to Survey Home page?')");
			WizardSteps.Add("usercontrols/DefineSurvey.ascx");
			WizardSteps.Add("usercontrols/DefineQuestions.ascx");
			WizardSteps.Add("usercontrols/DefineTemplate.ascx");
			WizardSteps.Add("usercontrols/DefineIntro.ascx");
			WizardSteps.Add("usercontrols/Summary.ascx");

			btnNext1.Attributes.Add("onmouseover","this.src='/ecn.images/images/next_h.gif';");
			btnNext1.Attributes.Add("onmouseout","this.src='/ecn.images/images/next.gif';");
			btnNext2.Attributes.Add("onmouseover","this.src='/ecn.images/images/next_h.gif';");
			btnNext2.Attributes.Add("onmouseout","this.src='/ecn.images/images/next.gif';");

			btnPrevious1.Attributes.Add("onmouseover","this.src='/ecn.images/images/Previous_h.gif';");
			btnPrevious1.Attributes.Add("onmouseout","this.src='/ecn.images/images/Previous.gif';");
			btnPrevious2.Attributes.Add("onmouseover","this.src='/ecn.images/images/Previous_h.gif';");
			btnPrevious2.Attributes.Add("onmouseout","this.src='/ecn.images/images/Previous.gif';");

			btnCancel1.Attributes.Add("onmouseover","this.src='/ecn.images/images/Cancel_h.gif';");
			btnCancel1.Attributes.Add("onmouseout","this.src='/ecn.images/images/Cancel.gif';");
			btnCancel2.Attributes.Add("onmouseover","this.src='/ecn.images/images/Cancel_h.gif';");
			btnCancel2.Attributes.Add("onmouseout","this.src='/ecn.images/images/Cancel.gif';");

			btnSave1.Attributes.Add("onmouseover","this.src='/ecn.images/images/Save_h.gif';");
			btnSave1.Attributes.Add("onmouseout","this.src='/ecn.images/images/Save.gif';");
			btnSave2.Attributes.Add("onmouseover","this.src='/ecn.images/images/Save_h.gif';");
			btnSave2.Attributes.Add("onmouseout","this.src='/ecn.images/images/Save.gif';");

			if (!IsPostBack)
			{
				SurveyID = getSurveyID() ;
				if (SurveyID > 0)
				{

                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);

					if (objSurvey != null)
					{
						StepIndex = objSurvey.CompletedStep==0?1:(objSurvey.CompletedStep==5?5:objSurvey.CompletedStep+1);

						EnableTabBar(StepIndex);
					}	
					else
					{
						lblErrorMessage.Text = "ERROR - Survey does not exist.";
						phError.Visible = true;
						btnNext1.Visible = false;
						btnNext2.Visible = false;
						return;
					}
				}
				else
				{
					StepIndex=1;
				}
				LoadWizardStep(true);
			}
			else
			{
				LoadWizardStep(false);
			}
		}

		void LoadWizardStep(bool Initialize) 
		{
			try
			{
				if (SurveyID > 0)
				{
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);
					EnableTabBar(objSurvey.CompletedStep==0?1:(objSurvey.CompletedStep==5?5:objSurvey.CompletedStep+1));
				}
				else
				{
					EnableTabBar(0);
				}

				ctlWizard = Page.LoadControl((string)WizardSteps[StepIndex-1]);

				ctlWizard.ID = "SurveyWizard";
				((IWizard)ctlWizard).SurveyID = SurveyID;

				phwizContent.Controls.Clear();
				phwizContent.Controls.Add(ctlWizard);

				if (Initialize)
					((IWizard)ctlWizard).Initialize();

				btnNext1.Visible = true;
				btnNext2.Visible = true;
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = ex.Message;
				phError.Visible = true;
				btnNext1.Visible = false;
				btnNext2.Visible = false;
			}
		}

		private void EnableTabBar(int completedStep)
		{
            var mainContent = Master.FindControl(ContentPlaceholderName) as ContentPlaceHolder;
		    if (mainContent == null || WizardSteps == null)
		    {
                return;
		    }

		    TabBarHelpers.EnableTabBar(
		        WizardSteps.Count, 
		        StepIndex,
		        completedStep, 
		        stepIndex => mainContent.FindControl($"{TabName}{stepIndex}") as ImageButton,
		        stepIndex => string.Format(CurrentWizardStepImageUrlTemplate, stepIndex),
		        stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex),
		        stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex),
		        stepIndex => string.Format(MouseOverImageUrlTemplate, stepIndex),
		        stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex));	
		}

		protected void btnPrevious_Click(object sender, System.EventArgs e)
		{
			StepIndex --;
			LoadWizardStep(true);
		}

		protected void btnNext_Click(object sender, System.EventArgs e)
		{
			if (((IWizard)ctlWizard).Save()) 
			{
				SurveyID = ((IWizard)ctlWizard).SurveyID;

				StepIndex ++;
				if (StepIndex == WizardSteps.Count+1)
					Response.Redirect( "default.aspx" );
				else
					LoadWizardStep(true);
			}
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect( "default.aspx" );
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			if (((IWizard)ctlWizard).Save()) 
				Response.Redirect( "default.aspx" );
		}

		protected void Page_PreRender(object sender, System.EventArgs e) 
		{
			if (((IWizard)ctlWizard) == null)
			{
				btnNext1.Visible=false;
				btnNext2.Visible=false;
				btnSave1.Visible=false;
				btnSave2.Visible=false;
				btnCancel1.Visible = false;
				btnCancel2.Visible = false;
				btnPrevious1.Visible = false;
				btnPrevious2.Visible = false;
				btnStep1.Visible=false;
				btnStep2.Visible=false;
				btnStep3.Visible=false;
				btnStep4.Visible=false;
				btnStep5.Visible=false;
			}
			else
			{
				if (((IWizard)ctlWizard).ErrorMessage != string.Empty) 
				{
					lblErrorMessage.Text = ((IWizard)ctlWizard).ErrorMessage;
					phError.Visible= true;
				}

				if (StepIndex == 1)
				{
					btnPrevious1.Visible = false;
					btnPrevious2.Visible = false;
				}
				else
				{
					btnPrevious1.Visible = true;
					btnPrevious2.Visible = true;
				}

				if (StepIndex == WizardSteps.Count)
				{
					btnNext1.Visible=true;
					btnNext2.Visible=true;
					btnNext1.Text="Finish&nbsp;&raquo;";
					btnNext2.Text="Finish&nbsp;&raquo;";
					btnSave1.Visible=false;
					btnSave2.Visible=false;
				}
				else
				{
					btnNext1.Visible=true;
					btnNext2.Visible=true;
					btnNext1.Text="Next&nbsp;&raquo;";
					btnNext2.Text="Next&nbsp;&raquo;";
					btnSave1.Visible=true;
					btnSave2.Visible=true;

				}
			}
		}

		private void btnStep1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SaveonTabClick(1);
		}

		private void btnStep2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SaveonTabClick(2);
		}

		private void btnStep3_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SaveonTabClick(3);
		}

		private void btnStep4_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SaveonTabClick(4);
		}

		private void btnStep5_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SaveonTabClick(5);
		}

		private void SaveonTabClick(int Step)
		{
			//((IWizard)ctlWizard).WizardID = WizardID;
			if (((IWizard)ctlWizard).Save()) 
			{
				StepIndex=Step;
				LoadWizardStep(true);
			}
		}



		protected void btnHome_Click(object sender, System.EventArgs e)
		{
			Response.Redirect( "default.aspx" );
//			if (((IWizard)ctlWizard) == null)
//			{
//				Response.Redirect( "default.aspx" );
//			}
//			else
//			{
//				if (((IWizard)ctlWizard).Save()) 
//					Response.Redirect( "default.aspx" );
//			}
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
			this.btnStep1.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep1_Click);
			this.btnStep2.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep2_Click);
			this.btnStep3.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep3_Click);
			this.btnStep4.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep4_Click);
			this.btnStep5.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep5_Click);

		}
		#endregion


	}
}
