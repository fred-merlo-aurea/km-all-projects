using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerNote
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CustomerNote;

        public static ECN_Framework_Entities.Accounts.CustomerNote GetByNoteID(int noteID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.CustomerNote customerNote = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customerNote = ECN_Framework_DataLayer.Accounts.CustomerNote.GetByNoteID(noteID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerNote.CustomerID && !SecurityCheck(customerNote, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return customerNote;
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerNote> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.CustomerNote> lCustomerNote = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lCustomerNote = ECN_Framework_DataLayer.Accounts.CustomerNote.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lCustomerNote, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return lCustomerNote;
        }

        public static void Save(ECN_Framework_Entities.Accounts.CustomerNote customerNote, KMPlatform.Entity.User user)
        {
            Validate(customerNote, user);
            customerNote.NoteID = ECN_Framework_DataLayer.Accounts.CustomerNote.Save(customerNote);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.CustomerNote customerNote, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!user.IsKMStaff)
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (customerNote.NoteID <= 0 && customerNote.CreatedUserID == null) 
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customerNote.NoteID > 0 && customerNote.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (string.IsNullOrWhiteSpace(customerNote.Notes))
                errorList.Add(new ECNError(Entity, Method, "Notes is missing"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.CustomerNote customerNote, KMPlatform.Entity.User user)
        {
            if (customerNote != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (customerNote.CustomerID != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.CustomerNote> lcustomerNote, KMPlatform.Entity.User user)
        {
            if (lcustomerNote != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from e in lcustomerNote
                                        join c in custList on e.CustomerID equals c.CustomerID
                                        select new { e.NoteID };

                    if (securityCheck.Count() != lcustomerNote.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from e in lcustomerNote
                                        where e.CustomerID != user.CustomerID
                                        select new { e.NoteID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

    }
}
