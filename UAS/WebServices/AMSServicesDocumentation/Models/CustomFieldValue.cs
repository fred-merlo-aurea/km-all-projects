using System;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// CustomFieldValue object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CustomFieldValue
    {
        #region Properties
        /// <summary>
        /// Product ID for the CustomFieldValue object.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }
        /// <summary>
        /// Product code for the CustomFieldValue object.
        /// </summary>
        [DataMember]
        public string ProductCode { get; set; }
        /// <summary>
        /// Name of the CustomFieldValue object.
        /// </summary>
        [DataMember]
        public string Name { get; set; }//ResponseGroup
        /// <summary>
        /// Value for the CustomFieldValue object.
        /// </summary>
        [DataMember]
        public string Value { get; set; }//Responsevalue
        /// <summary>
        /// Description for the CustomFieldValue object.
        /// </summary>
        [DataMember]
        public string Description { get; set; }//Responsedesc
        /// <summary>
        /// Display order for the CustomFieldValue object.
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }//Responsedesc
        /// <summary>
        /// If the CustomFieldValue object is an other field.
        /// </summary>
        [DataMember]
        public bool IsOther { get; set; }
        /// <summary>
        /// If the CustomFieldValue object is Consensus.
        /// </summary>
        [DataMember]
        public bool IsConsensus { get; set; }
        #endregion
    }
}