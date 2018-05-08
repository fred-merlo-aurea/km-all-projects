using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator.ContentReplacement
{
    public class RSSFeedByCampaignItemTokenReplacementContext
    {
        public int CampiagnItemID { get; set; }
        public string FeedName { get; set; }
        public ECN_Framework_Entities.Communicator.RSSFeed RSSFeed { get; set; }
    }
}
