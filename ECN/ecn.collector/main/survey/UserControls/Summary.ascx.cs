namespace ecn.collector.main.survey.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
    using System.Collections.Generic;

	public partial class Summary : System.Web.UI.UserControl, IWizard
	{

		int _surveyID = 0;

		public int SurveyID
		{
			set 
			{
				_surveyID = value;
			}
			get 
			{
				return _surveyID;
			}
		}

		string _errormessage = string.Empty;
		public string ErrorMessage
		{
			set 
			{
				_errormessage = value;
			}
			get 
			{
				return _errormessage;
			}
		}

		public void Initialize() 
		{
			if (SurveyID != 0)
			{
				try
				{
					string vp = System.Configuration.ConfigurationManager.AppSettings["Collector_VirtualPath"];
                    string hn = HttpContext.Current.Request.UrlReferrer.Host;
                    
                    txtBlastURL.Text = "http://" + Request.ServerVariables["HTTP_HOST"] + vp + "/front/default.aspx?sid=" + SurveyID + "&bid=%%blastid%%&uid=%%EmailAddress%%";

                    txtURL.Text = "http://" + Request.ServerVariables["HTTP_HOST"] + vp + "/front/default.aspx?sid=" + SurveyID;


					idPreview.NavigateUrl="~/front/view_survey.aspx?sid=" + SurveyID;
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);					
                    lblTitle.Text = objSurvey.SurveyTitle;
					lblActivationDate.Text = objSurvey.EnableDate.ToString();
					lblDeActivationDate.Text = objSurvey.DisableDate.ToString();
					lblStatus.Text = objSurvey.IsActive?"Active":"Inactive";

					rbStatus.ClearSelection();
					rbStatus.Items.FindByValue(objSurvey.IsActive?"Y":"N").Selected = true;
                    List<ECN_Framework_Entities.Collector.Question> questionList = ECN_Framework_BusinessLayer.Collector.Question.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    List<ECN_Framework_Entities.Collector.Page> PageList = ECN_Framework_BusinessLayer.Collector.Page.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    lblTotalPages.Text = PageList.Count.ToString();
                    lblTotalQuestions.Text = questionList.Count.ToString();
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(objSurvey.GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    List<ECN_Framework_Entities.Communicator.EmailGroup> emailGroupList = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByGroupID(objSurvey.GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (group!=null)
					{
                        lblGroupName.Text = group.GroupName;
                        lblTotalParticpants.Text = emailGroupList.Count.ToString();
					}
				}
				catch (Exception ex)
				{
					ErrorMessage = ex.Message;
				}
			}
		}

		public bool Save() 
		{
			if (Page.IsValid)
			{
				try
				{
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);										
					objSurvey.IsActive = rbStatus.SelectedItem.Value.ToUpper()=="Y"?true:false;
					objSurvey.CompletedStep = 5;
                    ECN_Framework_BusinessLayer.Collector.Survey.Save(objSurvey, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
				}
				catch (Exception ex)
				{
					ErrorMessage = ex.Message;
				}
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
