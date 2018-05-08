using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ProductTypes
    {
        public ProductTypes()
        {
            PubTypeID = 0;
            PubTypeDisplayName = string.Empty;
            ColumnReference = string.Empty;
            IsActive = false;
            SortOrder = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }

        #region Properties
        [DataMember(Name = "ProductTypeID")]
        public int PubTypeID { get; set; }
        [DataMember(Name = "ProductTypeDisplayName")]
        public string PubTypeDisplayName { get; set; }
        [DataMember]
        public string ColumnReference { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public DateTime? DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int? CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
