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
    public class Product
    {
        public Product() 
        {
            ProductID = -1;
            ProductName = string.Empty;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public bool HasWebsiteTarget { get; set; }    
        public List<ECNError> ErrorList { get; set; }
    }
}
