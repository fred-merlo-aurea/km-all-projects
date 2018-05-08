using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Tests.Setup.Interfaces;
using Moq;

namespace ecn.MarketingAutomation.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class CampaignItemMock : Mock<ICampaignItem>
    {
        public CampaignItemMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimCampaignItem.UpdateSendTimeInt32DateTime =
                UpdateSendTime;
        }

        private void UpdateSendTime(int campaignItemId, DateTime newSendTime)
        {
            Object.UpdateSendTime(campaignItemId, newSendTime);
        }
    }
}
