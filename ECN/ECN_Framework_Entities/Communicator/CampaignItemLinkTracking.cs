using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemLinkTracking
    {
        public CampaignItemLinkTracking()
        {
            CILTID = -1;
            CampaignItemID = null;
            LTPID = null;
            LTPOID = null;
            CustomValue = string.Empty;
        }

        [DataMember]
        public int CILTID { get; set; }
        [DataMember]
        public int? CampaignItemID { get; set; }
        [DataMember]
        public int? LTPID { get; set; }
        [DataMember]
        public int? LTPOID { get; set; }
        [DataMember]
        public string CustomValue { get; set; }
    }
}
