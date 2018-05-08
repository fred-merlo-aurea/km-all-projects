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
    public class EmailGroup
    {
        public EmailGroup()
        {
            EmailGroupID = -1;
            GroupID = -1;
            EmailID = -1;
            FormatTypeCode = string.Empty;
            SubscribeTypeCode = string.Empty;
            SMSEnabled = null;

            CustomerID = null;

            CreatedOn = null;
            LastChanged = null;
        }

        #region Properties
        [DataMember]
        public int EmailGroupID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public string FormatTypeCode { get; set; }
        [DataMember]
        public string SubscribeTypeCode { get; set; }
        [DataMember]
        public bool? SMSEnabled { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }
        [DataMember]
        public DateTime? LastChanged { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }


        //[DataMember]
        //public DateTime? CreatedDate { get; set; }
        //[DataMember]
        //public DateTime? UpdatedDate { get; set; }
        //[DataMember]
        //public int? CreatedUserID { get; set; }
        //[DataMember]
        //public int? UpdatedUserID { get; set; }
        //[DataMember]
        //public bool? IsDeleted { get; set; }
        #endregion
    }
}
