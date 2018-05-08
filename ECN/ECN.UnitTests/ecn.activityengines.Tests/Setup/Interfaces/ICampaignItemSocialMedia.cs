using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ICampaignItemSocialMedia
    {
        List<CampaignItemSocialMedia> GetByCampaignItemID(int campaignItemId);
    }
}
