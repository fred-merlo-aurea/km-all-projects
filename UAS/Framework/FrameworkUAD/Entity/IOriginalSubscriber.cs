using System.Collections.Generic;

namespace FrameworkUAD.Entity
{
    public interface IOriginalSubscriber : ISubscriber
    {
        int Score { get; set; }
        int EmailStatusID { get; set; }
        HashSet<SubscriberDemographicOriginal> DemographicOriginalList { get; set; }
    }
}
