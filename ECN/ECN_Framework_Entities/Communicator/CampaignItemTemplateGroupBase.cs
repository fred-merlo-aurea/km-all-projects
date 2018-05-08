using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public abstract class CampaignItemTemplateGroupBase : ModelBase
    {
        [DataMember]
        public int CampaignItemTemplateID { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
