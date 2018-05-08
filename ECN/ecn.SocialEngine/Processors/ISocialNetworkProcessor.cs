using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.SocialEngine.Processors
{
    public delegate void SetToErrorDelegate(ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism, int errorCode = 0);
    public interface ISocialNetworkProcessor
    {
        void Execute();
    }
}
