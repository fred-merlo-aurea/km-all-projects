using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class EditionActivityLog
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.EditionHistory;

        public static List<ECN_Framework_Entities.Publisher.EditionActivityLog> GetByEditionID(int editionID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.EditionActivityLog> ealList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ealList = ECN_Framework_DataLayer.Publisher.EditionActivityLog.GetByEditionID(editionID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(ealList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return ealList;
        }

        public static List<ECN_Framework_Entities.Publisher.EditionActivityLog> GetByEditionIDSessionID(int editionID, string sessionID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Publisher.EditionActivityLog> ealList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ealList = ECN_Framework_DataLayer.Publisher.EditionActivityLog.GetByEditionIDSessionID(editionID, sessionID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(ealList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return ealList;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Publisher.EditionActivityLog> leditionActivityLog, KMPlatform.Entity.User user)
        {
            if (leditionActivityLog != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from e in leditionActivityLog
                                        join c in custList on e.CustomerID equals c.CustomerID
                                        select new { e.EditionID };

                    if (securityCheck.Count() != leditionActivityLog.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from e in leditionActivityLog
                                        where e.CustomerID != user.CustomerID
                                        select new { e.EditionID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void DeleteByEditionID(int editionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Publisher.EditionActivityLog.DeleteByEditionID(editionID, user.UserID);
        }

        public static void Validate(ECN_Framework_Entities.Publisher.EditionActivityLog editionActivityLog)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                //if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(editionActivityLog.CustomerID.Value))
                //    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                if (editionActivityLog.EAID <= 0 && (editionActivityLog.CreatedUserID == null)) // || !KMPlatform.BusinessLogic.User.Exists(editionActivityLog.CreatedUserID.Value, editionActivityLog.CustomerID.Value)
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                if (editionActivityLog.EAID > 0 && (editionActivityLog.UpdatedUserID == null)) // || !KMPlatform.BusinessLogic.User.Exists(editionActivityLog.UpdatedUserID.Value, editionActivityLog.CustomerID.Value)
                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                scope.Complete();
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Publisher.EditionActivityLog editionActivityLog, KMPlatform.Entity.User user)
        {
            Validate(editionActivityLog);

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Publisher.EditionActivityLog.Save(editionActivityLog);
                scope.Complete();
            }
            return;
        }
    }
}
