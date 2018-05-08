using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class SmartFormsPrePopFields
    {
        public SmartFormsPrePopFields()
        {
            PrePopFieldID = -1;
            SFID = null;
            ProfileFieldName = string.Empty;
            DisplayName = string.Empty;
            DataType = string.Empty;
            ControlType = string.Empty;
            DataValues = string.Empty;
            Required = string.Empty;
            PrePopulate = string.Empty;
            SortOrder = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int PrePopFieldID { get; set; }
        [DataMember]
        public int? SFID { get; set; }
        [DataMember]
        public string ProfileFieldName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string ControlType { get; set; }
        [DataMember]
        public string DataValues { get; set; }
        [DataMember]
        public string Required { get; set; }
        [DataMember]
        public string PrePopulate { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
