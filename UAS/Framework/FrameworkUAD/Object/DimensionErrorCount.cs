using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class DimensionErrorCount
    {
        public DimensionErrorCount() { }
        #region Properties
        [DataMember]
        public int DimensionErrorTotal { get; set; }
        [DataMember]
        public int DimensionDistinctSubscriberCount { get; set; }
        #endregion
    }
}

