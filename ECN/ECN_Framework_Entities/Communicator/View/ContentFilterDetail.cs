using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator.View
{
    [Serializable]
    [DataContract]
    public class ContentFilterDetail : ECN_Framework_Entities.Communicator.ContentFilterDetail
    {
        public ContentFilterDetail() : base()
        {
            CustomerID = null;
            FilterName = string.Empty;
            ContentTitle = string.Empty;
        }

        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string FilterName { get; set; }
        [DataMember]
        public string ContentTitle { get; set; }
    }
}
