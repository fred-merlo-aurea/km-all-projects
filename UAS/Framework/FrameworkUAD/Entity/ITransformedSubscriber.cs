using System;
using System.Collections.Generic;

namespace FrameworkUAD.Entity
{
    public interface ITransformedSubscriber : ISubscriber
    {
        int? EmailStatusID { get; set; }
        string LatLonMsg { get; set; }
        bool IsLatLonValid { get; set; }
        HashSet<SubscriberDemographicTransformed> DemographicTransformedList { get; set; }
        Guid STRecordIdentifier { get; set; }
    }
}
