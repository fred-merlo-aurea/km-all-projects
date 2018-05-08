using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The CustomField object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CustomField
    {
        public CustomField() { }
        #region Properties
        /// <summary>
        /// Product ID for the CustomField object.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }
        /// <summary>
        /// Product code for the CustomField object.
        /// </summary>
        [DataMember]
        public string ProductCode { get; set; }
        /// <summary>
        /// Name of the CustomField object.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Display name of the CustomField object.
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }
        /// <summary>
        /// Display order for the CustomField object.
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// If the CustomField object is multiple value.
        /// </summary>
        [DataMember]
        public bool IsMultipleValue { get; set; }
        /// <summary>
        /// If the CustomField object is required.
        /// </summary>
        [DataMember]
        public bool IsRequired { get; set; }
        /// <summary>
        /// If the CustomField object is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// If the CustomField object is an adhoc.
        /// </summary>
        [DataMember]
        public bool IsAdHoc { get; set; }
        #endregion
    }
}