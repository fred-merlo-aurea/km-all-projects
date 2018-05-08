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
    public class Sample
    {
        public Sample()
        {
            SampleID = -1;
            CustomerID = null;
            SampleName = string.Empty;
            WinningBlastID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            ABWinnerType = null;
            DidNotReceiveAB = null;
            DeliveredOrOpened = string.Empty;

        }

        [DataMember]
        public int SampleID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string SampleName { get; set; }
        [DataMember]
        public int? WinningBlastID { get; set; }
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
        public string ABWinnerType { get; set; }
        [DataMember]
        public bool? DidNotReceiveAB { get; set; }
        [DataMember]
        public string DeliveredOrOpened { get; set; }
    }
}
