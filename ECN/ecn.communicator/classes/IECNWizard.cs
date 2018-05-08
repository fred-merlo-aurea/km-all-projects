using System;

namespace ecn.communicator.main.ECNWizard
{
    public interface IECNWizard
    {

        int CampaignItemID
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
