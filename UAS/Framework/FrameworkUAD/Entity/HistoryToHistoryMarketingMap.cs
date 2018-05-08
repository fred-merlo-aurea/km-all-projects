using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistoryToHistoryMarketingMap
    {
        public HistoryToHistoryMarketingMap() { }
        #region Properties
        [DataMember]
        public int HistoryID { get; set; }
        [DataMember]
        public int HistoryMarketingMapID { get; set; }
        #endregion
    }
}
