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
    public class SubscriberProductDemographic
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public string Value { get; set; }
        /// <summary>
        /// Demo Update Action
        /// </summary>
        [IgnoreDataMember]
        public FrameworkUAD_Lookup.Enums.DemographicUpdate DemoUpdateAction { get; set; }
        #endregion

        public SubscriberProductDemographic()
        {
            Name = string.Empty;
            Value = string.Empty;
            DemoUpdateAction = FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace;
        }
        public SubscriberProductDemographic(string name, string value, FrameworkUAD_Lookup.Enums.DemographicUpdate demoUpdateAction)
        {
            Name = name;
            Value = value;
            DemoUpdateAction = demoUpdateAction;
        }
    }
}