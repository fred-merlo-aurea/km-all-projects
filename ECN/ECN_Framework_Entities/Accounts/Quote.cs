using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class Quote
    {
        public Quote()
        {
            QuoteID = -1;
            CustomerID = -1;
            ChannelID = -1;
            CreatedDate = null;
            UpdatedDate = null;
            ApproveDate = null;
            StartDate = null;
            Salutation = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Company = string.Empty;
            BillType = string.Empty;
            CreatedUserID = -1;
            UpdatedUserID = -1;
            AccountManagerID = null;
            NBDIDs = string.Empty;
            Status = ECN_Framework_Common.Objects.Accounts.Enums.QuoteStatusEnum.Pending;
            Notes = string.Empty;
            TestUserName = string.Empty;
            TestPassword = string.Empty;
            InternalNotes = string.Empty;
            IsDeleted = null;
            ItemList = null;
        }

        [DataMember]
        public int QuoteID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int ChannelID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public DateTime? ApproveDate { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public string Salutation { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string BillType { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
        [DataMember]
        public int? AccountManagerID { get; set; }
        [DataMember]
        public string NBDIDs { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.QuoteStatusEnum Status { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string TestUserName { get; set; }
        [DataMember]
        public string TestPassword { get; set; }
        [DataMember]
        public string InternalNotes { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        public List<QuoteItem> ItemList { get; set; }
    }
}
