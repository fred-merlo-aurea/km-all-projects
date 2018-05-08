using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Configuration;
using ecn.common.classes;

namespace ecn.collector.classes {
	public class Question {
		private ArrayList _options;
		private ArrayList _gridStatements;
		private ArrayList _answers;

		private int _questionID;
		private int _pageID;
		private string _questionText;
		private int _questionNumber;
		private string _questionType;
		private string _gridControlType;
		private int _maxLength=0;
		private bool _isrequired;
		private int _gridvalidation=0;
		private bool _BranchingExists=false;
		private bool _AddTextControl=false;
        public static string collectordb = ConfigurationManager.AppSettings["collectordb"];

		#region Properites

		public int QuestionID 
		{
			get {return (this._questionID);}
			set {this._questionID = value;}
		}	

		public int PageID 
		{
			get {return (this._pageID);}
			set {this._pageID = value;}
		}	

		public string QuestionText 
		{
			get{return (this._questionText);}
			set {this._questionText = value;}
		}			
			
		public string QuestionForSql 
		{
			get { return DataFunctions.CleanString(_questionText);}
		}

		
		public int QuestionNumber 
		{
			get {return (this._questionNumber);}
			set {this._questionNumber = value;}
		}		
		
		public string QuestionType 
		{
			get {return (this._questionType);}
			set {this._questionType = value;}
		}
		
		public string GridControlType 
		{
			get {return (this._gridControlType);}
			set {this._gridControlType = value;}
		}
		public int MaxLength 
		{
			get {return (this._maxLength);}
			set {this._maxLength = value;}
		}
		public bool IsRequired
		{
			get {return (this._isrequired);}
			set {this._isrequired = value;}
		}

		public int GridValidation 
		{
			get {return (this._gridvalidation);}
			set {this._gridvalidation = value;}
		}	

		public bool BranchingExists
		{
			get {return (this._BranchingExists);}
			set {this._BranchingExists = value;}
		}

		public bool AddTextControl
		{
			get {return (this._AddTextControl);}
			set {this._AddTextControl = value;}
		}

		public ArrayList Options 
		{
			get { return _options;}
		}

		public ArrayList GridStatements 
		{
			get 
			{ 
				if (this.QuestionType != "grid") 
				{
					throw new ApplicationException("Non-grid type question doesn't have grid statement properties.");
				}
				return _gridStatements;}
		}

		public ArrayList Answers 
		{
			get {return (this._answers);}			
		}

		public bool HasAnswer 
		{
			get { return _answers.Count > 0;}
		}

		public bool IsCompleted 
		{
			get 
			{
				switch (QuestionType) 
				{
					case "grid":
						return GridStatements.Count == Answers.Count;						
					default:
						return HasAnswer;
				}			
			}
		}
		#endregion

		public Question(int questionID, int pageID, string questionText, int questionNumber, string questionType, int maxLength, bool isrequired, int gridvalidation, string gridcontroltype, bool addTextControl) 
		{
			_questionID =questionID;
			_pageID = pageID;
			_options = new ArrayList();
			_gridStatements = new ArrayList();
			_answers = new ArrayList();
			_questionText = questionText;
			_questionNumber = questionNumber;						
			_questionType = questionType;
			_maxLength = maxLength;
			_isrequired = isrequired;
			_gridvalidation = gridvalidation;
			_gridControlType = gridcontroltype;
			_AddTextControl= addTextControl;
		}

		#region Public Methods

		public void LoadGridStatements(int QuestionID) 
		{
			DataTable GridStatements = Survey.GetGridStatements(QuestionID);
			foreach(DataRow grid in GridStatements.Rows) 
			{
				Gridstatement statement = new Gridstatement(Convert.ToInt32(grid["GridStatementID"]), Convert.ToString(grid["GridStatement"]));
				_gridStatements.Add(statement);
			}
		}

