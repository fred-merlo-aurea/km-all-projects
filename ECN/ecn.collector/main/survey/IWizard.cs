using System;

namespace ecn.collector.main.survey
{
	public interface IWizard
	{

		int SurveyID
		{ 
			get;
			set; 
		}  

		string ErrorMessage
		{ 
			get;
			set; 
		}  

		void Initialize();

		bool Save();

	}
}
