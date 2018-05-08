using System;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Blast : IUserValidate
    {
        public bool HasValidID
        {
            get { return BlastID > 0; }
        }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string EmailFrom { get; set; }
        [DataMember]
        public string EmailFromName { get; set; }
        [DataMember]
        public DateTime? SendTime { get; set; }
        [DataMember]
        public int? AttemptTotal { get; set; }
        [DataMember]
        public int? SendTotal { get; set; }
        [DataMember]
        public int? SendBytes { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string BlastType { get; set; }
        [DataMember]
        public int? CodeID { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public DateTime? FinishTime { get; set; }
        [DataMember]
        public int? SuccessTotal { get; set; }
        [DataMember]
        public string BlastLog { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string Spinlock { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }
        [DataMember]
        public string TestBlast { get; set; }
        [DataMember]
        public string BlastFrequency { get; set; }
        [DataMember]
        public string RefBlastID { get; set; }
        [DataMember]
        public string BlastSuppression { get; set; }
        [DataMember]
        public bool? AddOptOuts_to_MS { get; set; }
        [DataMember]
        public string DynamicFromName { get; set; }
        [DataMember]
        public string DynamicFromEmail { get; set; }
        [DataMember]
        public string DynamicReplyToEmail { get; set; }
        [DataMember]
        public int? BlastEngineID { get; set; }
        [DataMember]
        public bool? HasEmailPreview { get; set; }
        [DataMember]
        public int? BlastScheduleID { get; set; }
        [DataMember]
        public int? OverrideAmount { get; set; }
        [DataMember]
        public bool? OverrideIsAmount { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public int? SMSOptInTotal { get; set; }
        [DataMember]
        public int? CampaignItemID { get; set; }
        [DataMember]
        public string NodeID { get; set; }
        [DataMember]
        public int? SampleID { get; set; }
        [DataMember]
        public bool? EnableCacheBuster { get; set; }
        [DataMember]
        public bool? IgnoreSuppression { get; set; }

        [DataMember]
        public string CampaignItemName_TriggerAction { get; set; }
        [DataMember]
        public string CampaignItemName_NoOpenAction { get; set; }

        //Codes(ID)
        //public ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes? StatusCodeID { get; set; }
        //public ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes? BlastTypeID { get; set; }
        //optional
        public ECN_Framework_Entities.Communicator.Group Group { get; set; }
        public ECN_Framework_Entities.Communicator.Layout Layout { get; set; }
        public KMPlatform.Entity.User CreatedUser { get; set; }
        public KMPlatform.Entity.User UpdatedUser { get; set; }
        public ECN_Framework_Entities.Communicator.BlastSchedule Schedule { get; set; }
        public ECN_Framework_Entities.Communicator.BlastFields Fields { get; set; }
    }
}
