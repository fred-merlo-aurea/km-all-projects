using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class LinkTrackingSettings
    {
        public LinkTrackingSettings()
        {
            LTSID = -1;
            LTID = -1;
            CustomerID = null;
            BaseChannelID = null;
            XMLConfig = string.Empty;
            CreatedUserID = null;
            CreatedDate = DateTime.MinValue;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
        }

        [DataMember]
        public int LTSID { get; set; }

        [DataMember]
        public int LTID { get; set; }

        [DataMember]
        public int? CustomerID { get; set; }

        [DataMember]
        public int? BaseChannelID { get; set; }

        [DataMember]
        public string XMLConfig { get; set; }

        [DataMember]
        public int? CreatedUserID { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int? UpdatedUserID { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
