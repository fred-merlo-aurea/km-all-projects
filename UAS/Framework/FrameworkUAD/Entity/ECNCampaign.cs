using System;
using System.Linq;
using System.Runtime.Serialization;


namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ECNCampaign
    {
        public ECNCampaign()
        {
            ECNCampaignID = 0;
            ECNCampaignName = string.Empty;
        }
        #region Properties
        [DataMember]
        public int ECNCampaignID { get; set; }
        [DataMember]
        public string ECNCampaignName { get; set; }
        #endregion
    }
}
