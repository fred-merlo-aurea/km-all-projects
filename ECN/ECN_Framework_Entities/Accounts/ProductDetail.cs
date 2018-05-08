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
    public class ProductDetail
    {
        public ProductDetail() 
        {
            ProductDetailID = -1;
            ProductID = null;
            ProductDetailName = string.Empty;
            ProductDetailDesc = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;  
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int ProductDetailID { get; set; }
        [DataMember]
        public int? ProductID { get; set; }
        [DataMember]
        public string ProductDetailName { get; set; }
        [DataMember]
        public string ProductDetailDesc { get; set; }
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
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
