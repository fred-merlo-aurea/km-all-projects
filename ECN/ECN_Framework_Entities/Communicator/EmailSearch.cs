using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class EmailSearch
    {
        public EmailSearch()
        {
            TotalRowsCount = 0;
            BaseChannelName = string.Empty;
            CustomerName = string.Empty;
            GroupName = string.Empty;
            EmailAddress = string.Empty;
            Subscribe = string.Empty;
            DateCreated = null;
            DateModified = null;
        }

        [DataMember]
        public int TotalRowsCount { get; set; }
        [DataMember]
        public string BaseChannelName { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Subscribe { get; set; }
        [DataMember]
        public DateTime? DateCreated { get; set; }
        [DataMember]
        public DateTime? DateModified { get; set; }
    }

    public class EmailSearchCSV
    {
        public EmailSearchCSV()
        {
            BaseChannelName = string.Empty;
            CustomerName = string.Empty;
            GroupName = string.Empty;
            EmailAddress = string.Empty;
            Subscribe = string.Empty;
            DateAdded = string.Empty;
            DateModified = string.Empty;
        }
        
        [DataMember]
        public string BaseChannelName { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Subscribe { get; set; }
        [DataMember]
        public string DateAdded { get; set; }
        [DataMember]
        public string DateModified { get; set; }
    }
}
