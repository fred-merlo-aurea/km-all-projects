using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class LinkTrackingParamOption
    {
        public LinkTrackingParamOption()
        {
            LTPOID = -1;
            DisplayName = string.Empty;
            ColumnName = null;
            Value = string.Empty;
            IsActive = null;
            CustomerID = null;
            BaseChannelID = null;
            IsDynamic = false;
            IsDefault = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
        }

        [DataMember]
        public int LTPOID { get; set; }
        [DataMember]
        public int LTPID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public bool IsDynamic { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
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
