using System;

namespace FrameworkUAD.Entity
{
    interface ISubscriberDemographic
    {
        int CreatedByUserID { get; set; }
        string MAFField { get; set; }
        bool NotExists { get; set; }
        int PubID { get; set; }
        Guid SORecordIdentifier { get; set; }
        string Value { get; set; }
        int DemographicUpdateCodeId { get; set; }
        bool IsAdhoc { get; set; }
        string ResponseOther { get; set; }
    }
}
