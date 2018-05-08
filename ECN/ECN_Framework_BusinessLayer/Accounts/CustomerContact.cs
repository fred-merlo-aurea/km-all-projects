using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerContact
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CustomerContact;

        public static bool Exists(int contactID)
        {
            return ECN_Framework_DataLayer.Accounts.CustomerContact.Exists(contactID);
        }

        public static ECN_Framework_Entities.Accounts.CustomerContact GetByContactID(int contactID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.CustomerContact customerContact = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customerContact = ECN_Framework_DataLayer.Accounts.CustomerContact.GetByContactID(contactID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerContact.CustomerID && !SecurityCheck(customerContact, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return customerContact;
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerContact> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.CustomerContact> lCustomerContact = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lCustomerContact = ECN_Framework_DataLayer.Accounts.CustomerContact.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lCustomerContact, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lCustomerContact;
        }

        public static void Save(ECN_Framework_Entities.Accounts.CustomerContact customerContact, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;

            if (customerContact.ContactID > 0)
            {
                if (!Exists(customerContact.ContactID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "Customer Contact is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(customerContact, user);
            customerContact.ContactID = ECN_Framework_DataLayer.Accounts.CustomerContact.Save(customerContact);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.CustomerContact customerContact, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!user.IsKMStaff)
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (customerContact.ContactID <= 0 && customerContact.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customerContact.ContactID > 0 && customerContact.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (string.IsNullOrWhiteSpace(customerContact.FirstName))
                errorList.Add(new ECNError(Entity, Method, "First Name is missing"));

            if (string.IsNullOrWhiteSpace(customerContact.LastName))
                errorList.Add(new ECNError(Entity, Method, "Last Name is missing"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.CustomerContact customerContact, KMPlatform.Entity.User user)
        {
            if (customerContact != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (customerContact.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.CustomerContact> lcustomerContact, KMPlatform.Entity.User user)
        {
            if (lcustomerContact != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from e in lcustomerContact
                                        join c in custList on e.CustomerID equals c.CustomerID
                                        select new { e.ContactID };

                    if (securityCheck.Count() != lcustomerContact.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from e in lcustomerContact
                                        where e.CustomerID != user.CustomerID
                                        select new { e.ContactID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

    }
}
