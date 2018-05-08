using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class EditionHistory
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.EditionHistory;

        public static bool Exists(int editionHistoryID)
        {
            return ECN_Framework_DataLayer.Publisher.EditionHistory.Exists(editionHistoryID);
        }

        public static bool ExistsByEditionID(int editionID)
        {
            return ECN_Framework_DataLayer.Publisher.EditionHistory.ExistsByEditionID(editionID);
        }

        public static ECN_Framework_Entities.Publisher.EditionHistory GetByEditionID(int editionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Publisher.EditionHistory editionHistory = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                editionHistory = ECN_Framework_DataLayer.Publisher.EditionHistory.GetByEditionID(editionID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(editionHistory, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return editionHistory;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Publisher.EditionHistory editionHistory, KMPlatform.Entity.User user)
        {
            if (editionHistory != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (editionHistory.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        public static void Validate(ECN_Framework_Entities.Publisher.EditionHistory editionHistory)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(editionHistory.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                if (editionHistory.EditionHistoryID <= 0 && (editionHistory.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(editionHistory.CreatedUserID.Value, editionHistory.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                if (editionHistory.EditionHistoryID > 0 && (editionHistory.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(editionHistory.UpdatedUserID.Value, editionHistory.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

                scope.Complete();
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Publisher.EditionHistory editionHistory, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (editionHistory.EditionHistoryID > 0)
            {
                if (!Exists(editionHistory.EditionHistoryID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "EditionHistoryID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(editionHistory);

            ECN_Framework_DataLayer.Publisher.EditionHistory.Save(editionHistory);

            return;
        }
    }
}