		public void Save(int participantID, int surveyID) {
			Survey.DeleteResponse(participantID, surveyID, this.QuestionNumber); 
			switch( this.QuestionType) {
				case "dropdown":
				case "checkbox":
				case "radio":	

					if (this.AddTextControl)
						Survey.DeleteResponse(participantID, surveyID, surveyID+ "_" + this.QuestionNumber+ "_TEXT");

					foreach(Answer answer in this.Answers) 
					{
						if (answer.ID == -1 && answer.AnswerValue.Trim() != string.Empty)
							Survey.InsertResponse(participantID, surveyID, answer.AnswerValue, this.QuestionNumber);
						else if (answer.ID == -2 && answer.AnswerValue.Trim() != string.Empty)
							Survey.InsertResponse(participantID, surveyID, answer.AnswerValue, surveyID+ "_" + this.QuestionNumber+ "_TEXT");
					}
					
					break;
				case "textbox":
					if (this.Answers.Count != 1) {
						throw new ApplicationException(string.Format("Expected only one answer for '{0}' type of question, but there are {1} answers.", this.QuestionType, this.Answers.Count ));
					}
					Answer ans = (Answer) this.Answers[0];
					if (ans.AnswerValue.Trim() == string.Empty) {
						break;
					}
					Survey.InsertResponse(participantID, surveyID, ans.AnswerValue, this.QuestionNumber);
					break;
				case "grid":
					foreach(Answer answer in this.Answers) {
						if (answer.AnswerValue.Trim() == "") {
							continue;
						}
						Survey.InsertResponse(participantID, surveyID, answer.AnswerValue, this.QuestionNumber, answer.ID);
					}
					break;
				default:
					throw new ApplicationException(string.Format("Unknown type of question -- {0}", this.QuestionType));
			}
		}

		public string OptionList {
			get {
				if (_options.Count == 0) {
					return "(-1)";
				}

				StringBuilder list = new StringBuilder("(");
				for(int i=0; i< _options.Count; i++) {
					if (i !=0) {
						list.Append(",");
					}
					list.Append( ((Option) _options[i]).ID);
				}
				list.Append(")");
				return list.ToString();
			}
		}

		public void LoadResponsesFromDT(DataTable responses) {
			if (responses.Rows.Count == 0){
				return;
			}
			string answer;
			switch (this.QuestionType) {
				case "checkbox":
					foreach(DataRow row in responses.Rows) {
						answer = row["DataValue"].ToString();
						if (answer.Trim() == string.Empty) {
							continue;
						}
						this.AddAnswer(answer);
					}
					break;
				case "dropdown":						
				case "radio":					
				case "textbox":
					answer = responses.Rows[0]["DataValue"].ToString();
					if (answer.Trim().Length > 0) {
						this.AddAnswer(answer);
					}
					break;
				case "grid":
					foreach(DataRow row in responses.Rows) {
						answer = row["DataValue"].ToString();
						if (answer.Trim() == string.Empty) {
							continue;
						}
						this.AddAnswer(Convert.ToInt32(row["SurveyGridID"]), answer);
					}
					break;
				default:
					throw new ApplicationException(string.Format("Unknown type of Question -- {0}", this.QuestionType));
				
			}
		}

		public void ResetAnswers() {
			_answers.Clear();
		}
		public void AddAnswer(int id, string val) {
			_answers.Add(new Answer(id, val));
		}

		public void AddAnswer(string val) {
			this.AddAnswer(-1, val);
		}

		public Answer GetAnswerByID(int id) {
			foreach(Answer answer in _answers) {
				if (answer.ID == id) {
					return answer;
				}
			}
			return new Answer(-1, "");
		}
		public override string ToString() {
			return string.Format("{0}. {1}", this.QuestionNumber,this.QuestionText);
		}
		#endregion

		#region Static Methods

