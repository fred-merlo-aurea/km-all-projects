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
    public class CustomFieldValue
    {
        #region Properties
        /// <summary>
        /// Product ID
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
        public string Name { get; set; }//ResponseGroup
        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public string Value { get; set; }//Responsevalue
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }//Responsedesc
        /// <summary>
        /// Display Order
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }//Responsedesc
        /// <summary>
        /// Is Other
        /// </summary>
        [DataMember]
        public bool IsOther { get; set; }
        /// <summary>
        /// Is Consensus
        /// </summary>
        [DataMember]
        public bool IsConsensus { get; set; }
        #endregion

        public CustomFieldValue()
        {
            this.ProductId = 0;
            this.ProductCode = string.Empty;
            this.Name = string.Empty;
            this.Value = string.Empty;
            this.Description = string.Empty;
            this.DisplayOrder = 0;
            this.IsOther = false;
            this.IsConsensus = false;
        }

        public CustomFieldValue(FrameworkUAD.Object.CustomFieldValue cfv)
        {
            this.ProductId = cfv.ProductId;
            this.ProductCode = cfv.ProductCode;
            this.Name = cfv.Name;
            this.Value = cfv.Value;
            this.Description = cfv.Description;
            this.DisplayOrder = cfv.DisplayOrder;
            this.IsOther = cfv.IsOther;
            this.IsConsensus = cfv.IsConsensus;
        }
    }
}