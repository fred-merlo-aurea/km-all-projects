using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Page
    {
        public Page()
        {
            PageID = -1;
            EditionID = -1;
            PageNumber = -1;
            DisplayNumber = string.Empty;
            Width = null;
            Height = null;
            TextContent = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            LinkList = new List<Link>();
            CustomerID = null;
        }

        [DataMember]
        public int PageID { get; set; }
        [DataMember]
        public int EditionID { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public string DisplayNumber { get; set; }
        [DataMember]
        public int? Width { get; set; }
        [DataMember]
        public int? Height { get; set; }
        [DataMember]
        public string TextContent { get; set; }
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
        public List<Link> LinkList { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
