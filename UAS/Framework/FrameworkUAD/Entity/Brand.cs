using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Brand
    {
        public Brand() 
        {
            BrandID = 0;
            BrandName = string.Empty;
            Logo = string.Empty;
            IsBrandGroup = false;
            IsDeleted = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
        }
        #region Properties
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string Logo { get; set; }
        [DataMember]
        public bool IsBrandGroup { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        #endregion
    }
}
