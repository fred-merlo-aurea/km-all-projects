using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Category
    {
        public Category()
        {
            CategoryID = -1;
            CategoryName = string.Empty;
            IsDeleted = null;
        }

        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
