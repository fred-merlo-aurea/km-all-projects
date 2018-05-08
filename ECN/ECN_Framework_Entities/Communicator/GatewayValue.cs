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
    public class GatewayValue : GatewayBase
    {
        public GatewayValue()
        {
            GatewayValueID = -1;
            GatewayID = -1;
            Field = string.Empty;
            IsLoginValidator = false;
            IsCaptureValue = false;
            IsStatic = false;
            Label = "";
            Value = string.Empty;
            IsDeleted = false;
            Comparator = string.Empty;
            NOT = false;
            FieldType = string.Empty;
            DatePart = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            HasFailed = false;
            IsBlank = false;
        }

        #region Properties
        [DataMember]
        public int GatewayValueID { get; set; }

        [DataMember]
        public string Field { get; set; }

        [DataMember]
        public bool IsLoginValidator { get; set; }

        [DataMember]
        public bool IsCaptureValue { get; set; }

        [DataMember]
        public bool IsStatic { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Comparator { get; set; }

        [DataMember]
        public bool NOT { get; set; }

        [DataMember]
        public string FieldType { get; set; }

        [DataMember]
        public string DatePart { get; set; }

        [DataMember]
        public bool HasFailed { get; set; }

        [DataMember]
        public bool IsBlank { get; set; }
        #endregion
    }
}
