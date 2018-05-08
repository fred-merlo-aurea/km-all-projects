using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Adhoc
    {
        public Adhoc()
        {
            AdhocID = 0;
            AdhocName = string.Empty;
            CategoryID = 0;
            SortOrder = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int AdhocID { get; set; }
        [DataMember]
        public string AdhocName { get; set; }
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string ColumnValue { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ColumnType { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
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
