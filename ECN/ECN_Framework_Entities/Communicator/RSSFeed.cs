using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public partial class RSSFeed
    {
        public RSSFeed()
        {
            FeedID = -1;
            CustomerID = -1;
            Name = string.Empty;
            URL = string.Empty;
            StoriesToShow = null;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            IsDeleted = false;
        }

        #region properties
        [DataMember]
        public int FeedID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public int? StoriesToShow { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }

        #endregion
    }
}
