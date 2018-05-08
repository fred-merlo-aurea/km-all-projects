using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace ecn.collector.main.survey.UserControls
{
	public partial class DefineSurvey  : System.Web.UI.UserControl, IWizard
	{

		int _surveyID = 0;
        		
		protected System.Web.UI.WebControls.Label EmailURL;

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
                    ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);					
					
					if (objSurvey != null)
					{
						plSurveyNew.Visible = false;
						lblCopySurveyID.Text = "";
						txtSurveyTitle.Text = objSurvey.SurveyTitle;
						txtSurveyDesc.Text = objSurvey.Description;
						if (objSurvey.EnableDate != null)
							txtActivationDate.Text = Convert.ToDateTime(objSurvey.EnableDate).ToString("MM/dd/yyyy");

                        if (objSurvey.DisableDate != null)
							txtDeActivationDate.Text = Convert.ToDateTime(objSurvey.DisableDate).ToString("MM/dd/yyyy");						
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
            try
            {
                ECN_Framework_Entities.Collector.Survey objSurvey;
                if (SurveyID == 0)
                {
                    objSurvey = new ECN_Framework_Entities.Collector.Survey();
                    objSurvey.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    objSurvey.CompletedStep = 1;
                }
                else
                {
                    objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    objSurvey.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                }
                objSurvey.SurveyTitle = txtSurveyTitle.Text;
                objSurvey.Description = txtSurveyDesc.Text;
                objSurvey.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                try
                {
                    objSurvey.EnableDate = Convert.ToDateTime(txtActivationDate.Text);
                }
                catch
                {
                    objSurvey.EnableDate = null;
                }
                try
                {
                    objSurvey.DisableDate = Convert.ToDateTime(txtDeActivationDate.Text);
                }
                catch
                {
                    objSurvey.DisableDate = null;
                }           
                ECN_Framework_BusinessLayer.Collector.Survey.Save(objSurvey, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                this._surveyID = objSurvey.SurveyID;
                if (lblCopySurveyID.Text != "" && Convert.ToInt32(lblCopySurveyID.Text) > 0)
                {
                    ECN_Framework_BusinessLayer.Collector.Survey.Copy(this._surveyID, Convert.ToInt32(lblCopySurveyID.Text), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
                return true;
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                ErrorMessage = "";
                foreach (ECN_Framework_Common.Objects.ECNError error in ex.ErrorList)
                {
                    ErrorMessage = ErrorMessage + error.ErrorMessage + "<br/>";
                }
                return false;
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion


		public void ServerSide_isDate (object source, ServerValidateEventArgs args) 
		{
			args.IsValid = false;
			try
			{
				DateTime dt = DateTime.Parse(args.Value);
			}
			catch (Exception) 
			{
				return;
			}
			args.IsValid = true;
		}


		protected void rbNewSurvey_CheckedChanged(object sender, System.EventArgs e)
		{
			plCopySurvey.Visible = false;
			reset();
		}

		protected void rbCopySurvey_CheckedChanged(object sender, System.EventArgs e)
		{
            List<ECN_Framework_Entities.Collector.Survey> surveyList = ECN_Framework_BusinessLayer.Collector.Survey.GetByCustomerID( ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID,  ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            drpSurvey.DataSource = surveyList;
			drpSurvey.DataBind();
			drpSurvey.Items.Insert(0, new ListItem("Select Existing Survey","0"));
			plCopySurvey.Visible = true;
		}

		protected void drpSurvey_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(Convert.ToInt32(drpSurvey.SelectedItem.Value), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);					
					
			if (objSurvey != null)
			{
				lblCopySurveyID.Text = drpSurvey.SelectedItem.Value;
				if (objSurvey.EnableDate != null)
                    txtActivationDate.Text = Convert.ToDateTime(objSurvey.EnableDate).ToString("MM/dd/yyyy");
                if (objSurvey.DisableDate != null)
                    txtDeActivationDate.Text = Convert.ToDateTime(objSurvey.DisableDate).ToString("MM/dd/yyyy");
			}
			else
			{
				reset();
			}			
		}

		private void reset()
		{
			lblCopySurveyID.Text = "";
			txtActivationDate.Text = "";
			txtDeActivationDate.Text = "";			
		}
	}
}