		public static ArrayList LoadQuestionsByPageID(int PageID) 
		{
			DataTable dtQuestions = Survey.GetAllQuestions(0, PageID);		
			
			ArrayList questions = new ArrayList();
			foreach(DataRow question in dtQuestions.Rows) 
			{
				Question q = new Question(Convert.ToInt32(question["questionID"]),Convert.ToInt32(question["pageID"]), question["questiontext"].ToString() ,Convert.ToInt32(question["number"]), question["format"].ToString(), Convert.ToInt32(question["MaxLength"]), Convert.ToBoolean(question["Required"]), (question.IsNull("gridValidation")?0:Convert.ToInt32(question["gridValidation"])), question["grid_control_type"].ToString(), Convert.ToBoolean(question["ShowTextControl"]));
				
				DataTable dtOptions = Survey.GetOptionValues(Convert.ToInt32(question["questionID"]));

				foreach(DataRow option in dtOptions.Rows) 
				{
					q.Options.Add(new Option(Convert.ToInt32(option["OptionID"]), option["OptionValue"].ToString(), option["OptionValue"].ToString(), Convert.ToInt32(option["PageID"]), (option.IsNull("score")?0:Convert.ToInt32(option["score"]))));
					if (Convert.ToInt32(option["PageID"]) != 0)
						q.BranchingExists = true;
				}

				if (q.QuestionType == "grid") 
					q.LoadGridStatements(Convert.ToInt32(question["questionID"]));

				questions.Add(q);				
			}			
			return questions;
		}

		public static ArrayList LoadQuestionsBySurveyID(int SurveyID) 
		{
			DataTable dtQuestions = Survey.GetAllQuestions(SurveyID, 0);		
			
			ArrayList questions = new ArrayList();
			foreach(DataRow question in dtQuestions.Rows) 
			{
				Question q = new Question(Convert.ToInt32(question["questionID"]),Convert.ToInt32(question["pageID"]), question["questiontext"].ToString() ,Convert.ToInt32(question["number"]), question["format"].ToString(), Convert.ToInt32(question["MaxLength"]), Convert.ToBoolean(question["Required"]), (question.IsNull("gridValidation")?0:Convert.ToInt32(question["gridValidation"])), question["grid_control_type"].ToString(), Convert.ToBoolean(question["ShowTextControl"]));
				
				DataTable dtOptions = Survey.GetOptionValues(Convert.ToInt32(question["questionID"]));

				foreach(DataRow option in dtOptions.Rows) 
				{
                    q.Options.Add(new Option(Convert.ToInt32(option["OptionID"]), option["OptionValue"].ToString(), option["OptionValue"].ToString(), Convert.ToInt32(option["PageID"]), (option.IsNull("score") ? 0 : Convert.ToInt32(option["score"]))));
					if (Convert.ToInt32(option["PageID"]) != 0)
						q.BranchingExists = true;
				}

				if (q.QuestionType == "grid") 
					q.LoadGridStatements(Convert.ToInt32(question["questionID"]));

				questions.Add(q);				
			}			
			return questions;
		}


		public static Question LoadQuestionsByID(int QuestionID) 
		{
			DataTable dtQuestion = DataFunctions.GetDataTable("select * from "+collectordb+"..question where questionID=" + QuestionID);		
			
			if (dtQuestion.Rows.Count > 0)
			{
				DataRow drQuestion = dtQuestion.Rows[0];
 
				Question q = new Question(Convert.ToInt32(drQuestion["questionID"]),Convert.ToInt32(drQuestion["pageID"]), drQuestion["questiontext"].ToString() ,Convert.ToInt32(drQuestion["number"]), drQuestion["format"].ToString(), Convert.ToInt32(drQuestion.IsNull("MaxLength")?"0":drQuestion["MaxLength"].ToString()), Convert.ToBoolean(drQuestion["Required"]), (drQuestion.IsNull("gridValidation")?0:Convert.ToInt32(drQuestion["gridValidation"])), drQuestion["grid_control_type"].ToString(), Convert.ToBoolean(drQuestion["ShowTextControl"]));
				
				DataTable dtOptions = Survey.GetOptionValues(Convert.ToInt32(drQuestion["questionID"]));

				foreach(DataRow option in dtOptions.Rows) 
				{
                    q.Options.Add(new Option(Convert.ToInt32(option["OptionID"]), option["OptionValue"].ToString(), option["OptionValue"].ToString(), Convert.ToInt32(option["PageID"]), (option.IsNull("score") ? 0 : Convert.ToInt32(option["score"]))));
					if (Convert.ToInt32(option["PageID"]) != 0)
						q.BranchingExists = true;
				}

				if (q.QuestionType == "grid") 
					q.LoadGridStatements(Convert.ToInt32(drQuestion["questionID"]));

				return q;

			}			
			return null;
		}
		#endregion

	}

}
