using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.communicator.includes
{	
	public partial class emailProfile_Survey : EmailProfileBaseControl
    {
        private string _emailId = string.Empty;
        private string _groupId=string.Empty;
		string collectordb=ConfigurationManager.AppSettings["collectordb"];

        protected override Label lblResultMessage
        {
            get
            {
                return MessageLabel;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "EmailID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            _groupId = GetFromQueryString("gID", "GroupID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            var surveyID = GetFromQueryString("surveyID", "Survey ID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
			var surveyName = getSurveyName(surveyID);
            SurveyNameLabel.Text = string.Format("{0} Details", surveyName);

			LoadSurveyDetails(surveyID);
		}

		private string getSurveyName(string sID) {
			string theDataFieldSetName = "";
			try {
				string sql = "SELECT Survey_title FROM "+collectordb+".dbo.Survey WHERE Survey_ID	= "+sID;
				theDataFieldSetName = DataFunctions.ExecuteScalar(sql).ToString();
			}catch {
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: DataFieldSetID specified does not Exist. Please click on the 'Profile' link in the email message that you received";
			}
			return theDataFieldSetName;
		}

		private void LoadSurveyDetails(string sID){
			string surveyAnswers_sql =	" SELECT g.ShortName, e.DataValue FROM  "+
                " GroupDatafields g JOIN EmailDataValues e ON g.GroupDatafieldsID = e.GroupDatafieldsID" +
				" WHERE  g.GroupID= "+_groupId+
				" AND e.EmailID= "+_emailId+" AND DataFieldSetID is null and surveyID = "+sID;
			DataTable surveyAnswersDT = DataFunctions.GetDataTable(surveyAnswers_sql);

			string surveyQuestions_sql =	" SELECT number, question_text FROM "+
									""+collectordb+".dbo.question WHERE survey_ID = "+sID+
									" ORDER BY number ";
			DataTable surveyQuestionsDT = DataFunctions.GetDataTable(surveyQuestions_sql);

			DataRow[] surveyCompletionDR = null;
			string surveyCompletionDate = "";
			try{
				surveyCompletionDR = surveyAnswersDT.Select("ShortName = '"+sID+"_completionDt'");
				surveyCompletionDate = surveyCompletionDR[0]["DataValue"].ToString();
			}catch{
				//survey not completed.
			}

			string surveyTable = "<table style='BORDER-RIGHT: #eeeeee 1px solid; BORDER-TOP: #eeeeee 1px solid; BORDER-LEFT: #eeeeee 1px solid; "+
						" BORDER-BOTTOM: #eeeeee 1px solid' cellSpacing='2' cellPadding='2' width=100%>";
			surveyTable += "<TR class='tableHeader1'><TD>Survey was completed on: "+surveyCompletionDate+"</td></tr>";
			foreach ( DataRow surveyQuestionsDR in surveyQuestionsDT.Rows ) {
				surveyTable += "<TR class='tableHeader1'><TD>"+surveyQuestionsDR["number"]+". "+surveyQuestionsDR["question_text"]+"</TD></tr>";
				DataRow[] surveyAnswersDR = surveyAnswersDT.Select("ShortName = '"+sID+"_"+surveyQuestionsDR["number"].ToString()+"'");
				for(int i=0; i < surveyAnswersDR.Length; i++){
					surveyTable += "<TR class='tableContent'><TD>"+surveyAnswersDR[i]["DataValue"]+"</TD></tr>";
				}
				surveyTable += "<TR><TD height='6px'></TD></tr>";
			}
			surveyTable += "</table>";

			SurveyDetailsLabel.Text = surveyTable;
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
