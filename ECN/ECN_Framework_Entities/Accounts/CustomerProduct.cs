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
    public class CustomerProduct
    {
        public CustomerProduct() 
        {
            CustomerProductID = -1;
            CustomerID = null;
            ProductDetailID = null;
            Active = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;  
        }

        [DataMember]
        public int CustomerProductID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? ProductDetailID { get; set; }
        [DataMember]
        public string Active { get; set; }//char
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; } 
    }
}
