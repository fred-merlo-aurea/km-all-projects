using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_Entities.Communicator;
using Moq;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimCampaignItemSocialMedia;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class CampaignItemSocialMediaMock : Mock<ICampaignItemSocialMedia>
    {
        public CampaignItemSocialMediaMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByCampaignItemIDInt32 = GetByCampaignItemID;
        }

        private List<CampaignItemSocialMedia> GetByCampaignItemID(int campaignItemId)
        {
            return Object.GetByCampaignItemID(campaignItemId);
        }
    }
}
