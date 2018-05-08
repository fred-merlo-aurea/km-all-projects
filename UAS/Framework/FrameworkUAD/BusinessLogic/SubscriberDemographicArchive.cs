using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberDemographicArchive
    {
        public List<Entity.SubscriberDemographicArchive> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicArchive> x = null;
            x = DataAccess.SubscriberDemographicArchive.SelectPublication(pubID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicArchive> SelectSubscriberOriginal(Guid SARecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicArchive> x = null;
            x = DataAccess.SubscriberDemographicArchive.SelectSubscriberOriginal(SARecordIdentifier, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicArchive> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicArchive> x = null;
            x = DataAccess.SubscriberDemographicArchive.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }

        public int Save(Entity.SubscriberDemographicArchive x, KMPlatform.Object.ClientConnections client)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SDArchiveID = DataAccess.SubscriberDemographicArchive.Save(x, client);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriberDemographicArchive"));
                }
            }

            return x.SDArchiveID;
        }
    }
}
