using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using ecn.communicator.classes;
using ecn.common.classes;
using ecn.collector.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes
{	
	public partial class emailProfile_Survey : EmailProfileBaseControl
    {
        DataTable surveysForEmailProfileDT = null;
		string collectordb=ConfigurationManager.AppSettings["collectordb"];
        private string _emailId = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return messageLabel;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "emailID specified does not Exist. Please click on the 'Profile' link in the email message that you received");

			loadSurveyForEmailProfile();
		}

        private void loadSurveyForEmailProfile() {
            string surveysForEmailProfileSQL = "SELECT inn.*, EmailSubject " +
                        " FROM " +
                        "(SELECT s.Survey_title AS 'SurveyTitle', g.GroupName AS 'SurveyList', s.Survey_ID AS 'SurveyID', "+
                        "   MAX(CASE WHEN ShortName LIKE '%_completionDt' THEN DataValue END) AS 'CompletedDate', " +
                        "   MAX(CASE WHEN ShortName LIKE '%_blastID' THEN DataValue END) AS 'BlastID' " +
                        "   FROM ecn5_collector..Survey s " +
                        "   JOIN Groups g ON s.Group_ID = g.GroupID " +
                        "   JOIN EmailGroups eg ON g.GroupID = eg.GroupID " +
                        "   JOIN GroupDataFields gdf ON g.GroupID = gdf.GroupID AND (shortname = CONVERT(VARCHAR, s.Survey_id) + '_completionDt' OR shortname = CONVERT(VARCHAR, s.Survey_id) + '_BlastID') " +
                        "   JOIN EmailDataValues edv ON gdf.GroupDataFieldsID = edv.GroupDataFieldsID AND edv.EmailID = eg.EmailID " +
                        "   WHERE eg.EmailID = " + _emailId + " and s.IsDeleted = 0 and gdf.IsDeleted = 0 " +
                        "   Group BY  s.Survey_title,g.GroupName, s.Survey_ID " +
                        ") inn LEFT OUTER JOIN Blast b ON  b.BlastID= inn.BlastID where b.StatusCode <> 'Deleted' ";

            surveysForEmailProfileDT = DataFunctions.GetDataTable(surveysForEmailProfileSQL);
            surveysDataList.DataSource = surveysForEmailProfileDT.DefaultView;
            surveysDataList.DataBind();
		}

        protected void surveysDataList_ItemDataBound( object sender, DataListItemEventArgs e ) {
            Repeater repQuestions = e.Item.FindControl("repQuestions") as Repeater;
            loadQuestionGrid(repQuestions, (int) surveysDataList.DataKeys[(int)e.Item.ItemIndex]);
        }

        private void loadQuestionGrid(Repeater repQuestions, int surveyID) {
            ViewState["SURVEYID"] = surveyID.ToString();
            repQuestions.DataSource = Survey.GetAllQuestions(Convert.ToInt32(surveyID), 0);
            repQuestions.DataBind();
        }

        private DataTable loadAnswersGrid( int QuestionID ) {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["col"]);
            SqlCommand cmd = new SqlCommand("sp_getQuestionResponse", conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            int emailId;
            int.TryParse(_emailId, out emailId);

            cmd.Parameters.Add("@question_ID", SqlDbType.Int);
            cmd.Parameters["@question_ID"].Value = QuestionID;
            cmd.Parameters.Add("@emailID", SqlDbType.Int);
            cmd.Parameters["@emailID"].Value = emailId;
            cmd.Parameters.Add("@filterstr", SqlDbType.VarChar);
            cmd.Parameters["@filterstr"].Value = "";
            cmd.Parameters.Add("@count", SqlDbType.Int);
            cmd.Parameters["@count"].Value = 0;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            da.Fill(ds, "sp_getQuestionResponse");
            conn.Close();
            return ds.Tables[0];
        }

        public void repQuestions_ItemDataBound( object sender, RepeaterItemEventArgs e ) {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                Label lblQuestionID = (Label)e.Item.FindControl("lblQuestionID");
                PlaceHolder plresponse = (PlaceHolder)e.Item.FindControl("plresponse");
                DataGrid dgGridResponse = (DataGrid)e.Item.FindControl("dgGridResponse");

                Question question = Question.LoadQuestionsByID(Convert.ToInt32(lblQuestionID.Text));

                int emailId;
                int.TryParse(_emailId, out emailId);

                question.LoadResponsesFromDT(Survey.GetResponses(Convert.ToInt32(ViewState["SURVEYID"].ToString()), question.QuestionNumber, emailId));

                if(question.AddTextControl) {
                    try {
                        string txtvalue = Survey.GetTEXTResponses(Convert.ToInt32(ViewState["SURVEYID"].ToString()), question.QuestionNumber, emailId);

                        if(txtvalue != string.Empty)
                            question.AddAnswer(-2, txtvalue);
                    } catch { }
                }

                if(question.QuestionType.ToLower() == "grid") {
                    dgGridResponse.DataSource = loadAnswersGrid(question.QuestionID);
                    dgGridResponse.DataBind();

                    dgGridResponse.Visible = true;
                } else {
                    dgGridResponse.Visible = false;
                    plresponse.Controls.Add(createOptionControl(question));
                }

                if(question.AddTextControl) {
                    Control c = createOptionTextControl(question);

                    if(c != null) {
                        plresponse.Controls.Add(c);
                    }
                }
            }
        }

        private Control createOptionTextControl( Question question ) {
            foreach(Answer answer in question.Answers) {
                if(answer.ID == -2) {
                    TextBox txtOptions = new TextBox();
                    txtOptions.Width = 300;
                    txtOptions.ReadOnly = true;
                    txtOptions.CssClass = "label10";
                    txtOptions.Text = answer.AnswerValue;
                    return txtOptions;
                }
            }
            return null;

        }

        private Control createOptionControl( Question question ) {
            switch(question.QuestionType.ToLower()) {
                case "checkbox":
                    CheckBoxList chkOption = new CheckBoxList();
                    chkOption.DataSource = question.Options;
                    chkOption.DataTextField = "OptionText";
                    chkOption.DataValueField = "OptionValue";
                    chkOption.CssClass = "label10";
                    chkOption.Enabled = false;
                    chkOption.DataBind();

                    chkOption.ClearSelection();
                    foreach(Answer answer in question.Answers) {
                        try {
                            if(answer.ID == -1)
                                chkOption.Items.FindByValue(answer.AnswerValue).Selected = true;
                        } catch { }
                    }

                    return chkOption;
                case "dropdown":
                    DropDownList ddlOption = new DropDownList();
                    ddlOption.CssClass = "label10";
                    ddlOption.DataSource = question.Options;
                    ddlOption.DataTextField = "OptionText";
                    ddlOption.DataValueField = "OptionValue";
                    ddlOption.Enabled = false;
                    ddlOption.DataBind();
                    ddlOption.Items.Insert(0, new ListItem("", ""));
                    ddlOption.ClearSelection();
                    foreach(Answer answer in question.Answers) {
                        try {
                            if(answer.ID == -1)
                                ddlOption.Items.FindByValue(answer.AnswerValue).Selected = true;
                        } catch { }
                    }

                    return ddlOption;
                case "radio":
                    RadioButtonList rdoOptions = new RadioButtonList();
                    rdoOptions.Width = 700;
                    rdoOptions.CssClass = "label10";
                    rdoOptions.DataSource = question.Options;
                    rdoOptions.Enabled = false;
                    rdoOptions.DataTextField = "OptionText";
                    rdoOptions.DataValueField = "OptionValue";
                    rdoOptions.DataBind();
                    rdoOptions.ClearSelection();
                    foreach(Answer answer in question.Answers) {
                        try {
                            if(answer.ID == -1)
                                rdoOptions.Items.FindByValue(answer.AnswerValue).Selected = true;
                        } catch { }
                    }
                    return rdoOptions;
                case "textbox":
                    TextBox txtOptions = new TextBox();
                    txtOptions.CssClass = "label10";
                    if(question.MaxLength > 0 && question.MaxLength < 40) {
                        txtOptions.Attributes.Add("style", "WIDTH:99%; HEIGHT:20px");
                    } else {
                        txtOptions.TextMode = TextBoxMode.MultiLine;
                        txtOptions.Attributes.Add("style", "WIDTH:99%; HEIGHT:50px");
                    }
                    txtOptions.ReadOnly = true;

                    foreach(Answer answer in question.Answers) {
                        txtOptions.Text = answer.AnswerValue;
                    }

                    return txtOptions;

                default:
                    throw new ApplicationException(string.Format("Unknown type of control -- {0}", question.QuestionType.ToLower()));

            }
        }

        public void dgGridResponse_ItemDataBound( object sender, System.Web.UI.WebControls.DataGridItemEventArgs e ) {
            if(e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item) {
                e.Item.Cells[0].Visible = false;
                e.Item.Cells[1].Visible = false;
                e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Item.Cells[2].Width = Unit.Percentage(30);

                for(int i = 3; i < e.Item.Cells.Count; i++) {
                    if(i % 2 == 0) {
                        if(e.Item.Cells[i].Text == "0")
                            e.Item.Cells[i].Text = "";
                        else
                            e.Item.Cells[i].Text = "<img src='/ecn.images/images/tick.gif' border='0'>";
                    } else
                        e.Item.Cells[i].Visible = false;

                }
            } else if(e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer) {
                e.Item.Cells[0].Visible = false;
                e.Item.Cells[1].Visible = false;
                e.Item.Cells[2].Text = "";

                for(int i = 3; i < e.Item.Cells.Count; i++) {
                    if(i % 2 != 0)
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
