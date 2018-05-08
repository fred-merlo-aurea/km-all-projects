using System;

namespace ecn.communicator.main.Wizard
{
	public interface IWizard
	{

		int WizardID
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
