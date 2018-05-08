using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FileProcessingStat
    {
        public FileProcessingStat() { }
        #region Properties
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public int ProfileCount { get; set; }
        [DataMember]
        public int DemographicCount { get; set; }
        [DataMember]
        public DateTime ProcessDate { get; set; }
        #endregion
    }
}
