using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class CustomerDiskUsage
    {
        public CustomerDiskUsage()
        {
            UsageID = -1;
            ChannelID = null;
            CustomerID = null;
            SizeInBytes = string.Empty;
            DateMonitored = null;
        }

        [DataMember]
        public int UsageID { get; set; }
        [DataMember]
        public int? ChannelID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string SizeInBytes { get; set; }
        [DataMember]
        public DateTime? DateMonitored { get; set; }

    }
}
