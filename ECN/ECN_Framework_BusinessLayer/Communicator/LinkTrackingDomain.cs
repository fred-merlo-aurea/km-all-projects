using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LinkTrackingDomain
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DomainTracker;

        public static int Save(ECN_Framework_Entities.Communicator.LinkTrackingDomain item, KMPlatform.Entity.User user)
        {
            Validate(item, user);
            using (TransactionScope scope = new TransactionScope())
            {
                item.LinkTrackingDomainID = ECN_Framework_DataLayer.Communicator.LinkTrackingDomain.Save(item);
                scope.Complete();
            }
            return item.LinkTrackingDomainID;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.LinkTrackingDomain item, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (item.CustomerID == -1)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (Exists(item.Domain, item.CustomerID, item.LTID))
                {
                    errorList.Add(new ECNError(Entity, Method, "This domain already exists. Please select a different domain."));
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static bool Exists(string domainName, int customerID, int LTID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkTrackingDomain.Exists(domainName, customerID, LTID);
                scope.Complete();
            }
            return exists;
        }

        public static void Delete(int linkTrackingDomainID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingDomain.Delete(linkTrackingDomainID, user.UserID);
                scope.Complete();
            }
        }

        public static void DeleteAll(int customerID, int LTID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LinkTrackingDomain.DeleteAll(customerID, LTID, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingDomain> GetByCustomerID(int customerID, int LTID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingDomain> linkTrackingDomainList = new List<ECN_Framework_Entities.Communicator.LinkTrackingDomain>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkTrackingDomainList = ECN_Framework_DataLayer.Communicator.LinkTrackingDomain.GetByCustomerID(customerID, LTID);
                scope.Complete();
            }
            return linkTrackingDomainList;
        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingDomain GetByDomain(string domain, int LTID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingDomain linkTrackingDomain = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkTrackingDomain = ECN_Framework_DataLayer.Communicator.LinkTrackingDomain.GetByDomain(domain, user.CustomerID, LTID);
                scope.Complete();
            }
            return linkTrackingDomain;
        }


    }
}
