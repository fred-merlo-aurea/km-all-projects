using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class SubscriptionManagement
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.SubscriptionManagement;

        public static bool Exists(int SMID, int CustomerID)
        {
            bool exists = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.SubscriptionManagement.Exists(SMID, CustomerID);
                scope.Complete();
            }

            return exists;
        }

        public static List<ECN_Framework_Entities.Accounts.SubscriptionManagement> GetByBaseChannelID(int BaseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagement> retList = new List<ECN_Framework_Entities.Accounts.SubscriptionManagement>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.SubscriptionManagement.GetByBaseChannelID(BaseChannelID);
                scope.Complete();
            }

            return retList;
        }

        public static ECN_Framework_Entities.Accounts.SubscriptionManagement GetBySubscriptionManagementID(int SMID)
        {
            ECN_Framework_Entities.Accounts.SubscriptionManagement retItem = new ECN_Framework_Entities.Accounts.SubscriptionManagement();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Accounts.SubscriptionManagement.GetBySubscriptionManagementID(SMID);
                scope.Complete();
            }

            return retItem;
        }

        public static bool Exists(string PageName, int CustomerID)
        {
            bool exists = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.SubscriptionManagement.Exists(PageName, CustomerID);
                scope.Complete();
            }

            return exists;
        }

        public static void Delete(int SMID, KMPlatform.Entity.User User)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(SMID, User.CustomerID))
            {
                //probably need to add validation to make sure that no blasts have been sent out using the current SM page
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    //Delete the SM Group and do the deleting of UDFs in ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.Delete()
                    ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.Delete(SMID, User);

                    ECN_Framework_DataLayer.Accounts.SubscriptionManagement.Delete(SMID);

                    scope.Complete();
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Subscription Management Page does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static void Validate(ECN_Framework_Entities.Accounts.SubscriptionManagement sm, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (sm.SubscriptionManagementID > 0)
            {
                if (!Exists(sm.SubscriptionManagementID, user.CustomerID))
                {
                    errorList.Add(new ECNError { Entity = Enums.Entity.SubscriptionManagement, ErrorMessage = "Subscription Management Page doesn't exist", Method = Method });
                }
            }
            if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(sm.Name))
            {
                errorList.Add(new ECNError { Entity = Enums.Entity.SubscriptionManagement, ErrorMessage = "Invalid Name", Method = Method });
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }

        }

        public static int Save(ECN_Framework_Entities.Accounts.SubscriptionManagement sm, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            List<ECNError> errorList = new List<ECNError>();
            Validate(sm, user);

            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                if (sm.SubscriptionManagementID > 0)
                {
                    sm.UpdatedUserID = user.UserID;
                }
                else
                {
                    sm.CreatedUserID = user.UserID;
                }
                retID = ECN_Framework_DataLayer.Accounts.SubscriptionManagement.Save(sm);
                scope.Complete();
            }
            return retID;
        }
    }
}
