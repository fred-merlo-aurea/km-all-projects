using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberDemographicInvalid
    {
        public List<Entity.SubscriberDemographicInvalid> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicInvalid> x = null;
            x = DataAccess.SubscriberDemographicInvalid.SelectPublication(pubID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicInvalid> SelectSubscriberOriginal(Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicInvalid> x = null;
            x = DataAccess.SubscriberDemographicInvalid.SelectSubscriberOriginal(SORecordIdentifier, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicInvalid> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicInvalid> x = null;
            x = DataAccess.SubscriberDemographicInvalid.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }
         
        public int Save(Entity.SubscriberDemographicInvalid x, KMPlatform.Object.ClientConnections client)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SDInvalidID = DataAccess.SubscriberDemographicInvalid.Save(x, client);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriberDemographicInvalid"));
                }
            }

            return x.SDInvalidID;
        }
    }
}
