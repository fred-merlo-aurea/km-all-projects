using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerConfig
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CustomerConfig;

        public static bool Exists(int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.CustomerConfig.Exists(customerID);
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerConfig> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.CustomerConfig> lCustomerConfig = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lCustomerConfig = ECN_Framework_DataLayer.Accounts.CustomerConfig.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lCustomerConfig, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lCustomerConfig;
        }

        public static int Save(ECN_Framework_Entities.Accounts.CustomerConfig customerConfig, KMPlatform.Entity.User user)
        {
            Validate(customerConfig, user);
            return customerConfig.CustomerConfigID = ECN_Framework_DataLayer.Accounts.CustomerConfig.Save(customerConfig);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.CustomerConfig customerConfig, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && KM.Platform.User.IsChannelAdministrator(user))
            {
                using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerConfig.CustomerID.Value, false).BaseChannelID != ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID)
                    {
                        throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                    supressscope.Complete();
                }
            }

            if (customerConfig.CustomerConfigID <= 0 && customerConfig.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customerConfig.CustomerConfigID > 0 && customerConfig.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.CustomerConfig> lCustomerConfig, KMPlatform.Entity.User user)
        {
            if (lCustomerConfig != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from l in lCustomerConfig
                                        join c in custList on l.CustomerID equals c.CustomerID
                                        select new { l.CustomerConfigID };

                    if (securityCheck.Count() != lCustomerConfig.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from l in lCustomerConfig
                                        where l.CustomerID != user.CustomerID
                                        select new { l.CustomerConfigID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Accounts.CustomerConfig.Delete(customerID, user.UserID);
        }
    }
}
