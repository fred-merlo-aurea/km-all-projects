using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class SubscriberDemographicFinal
    {
        public List<Entity.SubscriberDemographicFinal> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicFinal> x = null;
            x = DataAccess.SubscriberDemographicFinal.SelectPublication(pubID, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicFinal> SelectSubscriberOriginal(Guid SFRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicFinal> x = null;
            x = DataAccess.SubscriberDemographicFinal.SelectSubscriberOriginal(SFRecordIdentifier, client).ToList();
            return x;
        }
        public List<Entity.SubscriberDemographicFinal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicFinal> x = null;
            x = DataAccess.SubscriberDemographicFinal.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            return x;
        }
        /// <summary>
        /// - this will return all demos for all SF records for the passed in processCode
        /// - you will need to correctly seperate the list by SFRecordIdentifier
        /// - did it this way to have just one call 
        /// </summary>
        /// <param name="processCode"></param>
        /// <param name="client"></param>
        /// <returns>List<Entity.SubscriberDemographicFinal></returns>
        public List<Entity.SubscriberDemographicFinal> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberDemographicFinal> x = null;
            x = DataAccess.SubscriberDemographicFinal.Select(processCode, client).ToList();
            return x;
        }
        public int Save(Entity.SubscriberDemographicFinal x, KMPlatform.Object.ClientConnections client)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    x.SDFinalID = DataAccess.SubscriberDemographicFinal.Save(x, client);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriberDemographicFinal"));
                }
            }

            return x.SDFinalID;
        }
    }
}
