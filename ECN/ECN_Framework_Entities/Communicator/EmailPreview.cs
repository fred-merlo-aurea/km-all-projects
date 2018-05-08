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
    public class EmailPreview
    {
        public EmailPreview() 
        { 
        }

        [DataMember]
        public int EmailTestID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string ZipFile { get; set; }
        [DataMember]
        public int CreatedByID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public TimeSpan TimeCreated { get; set; }
        [DataMember]
        public int LinkTestID { get; set; }

        [DataMember]
        public Guid BaseChannelGUID { get; set; }
    }
}
