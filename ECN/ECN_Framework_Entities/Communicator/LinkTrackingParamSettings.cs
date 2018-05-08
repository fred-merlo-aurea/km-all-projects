using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class LinkTrackingParamSettings
    {
        public LinkTrackingParamSettings()
        {
            LTPSID = -1;
            LTPID = -1;
            CustomerID = null;
            BaseChannelID = null;
            DisplayName = string.Empty;
            AllowCustom = false;
            IsRequired = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
        }

        [DataMember]
        public int LTPSID { get; set; }
        [DataMember]
        public int LTPID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public bool AllowCustom { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
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
