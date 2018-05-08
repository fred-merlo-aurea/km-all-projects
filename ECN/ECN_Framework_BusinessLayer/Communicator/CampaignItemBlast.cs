using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;
using EntitesCampaignItem = ECN_Framework_Entities.Communicator.CampaignItem;
using EntitiesCampaignItemBlast = ECN_Framework_Entities.Communicator.CampaignItemBlast;
using PlatformUser = KMPlatform.Entity.User;
using PlatformEnums = KMPlatform.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemBlast
    {
        private const string AB = "ab";
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemBlast;

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            else if(AccessCode == KMPlatform.Enums.Access.Delete)
            {
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                    return true;
            }
            return false;
        }

        public static bool Exists(int campaignItemID, int campaignItemBlastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Exists(campaignItemID, campaignItemBlastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static void Save(int campaignItemId, List<EntitiesCampaignItemBlast> campaignItemBlasts, PlatformUser user)
        {
            SaveGeneric(false, campaignItemId, campaignItemBlasts, user);
        }

        public static void Save_UseAmbientTransaction(int campaignItemId, List<EntitiesCampaignItemBlast> campaignItemBlasts, PlatformUser user)
        {
            SaveGeneric(true, campaignItemId, campaignItemBlasts, user);
        }

        private static void SaveGeneric(bool useAmbient, int campaignItemId, List<EntitiesCampaignItemBlast> campaignItemBlasts, PlatformUser user)
        {
            if (!HasPermission(PlatformEnums.Access.Edit, user))
            {
                throw new SecurityException();
            }

            EntitesCampaignItem campaignItem = null;
            var dbList = new List<EntitiesCampaignItemBlast>();

            if (useAmbient)
            {
                campaignItem = CampaignItem.GetByCampaignItemID_UseAmbientTransaction(campaignItemId, user, false);
                dbList = GetByCampaignItemID_UseAmbientTransaction(campaignItemId, user, true);
            }
            else
            {
                campaignItem = CampaignItem.GetByCampaignItemID(campaignItemId, user, false);
                dbList = GetByCampaignItemID(campaignItemId, user, true);
            }
            
            var saveList = new List<EntitiesCampaignItemBlast>();
            var deleteList = new List<EntitiesCampaignItemBlast>();

            int previousCIBIDforAB = -1;
            foreach (var campaignItemBlast in campaignItemBlasts)
            {
                var found = false;
                foreach (var item in dbList)
                {
                    if (campaignItem.CampaignItemType.Equals(AB, StringComparison.OrdinalIgnoreCase))
                    {
                        var addToSave = false;
                        if (useAmbient)
                        {
                            if (campaignItemBlast.GroupID == item.GroupID && 
                                campaignItemBlast.LayoutID == item.LayoutID && 
                                campaignItemBlast.EmailSubject == item.EmailSubject)
                            {
                                addToSave = true;
                            }
                        }
                        else
                        {
                            if (campaignItemBlast.GroupID == item.GroupID && campaignItemBlast.LayoutID == item.LayoutID && 
                                campaignItemBlast.EmailSubject == item.EmailSubject && previousCIBIDforAB != item.CampaignItemBlastID)
                            {
                                previousCIBIDforAB = item.CampaignItemBlastID;
                                addToSave = true;
                            }
                        }

                        if (addToSave)
                        {
                            campaignItemBlast.CampaignItemBlastID = item.CampaignItemBlastID;
                            campaignItemBlast.BlastID = item.BlastID;
                            saveList.Add(campaignItemBlast);
                            found = true;
                            break;
                        }
                    }
                    else
                    {
                        if (campaignItemBlast.GroupID == item.GroupID)
                        {
                            campaignItemBlast.CampaignItemBlastID = item.CampaignItemBlastID;
                            campaignItemBlast.BlastID = item.BlastID;
                            saveList.Add(campaignItemBlast);
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    saveList.Add(campaignItemBlast);
                }
            }

            foreach (var dbItem in dbList)
            {
                var found = false;
                foreach (var newItem in campaignItemBlasts)
                {
                    if (newItem.GroupID == dbItem.GroupID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    deleteList.Add(dbItem);
                }
            }

            using (var scope = new TransactionScope())
            {
                foreach (var deleteItem in deleteList)
                {
                    if (!AccessCheck.CanAccessByCustomer(deleteItem, ServiceCode, ServiceFeatureCode, PlatformEnums.Access.Edit, user))
                    {
                        throw new SecurityException();
                    }

                    CampaignItemBlastRefBlast.Delete(deleteItem.CampaignItemBlastID, user, true);
                    Delete(deleteItem.CampaignItemID.Value, deleteItem.CampaignItemBlastID, user);
                    CampaignItemBlastFilter.DeleteByCampaignItemBlastID(deleteItem.CampaignItemBlastID);
                }

                foreach (var saveItem in saveList)
                {
                    if (useAmbient)
                    {
                        Validate_UseAmbientTransaction(saveItem, user);
                    }
                    else
                    {
                        Validate(saveItem, user);
                    }

                    if (!AccessCheck.CanAccessByCustomer(saveItem, ServiceCode, ServiceFeatureCode, PlatformEnums.Access.Edit, user))
                    {
                        throw new SecurityException();
                    }

                    saveItem.CampaignItemBlastID = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Save(saveItem);
                    CampaignItemBlastFilter.DeleteByCampaignItemBlastID(saveItem.CampaignItemBlastID);

                    if (saveItem.Filters != null)
                    {
                        foreach (var campaignItemBlastFilter in saveItem.Filters)
                        {
                            campaignItemBlastFilter.CampaignItemBlastFilterID = -1;
                            campaignItemBlastFilter.CampaignItemBlastID = saveItem.CampaignItemBlastID;
                            campaignItemBlastFilter.CampaignItemSuppresionID = null;
                            campaignItemBlastFilter.IsDeleted = false;
                            CampaignItemBlastFilter.Save(campaignItemBlastFilter);
                        }
                    }

                    if (saveItem.RefBlastList != null)
                    {
                        foreach (var campaignItemRefBlast in saveItem.RefBlastList)
                        {
                            campaignItemRefBlast.CampaignItemBlastID = saveItem.CampaignItemBlastID;
                            CampaignItemBlastRefBlast.Save(campaignItemRefBlast, user, true);
                        }
                    }
                }

                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast, KMPlatform.Entity.User user, bool ignoreArchivedGroup = false)
        {
            Validate(ciBlast, user, ignoreArchivedGroup);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //get list of CampaignItemBlast from database
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(ciBlast.CampaignItemID.Value, user, false);

            //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
            //bool updateBlastTable = false;
            //var blastExists = dbList.Where(x => x.BlastID != null);
            //if (blastExists.Any())
            //    updateBlastTable = true;

            using (TransactionScope scope = new TransactionScope())
            {
                ciBlast.CampaignItemBlastID = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Save(ciBlast);

                if (ciBlast.Filters != null && ciBlast.Filters.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlast.Filters)
                    {
                        //check for copying campaign item 
                        if (cibf.CampaignItemBlastID != ciBlast.CampaignItemBlastID)
                        {
                            //the id's don't match so it's a copy so we need to create new ones
                            cibf.CampaignItemBlastFilterID = -1;
                            cibf.CampaignItemBlastID = ciBlast.CampaignItemBlastID;
                            cibf.CampaignItemSuppresionID = null;
                            cibf.CampaignItemTestBlastID = null;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }

                    }
                }
                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(ciBlast.CampaignItemID.Value, user);

                scope.Complete();
            }

            return ciBlast.CampaignItemBlastID;
        }

        public static int Save_UseAmbientTransaction(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast, KMPlatform.Entity.User user)
        {
            Validate_UseAmbientTransaction(ciBlast, user);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //get list of CampaignItemBlast from database
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(ciBlast.CampaignItemID.Value, user, false);

            //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
            //bool updateBlastTable = false;
            //var blastExists = dbList.Where(x => x.BlastID != null);
            //if (blastExists.Any())
            //    updateBlastTable = true;

            using (TransactionScope scope = new TransactionScope())
            {
                ciBlast.CampaignItemBlastID = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Save(ciBlast);

                if (ciBlast.Filters != null && ciBlast.Filters.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlast.Filters)
                    {
                        //check for copying campaign item 
                        if (cibf.CampaignItemBlastID != ciBlast.CampaignItemBlastID)
                        {
                            //the id's don't match so it's a copy so we need to create new ones
                            cibf.CampaignItemBlastFilterID = -1;
                            cibf.CampaignItemBlastID = ciBlast.CampaignItemBlastID;
                            cibf.CampaignItemSuppresionID = null;
                            cibf.CampaignItemTestBlastID = null;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }

                    }
                }
                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(ciBlast.CampaignItemID.Value, user);

                scope.Complete();
            }

            return ciBlast.CampaignItemBlastID;
        }

        public static int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast, KMPlatform.Entity.User user)
        {
            Validate_NoAccessCheck(ciBlast, user);

            
            //get list of CampaignItemBlast from database
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(ciBlast.CampaignItemID.Value, false);

            //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
            //bool updateBlastTable = false;
            //var blastExists = dbList.Where(x => x.BlastID != null);
            //if (blastExists.Any())
            //    updateBlastTable = true;

            using (TransactionScope scope = new TransactionScope())
            {
                ciBlast.CampaignItemBlastID = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Save(ciBlast);

                if (ciBlast.Filters != null && ciBlast.Filters.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlast.Filters)
                    {
                        //check for copying campaign item 
                        if (cibf.CampaignItemBlastID != ciBlast.CampaignItemBlastID)
                        {
                            //the id's don't match so it's a copy so we need to create new ones
                            cibf.CampaignItemBlastFilterID = -1;
                            cibf.CampaignItemBlastID = ciBlast.CampaignItemBlastID;
                            cibf.CampaignItemSuppresionID = null;
                            cibf.CampaignItemTestBlastID = null;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }

                    }
                }
                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(ciBlast.CampaignItemID.Value, user);

                scope.Complete();
            }

            return ciBlast.CampaignItemBlastID;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast, KMPlatform.Entity.User user, bool ignoreArchivedGroup = false)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (ciBlast.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (ciBlast.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(ciBlast.CampaignItemID.Value, user, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, ciBlast.CampaignItemID.Value, ciBlast.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemBlast as there are Sent or Active Blasts"));
                        else
                        {
                            if (ciBlast.GroupID != null)
                            {
                                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(ciBlast.GroupID.Value, user);
                                if (group.Archived.HasValue && group.Archived.Value && !ignoreArchivedGroup)
                                    errorList.Add(new ECNError(Entity, Method, "Archived Groups not allowed. Group: " + group.GroupName));
                                if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(ciBlast.GroupID.Value, ciBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                                else
                                {
                                    System.Collections.Generic.List<string> listLY = null;
                                    if (ciBlast.LayoutID != null)
                                    {
                                        if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(ciBlast.LayoutID.Value, ciBlast.CustomerID.Value))
                                            errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                                        else if(ECN_Framework_BusinessLayer.Communicator.Layout.IsArchived(ciBlast.LayoutID.Value, ciBlast.CustomerID.Value))
                                        {
                                            errorList.Add(new ECNError(Entity, Method, "Layout is archived"));
                                        }
                                        else
                                        {
                                            listLY = Layout.ValidateLayoutContent(ciBlast.LayoutID.Value);
                                            Group.ValidateDynamicTags(ciBlast.GroupID.Value, ciBlast.LayoutID.Value, user);
                                            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ciBlast.LayoutID.Value, false);
                                            ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.Value);
                                            List<string> templateCheck = new List<string>();
                                            templateCheck.Add(template.TemplateSource);
                                            templateCheck.Add(template.TemplateText);
                                            Group.ValidateDynamicStringsForTemplate(templateCheck, ciBlast.GroupID.Value, user);
                                        }
                                    }
                                    if (listLY == null)
                                        listLY = new System.Collections.Generic.List<string>();
                                    if (ciBlast.DynamicFromName.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicFromName.Trim().ToLower());
                                    }
                                    if (ciBlast.DynamicFromEmail.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicFromEmail.Trim().ToLower());
                                    }
                                    if (ciBlast.DynamicReplyTo.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicReplyTo.Trim().ToLower());
                                    }
                                    if (ciBlast.EmailSubject.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.EmailSubject.Trim().ToLower());
                                    }
                                    Group.ValidateDynamicStrings(listLY, ciBlast.GroupID.Value, user);
                                }
                            }
                            if (!string.IsNullOrEmpty(ciBlast.EmailFrom) && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(ciBlast.EmailFrom)))
                                errorList.Add(new ECNError(Entity, Method, "From Email is invalid"));

                            if (!string.IsNullOrEmpty(ciBlast.ReplyTo) && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(ciBlast.ReplyTo)))
                                errorList.Add(new ECNError(Entity, Method, "Reply To is invalid"));

                            if (ciBlast.SocialMediaID != null && (!ECN_Framework_BusinessLayer.Communicator.SocialMedia.Exists(ciBlast.SocialMediaID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "SocialMediaID is invalid"));

                            if (ciBlast.BlastID != null && (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(ciBlast.BlastID.Value, ciBlast.CustomerID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));

                            if(ciBlast.Filters != null && ciBlast.Filters.Count(x => x.SmartSegmentID.HasValue) > 0)
                            {
                                foreach(ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlast.Filters.Where(x => x.SmartSegmentID.HasValue).ToList())
                                {
                                    string[] blastIDs = cibf.RefBlastIDs.Split(',');
                                    foreach (string s in blastIDs)
                                    {
                                        if (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(Convert.ToInt32(s), ciBlast.CustomerID.Value))
                                        {
                                            errorList.Add(new ECNError(Entity, Method, "Smart Segment BlastID is invalid"));
                                            break;
                                        }
                                    }
                                }
                            }

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(ciBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                                if (ciBlast.CampaignItemBlastID <= 0 && (ciBlast.CreatedUserID == null || (ciBlast.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(ciBlast.CreatedUserID.Value, false)))))
                                {
                                    if (ciBlast.CampaignItemBlastID <= 0 && (ciBlast.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(ciBlast.CreatedUserID.Value, ciBlast.CustomerID.Value)))
                                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                                }
                                if (ciBlast.CampaignItemBlastID > 0 && (ciBlast.UpdatedUserID == null || (ciBlast.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(ciBlast.UpdatedUserID.Value, false)))))
                                {
                                    if (ciBlast.CampaignItemBlastID > 0 && (ciBlast.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(ciBlast.UpdatedUserID.Value, ciBlast.CustomerID.Value)))
                                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                                }
                                scope.Complete();
                            }
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (ciBlast.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (ciBlast.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(ciBlast.CampaignItemID.Value, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, ciBlast.CampaignItemID.Value, ciBlast.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemBlast as there are Sent or Active Blasts"));
                        else
                        {
                            if (ciBlast.GroupID != null)
                            {
                                if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(ciBlast.GroupID.Value, ciBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                                else
                                {
                                    System.Collections.Generic.List<string> listLY = null;
                                    if (ciBlast.LayoutID != null)
                                    {
                                        if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(ciBlast.LayoutID.Value, ciBlast.CustomerID.Value))
                                            errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                                        else
                                        {
                                            listLY = Layout.ValidateLayoutContent(ciBlast.LayoutID.Value);
                                            Group.ValidateDynamicTags(ciBlast.GroupID.Value, ciBlast.LayoutID.Value, user);
                                        }
                                    }
                                    if (listLY == null)
                                        listLY = new System.Collections.Generic.List<string>();
                                    if (ciBlast.DynamicFromName.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicFromName.Trim().ToLower());
                                    }
                                    if (ciBlast.DynamicFromEmail.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicFromEmail.Trim().ToLower());
                                    }
                                    if (ciBlast.DynamicReplyTo.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicReplyTo.Trim().ToLower());
                                    }
                                    if (ciBlast.EmailSubject.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.EmailSubject.Trim().ToLower());
                                    }
                                    Group.ValidateDynamicStrings(listLY, ciBlast.GroupID.Value, user);
                                }
                            }
                            if (ciBlast.SocialMediaID != null && (!ECN_Framework_BusinessLayer.Communicator.SocialMedia.Exists(ciBlast.SocialMediaID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "SocialMediaID is invalid"));

                            if (ciBlast.BlastID != null && (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(ciBlast.BlastID.Value, ciBlast.CustomerID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(ciBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                                
                                scope.Complete();
                            }
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_UseAmbientTransaction(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (ciBlast.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (ciBlast.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_UseAmbientTransaction(ciBlast.CampaignItemID.Value, user, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists_UseAmbientTransaction(item.CampaignID.Value, ciBlast.CampaignItemID.Value, ciBlast.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent_UseAmbientTransaction(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemBlast as there are Sent or Active Blasts"));
                        else
                        {
                            if (ciBlast.GroupID != null)
                            {
                                if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists_UseAmbientTransaction(ciBlast.GroupID.Value, ciBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                                else
                                {
                                    System.Collections.Generic.List<string> listLY = null;
                                    if (ciBlast.LayoutID != null)
                                    {
                                        if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists_UseAmbientTransaction(ciBlast.LayoutID.Value, ciBlast.CustomerID.Value))
                                            errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                                        else
                                        {
                                            listLY = Layout.ValidateLayoutContent_UseAmbientTransaction(ciBlast.LayoutID.Value);
                                            Group.ValidateDynamicTags_UseAmbientTransaction(ciBlast.GroupID.Value, ciBlast.LayoutID.Value, user);
                                        }
                                    }
                                    if (listLY == null)
                                        listLY = new System.Collections.Generic.List<string>();
                                    if (ciBlast.DynamicFromName.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicFromName.Trim().ToLower());
                                    }
                                    if (ciBlast.DynamicFromEmail.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicFromEmail.Trim().ToLower());
                                    }
                                    if (ciBlast.DynamicReplyTo.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.DynamicReplyTo.Trim().ToLower());
                                    }
                                    if (ciBlast.EmailSubject.Trim().Length > 0)
                                    {
                                        listLY.Add(ciBlast.EmailSubject.Trim().ToLower());
                                    }
                                    Group.ValidateDynamicStrings(listLY, ciBlast.GroupID.Value, user);
                                }
                            }
                            if (ciBlast.SocialMediaID != null && (!ECN_Framework_BusinessLayer.Communicator.SocialMedia.Exists_UseAmbientTransaction(ciBlast.SocialMediaID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "SocialMediaID is invalid"));

                            if (ciBlast.BlastID != null && (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists_UseAmbientTransaction(ciBlast.BlastID.Value, ciBlast.CustomerID.Value)))
                                errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(ciBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                                if (ciBlast.CreatedUserID == null || (ciBlast.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(ciBlast.CreatedUserID.Value, false))))
                                {
                                    if (ciBlast.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(ciBlast.CreatedUserID.Value, ciBlast.CustomerID.Value))
                                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                                }
                                if (ciBlast.CampaignItemBlastID > 0 && (ciBlast.UpdatedUserID == null || (ciBlast.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(ciBlast.UpdatedUserID.Value, false)))))
                                {
                                    if (ciBlast.CampaignItemBlastID > 0 && (ciBlast.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(ciBlast.UpdatedUserID.Value, ciBlast.CustomerID.Value)))
                                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                                }
                                scope.Complete();
                            }
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemBlast GetByCampaignItemBlastID(int campaignItemBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlast = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlast != null && getChildren)
            {
                ciBlast.Blast = Blast.GetByBlastID(ciBlast.BlastID.Value, user, getChildren);
            }
            ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID(campaignItemBlastID, user);

            return ciBlast;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemBlast GetByBlastID(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlast = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlast != null && getChildren)
            {
                ciBlast.Blast = Blast.GetByBlastID(ciBlast.BlastID.Value, user, getChildren);
                ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID, user);
                ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID);
            }


            return ciBlast;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemBlast GetByBlastID_NoAccessCheck(int blastID, bool getChildren)
        {

            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlast = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (ciBlast != null && getChildren)
            {
                ciBlast.Blast = Blast.GetByBlastID_NoAccessCheck(ciBlast.BlastID.Value, getChildren);
                ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID_NoAccessCheck(ciBlast.CampaignItemBlastID);
                ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID);
            }


            return ciBlast;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlast> GetByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlastList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID(ciBlast.BlastID.Value, user, getChildren);
                    }
                    ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID, user);
                    ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID);
                }
            }

            return ciBlastList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlast> GetByCampaignItemID_UseAmbientTransaction(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
            using (TransactionScope scope = new TransactionScope())
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlastList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID_UseAmbientTransaction(ciBlast.BlastID.Value, user, getChildren);
                    }
                    ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID_UseAmbientTransaction(ciBlast.CampaignItemBlastID, user);
                    ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemBlastID_UseAmbientTransaction(ciBlast.CampaignItemBlastID);
                }
            }

            return ciBlastList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlast> GetByCampaignItemID_NoAccessCheck(int campaignItemID, bool getChildren)
        {

            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID_NoAccessCheck(ciBlast.BlastID.Value, getChildren);
                    }
                    ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID_NoAccessCheck(ciBlast.CampaignItemBlastID);
                    ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID);
                }
            }

            return ciBlastList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlast> GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(int campaignItemID, bool getChildren)
        {

            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
            using (TransactionScope scope = new TransactionScope())
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID_NoAccessCheck_UseAmbientTransaction(ciBlast.BlastID.Value, getChildren);
                    }
                    ciBlast.RefBlastList = CampaignItemBlastRefBlast.GetByCampaignItemBlastID_NoAccessCheck_UseAmbientTransaction(ciBlast.CampaignItemBlastID);
                    ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemBlastID_UseAmbientTransaction(ciBlast.CampaignItemBlastID);
                }
            }

            return ciBlastList;
        }

        public static void Delete(int campaignItemID, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {
            //ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID, user, false);

            if (ciBlastList.Count > 0)
            {
                //get list of CampaignItemBlast from database
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID, user, false);

                //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                //bool updateBlastTable = false;
                //if (!overrideUpdate)
                //{
                //    var blastExists = dbList.Where(x => x.BlastID != null);
                //    if (blastExists.Any())
                //        updateBlastTable = true;
                //}

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                    {
                        if (ciBlast.BlastID != null)
                        {
                            Blast.Delete(ciBlast.BlastID.Value, user);
                        }
                    }
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Delete(ciBlastList[0].CampaignItemBlastID, user, true);
                    ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Delete(campaignItemID, user.CustomerID, user.UserID);

                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(campaignItemID, user);

                    scope.Complete();
                }

            }
        }

        public static void Delete(int campaignItemID, int campaignItemBlastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(campaignItemBlastID, user, false);

            if (ciBlast != null)
            {
                //get list of CampaignItemBlast from database
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID, user, false);

                //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                //bool updateBlastTable = false;
                //var blastExists = dbList.Where(x => x.BlastID != null);
                //if (blastExists.Any())
                //    updateBlastTable = true;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (ciBlast.BlastID != null)
                    {
                        Blast.Delete(ciBlast.BlastID.Value, user);
                    }
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Delete(campaignItemBlastID, user, true);
                    ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Delete(campaignItemID, campaignItemBlastID, user.CustomerID, user.UserID);

                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(campaignItemID, user);

                    scope.Complete();
                }
            }
            else
            {
                List<ECNError> errorList = new List<ECNError>();
                errorList.Add(new ECNError(Entity, Method, "Item does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static void Delete_NoAccessCheck(int campaignItemID, int campaignItemBlastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(campaignItemBlastID, user, false);

            if (ciBlast != null)
            {
                //get list of CampaignItemBlast from database
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID, user, false);

                //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                //bool updateBlastTable = false;
                //var blastExists = dbList.Where(x => x.BlastID != null);
                //if (blastExists.Any())
                //    updateBlastTable = true;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (ciBlast.BlastID != null)
                    {
                        Blast.Delete(ciBlast.BlastID.Value, user);
                    }
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Delete(campaignItemBlastID, user, true);
                    ECN_Framework_DataLayer.Communicator.CampaignItemBlast.Delete(campaignItemID, campaignItemBlastID, user.CustomerID, user.UserID);

                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(campaignItemID, user);

                    scope.Complete();
                }
            }
            else
            {
                List<ECNError> errorList = new List<ECNError>();
                errorList.Add(new ECNError(Entity, Method, "Item does not exist"));
                throw new ECNException(errorList);
            }
        }

        internal static void UpdateBlastID(int campaignItemBlastID, int blastID, int userID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemBlast.UpdateBlastID(campaignItemBlastID, blastID, userID);
                scope.Complete();
            }
        }

    }
}
