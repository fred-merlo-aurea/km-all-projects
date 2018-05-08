using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ImportErrorSummary
    {
        public ImportErrorSummary() { }
        #region Properties
        [DataMember]
        public string PubCode { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ClientMessage { get; set; }
        [DataMember]
        public int ErrorCount { get; set; }
        #endregion
    }
}
