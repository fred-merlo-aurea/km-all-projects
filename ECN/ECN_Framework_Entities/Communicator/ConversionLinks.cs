using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class ConversionLinks
    {
        public ConversionLinks()
        {
            LinkID = -1;
            LayoutID = null;
            LinkURL = string.Empty;
            LinkParams = string.Empty;
            LinkName = string.Empty;
            IsActive = string.Empty;
            SortOrder = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int LinkID { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public string LinkURL { get; set; }
        [DataMember]
        public string LinkParams { get; set; }
        [DataMember]
        public string LinkName { get; set; }
        [DataMember]
        public string IsActive { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
