using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;

namespace UAD.API.Models
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class CustomField
    {
        #region Properties
        /// <summary>
        /// Product Id
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }
        /// <summary>
        /// Product Code
        /// </summary>
        [DataMember]
        public string ProductCode { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Display Name
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }
        /// <summary>
        /// Display Order
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Is Multiple Value
        /// </summary>
        [DataMember]
        public bool IsMultipleValue { get; set; }
        /// <summary>
        /// Is Required
        /// </summary>
        [DataMember]
        public bool IsRequired { get; set; }
        /// <summary>
        /// Is Active
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// Is AdHoc
        /// </summary>
        [DataMember]
        public bool IsAdHoc { get; set; }
        #endregion

        public CustomField()
        {
            this.ProductId = 0;
            this.ProductCode = "";
            this.Name = "";
            this.DisplayName = "";
            this.DisplayOrder = 0;
            this.IsMultipleValue = false;
            this.IsRequired = false;
            this.IsActive = false;
            this.IsAdHoc = false;
        }

        public CustomField(FrameworkUAD.Object.CustomField cf)
        {
            this.ProductId = cf.ProductId;
            this.ProductCode = cf.ProductCode;
            this.Name = cf.Name;
            this.DisplayName = cf.DisplayName;
            this.DisplayOrder = cf.DisplayOrder;
            this.IsMultipleValue = cf.IsMultipleValue;
            this.IsRequired = cf.IsRequired;
            this.IsActive = cf.IsActive;
            this.IsAdHoc = cf.IsAdHoc;
        }
    }
}