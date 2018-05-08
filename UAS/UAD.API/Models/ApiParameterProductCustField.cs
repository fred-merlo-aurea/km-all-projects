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
    public class ApiParameterProductCustField
    {
        /// <summary>
        /// Product Code
        /// </summary>
        [DataMember]
        public string productCode { get; set; }
        /// <summary>
        /// Custom Field Name
        /// </summary>
        [DataMember]
        public string customFieldName { get; set; }
    }
}