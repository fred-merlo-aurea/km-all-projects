using System;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public abstract class BlastAbstract : Blast
    {
        public BlastAbstract()//was protected
        {
            BlastID = -1;
            CustomerID = null;
            EmailSubject = string.Empty;
            EmailFrom = string.Empty;
            EmailFromName = string.Empty;
            SendTime = null;
            AttemptTotal = null;
            SendTotal = null;
            SendBytes = null;
            StatusCode = string.Empty;
            BlastType = string.Empty;
            CodeID = null;
            LayoutID = null;
            GroupID = null;
            FinishTime = null;
            SuccessTotal = null;
            BlastLog = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            Spinlock = "n";
            ReplyTo = string.Empty;
            TestBlast = string.Empty;
            BlastFrequency = string.Empty;
            RefBlastID = string.Empty;
            BlastSuppression = string.Empty;
            AddOptOuts_to_MS = null;
            DynamicFromName = string.Empty;
            DynamicFromEmail = string.Empty;
            DynamicReplyToEmail = string.Empty;
            BlastEngineID = null;
            HasEmailPreview = false;
            BlastScheduleID = null;
            OverrideAmount = null;
            OverrideIsAmount = null;
            StartTime = null;
            SMSOptInTotal = null;
            CampaignItemID = null;
            NodeID = string.Empty;
            SampleID = null;
            EnableCacheBuster = null;
            IgnoreSuppression = false;
            //Codes(ID)
            //StatusCodeID = null;
            //BlastTypeID = null;
            //validation
            //ErrorList = new List<ValidationError>();
            //optional
            Group = null;
            Layout = null;
            CreatedUser = null;
            UpdatedUser = null;
            Schedule = null;
            Fields = null;
        }
    }
}
