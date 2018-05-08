using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Batch
    {
        public Batch()
        {
            this.BatchCount = 1;
            this.PublicationID = 0;
            this.UserID = 0;
            this.IsActive = true;
            this.DateCreated = DateTime.Now;
            this.BatchNumber = 1;
            HistoryList = new List<History>();
        }
        #region Properties
        [DataMember]
        public int BatchID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int BatchCount { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateFinalized { get; set; }
        [DataMember]
        public int BatchNumber { get; set; }
        #endregion

        [DataMember]
        public List<History> HistoryList { get; set; }
    }
}
