using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class AdhocCategory
    {
        public AdhocCategory() 
        {
            CategoryID = 0;
            CategoryName = string.Empty;
            SortOrder = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
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

        [DataMember]
        public List<Adhoc> AdhocList { get; set; }
    }
}
