using System.Collections.Generic;

namespace FrameworkSubGen.DataAccess
{
    public class SubscriberDemographic
    {
        public static bool Save(IEnumerable<Entity.SubscriberDemographic> subscriberDemographics)
        { 
            var columnNames = new[]
                                  {
                                      "subscriber_id",
                                      "account_id",
                                      "field_id",
                                      "text_value",
                                      "DateCreated",
                                      "IsProcessed",
                                  };

            return BulkDataInsert.BulkInsert(subscriberDemographics, "SubscriberDemographic", columnNames, null, "FrameworkSubGen.DataAccess.SubscriberDemographic", "Save");
        }
    }
}
