using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts.View
{
    [Serializable]
    [DataContract]
    public class CustomerDiskUsage : ECN_Framework_Entities.Accounts.CustomerDiskUsage
    {
        public CustomerDiskUsage() : base()
        {
            CustomerName = string.Empty;
            AllowedStorage = string.Empty;
        }

        [DataMember]
        public string CustomerName { get; set; }
        public string AllowedStorage { get; set; }
    }
}
