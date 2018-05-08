using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class Dimension
    {
        public Dimension() { }
        #region Properties
        [DataMember]
        public int ResponseGroupID { get; set; }
        [DataMember]
        public string ResponseGroupName { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public int CodeSheetID { get; set; }
        [DataMember]
        public string Responsevalue { get; set; }
        [DataMember]
        public string SubGenResponseGroupName { get; set; }
        #endregion
    }
}
