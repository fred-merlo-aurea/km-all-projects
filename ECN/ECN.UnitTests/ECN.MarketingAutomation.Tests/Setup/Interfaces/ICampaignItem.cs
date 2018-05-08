using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.MarketingAutomation.Tests.Setup.Interfaces
{
    public interface ICampaignItem
    {
        void UpdateSendTime(int campaignItemId, DateTime newSendTime);
    }
}
