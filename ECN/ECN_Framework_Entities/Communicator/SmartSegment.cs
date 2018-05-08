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
    public class SmartSegment
    {
        public SmartSegment()
        {
            SmartSegmentID = -1;
            SmartSegmentName = string.Empty;
            SmartSegmentOldID = -1;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        #region Properties
        [DataMember]
        public int SmartSegmentID { get; set; }
        [DataMember]
        public string SmartSegmentName { get; set; }
        [DataMember]
        public int SmartSegmentOldID { get; set; }
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
        #endregion
    }
}
