using System;

namespace ecn.collector.classes
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
