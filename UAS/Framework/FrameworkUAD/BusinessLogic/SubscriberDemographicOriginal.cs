using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberDemographicOriginal
    {
        public List<Entity.SubscriberDemographicOriginal> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicOriginal> x = null;
            x = DataAccess.SubscriberDemographicOriginal.SelectPublication(pubID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicOriginal> SelectSubscriberOriginal(Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicOriginal> x = null;
            x = DataAccess.SubscriberDemographicOriginal.SelectSubscriberOriginal(SORecordIdentifier, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicOriginal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicOriginal> x = null;
            x = DataAccess.SubscriberDemographicOriginal.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }

        public List<Entity.SubscriberDemographicOriginal> SelectForSORecordIdentifier(string SORecordIdentifierList, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicOriginal> x = null;
            x = DataAccess.SubscriberDemographicOriginal.SelectForSORecordIdentifier(SORecordIdentifierList, client);
            return x;
        }

        public int Save(Entity.SubscriberDemographicOriginal x, KMPlatform.Object.ClientConnections client)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SDOriginalID = DataAccess.SubscriberDemographicOriginal.Save(x, client);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriberDemographicOriginal"));
                }
            }

            return x.SDOriginalID;
        }

    }
}
