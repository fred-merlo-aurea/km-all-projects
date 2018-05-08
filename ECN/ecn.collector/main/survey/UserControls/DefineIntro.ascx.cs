using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.collector.main.survey.UserControls
{
	public partial class DefineIntro : System.Web.UI.UserControl, IWizard
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
			if (SurveyID > 0)
			{
				try
                {
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);					
					IntroHTML.Text = objSurvey.IntroHTML;
                    ThankYouHTML.Text = objSurvey.ThankYouHTML;

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
                    objSurvey.IntroHTML = IntroHTML.Text;
                    objSurvey.ThankYouHTML = ThankYouHTML.Text;
					objSurvey.CompletedStep = 4;
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
