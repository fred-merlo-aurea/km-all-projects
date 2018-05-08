using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using ECN_Framework_Common.Objects;
using System.Collections.Generic;


namespace ecn.collector.main.report
{

    public partial class ViewResponse : Page
	{
        private int SurveyID 
		{
			get 
			{
				try{return Convert.ToInt32(Request.QueryString["surveyID"]);}
				catch{return 0;}
			}
		}

		private int EmailID
		{
			get 
			{
				try{return Convert.ToInt32(Request.QueryString["EmailID"]);}
				catch{return 0;}
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
                Master.CurrentMenuCode = ECN_Framework_Common.Objects.Collector.Enums.MenuCode.SURVEY; 
				lnkToRespondents.NavigateUrl="javascript:history.back();";
				lnkSurveyResults.NavigateUrl="survey_report.aspx?surveyID=" + SurveyID.ToString();
                ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);
                lblSurveyTitle.Text = s.SurveyTitle; ;
				LoadRespondent();
				LoadQuestionGrid();
			}
		}

		private void LoadRespondent()
		{
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);           
            ECN_Framework_Entities.Communicator.Email Email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(EmailID, Master.UserSession.CurrentUser);
			if (Email!=null)
			{
                lblEmailAddress.Text = Email.EmailAddress;
                ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(SurveyID.ToString() + "_" + "completionDt", s.GroupID, Master.UserSession.CurrentUser);
                if (gdf != null)
                {
                    List<ECN_Framework_Entities.Communicator.EmailDataValues> edvList = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(gdf.GroupDataFieldsID, EmailID, Master.UserSession.CurrentUser);
                    lblCompletedDate.Text = edvList[0].DataValue;
                }
			}
		}

		private void LoadQuestionGrid() 
		{
            List<ECN_Framework_Entities.Collector.Question> qList = ECN_Framework_BusinessLayer.Collector.Question.GetBySurveyID(SurveyID, Master.UserSession.CurrentUser);
            var result = (from src in qList
                          orderby src.Number
                          select src).ToList();
            repQuestions.DataSource = result;
			repQuestions.DataBind();
		}

		private DataTable LoadAnswersGrid(int QuestionID)
		{
            return ECN_Framework_BusinessLayer.Collector.Participant.GetQuestionResponse(EmailID, QuestionID, string.Empty, 0);
		}

		public void repQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Label lblQuestionID = (Label)e.Item.FindControl("lblQuestionID");
				PlaceHolder plresponse = (PlaceHolder)e.Item.FindControl("plresponse");
				DataGrid dgGridResponse = (DataGrid) e.Item.FindControl("dgGridResponse");
                ECN_Framework_Entities.Collector.Question question = ECN_Framework_BusinessLayer.Collector.Question.GetByQuestionID(Convert.ToInt32(lblQuestionID.Text));

                List<ECN_Framework_Entities.Collector.Response> responseList = ECN_Framework_BusinessLayer.Collector.Response.GetByQuestion(SurveyID, question.Number, EmailID, Master.UserSession.CurrentUser);
                ECN_Framework_BusinessLayer.Collector.Question.LoadResponses(question, responseList);

				if (question.ShowTextControl)
				{
					try
					{

                        string txtvalue = ECN_Framework_BusinessLayer.Collector.Response.GetTEXTResponses(SurveyID, question.Number, EmailID, Master.UserSession.CurrentUser);
                        if (txtvalue != string.Empty)
                            ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, -2, txtvalue);
					}
					catch{}
				}

				if (question.Format.ToLower() == "grid")
				{
					dgGridResponse.DataSource = LoadAnswersGrid(question.QuestionID);
					dgGridResponse.DataBind();

					dgGridResponse.Visible = true;
				}
				else
				{
					dgGridResponse.Visible = false;
					plresponse.Controls.Add(CreateOptionControl(question));
				}

				if (question.ShowTextControl)
				{
					Control c = CreateValueControl(question);

					if (c != null)
					{
						plresponse.Controls.Add(c);	
					}
				}

			}
		}

        private Control CreateValueControl(ECN_Framework_Entities.Collector.Question question)
		{
            foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
            {
				if (answer.ID==-2)
				{
					TextBox txtOptions = new TextBox();
					txtOptions.Width =300;
					txtOptions.ReadOnly = true;
					txtOptions.CssClass="label10";
					txtOptions.Text = answer.Value;
					return txtOptions;
				}
			}
			return null;

		}

        private Control CreateOptionControl(ECN_Framework_Entities.Collector.Question question) 
		{
            List<ECN_Framework_Entities.Collector.ResponseOptions> roList = ECN_Framework_BusinessLayer.Collector.ResponseOptions.GetByQuestionID(question.QuestionID, Master.UserSession.CurrentUser);

			switch (question.Format.ToLower()) 
			{
				case "checkbox":
					CheckBoxList chkOption = new CheckBoxList();
                    chkOption.DataSource = roList;
                    chkOption.DataTextField = "OptionValue";
					chkOption.DataValueField = "OptionID";
					chkOption.CssClass = "label10";
					chkOption.Enabled=false;
					chkOption.DataBind();
					
					chkOption.ClearSelection();
                    foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
                    {
						try
						{
							if (answer.ID==-1)
								chkOption.Items.FindByText(answer.Value).Selected = true;
						}
						catch{}
					}

					return chkOption;
				case "dropdown":
					DropDownList ddlOption = new DropDownList();	
					ddlOption.CssClass = "label10";
                    ddlOption.DataSource = roList;
					ddlOption.DataTextField = "OptionValue";
					ddlOption.DataValueField = "OptionID";
					ddlOption.Enabled=false;
					ddlOption.DataBind();	
					ddlOption.Items.Insert(0, new ListItem("",""));
					ddlOption.ClearSelection();
                    foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
                    {
						try
						{
							if (answer.ID==-1)
								ddlOption.Items.FindByText(answer.Value).Selected = true;
						}
						catch{}
					}
		
					return ddlOption;					
				case "radio":
					RadioButtonList rdoOptions = new RadioButtonList();
					rdoOptions.Width = 700;
					rdoOptions.CssClass = "label10";
                    rdoOptions.DataSource = roList;
					rdoOptions.Enabled=false;
					rdoOptions.DataTextField = "OptionValue";
					rdoOptions.DataValueField = "OptionID";
					rdoOptions.DataBind();
						rdoOptions.ClearSelection();
                        foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
                        {
						try
						{
							if (answer.ID==-1)
								rdoOptions.Items.FindByText(answer.Value).Selected = true;
						}
						catch{}
					}
					return rdoOptions;
				case "textbox":
					TextBox txtOptions = new TextBox();
					txtOptions.CssClass="label10";
					if (question.MaxLength > 0 && question.MaxLength < 40)
					{
						txtOptions.Width =300;
						txtOptions.Attributes.Add("style","width:250px");
					}
					else
					{
						txtOptions.TextMode = TextBoxMode.MultiLine;
						txtOptions.Columns=150;
                        txtOptions.Rows = 5;
					}
					txtOptions.ReadOnly = true;

                    foreach (ECN_Framework_Entities.Collector.Response answer in question.ResponseList)
                    {
						txtOptions.Text = answer.Value;
					}
					
					return txtOptions;

				default:
					throw new ApplicationException(string.Format("Unknown type of control -- {0}", question.Format.ToLower()));

			}
		}

		public void dgGridResponse_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				e.Item.Cells[0].Visible = false;
				e.Item.Cells[1].Visible = false;
				e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Left;
				e.Item.Cells[2].Width= Unit.Percentage(30);

				for (int i=3;i<e.Item.Cells.Count;i++)
				{
					if (i % 2 == 0)
					{
						if (e.Item.Cells[i].Text == "0" )
							e.Item.Cells[i].Text = ""; 
						else
							e.Item.Cells[i].Text = "<img src='/ecn.images/images/tick.gif' border='0'>";
					}
					else
						e.Item.Cells[i].Visible = false;
 						
				}
			}
			else if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
			{
				e.Item.Cells[0].Visible = false;
				e.Item.Cells[1].Visible = false;
				e.Item.Cells[2].Text="";

				for (int i=3;i<e.Item.Cells.Count;i++)
				{
					if (i % 2 != 0)
						e.Item.Cells[i].Visible = false;
				}
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

        protected void btnDeleteResponse_Click(object sender, EventArgs e)
        {
            try
            {
                ECN_Framework_BusinessLayer.Collector.Response.Delete(SurveyID, EmailID, Master.UserSession.CurrentUser);
                Response.Redirect("survey_report.aspx?surveyID=" + SurveyID);
            }
            catch (Exception ex)
            {
                Response.Write("ERROR : " + ex.Message);
            }
        }
	}
}
