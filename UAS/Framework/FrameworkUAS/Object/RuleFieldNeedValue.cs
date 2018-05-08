using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class RuleFieldNeedValue
    {
        public RuleFieldNeedValue() { }
        #region Properties
        [DataMember]
        public int RuleFieldId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string DataTable { get; set; }
        [DataMember]
        public string Field { get; set; }
        #endregion
    }
}
