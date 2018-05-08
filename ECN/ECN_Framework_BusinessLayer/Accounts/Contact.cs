using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Contact
    {
        public static Enums.Entity Entity = Enums.Entity.Contact;

        public static ECN_Framework_Entities.Accounts.Contact GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.Contact contact = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contact = ECN_Framework_DataLayer.Accounts.Contact.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != contact.CustomerID && !SecurityCheck(contact, user))
                throw new SecurityException();

            return contact;
        }

    

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.Contact contact, KMPlatform.Entity.User user)
        {
            if (contact != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (contact.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        public static void Save(ECN_Framework_Entities.Accounts.Contact contact, KMPlatform.Entity.User user)
        {
            Validate(contact, user);
            contact.BillingContactID = ECN_Framework_DataLayer.Accounts.Contact.Save(contact);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.Contact contact, KMPlatform.Entity.User user)
        {
            Enums.Method Method = Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new SecurityException();
            }

            using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (KM.Platform.User.IsChannelAdministrator(user) && !KM.Platform.User.IsSystemAdministrator(user))
                {
                    if (Customer.GetByCustomerID(contact.CustomerID, false).BaseChannelID != Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID)
                    {
                        throw new SecurityException();
                    }
                } supressscope.Complete();
            }

            if (contact.BillingContactID <= 0 && contact.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (contact.BillingContactID > 0 && contact.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (String.IsNullOrWhiteSpace(contact.FirstName))
                errorList.Add(new ECNError(Entity, Method, "Billing First Name is missing"));

            if (String.IsNullOrWhiteSpace(contact.LastName))
                errorList.Add(new ECNError(Entity, Method, "Billing Last Name is missing"));

            if (String.IsNullOrWhiteSpace(contact.ContactTitle))
                errorList.Add(new ECNError(Entity, Method, "Billing Title is missing"));

            if (String.IsNullOrWhiteSpace(contact.Phone))
                errorList.Add(new ECNError(Entity, Method, "Billing Phone is missing"));

            //if (string.IsNullOrWhiteSpace(contact.Fax))
            //    errorList.Add(new ECNError(Entity, Method, "Billing Fax is missing"));

            if (String.IsNullOrWhiteSpace(contact.Email))
                errorList.Add(new ECNError(Entity, Method, "Billing Email is missing"));

            if (String.IsNullOrWhiteSpace(contact.StreetAddress))
                errorList.Add(new ECNError(Entity, Method, "Billing Address is missing"));

            if (String.IsNullOrWhiteSpace(contact.City))
                errorList.Add(new ECNError(Entity, Method, "Billing City is missing"));

            if (String.IsNullOrWhiteSpace(contact.Zip))
                errorList.Add(new ECNError(Entity, Method, "Billing Zip is missing"));

            if (String.IsNullOrWhiteSpace(contact.Country))
                errorList.Add(new ECNError(Entity, Method, "Billing Country is missing"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
