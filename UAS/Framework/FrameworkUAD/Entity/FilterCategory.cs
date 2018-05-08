using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FilterCategory
    {
        public FilterCategory() 
        {
            FilterCategoryID = 0;
            CategoryName = string.Empty;
            CreatedUserID = 0;
            CreatedDate = DateTime.Now;
            UpdatedUserID = 0;
            UpdatedDate = DateTime.Now;
            IsDeleted = false;
            ParentID = null;
        }
        #region Properties
        [DataMember]
        public int FilterCategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? ParentID { get; set; }
        #endregion
    }
}
