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
    public class SubscriberConsensusDemographic
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// DisplayName
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public string Value { get; set; }
        #endregion

        public SubscriberConsensusDemographic()
        {
            Name = string.Empty;
            DisplayName = string.Empty;
            Value = string.Empty;
        }
        public SubscriberConsensusDemographic(string name, string displayname, string value)
        {
            Name = name;
            DisplayName = displayname;
            Value = value;
        }
    }
}