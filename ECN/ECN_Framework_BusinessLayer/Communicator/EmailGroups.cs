using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EmailGroup
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        
        static readonly KMPlatform.Enums.ServiceFeatures EmailsServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Email;
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.EmailGroup;

        //KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Add_Emails.ToString()) ||
        //KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Import_Data.ToString()) ||
        //KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Clean_Emails.ToString()))


        public static void Validate(ECN_Framework_Entities.Communicator.EmailGroup emailGroup)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            if (emailGroup.CustomerID <= 0)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }

            if (emailGroup.GroupID <= 0)
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));

            if (emailGroup.FormatTypeCode == null)
                errorList.Add(new ECNError(Entity, Method, "FormatTypeCode is invalid"));

            if (emailGroup.SubscribeTypeCode == null)
                errorList.Add(new ECNError(Entity, Method, "SubscribeTypeCode is invalid"));
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static bool ValidForTracking(int blastID, int emailID)
        {
            bool track = false;

            if (blastID == 0 || emailID == 0)
            {
                return track;
            }

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (ECN_Framework_DataLayer.Communicator.EmailGroup.ValidForTracking(blastID, emailID) > 0)
                    {
                        track = true;
                    }
                    scope.Complete();
                }
            }
            catch (Exception) { }
            return track;
        }

        public static bool Exists(int emailID, int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.EmailGroup.Exists(emailID, groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(string emailAddress, int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.EmailGroup.Exists(emailAddress, groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.EmailGroup GetByEmailIDGroupID(int emailID, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailGroup = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByEmailIDGroupID(emailID, groupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailGroup, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailGroup; 
        }

        public static ECN_Framework_Entities.Communicator.EmailGroup GetByEmailIDGroupID_NoAccessCheck(int emailID, int groupID)
        {
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailGroup = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByEmailIDGroupID(emailID, groupID);
                scope.Complete();
            }

            return emailGroup;
        }

        public static ECN_Framework_Entities.Communicator.EmailGroup GetByEmailAddressGroupID(string emailAddress, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailGroup = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(emailAddress, groupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailGroup, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailGroup;
        }

        public static ECN_Framework_Entities.Communicator.EmailGroup GetByEmailAddressGroupID_NoAccessCheck(string emailAddress, int groupID)
        {
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailGroup = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(emailAddress, groupID);
                scope.Complete();
            }
            return emailGroup;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailGroup> GetByEmailID(int emailID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.EmailGroup> emailGroupList = new List<ECN_Framework_Entities.Communicator.EmailGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailGroupList = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByEmailID(emailID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailGroupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailGroupList; 
        }

        public static List<ECN_Framework_Entities.Communicator.EmailGroup> GetByGroupID(int groupID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.EmailGroup> emailGroupList = new List<ECN_Framework_Entities.Communicator.EmailGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailGroupList = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByGroupID(groupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailGroupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailGroupList;
        }

        public static int ValidateEmails(int groupID, int userID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.EmailGroup.ValidateEmails(groupID, userID);
                scope.Complete();
            }
            return count;
        }

        public static int DeleteBadEmails(int groupID, int userID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            int count = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                count = ECN_Framework_DataLayer.Communicator.EmailGroup.DeleteBadEmails(groupID, userID);
                scope.Complete();
            }
            return count;
        }

        public static void Delete(int groupID, int emailID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(emailID, groupID, user.CustomerID))
            {
                //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                GetByEmailIDGroupID(emailID, groupID, user);

                if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                string blastIDs = ECN_Framework_BusinessLayer.Communicator.Blast.GetSentByGroupID(groupID);
                if (ECN_Framework_BusinessLayer.Activity.BlastActivitySends.ActivityByBlastIDsEmailID(blastIDs, emailID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Email has activity for this Group"));
                    throw new ECNException(errorList);
                }

                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, user);
                if (group.MasterSupression != null && group.MasterSupression.Value == 1)
                {
                    DeleteFromMasterSuppressionGroup(group.GroupID, emailID, user);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.EmailGroup.Delete(groupID, emailID, user.UserID);
                        scope.Complete();
                    }
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Email Address does not exist in the Group"));
                throw new ECNException(errorList);
            }
        }

        public static void Delete_NoAccessCheck(int groupID, int emailID, int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(emailID, groupID, customerID))
            {
                //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                GetByEmailIDGroupID_NoAccessCheck(emailID, groupID);

                
                string blastIDs = ECN_Framework_BusinessLayer.Communicator.Blast.GetSentByGroupID(groupID);
                if (ECN_Framework_BusinessLayer.Activity.BlastActivitySends.ActivityByBlastIDsEmailID(blastIDs, emailID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Email has activity for this Group"));
                    throw new ECNException(errorList);
                }

                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                if (group.MasterSupression != null && group.MasterSupression.Value == 1)
                {
                    DeleteFromMasterSuppressionGroup_NoAccessCheck(group.GroupID, emailID, customerID, user);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.EmailGroup.Delete(groupID, emailID, user.UserID);
                        scope.Complete();
                    }
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Email Address does not exist in the Group"));
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int groupID, KMPlatform.Entity.User user)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            Group.GetByGroupID(groupID, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.Delete(groupID, user.UserID);
                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.EmailGroup emailGroup, bool emailAddressOnly, string xmlProfile, string xmlUDF, KMPlatform.Entity.User user, string filename="", string source = "")
        {
            Validate(emailGroup);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailGroup,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                emailGroup.EmailGroupID = ECN_Framework_DataLayer.Communicator.EmailGroup.Save(emailGroup, emailAddressOnly, xmlProfile, xmlUDF, user.UserID, filename, source);
                scope.Complete();
            }
            return emailGroup.EmailGroupID;
        }

        public static DataTable ImportEmails(KMPlatform.Entity.User user, int customerID, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string filename = "", string source = "")
        {
            DataTable dtReturn = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ImportEmails) && !KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ExternalImport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, user);
            if (group.MasterSupression == null || group.MasterSupression.Value == 0)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmails(user.UserID, customerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, filename, source);
                    scope.Complete();
                }
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportMSEmails(user.UserID, customerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, filename,source);
                    scope.Complete();
                }
            }

            return dtReturn;
        }

        public static DataTable ImportEmails_PreImportResults(KMPlatform.Entity.User user, int customerID, int groupID, string xmlProfile)
        {
            DataTable dtReturn = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ImportEmails) && !KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ExternalImport))
                throw new ECN_Framework_Common.Objects.SecurityException();

            dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmails_PreImportResults(customerID, groupID, xmlProfile);

            return dtReturn;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static DataTable ImportEmails_NoAccessCheck(KMPlatform.Entity.User user, int customerID, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string filename = "",string source = "")
        {
            DataTable dtReturn = null;

            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
            if (group.MasterSupression == null || group.MasterSupression.Value == 0)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmails(user.UserID, customerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, filename, source);
                    scope.Complete();
                }
            }
            else
        {
                using (TransactionScope scope = new TransactionScope())
                {
                    dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportMSEmails(user.UserID, customerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, filename);
                    scope.Complete();
                }
            }

            return dtReturn;
        }

        public static DataTable ExportFromImportEmails(KMPlatform.Entity.User user, string filename, string actionCode)
        {
            DataTable dtReturn = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ImportEmails))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ExportFromImportEmails(filename, actionCode);
                scope.Complete();
            }

            return dtReturn;
        }

        public static DataTable ImportEmailsOverrideCustomer(KMPlatform.Entity.User user, int customerID, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly)
        {
            DataTable dtReturn = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ImportEmails))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmails(user.UserID, customerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly);
                scope.Complete();
            }

            return dtReturn;
        }

        public static DataTable ImportEmailsWithDupes(KMPlatform.Entity.User user, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string compositeKey, bool overwriteWithNULL, string source)
        {
            DataTable dtReturn = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.ImportEmails))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!KMPlatform.BusinessLogic.Client.HasServiceFeature(user.CurrentClient.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AddDupeSubscribers))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmailsWithDupes(user.UserID, user.CustomerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, compositeKey, overwriteWithNULL, source);
                scope.Complete();
            }

            return dtReturn;
        }

        public static DataTable ImportEmailsWithDupes_NoAccessCheck(KMPlatform.Entity.User user, int groupID, string xmlProfile, string xmlUDF, string formatTypeCode, string subscribeTypeCode, bool emailAddressOnly, string compositeKey, bool overwriteWithNULL, string source)
        {
            DataTable dtReturn = null;

            using (TransactionScope scope = new TransactionScope())
            {
                dtReturn = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmailsWithDupes(user.UserID, user.CustomerID, groupID, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, compositeKey, overwriteWithNULL, source);
                scope.Complete();
            }

            return dtReturn;
        }

        public static DataTable GetColumnNames()
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.GetColumnNames();
                scope.Complete();
            }
            return dt;
        }

        public static DataTable ImportEmailsToCS(KMPlatform.Entity.User user, int baseChannelID, string xml)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmailsToCS(user.UserID, baseChannelID, xml);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable ImportEmailsToGlobalMS(KMPlatform.Entity.User user, int baseChannelID, string xml)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmailsToGlobalMS(user.UserID, baseChannelID, xml);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable ImportEmailsToNoThreshold(KMPlatform.Entity.User user, int baseChannelID, string xml)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmailsToNoThreshold(user.UserID, baseChannelID, xml);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetBestProfileForEmailAddress(int groupID, int customerID, string emailAddress)
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailGroup.GetBestProfileForEmailAddress(groupID, customerID, emailAddress);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataTable GetGroupEmailProfilesWithUDF(int groupID, int customerID, string filter, string subscribeType, string profFilter = "")
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(groupID, customerID, filter, subscribeType, profFilter);
                scope.Complete();
            }
            return dtProfiles;
        }
        public static DataTable GetGroupEmailProfilesWithUDF(int groupID, int customerID, string subscribeType, DateTime fromDate, DateTime toDate, bool recent,string filter, string profFilter = "")
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(groupID, customerID, subscribeType, fromDate,toDate,recent,filter, profFilter);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataTable PreviewFilteredEmails(int groupID, int customerID, string filter)
        {
            DataTable dtPreview= null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtPreview = ECN_Framework_DataLayer.Communicator.EmailGroup.PreviewFilteredEmails(groupID, customerID, filter);
                scope.Complete();
            }
            return dtPreview;
        }

        public static DataTable PreviewFilteredEmails_Paging(int groupID, int customerID, string filter, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            DataTable dtPreview = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtPreview = ECN_Framework_DataLayer.Communicator.EmailGroup.PreviewFilteredEmails_Paging(groupID, customerID, filter, sortColumn, sortDirection, pageNumber, pageSize);
                scope.Complete();
            }
            return dtPreview;
        }


        public static DataTable GetByUserID(int customerID, int userID)
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByUserID(customerID, userID);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataTable GetByBounceScore(int customerID, int? groupID, int bounceScore, string bounceCondition)
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailGroup.GetByBounceScore(customerID, groupID, bounceScore, bounceCondition);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataSet GetBySearchStringPaging(int customerID, int groupID, int pageNo, int pageSize, DateTime fromDate, DateTime toDate, bool recent, string filter, string sortColumn = "EmailID", string sortDirection = "ASC")
        {
            DataSet dsProfilesList = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsProfilesList = ECN_Framework_DataLayer.Communicator.EmailGroup.GetBySearchStringPaging(customerID, groupID, pageNo, pageSize, fromDate, toDate, recent, filter, sortColumn, sortDirection);
                scope.Complete();
            }

            return dsProfilesList;
        }

        public static DataSet GetBySearchStringPaging(int customerID, int groupID, int pageNo, int pageSize, string searchString)
        {
            DataSet dsProfilesList = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsProfilesList = ECN_Framework_DataLayer.Communicator.EmailGroup.GetBySearchStringPaging(customerID, groupID, pageNo, pageSize, searchString);
                scope.Complete();
            }

            return dsProfilesList;
        }


        public static void DeleteFromMasterSuppressionGroup(int groupID, int emailID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (KM.Platform.User.IsSystemAdministrator(user))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.EmailGroup.DeleteFromMasterSuppressionGroup(user.CustomerID, emailID);
                    scope.Complete();
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Access Denied. Only System Administrators can delete an email address from Master Suppression Group"));
                throw new ECNException(errorList);
            }
        }

        public static void DeleteFromMasterSuppressionGroup_NoAccessCheck(int groupID, int emailID,int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (KM.Platform.User.IsSystemAdministrator(user))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.EmailGroup.DeleteFromMasterSuppressionGroup(customerID, emailID);
                    scope.Complete();
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Access Denied. Only System Administrators can delete an email address from Master Suppression Group"));
                throw new ECNException(errorList);
            }
        }

        public static void UnsubscribeSubscribers(int groupID, string emailAddresses, KMPlatform.Entity.User user)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetByGroupID(groupID, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.UnsubscribeSubscribers(groupID, emailAddresses);
                scope.Complete();
            }
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static void UnsubscribeSubscribers_NoAccessCheck(int groupID, string emailAddresses)
        {

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.UnsubscribeSubscribers(groupID, emailAddresses);
                scope.Complete();
            }
        }

        public static void AddToMasterSuppression(int customerID, int emailID, KMPlatform.Entity.User user)
        {

            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.AddToMasterSuppression(customerID, emailID);
                scope.Complete();
            }
        }

        public static int UnsubscribeSubscribersInFolder(int folderID, string emailIDs, KMPlatform.Entity.User user)
        {
            int success = 0;
            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                success = ECN_Framework_DataLayer.Communicator.EmailGroup.UnsubscribeSubscribersInFolder(folderID, emailIDs);
                scope.Complete();
            }
            return success;
        }

        public static void UnsubscribeBounces(int blastID, string isp, KMPlatform.Entity.User user)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.UnsubscribeBounces(blastID, isp);
                scope.Complete();
            }
        }

        public static int GetEmailIDFromComposite(int groupID, int customerID, string emailAddress, string compositeKey, string compositeKeyValue, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            int emailID = -1;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailID = ECN_Framework_DataLayer.Communicator.EmailGroup.GetEmailIDFromComposite(groupID, customerID, emailAddress, compositeKey, compositeKeyValue);
                scope.Complete();
            }
            return emailID;
        }

        public static int GetEmailIDFromWhatEmail(int groupID, int customerID, string emailAddress, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            int emailID = -1;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailID = ECN_Framework_DataLayer.Communicator.EmailGroup.GetEmailIDFromWhatEmail(groupID, customerID, emailAddress);
                scope.Complete();
            }
            return emailID;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static int GetEmailIDFromWhatEmail_NoAccessCheck(int groupID, int customerID, string emailAddress)
        {
            int emailID = -1;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailID = ECN_Framework_DataLayer.Communicator.EmailGroup.GetEmailIDFromWhatEmail(groupID, customerID, emailAddress);
                scope.Complete();
            }
            return emailID;
        }

        public static int GetSubscriberCount(int groupID, int customerID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, EmailsServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            int emailCount = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailCount = ECN_Framework_DataLayer.Communicator.EmailGroup.GetSubscriberCount(groupID, customerID);
                scope.Complete();
            }
            return emailCount;
        }


        public static int GetSubscriberCount_NoAccessCheck(int groupID, int customerID)
        {

            int emailCount = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailCount = ECN_Framework_DataLayer.Communicator.EmailGroup.GetSubscriberCount(groupID, customerID);
                scope.Complete();
            }
            return emailCount;
        }

        public static DataTable GetSubscriberStatus(string emailAddress, KMPlatform.Entity.User user)
        {
            DataTable dtProfiles = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtProfiles = ECN_Framework_DataLayer.Communicator.EmailGroup.GetSubscriberStatus(user.CustomerID, emailAddress);
                scope.Complete();
            }
            return dtProfiles;
        }

        public static DataTable GetUnsubscribesForDay(int daysBack, int folderID, int customerID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.GetUnsubscribesForDay(daysBack, folderID, customerID);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetUnsubscribesForCurrentBackToDay(int daysBack, int customerID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.GetUnsubscribesForCurrentBackToDay(daysBack, customerID);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetEmailsFromOtherGroupsToUnsubscribe(string emailIDs, int folderID, int customerID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.EmailGroup.GetEmailsFromOtherGroupsToUnsubscribe(emailIDs, folderID, customerID);
                scope.Complete();
            }
            return dt;
        }

        public static void CopyProfileFromGroup(int sourceGroupID, int destinationGroupID, int emailID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailGroup.CopyProfileFromGroup(sourceGroupID, destinationGroupID, emailID);
                scope.Complete();
            }
        }

        public static void ImportEmailToGroups(string xml, int userId)
        {
            var emailGroupsImport = new EmailGroupsImport();
            emailGroupsImport.Import(xml, userId);
        }

        public static void ImportEmailToGroups_NoAccessCheck(string xml,int userId)
        {
            var emailGroupsImport = new EmailGroupsImportNoAccess();
            emailGroupsImport.Import(xml, userId);
        }

        public static bool EmailExistsInCustomerSeedList(int emailID, int customerID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.EmailGroup.EmailExistsInCustomerSeedList(emailID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static DataTable GetGroupStats(int groupID, int customerID)
        {
            DataTable retDT = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retDT = ECN_Framework_DataLayer.Communicator.EmailGroup.GetGroupStats(groupID, customerID);
                scope.Complete();
            }
            return retDT;
        }

        public static int FDSubscriberLogin(int groupID, string emailAddress, int UDFID, string UDFValue, string Password, string User1, string User2, string User3, string User4, string User5, string User6)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.EmailGroup.FDSubscriberLogin(groupID, emailAddress, UDFID, UDFValue, Password, User1, User2, User3, User4, User5, User6);
                scope.Complete();
            }
            return count;
        }
    }
}
