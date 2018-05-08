using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberDemographicTransformed
    {
        public List<Entity.SubscriberDemographicTransformed> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicTransformed> x = null;
            x = DataAccess.SubscriberDemographicTransformed.SelectPublication(pubID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicTransformed> SelectSubscriberOriginal(Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicTransformed> x = null;
            x = DataAccess.SubscriberDemographicTransformed.SelectSubscriberOriginal(SORecordIdentifier, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicTransformed> SelectSubscriberTransformed(Guid STRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicTransformed> x = null;
            x = DataAccess.SubscriberDemographicTransformed.SelectSubscriberTransformed(STRecordIdentifier, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicTransformed> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicTransformed> x = null;
            x = DataAccess.SubscriberDemographicTransformed.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }

        public int Save(Entity.SubscriberDemographicTransformed x, KMPlatform.Object.ClientConnections client)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SubscriberDemographicTransformedID = DataAccess.SubscriberDemographicTransformed.Save(x, client);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriberDemographicTransformed"));
                }
            }

            return x.SubscriberDemographicTransformedID;
        }
        public System.Data.DataTable MafFieldUpdateAction(string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.SubscriberDemographicTransformed.MafFieldUpdateAction(processCode, client);
        }
    }
}
