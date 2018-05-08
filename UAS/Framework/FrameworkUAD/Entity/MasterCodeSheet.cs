using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class MasterCodeSheet
    {
        public MasterCodeSheet()
        {
            MasterID = 0;
            MasterValue = string.Empty;
            MasterDesc = string.Empty;
            MasterGroupID = 0;
            MasterDesc1 = string.Empty;
            EnableSearching = false;
            SortOrder = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }

        #region Properties
        [DataMember]
        public int MasterID { get; set; }
        [DataMember]
        public string MasterValue { get; set; }
        [DataMember]
        public string MasterDesc { get; set; }
        [DataMember]
        public int MasterGroupID { get; set; }
        [DataMember]
        public string MasterDesc1 { get; set; }
        [DataMember]
        public bool EnableSearching { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
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
