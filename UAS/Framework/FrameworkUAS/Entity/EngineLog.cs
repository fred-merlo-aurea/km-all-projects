using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace FrameworkUAS.Entity
{
    /// <summary>
    /// for tracking run and refresh status of different engines for each client
    /// </summary>
    [Serializable]
    [DataContract]
    public class EngineLog
    {
        public EngineLog() { }
        #region Properties
        [DataMember]
        public int EngineLogId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string Engine { get; set; }
        [DataMember]
        public string CurrentStatus { get; set; }
        [DataMember]
        public DateTime LastRefreshDate { get; set; }
        [DataMember]
        public TimeSpan LastRefreshTime { get; set; }
        [DataMember]
        public bool IsRunning { get; set; }
        [DataMember]
        public DateTime LastRunningCheckDate { get; set; }
        [DataMember]
        public TimeSpan LastRunningCheckTime { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        #endregion
    }
}
