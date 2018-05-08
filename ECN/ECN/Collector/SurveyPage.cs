using System;
using System.Data;
using System.Collections;
using ecn.common.classes;

namespace ecn.collector.classes
{
	
	
	/// Used to stored the current survey pages(page details, questions and answers)
	
	public class SurveyPage
	{
		private bool _isIntroPage = false;
		private bool _isThankYouPage = false;
		private bool _isSkipped = false;
		private int _pageID = 0;
		private string _pageHeader = string.Empty;
		private string _pageDescription = string.Empty;
		private ArrayList _questions;

		public bool IsIntroPage
		{
			get {return (this._isIntroPage);}
			set {this._isIntroPage = value;}
		}

		public bool IsThankYouPage
		{
			get {return (this._isThankYouPage);}
			set {this._isThankYouPage = value;}
		}

		public bool IsSkipped
		{
			get {return (this._isSkipped);}
			set {this._isSkipped = value;}
		}

		public int PageID 
		{
			get {return (this._pageID);}
			set {this._pageID = value;}
		}	

		public string PageHeader
		{
			get {return (this._pageHeader);}
			set {this._pageHeader = value;}
		}	

		public string PageDescription 
		{
			get {return (this._pageDescription);}
			set {this._pageDescription = value;}
		}	

		public ArrayList Questions 
		{
			get {return (this._questions);}	
			set {this._questions = value;}
		}

		public SurveyPage(bool isIntroPage, bool isThankYouPage)
		{
			_isIntroPage = isIntroPage;
			_isThankYouPage = isThankYouPage;
		}

		public SurveyPage(int pageID, string pageHeader, string pageDescription)
		{
			_pageID = pageID;
			_pageHeader = pageHeader;
			_pageDescription = pageDescription;
		}

		public static ArrayList LoadSurveyPages(int surveyID) 
		{
			ArrayList SurveyDetails = new ArrayList();
			DataTable dtSurvey = DataFunctions.GetDataTable("select isnull(introHTML,'') as introHTML, isnull(ThankYouHTML,'') as ThankYouHTML  from survey where SurveyID = " + surveyID);
			DataTable dtSurveyPage = DataFunctions.GetDataTable("select distinct p.PageID, p.number, isnull(PageHeader,'') as PageHeader, isnull(PageDesc,'') as PageDesc  from page p join question q on p.PageID = q.PageID where p.SurveyID = " + surveyID + " order by p.number");
			
			if (dtSurvey.Rows[0]["introHTML"].ToString() != string.Empty)
			{
				SurveyPage ss = new SurveyPage(true,false);
				SurveyDetails.Add(ss);
			}

			foreach(DataRow page in dtSurveyPage.Rows) 
			{
				SurveyPage ss = new SurveyPage(Convert.ToInt32(page["PageID"]), page["PageHeader"].ToString(), page["PageDesc"].ToString());
				ss.Questions = ecn.collector.classes.Question.LoadQuestionsByPageID(Convert.ToInt32(page["PageID"]));
				SurveyDetails.Add(ss);				
			}	
		
			//Create Default Thankyou Page
			SurveyDetails.Add(new SurveyPage(false,true));
			
			return SurveyDetails;
		}
	}
}
