using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Communicator
{
    //wgh - this is for blast engine dynamic content possibly
        [Serializable]
    public class BlastEmails
    {
        BlastEmails()
        {
            BlastID = null;
            EmailID = null;
            MailRoute = string.Empty;
            EmailAddress = string.Empty;
            CustomerID = null;
            GroupID = null;
            FormatTypeCode = string.Empty;
            SubscribeTypeCode = string.Empty;
            ConversionTrkCDE = string.Empty;
            CreatedOn = null;
            LastChanged = null;
            BounceAddress = string.Empty;
            CurrDate = null;
            IsMTAPriority = null;
            Additions = null;
        }

        int? BlastID;
        int? EmailID;
        string MailRoute;
        string EmailAddress;
        int? CustomerID;
        int? GroupID;
        string FormatTypeCode;
        string SubscribeTypeCode;
        string ConversionTrkCDE;
        DateTime? CreatedOn;
        DateTime? LastChanged;
        string BounceAddress;
        DateTime? CurrDate;
        bool? IsMTAPriority;
        List<BlastEmailsAdditional> Additions;
    }

    public class BlastEmailsAdditional
    {
        string FieldName;
        string FieldValue;
    }
}
