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
    public class ApiParameterDemo
    {
        /// <summary>
        /// Email Address
        /// </summary>
        [DataMember]
        public string emailAddress { get; set; }
        /// <summary>
        /// Dimensions
        /// </summary>
        [DataMember]
        public List<Models.SubscriberConsensusDemographic> dimensions { get; set; }
    }
}