using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class CustomFieldValue
    {
        public CustomFieldValue() 
        {
            ProductId = 0;
            ProductCode = string.Empty;
            Name = string.Empty;
            Value = string.Empty;
            Description = string.Empty;
            DisplayOrder = 0;
            IsOther = false;
            IsConsensus = false;
        }
        #region Properties
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string Name { get; set; }//ResponseGroup
        [DataMember]
        public string Value { get; set; }//Responsevalue
        [DataMember]
        public string Description { get; set; }//Responsedesc
        [DataMember]
        public int DisplayOrder { get; set; }//Responsedesc
        [DataMember]
        public bool IsOther { get; set; }
        [DataMember]
        public bool IsConsensus { get; set; }
        #endregion
    }
}
