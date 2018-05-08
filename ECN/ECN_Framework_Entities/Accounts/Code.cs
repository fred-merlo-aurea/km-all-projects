﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class Code
    {
        public Code()
        {
            CodeID = -1;
            CodeType = string.Empty;
            CodeTypeCode = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.Unknown;
            CodeName = string.Empty;
            CodeValue = string.Empty;
            CodeDescription = string.Empty;
            SystemFlag = string.Empty;
            SortCode = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int CodeID { get; set; }
        [DataMember]
        public string CodeType { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.CodeType CodeTypeCode { get; set; }
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public string CodeDescription { get; set; }
        [DataMember]
        public string SystemFlag { get; set; }
        [DataMember]
        public int? SortCode { get; set; }
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
    }
}