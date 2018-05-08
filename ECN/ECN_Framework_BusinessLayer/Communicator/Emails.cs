using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Email
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Email;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Email;

        //KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Add_Emails.ToString()) ||
        //KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Import_Data.ToString()) ||
        //KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Clean_Emails.ToString()))

        public static DataTable EmailSearch(string FilterType, string searchTerm, KMPlatform.Entity.User user, string currentBaseChannel, string customerID, int currentPage, int pageSize, string sortColumn, string sortDirection)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.EmailSearch, KMPlatform.Enums.Access.FullAccess))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable emails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emails = ECN_Framework_DataLayer.Communicator.Email.EmailSearch(FilterType, searchTerm, currentBaseChannel, customerID,currentPage, pageSize, sortColumn, sortDirection);
                scope.Complete();
            }
            return emails;
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailIDGroupID(int emailID, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Email email = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                email = ECN_Framework_DataLayer.Communicator.Email.GetByEmailIDGroupID(emailID, groupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(email, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return email;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Email GetByEmailIDGroupID_NoAccessCheck(int emailID, int groupID)
        {
            ECN_Framework_Entities.Communicator.Email email = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                email = ECN_Framework_DataLayer.Communicator.Email.GetByEmailIDGroupID(emailID, groupID);
                scope.Complete();
            }
            return email;
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailID(int emailID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Email email = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                email = ECN_Framework_DataLayer.Communicator.Email.GetByEmailID(emailID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(email, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return email;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Email GetByEmailID_NoAccessCheck(int emailID)
        {
            ECN_Framework_Entities.Communicator.Email email = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                email = ECN_Framework_DataLayer.Communicator.Email.GetByEmailID(emailID);
                scope.Complete();
            }

            return email;
        }

        public static List<ECN_Framework_Entities.Communicator.Email> GetByGroupID(int groupID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Email> emailList = new List<ECN_Framework_Entities.Communicator.Email>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailList = ECN_Framework_DataLayer.Communicator.Email.GetByGroupID(groupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailList;

        }

        public static bool Exists(int emailID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Email.Exists(emailID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(string emailAddress, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Email.Exists(emailAddress, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_BaseChannel(string emailaddress, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Email.Exists_BaseChannel(emailaddress, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsByGroup(string emailAddress, int groupID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Email.ExistsByGroup(emailAddress, groupID);
                scope.Complete();
            }
            return exists;
        }

        public static bool IsValidEmailAddress(string emailAddress)
        {
            bool isValid = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                isValid = ECN_Framework_DataLayer.Communicator.Email.IsValidEmailAddress(emailAddress);
                scope.Complete();
            }
            return isValid;
        }

        public static bool IsValidEmailAddress_UseAmbientTransaction(string emailAddress)
        {
            bool isValid = false;
            using (TransactionScope scope = new TransactionScope())
            {
                isValid = ECN_Framework_DataLayer.Communicator.Email.IsValidEmailAddress(emailAddress);
                scope.Complete();
            }
            return isValid;
        }

        public static bool IsValidEmailAddressForBlast(int EmailID, string EmailAddress, int CustomerID, int refGroupID, int refBlastID)
        {
            bool isValid = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                isValid = ECN_Framework_DataLayer.Communicator.Email.IsValidEmailAddressForBlast(EmailID, EmailAddress, CustomerID, refGroupID, refBlastID);
                scope.Complete();
            }
            return isValid;
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailAddress(string emailAddress, int customerID)
        {
            ECN_Framework_Entities.Communicator.Email email = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                email = ECN_Framework_DataLayer.Communicator.Email.GetByEmailAddress(emailAddress, customerID);
                scope.Complete();
            }
            return email;
        }

        public static ECN_Framework_Entities.Communicator.Email GetByEmailAddress(string emailAddress, int customerID, int emailID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Email email = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                email = ECN_Framework_DataLayer.Communicator.Email.GetByEmailAddress(emailAddress, customerID, emailID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(email, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return email;
        }

        public static bool Exists(string emailAddress, int customerID, int emailID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Email.Exists(emailAddress, customerID, emailID);
                scope.Complete();
            }
            return exists;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Email email)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (email.EmailID <= 0)
                errorList.Add(new ECNError(Entity, Method, "EmailID is invalid"));

            if (!IsValidEmailAddress(email.EmailAddress))
                errorList.Add(new ECNError(Entity, Method, "Email Address is invalid"));

            if (Exists(email.EmailAddress, email.CustomerID, email.EmailID))
            {
                errorList.Add(new ECNError(Entity, Method, "Email Address already exists for this account"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.Email email)
        {
            Validate(email);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Email.Save(email);
                scope.Complete();
            }
        }

        public static void Save(KMPlatform.Entity.User user, ECN_Framework_Entities.Communicator.Email email, int groupID, string source = "")
        {
            Validate(email);
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(email, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Email.Save(email);
                StringBuilder xmlUDF = new StringBuilder("");
                xmlUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>");
                StringBuilder xmlProfile = new StringBuilder("");
                xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
                xmlProfile.Append("<emailaddress>" + email.EmailAddress + "</emailaddress>");
                xmlProfile.Append("<subscribetypecode>" + email.SubscribeTypeCode + "</subscribetypecode>");
                xmlProfile.Append("</Emails></XML>");
                EmailGroup.ImportEmails(user, user.CustomerID, groupID, xmlProfile.ToString(), xmlUDF.ToString(), email.FormatTypeCode, email.SubscribeTypeCode, true, "", source);
                scope.Complete();
            }
        }

        public static DataTable GetColumnNames()
        {
            DataTable dt = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Email.GetColumnNames();
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetColumnNames_UseAmbientTransaction()
        {
            DataTable dt = null;

            using (TransactionScope scope = new TransactionScope())
            {
                dt = ECN_Framework_DataLayer.Communicator.Email.GetColumnNames();
                scope.Complete();
            }
            return dt;
        }

        public static void UpdateEmailAddress(int groupID, int customerID, string newEmailAddress, string oldEmailAddress, KMPlatform.Entity.User user, string source)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Email.UpdateEmailAddress(groupID, customerID, newEmailAddress, oldEmailAddress, source);
                scope.Complete();
            }
        }

        public static void MergeProfiles(int OldEmailID, int NewEmailID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Email oldEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(OldEmailID, user);
            ECN_Framework_Entities.Communicator.Email newEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(NewEmailID, user);

            if (oldEmail != null && newEmail != null && oldEmail.CustomerID != newEmail.CustomerID)
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Email.MergeProfiles(OldEmailID, NewEmailID);
                scope.Complete();
            }
        }

        public static string GetGUIDForDummyEmail()
        {
            string GUID = string.Empty;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                GUID = ECN_Framework_DataLayer.DataFunctions.GetNewGUID();
                scope.Complete();
            }

            return GUID;
        }

        public static void UpdateEmail_BaseChannel(string OldEmail, string NewEmail, int BaseChannelID, int UserID, string source)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Email.UpdateEmail_BaseChannel(OldEmail, NewEmail, BaseChannelID, UserID,source);
                scope.Complete();
            }
        }

        public static void UpdateEmail_Customer(string OldEmail, string NewEmail, int CustomerID, int UserID, string source)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Email.UpdateEmail_Customer(OldEmail, NewEmail, CustomerID, UserID,source);
                scope.Complete();
            }
        }

        public static DataTable GetEmailsForWATT_NXTBookSync(int groupID, bool job1, DateTime? dateFrom, string Field = "", string FieldValue = "", bool DoFullNXTBookSync = false)
        {
            DataTable retList = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.Email.GetEmailsForWATT_NXTBookSync(groupID,job1, dateFrom, Field, FieldValue, DoFullNXTBookSync);
                scope.Complete();
            }

            return retList;
        }

    }
}
