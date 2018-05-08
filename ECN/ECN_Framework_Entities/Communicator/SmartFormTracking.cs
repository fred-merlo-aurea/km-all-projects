using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{

    [Serializable]
    [DataContract]
    public class SmartFormTracking
    {
        public SmartFormTracking()
        {
            SALID = -1;
            BlastID = null;
            CustomerID = null;
            SFID = null;
            GroupID = null;
            ReferringUrl = string.Empty;
            ActivityDate = null;
        }


        [DataMember]
        public int SALID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? SFID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public string ReferringUrl { get; set; }
        [DataMember]
        public DateTime? ActivityDate { get; set; }
    }
}
