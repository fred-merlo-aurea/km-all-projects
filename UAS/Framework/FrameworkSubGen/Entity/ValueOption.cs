using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class ValueOption : IEntity
    {
        public ValueOption() { }
        #region Properties
        [DataMember]
        public int option_id { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public string display_as { get; set; }
        [DataMember]
        public bool disqualifier { get; set; }
        [DataMember]
        public bool active { get; set; }
        [DataMember]
        public int order { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public int field_id { get; set; }
        [DataMember]
        public int KMCodeSheetID { get; set; }
        [DataMember]
        public string KMProductCode { get; set; }
        [DataMember]
        public int KMProductId { get; set; }
        #endregion
    }
}
