using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    public class SubscriberArchiveBase : SubscriberOriginalBase
    {
        public SubscriberArchiveBase()
        {
            Initialize();
        }

        public SubscriberArchiveBase(string processCode)
            : base(processCode)
        {
            Initialize();
        }

        [DataMember]
        public int CGrp_Cnt { get; set; }

        [DataMember]
        public Guid CGrp_No { get; set; }

        [DataMember]
        [StringLength(2)]
        public string CGrp_Rank { get; set; }

        [DataMember]
        public bool EmailExists { get; set; }

        [DataMember]
        public bool FaxExists { get; set; }

        [DataMember]
        public int IGrp_Cnt { get; set; }

        [DataMember]
        public Guid IGrp_No { get; set; }

        [DataMember]
        [StringLength(2)]
        public string IGrp_Rank { get; set; }

        [DataMember]
        public bool PhoneExists { get; set; }

        [DataMember]
        public DateTime? QDate { get; set; }

        [DataMember]
        public bool StatList { get; set; }

        [DataMember]
        public DateTime? TransactionDate { get; set; }

        private void Initialize()
        {
            PhoneExists = false;
            FaxExists = false;
            EmailExists = false;
            IGrp_Cnt = NoInt;
            CGrp_No = Guid.NewGuid();
            CGrp_Cnt = NoInt;
            StatList = false;
            IGrp_Rank = string.Empty;
            CGrp_Rank = string.Empty;
        }
    }
}