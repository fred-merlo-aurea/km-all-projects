using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class CustomField : IEntity
    {
        public CustomField()
        {
            value_options = new List<ValueOption>();
        }
        #region Properties
        [DataMember]
        public int field_id { get; set; }
        [DataMember]
        public int subscriber_id { get; set; }//only populated for CDC
        [DataMember]
        [StringLength(255, ErrorMessage = "The value cannot exceed 255 characters.")]
        public string name { get; set; }
        [DataMember]
        [StringLength(255, ErrorMessage = "The value cannot exceed 255 characters.")]
        public string display_as { get; set; }
        [DataMember]
        public Enums.HtmlFieldType type { get; set; }//HTML field type: checkbox, select, radio, text, textarea
        [DataMember]
        public bool allow_other { get; set; }
        [DataMember]
        [StringLength(255, ErrorMessage = "The value cannot exceed 255 characters.")]
        public string text_value { get; set; }
        [DataMember]
        public List<ValueOption> value_options { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public int KMResponseGroupID { get; set; }
        [DataMember]
        public string KMProductCode	{ get; set; }
        [DataMember]
        public int KMProductId { get; set; }
        #endregion
    }
}
