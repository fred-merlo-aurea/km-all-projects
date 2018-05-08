using System;

namespace ecn.communicator.main.SMSWizard
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
