using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class TransformSplitInfo
    {
        #region
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public string Delimiter { get; set; }
        #endregion
    }
}
