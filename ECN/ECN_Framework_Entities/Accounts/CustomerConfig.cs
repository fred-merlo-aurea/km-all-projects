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
    public class CustomerConfig
    {
        public CustomerConfig() 
        {
            CustomerConfigID = -1;
            CustomerID = null;
            ProductID = null;
            ConfigNameID = ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.Unknown;
            ConfigName = string.Empty;
            ConfigValue = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int CustomerConfigID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? ProductID { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.ConfigName ConfigNameID { get; set; }
        [DataMember]
        public string ConfigName { get; set; }
        [DataMember]
        public string ConfigValue { get; set; }
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
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
