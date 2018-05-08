using System.Collections.Generic;

namespace FrameworkSubGen.DataAccess
{
    public class SubscriberDemographicDetail
    {
        public static bool Save(IEnumerable<Entity.SubscriberDemographicDetail> subscriberDemographicDetails)
        {
            var columnNames = new[]
                                  {
                                     "subscriber_id",
                                     "option_id",
                                     "field_id",
                                     "account_id",
                                     "value",
                                     "DateCreated",
                                     "IsProcessed"
                                   };

            return BulkDataInsert.BulkInsert(subscriberDemographicDetails, "SubscriberDemographicDetail", columnNames, null, "FrameworkSubGen.DataAccess.SubscriberDemographicDetail", "Save");
        }
    }
}
