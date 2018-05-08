using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerLicense
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CustomerNote;

        public static List<ECN_Framework_Entities.Accounts.CustomerLicense> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.CustomerLicense> lCustomerLicense = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lCustomerLicense = ECN_Framework_DataLayer.Accounts.CustomerLicense.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lCustomerLicense, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lCustomerLicense;
        }

        public static ECN_Framework_Entities.Accounts.CustomerLicense GetByCLID(int CLID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.CustomerLicense customerLicense = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customerLicense = ECN_Framework_DataLayer.Accounts.CustomerLicense.GetByCLID(CLID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerLicense.CustomerID && !SecurityCheck(customerLicense, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return customerLicense;
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.CustomerLicense customerLicense, KMPlatform.Entity.User user)
        {
            if (customerLicense != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (customerLicense.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.CustomerLicense> lCustomerLicense, KMPlatform.Entity.User user)
        {
            if (lCustomerLicense != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from l in lCustomerLicense
                                        join c in custList on l.CustomerID equals c.CustomerID
                                        select new { l.CLID };

                    if (securityCheck.Count() != lCustomerLicense.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from l in lCustomerLicense
                                        where l.CustomerID != user.CustomerID
                                        select new { l.CLID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Delete(int CLID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Accounts.CustomerLicense.Delete(CLID, user.UserID);
        }

        public static void Save(ECN_Framework_Entities.Accounts.CustomerLicense customerLicense, KMPlatform.Entity.User user)
        {
            Validate(customerLicense, user);
            customerLicense.CLID = ECN_Framework_DataLayer.Accounts.CustomerLicense.Save(customerLicense);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.CustomerLicense customerLicense, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }


            if (user.IsKMStaff)
            {
                //if (!staff.LicenseUpdateFlag)
                //{
                //    throw new ECN_Framework_Common.Objects.SecurityException();
                //}
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (customerLicense.CLID <= 0 && customerLicense.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customerLicense.CLID > 0 && customerLicense.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
