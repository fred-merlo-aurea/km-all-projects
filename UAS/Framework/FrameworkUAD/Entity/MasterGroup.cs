using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class MasterGroup
    {
        public MasterGroup()
        {
            MasterGroupID = 0;
            Name = string.Empty;
            Description = string.Empty;
            DisplayName = string.Empty;
            ColumnReference = string.Empty;
            SortOrder = 0;
            IsActive = false;
            EnableSubReporting = false;
            EnableSearching = false;
            EnableAdhocSearch = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int MasterGroupID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ColumnReference { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool EnableSubReporting { get; set; }
        [DataMember]
        public bool EnableSearching { get; set; }
        [DataMember]
        public bool EnableAdhocSearch { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
