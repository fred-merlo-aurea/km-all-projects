using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Link
    {
        public Link()
        {
            LinkID = -1;
            PageID = -1;
            LinkType = string.Empty; 
            LinkURL = string.Empty; 
            x1 = -1;
            y1 = -1;
            x2 = -1;
            y2 = -1;
            Alias = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int LinkID { get; set; }
        [DataMember]
        public int PageID { get; set; }
        [DataMember]
        public string LinkType { get; set; }
        [DataMember]
        public string LinkURL { get; set; }
        [DataMember]
        public int x1 { get; set; }
        [DataMember]
        public int y1 { get; set; }
        [DataMember]
        public int x2 { get; set; }
        [DataMember]
        public int y2 { get; set; }
        [DataMember]
        public string Alias { get; set; }
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
        public int? CustomerID { get; set; }
    }
}
