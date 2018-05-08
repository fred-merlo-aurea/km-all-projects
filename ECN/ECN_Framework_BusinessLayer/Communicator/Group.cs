using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Group
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Groups;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Group;

        public static bool Exists(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.Exists(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }
        public static bool ExistsByGroupNameByCustomerID(string groupname, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.ExistsByGroupNameByCustomerID(groupname, customerID);
                scope.Complete();
            }
            return exists;
        }
        public static bool IsArchived(int groupID, int customerID)
        {
            bool IsArchived = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                IsArchived = ECN_Framework_DataLayer.Communicator.Group.IsArchived(groupID, customerID);
                scope.Complete();
            }
            return IsArchived;
        }

        public static bool Exists_UseAmbientTransaction(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.Exists(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.Group GetMasterSuppressionGroup(int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Group group = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                group = ECN_Framework_DataLayer.Communicator.Group.GetMasterSuppressionGroup(customerID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(group, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return group;
        }

        public static ECN_Framework_Entities.Communicator.Group GetSeedListGroupByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Group groups = new ECN_Framework_Entities.Communicator.Group();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groups = ECN_Framework_DataLayer.Communicator.Group.GetSeedListByCustomerID(customerID);
                scope.Complete();
            }
            
            return groups;
        }

        public static ECN_Framework_Entities.Communicator.Group GetMasterSuppressionGroup_NoAccessCheck(int customerID)
        {
            ECN_Framework_Entities.Communicator.Group group = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                group = ECN_Framework_DataLayer.Communicator.Group.GetMasterSuppressionGroup(customerID);
                scope.Complete();
            }

            return group;
        }

        public static bool Exists(int groupID, string groupName, int folderID, int customerID)
        {

            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.Exists(groupID, groupName, folderID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool CheckForExistingSeedlist(int? groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.CheckForExistingSeedlist(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool SuppressionGroupsExists(string groupIDs, int customerID)
        {
            return ECN_Framework_DataLayer.Communicator.Group.SuppressionGroupsExists(groupIDs, customerID);
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Group GetByGroupID_NoAccessCheck(int groupID)
        {
            ECN_Framework_Entities.Communicator.Group group = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                group = ECN_Framework_DataLayer.Communicator.Group.GetByGroupID(groupID);
                scope.Complete();
            }

            return group;
        }

        public static ECN_Framework_Entities.Communicator.Group GetByGroupID_NoAccessCheck_UseAmbientTransaction(int groupID)
        {
            ECN_Framework_Entities.Communicator.Group group = null;
            using (TransactionScope scope = new TransactionScope())
            {
                group = ECN_Framework_DataLayer.Communicator.Group.GetByGroupID(groupID);
                scope.Complete();
            }

            return group;
        }

        public static ECN_Framework_Entities.Communicator.Group GetByGroupID(int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Group group = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                group = ECN_Framework_DataLayer.Communicator.Group.GetByGroupID(groupID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(group, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return group;
        }
        
        public static List<ECN_Framework_Entities.Communicator.Group> GetByCustomerID(int customerID, KMPlatform.Entity.User user, string archiveFilter = "active")
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByCustomerID(customerID, archiveFilter);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(groupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return groupList;
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByCustomerID_NoAccessCheck(int customerID,  string archiveFilter = "active")
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByCustomerID(customerID, archiveFilter);
                scope.Complete();
            }

            return groupList;
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByGroupSearch(string name, int? folderID, KMPlatform.Entity.User user,bool? archived = null)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByGroupSearch(name, folderID, user.CustomerID, user.UserID,archived);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(groupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return groupList;
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByGroupSearch(string name, int? folderID, int customerID,KMPlatform.Entity.User user, bool? archived = null)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByGroupSearch(name, folderID, customerID, user.UserID, archived);
                scope.Complete();
            }
            
            return groupList;
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByFolderIDCustomerID(int folderID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByFolderIDCustomerID(folderID, user.CustomerID, user.UserID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(groupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return groupList;
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByFolderIDCustomerID_NoAccessCheck(int folderID, int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByFolderIDCustomerID(folderID,customerID, user.UserID);
                scope.Complete();
            }

            return groupList;
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByFolderID(int folderID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = new List<ECN_Framework_Entities.Communicator.Group>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                groupList = ECN_Framework_DataLayer.Communicator.Group.GetByFolderID(folderID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(groupList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return groupList;
        }
        
        public static bool FolderUsed(int folderID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.FolderUsed(folderID);
                scope.Complete();
            }
            return exists;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Group group)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            if (group.CustomerID <= 0)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (group.FolderID == null || (group.FolderID > 0 && !Folder.Exists(group.FolderID.Value, group.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP)))
                {
                    errorList.Add(new ECNError(Entity, Method, "FolderID is invalid"));
                }
                else
                {
                    if (group.GroupName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "GroupName cannot be empty"));
                    else if (Exists(group.GroupID, group.GroupName, group.FolderID.Value, group.CustomerID))
                        errorList.Add(new ECNError(Entity, Method, "GroupName already exists in this folder"));
                    else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(group.GroupName))
                        errorList.Add(new ECNError(Entity, Method, "GroupName has invalid characters"));
                    else if (group.MasterSupression == 0 && group.GroupName.ToLower().Equals("Master Suppression"))
                    {
                        errorList.Add(new ECNError(Entity, Method, "Cannot name a group Master Suppression"));
                    }
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(group.CustomerID))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (group.GroupName != "Master Suppression")
                    {
                        if (group.CreatedUserID == null || (group.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(group.CreatedUserID.Value, false))))
                        {
                            if (group.GroupID < 0 && (group.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(group.CreatedUserID.Value, group.CustomerID)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                        }
                        if (group.GroupID > 0 && (group.UpdatedUserID == null || (group.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(group.UpdatedUserID.Value, false)))))
                        {
                            if (group.GroupID > 0 && (group.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(group.UpdatedUserID.Value, group.CustomerID)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    }

                    scope.Complete();
                }
            }

            if (group.OwnerTypeCode.ToLower() != "customer")
                errorList.Add(new ECNError(Entity, Method, "OwnerTypeCode is invalid"));

            if (group.MasterSupression != null && group.MasterSupression.Value != 1 && group.MasterSupression.Value != 0)
                errorList.Add(new ECNError(Entity, Method, "MasterSuppression is invalid"));

            if (group.PublicFolder == null || (group.PublicFolder.Value != 1 && group.PublicFolder.Value != 0))
                errorList.Add(new ECNError(Entity, Method, "PublicFolder is invalid"));

            if (group.AllowUDFHistory != "Y" && group.AllowUDFHistory != "N")
                errorList.Add(new ECNError(Entity, Method, "AllowUDFHistory is invalid"));

            if (group.IsSeedList == null || (group.IsSeedList.Value && CheckForExistingSeedlist(group.GroupID, group.CustomerID)))
                errorList.Add(new ECNError(Entity, Method, "IsSeedList is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Archive(int groupID,bool Archive,int customerID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if(Archive)
                Archive_Validate(groupID, customerID);

            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Group.Archive(groupID,Archive , user.UserID);
                scope.Complete();
            }

        }
        
        public static void Archive_Validate(int groupID, int customerID)
        {
            
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            //if(ECN_Framework_BusinessLayer.FormDesigner.Form.ActiveByGroup(groupID,customerID))
            //{
            //    errorList.Add(new ECNError(Entity, Method, "Group is used in Forms"));
            //}
            if(errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.Group group, KMPlatform.Entity.User user)
        {
            if (group.CreatedUserID == null && group.UpdatedUserID != null)
                group.CreatedUserID = group.UpdatedUserID;

            List<ECNError> errorList = new List<ECNError>();
            Validate(group);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(group, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                bool newGroup = true;
                if (group.GroupID > 0)
                {
                    newGroup = false;
                }
                group.GroupID = ECN_Framework_DataLayer.Communicator.Group.Save(group);
                int groupID = group.GroupID;
                List<ECN_Framework_Entities.Communicator.GroupConfig> groupConfigList = ECN_Framework_BusinessLayer.Communicator.GroupConfig.GetByCustomerID(user.CustomerID, user);
                if (newGroup && (!group.MasterSupression.HasValue || !group.MasterSupression.Equals(1)))
                {
                    foreach (ECN_Framework_Entities.Communicator.GroupConfig grpConfig in groupConfigList)
                    {
                        ECN_Framework_Entities.Communicator.GroupDataFields gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                        gdf.CustomerID = group.CustomerID;
                        gdf.ShortName = grpConfig.ShortName;
                        gdf.LongName = grpConfig.ShortName;
                        gdf.IsPublic = grpConfig.IsPublic;
                        gdf.GroupID = groupID;
                        gdf.CreatedUserID = user.UserID;
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, user);
                    }
                }
                if (ECN_Framework_BusinessLayer.Communicator.UserGroup.Exists(user.UserID))
                {
                    ECN_Framework_Entities.Communicator.UserGroup userGroup = new ECN_Framework_Entities.Communicator.UserGroup();
                    userGroup.CreatedUserID = group.CreatedUserID;
                    userGroup.UserID = user.UserID;
                    userGroup.GroupID = groupID;
                    ECN_Framework_BusinessLayer.Communicator.UserGroup.Save(userGroup, user);
                }
                scope.Complete();
            }
            return group.GroupID;
        }

        public static bool GroupUsedInDomainTracking(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInDomainTracking(groupID);
                scope.Complete();
            }
            return exists;
        }

        public static bool GroupUsedInJointForms(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInJointForms(groupID);
                scope.Complete();
            }
            return exists;
        }

        public static bool GroupUsedInSurveys(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInSurveys(groupID);
                scope.Complete();
            }
            return exists;
        }

        public static bool GroupUsedInDigitalEditions(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInDigitalEditions(groupID);
                scope.Complete();
            }
            return exists;
        }

        public static bool GroupUsedInSubMgmt(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInSubMgmt(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool GroupUsedInPubNewsletters(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInPubNewsletters(groupID);
                scope.Complete();
            }
            return exists;
        }

        public static bool GroupUsedInMasterSuppression(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInMasterSuppression(groupID);
                scope.Complete();
            }
            return exists;
        }

        public static void Delete(int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(groupID, user.CustomerID))
            {
                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInDomainTracking(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in DomainTracking(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInJointForms(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in JointForm(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInMasterSuppression(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in MasterSuppression. Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInPubNewsletters(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in PubNewsletter(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInSurveys(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in Survey(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInDigitalEditions(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in DigitalEdition(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInSubMgmt(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in Subscription Management. Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.Communicator.Group.GroupUsedInCampaignItemTemplate(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in Campaign Item Template(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (ECN_Framework_BusinessLayer.FormDesigner.Form.ActiveByGroup(groupID, user.CustomerID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in Form(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                List<ECN_Framework_Entities.Communicator.MarketingAutomation> checkList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
                checkList = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(groupID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Group);
                if (checkList.Count > 0)
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in Marketing Automation(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                if (!ECN_Framework_BusinessLayer.Communicator.Blast.ActivePendingOrSentByGroup(groupID, user.CustomerID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByGroupID(groupID, user);

                    if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                        throw new ECN_Framework_Common.Objects.SecurityException();

                    List<ECN_Framework_Entities.Communicator.Filter> filterList = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(groupID, false, user);
                    using (TransactionScope scope = new TransactionScope())
                    {                        
                        foreach (ECN_Framework_Entities.Communicator.Filter filter in filterList)
                        {
                            ECN_Framework_BusinessLayer.Communicator.Filter.Delete(filter.FilterID, user);
                        }
                        ECN_Framework_BusinessLayer.Communicator.EmailGroup.Delete(groupID, user);
                        ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Delete(groupID, user);
                        ECN_Framework_DataLayer.Communicator.Group.Delete(groupID, user.CustomerID, user.UserID);                        
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Group is used in blast(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                } 
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Group does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static DataTable GetByGroupName(string groupName, string searchCriteria, KMPlatform.Entity.User user, int folderID = 0, int currentPage = 1, int pageSize = 15, bool allFolders = true, string archiveFilter = "all", string sortColumn = "GroupName", string sortDirection = "ASC")
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
            DataTable dtGroup = null;

            //groupName = ECN_Framework_Common.Functions.StringFunctions.CleanString(groupName);
            //searchCriteria = ECN_Framework_Common.Functions.StringFunctions.CleanString(searchCriteria);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetByGroupName(customer.BaseChannelID.Value, groupName.Replace("'", "\'"), searchCriteria, folderID, allFolders, user.CustomerID, user.UserID,sortColumn, sortDirection, currentPage,pageSize,archiveFilter);
                scope.Complete();
            }

            return dtGroup;
        }

        

        public static DataTable GetByGroupName_NoAccessCheck(string groupName, string searchCriteria, int CustomerID, int userID, int folderID = 0, int currentPage = 1, int pageSize = 15, bool allFolders = true, string archiveFilter = "all", int? SubscriberLimit  = null, string sortColumn = "GroupName", string sortDirection = "ASC")
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
            DataTable dtGroup = null;

            //groupName = ECN_Framework_Common.Functions.StringFunctions.CleanString(groupName);
            //searchCriteria = ECN_Framework_Common.Functions.StringFunctions.CleanString(searchCriteria);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetByGroupName(customer.BaseChannelID.Value, groupName.Replace("'", "\'"), searchCriteria, folderID, allFolders, CustomerID, userID,sortColumn, sortDirection, currentPage, pageSize, archiveFilter, SubscriberLimit);
                scope.Complete();
            }

            return dtGroup;
        }


        public static DataTable GetByProfileName(string profileName, string searchCriteria, KMPlatform.Entity.User user, int folderID = 0, int currentPage = 1, int pageSize = 15, bool allFolders = true, string archiveFilter = "all", string sortColumn = "GroupName", string sortDirection = "ASC")
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
            DataTable dtGroup = null;

            //profileName = ECN_Framework_Common.Functions.StringFunctions.CleanString(profileName);
            //searchCriteria = ECN_Framework_Common.Functions.StringFunctions.CleanString(searchCriteria);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetByProfileName(customer.BaseChannelID.Value, profileName, searchCriteria, folderID,currentPage, pageSize, allFolders, user.CustomerID, user.UserID, sortColumn, sortDirection, archiveFilter);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetByProfileName_NoAccessCheck(string profileName, string searchCriteria, int CustomerID, int UserID, int folderID = 0, int currentPage = 1, int pageSize = 15, bool allFolders = true, string archiveFilter = "all", int? SubscriberLimit = null, string sortColumn = "GroupName", string sortDirection = "ASC")
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
            DataTable dtGroup = null;


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetByProfileName(customer.BaseChannelID.Value, profileName.Replace("'", "''"), searchCriteria, folderID, currentPage, pageSize, allFolders, CustomerID, UserID, sortColumn, sortDirection, archiveFilter, SubscriberLimit);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetSubscribers(int folderID, KMPlatform.Entity.User user, int pageIndex, int pageSize, bool allFolders = false, string archiveFilter = "all")
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
            DataTable dtGroup = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetSubscribers(customer.BaseChannelID.Value, user.CustomerID, user.UserID, folderID,pageIndex,pageSize, allFolders,archiveFilter);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetSubscribers_NoAccessCheck(int folderID, int CustomerID,int UserID, int pageIndex, int pageSize, bool allFolders = false, string archiveFilter = "all", int? SubscriberLimit = null)
        {
            DataTable dtGroup = null;
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetSubscribers(customer.BaseChannelID.Value, CustomerID, UserID, folderID, pageIndex, pageSize, allFolders, archiveFilter, SubscriberLimit);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetTransactional(int customerID,string searchField, string searchWhere, string searchCriteria, int pageIndex, int pageSize, int FolderID, bool allFolders, string archiveFilter, KMPlatform.Entity.User user)
        {
            DataTable dtGroup = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetTransactional(customerID,searchField,searchWhere, searchCriteria, pageIndex, pageSize,FolderID, allFolders, archiveFilter);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetTransactional_NoAccessCheck(int customerID, string searchField,string searchWhere, string searchCriteria, int pageIndex, int pageSize, int FolderID, bool allFolders, string archiveFilter)
        {
            DataTable dtGroup = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetTransactional(customerID, searchField, searchWhere,searchCriteria, pageIndex, pageSize, FolderID, allFolders, archiveFilter);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetNONTransactional(int customerID, KMPlatform.Entity.User user)
        {
            DataTable dtGroup = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetNONTransactional(customerID);
                scope.Complete();
            }

            return dtGroup;
        }

        public static DataTable GetGroupDR(int customerID, int userID, KMPlatform.Entity.User user)
        {
            DataTable dtGroup = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtGroup = ECN_Framework_DataLayer.Communicator.Group.GetGroupDR(customerID, userID);
                scope.Complete();
            }

            return dtGroup;
        }

        public static void ValidateDynamicTags(int groupId, int layoutId, KMPlatform.Entity.User user)
        {
            var groupValidateDynamicTags = new GroupValidateDynamicTags();
            groupValidateDynamicTags.ValidateDynamicTags(groupId, layoutId, user);
        }

        public static void ValidateDynamicTags_UseAmbientTransaction(int groupId, int layoutId, KMPlatform.Entity.User user)
        {
            var groupValidateDynamicTags = new GroupValidateDynamicTagsAmbientTransaction();
            groupValidateDynamicTags.ValidateDynamicTags(groupId, layoutId, user);
        }

        public static void ValidateDynamicStrings(System.Collections.Generic.List<string> listLY, int groupID, KMPlatform.Entity.User user)
        {
            var groupValidateDynamicStrings = new GroupValidateDynamicStrings();
            groupValidateDynamicStrings.ValidateDynamicStrings(listLY, groupID, user);
        }

        public static void ValidateDynamicStringsForTemplate(System.Collections.Generic.List<string> listLY, int groupID, KMPlatform.Entity.User user)
        {
            var groupValidateDynamicStringsForTemplate = new GroupValidateDynamicStringsTemplate();
            groupValidateDynamicStringsForTemplate.ValidateDynamicStrings(listLY, groupID, user);
        }


        public static DataTable GetMSByDateRangeForCustomers(string startDate, string endDate, string customerIDs)
        {
            return ECN_Framework_DataLayer.Communicator.Group.GetMSByDateRangeForCustomers(startDate, endDate, customerIDs);
        }

        private static bool GroupUsedInCampaignItemTemplate(int groupID, int customerID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Group.GroupUsedInCampaignItemTemplate(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }
    }
}
