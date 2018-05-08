using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Address : IEntity
    {
        public Address() { }
        #region Properties
        [DataMember]
        public int address_id { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string first_name { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string last_name { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string address { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string address_line_2 { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string company { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string city { get; set; }
        [DataMember]
        [StringLength(60, ErrorMessage = "The value cannot exceed 60 characters.")]
        public string state { get; set; }
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string country { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string country_name { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string country_abbreviation { get; set; }
        [DataMember]
        public decimal latitude { get; set; }
        [DataMember]
        public decimal longitude { get; set; }
        [DataMember]
        public bool verified { get; set; }
        [DataMember]
        [StringLength(12, ErrorMessage = "The value cannot exceed 12 characters.")]
        public string zip_code { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
