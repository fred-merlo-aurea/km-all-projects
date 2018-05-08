using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistoryToUserLog
    {
        public HistoryToUserLog() { }
        #region Properties
        [DataMember]
        public int HistoryID { get; set; }
        [DataMember]
        public int UserLogID { get; set; }
        #endregion
    }
}
