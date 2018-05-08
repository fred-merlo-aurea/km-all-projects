using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ReportGroups
    {
        public ReportGroups() { }

        #region Properties
        [DataMember]
        public int ReportGroupID { get; set; }
        [DataMember]
        public int ResponseTypeID { get; set; }
        [DataMember]
        public int ResponseGroupID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        #endregion        			
    }
}
