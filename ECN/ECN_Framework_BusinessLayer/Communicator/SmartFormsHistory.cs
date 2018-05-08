using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class SmartFormsHistory
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.SmartForm;
        private static ECN_Framework_Common.Objects.Enums.Entity Entity = Enums.Entity.SmartFormsHistory;
            static readonly uint timeStamp = (uint)DateTime.Now.Ticks;

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups_Full_Access.ToString()))
                if (KM.Platform.User.IsSystemAdministrator(user))
                return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups_Full_Access.ToString()))
                if (KM.Platform.User.IsSystemAdministrator(user))
                    return true;
            }
            else if(AccessCode == KMPlatform.Enums.Access.Delete)
            {
                if (KM.Platform.User.IsSystemAdministrator(user))
                    return true;
            }
            return false;
        }

        public static List<ECN_Framework_Entities.Communicator.SmartFormsHistory> GetByGroupID(int groupID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.SmartFormsHistory> historyList = new List<ECN_Framework_Entities.Communicator.SmartFormsHistory>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                historyList = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.GetByGroupID(groupID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(historyList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return historyList;
        }

        public static ECN_Framework_Entities.Communicator.SmartFormsHistory GetBySmartFormID(int smartFormID, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.SmartFormsHistory history = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                history = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.GetBySmartFormID(smartFormID, groupID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(history, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return history;
        }

        public static ECN_Framework_Entities.Communicator.SmartFormsHistory GetBySmartFormID_NoAccessCheck(int smartFormID, int groupID)
        {
            ECN_Framework_Entities.Communicator.SmartFormsHistory history = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                history = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.GetBySmartFormID(smartFormID, groupID);
                scope.Complete();
            }

            return history;
        }

        private static ECN_Framework_Entities.Communicator.SmartFormsHistory GetBySmartFormID(int smartFormID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.SmartFormsHistory history = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                history = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.GetBySmartFormID(smartFormID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(history, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return history;
        }

        public static void Delete(int smartFormID, KMPlatform.Entity.User user)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetBySmartFormID(smartFormID, user);

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.SmartFormsHistory.Delete(smartFormID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.SmartFormsHistory history)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            List<string> arEmails = (history.Response_AdminEmail.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList());

            //if any field is present - Email Address and Email Subject are required - Automated Internal Email
            //Email address can be list of emails
            if (arEmails.Count>0 || !String.IsNullOrEmpty(history.Response_AdminMsgSubject) || !String.IsNullOrEmpty(history.Response_AdminMsgBody))
            {
                if (arEmails.Count < 1)
                { errorList.Add(new ECNError(Entity, Method, "Admin Email Address must be a valid email address")); }
                else
                {
                    foreach (string emailAddress in arEmails)
                    {
                        if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(emailAddress))
                        {
                            errorList.Add(new ECNError(Entity, Method, "Admin Email Address must be a valid email address"));
                            break;
                        }
                    }
                }

                if (String.IsNullOrEmpty(history.Response_AdminMsgSubject))
                    errorList.Add(new ECNError(Entity, Method, "Admin Message Subject must not be empty "));

                if (String.IsNullOrEmpty(history.Response_AdminMsgBody))
                    errorList.Add(new ECNError(Entity, Method, "Admin Message Body must not be empty "));
            }


            //if any field is present - all required - Automated Response to Submitted Email
            if (!String.IsNullOrEmpty(history.Response_FromEmail) || !String.IsNullOrEmpty(history.Response_UserMsgSubject) || !String.IsNullOrEmpty(history.Response_UserMsgBody))
            {
                
                if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(history.Response_FromEmail))
                    errorList.Add(new ECNError(Entity, Method, "From Email Address must be a valid email address"));          
                if (String.IsNullOrEmpty(history.Response_UserMsgSubject))
                    errorList.Add(new ECNError(Entity, Method, "From Message Subject must not be empty "));
                if (String.IsNullOrEmpty(history.Response_UserMsgBody))
                    errorList.Add(new ECNError(Entity, Method, "From Message Body must not be empty "));
            }

            
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }       
        }

        public static int Save(ECN_Framework_Entities.Communicator.SmartFormsHistory history, KMPlatform.Entity.User user)
        {
            Validate(history);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(history, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                history.SmartFormID = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.Save(history);
                scope.Complete();
            }

            return history.SmartFormID;
        }

        public static bool Exists(int smartFormID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.Exists(smartFormID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static int GetGroupID(int customerID, int smartFormID)
        {
            int groupID = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupID = ECN_Framework_DataLayer.Communicator.SmartFormsHistory.GetGroupID(customerID, smartFormID);
                scope.Complete();
            }

            return groupID;
        }
    }
}
