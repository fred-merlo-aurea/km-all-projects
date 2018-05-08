using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using KMPlatform.Entity;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Campaign
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Campaign;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Campaign;

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
            if(AccessCode == KMPlatform.Enums.Access.Delete)
            {
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                    return true;
            }
            return false;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Campaign campaign)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (campaign.CustomerID == null || (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(campaign.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                }
                else
                {
                    if (campaign.CampaignName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "CampaignName cannot be empty"));
                    else
                        if (Exists(campaign.CampaignID,campaign.CampaignName, campaign.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CampaignName (" + campaign.CampaignName + ") already exists"));

                    if (campaign.CampaignID <= 0 && (campaign.CreatedUserID == null || (campaign.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(campaign.CreatedUserID.Value, false)))))
                    {
                        if (campaign.CampaignID <= 0 && (campaign.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(campaign.CreatedUserID.Value, campaign.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (campaign.CampaignID > 0 && (campaign.UpdatedUserID == null || (campaign.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(campaign.UpdatedUserID.Value, false)))))
                    {
                        if (campaign.CampaignID > 0 && (campaign.UpdatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(campaign.UpdatedUserID.Value, campaign.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(campaign.CampaignName))
                        errorList.Add(new ECNError(Entity, Method, "CampaignName has invalid characters"));
                }
                scope.Complete();
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_UseAmbientTransaction(ECN_Framework_Entities.Communicator.Campaign campaign)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (campaign.CustomerID == null || (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(campaign.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                }
                else
                {
                    if (campaign.CampaignName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "CampaignName cannot be empty"));
                    else
                        if (Exists(campaign.CampaignID, campaign.CampaignName, campaign.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignName (" + campaign.CampaignName + ") already exists"));

                    if (campaign.CampaignID <= 0 && (campaign.CreatedUserID == null || (campaign.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(campaign.CreatedUserID.Value, false)))))
                    {
                        if (campaign.CampaignID <= 0 && (campaign.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(campaign.CreatedUserID.Value, campaign.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (campaign.CampaignID > 0 && (campaign.UpdatedUserID == null || (campaign.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(campaign.UpdatedUserID.Value, false)))))
                    {
                        if (campaign.CampaignID > 0 && (campaign.UpdatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(campaign.UpdatedUserID.Value, campaign.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(campaign.CampaignName))
                        errorList.Add(new ECNError(Entity, Method, "CampaignName has invalid characters"));
                }
                scope.Complete();
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.Campaign campaign, KMPlatform.Entity.User user)
        {
            Validate(campaign);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                campaign.CampaignID = ECN_Framework_DataLayer.Communicator.Campaign.Save(campaign);
                scope.Complete();
            }

            return campaign.CampaignID;
        }

        public static int Save_UseAmbientTransaction(ECN_Framework_Entities.Communicator.Campaign campaign, KMPlatform.Entity.User user)
        {
            Validate_UseAmbientTransaction(campaign);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                campaign.CampaignID = ECN_Framework_DataLayer.Communicator.Campaign.Save(campaign);
                scope.Complete();
            }

            return campaign.CampaignID;
        }

        public static bool Exists(int campaignID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Campaign.Exists(campaignID, customerID);
                scope.Complete();
            }
            return exists; 
        }

        public static bool Exists_UseAmbientTransaction(int campaignID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.Campaign.Exists(campaignID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int campaignID,string campaignName, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Campaign.Exists(campaignID,campaignName, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignID(int campaignID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByCampaignID(campaignID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaign != null && getChildren)
            {
                campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
            }

            return campaign;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignID_NoAccessCheck(int campaignID,  bool getChildren)
        {
            
            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByCampaignID(campaignID);
                scope.Complete();
            }

           
            if (campaign != null && getChildren)
            {
                campaign.ItemList = CampaignItem.GetByCampaignID_NoAccessCheck(campaign.CampaignID, getChildren);
            }

            return campaign;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignName(string campaignName, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByCampaignName(campaignName, user.CustomerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaign != null && getChildren)
            {
                campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
            }

            return campaign;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignName(string campaignName, int customerID,KMPlatform.Entity.User user,  bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByCampaignName(campaignName, customerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaign != null && getChildren)
            {
                campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
            }

            return campaign;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaign != null && getChildren)
            {
                 campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
            }

            return campaign;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByBlastID(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {
            
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaign != null && getChildren)
            {
                campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
            }

            return campaign;
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByBlastID_UseAmbientTransaction(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            using (TransactionScope scope = new TransactionScope())
            {
                campaign = ECN_Framework_DataLayer.Communicator.Campaign.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaign, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaign != null && getChildren)
            {
                campaign.ItemList = CampaignItem.GetByCampaignID_UseAmbientTransaction(campaign.CampaignID, user, getChildren);
            }

            return campaign;
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> GetByCustomerID(int customerID, KMPlatform.Entity.User user, bool getChildren)
        {

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.Campaign> campaignList = new List<ECN_Framework_Entities.Communicator.Campaign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignList = ECN_Framework_DataLayer.Communicator.Campaign.GetByCustomerID(customerID);
                scope.Complete();
            }

            //if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaignList, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns, user))
            //    throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaignList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Campaign campaign in campaignList)
                {
                    campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
                }
            }

            return campaignList;
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> GetByCustomerID_NoAccessCheck(int customerID, bool getChildren)
        {

            List<ECN_Framework_Entities.Communicator.Campaign> campaignList = new List<ECN_Framework_Entities.Communicator.Campaign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignList = ECN_Framework_DataLayer.Communicator.Campaign.GetByCustomerID(customerID);
                scope.Complete();
            }

            //if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaignList, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns, user))
            //    throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaignList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Campaign campaign in campaignList)
                {
                    campaign.ItemList = CampaignItem.GetByCampaignID_NoAccessCheck(campaign.CampaignID, getChildren);
                }
            }

            return campaignList;
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> Search(int customerID, string criteria, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Campaign> campaignList = new List<ECN_Framework_Entities.Communicator.Campaign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignList = ECN_Framework_DataLayer.Communicator.Campaign.Search(customerID, criteria);
                scope.Complete();
            }
            if (campaignList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Campaign campaign in campaignList)
                {
                    campaign.ItemList = CampaignItem.GetByCampaignID_NoAccessCheck(campaign.CampaignID, getChildren);
                }
            }

            return campaignList;

        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> GetByCustomerID_NonArchived(int customerID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.Campaign> campaignList = new List<ECN_Framework_Entities.Communicator.Campaign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignList = ECN_Framework_DataLayer.Communicator.Campaign.GetByCustomerID_NonArchived(customerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(campaignList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (campaignList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Campaign campaign in campaignList)
                {
                    campaign.ItemList = CampaignItem.GetByCampaignID(campaign.CampaignID, user, getChildren);
                }
            }

            return campaignList;
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> GetByCustomerID_NonArchived_NoAccessCheck(int customerID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Campaign> campaignList = new List<ECN_Framework_Entities.Communicator.Campaign>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignList = ECN_Framework_DataLayer.Communicator.Campaign.GetByCustomerID_NonArchived(customerID);
                scope.Complete();
            }

            if (campaignList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Campaign campaign in campaignList)
                {
                    campaign.ItemList = CampaignItem.GetByCampaignID_NoAccessCheck(campaign.CampaignID, getChildren);
                }
            }

            return campaignList;
        }

        public static DataTable GetCampaignDetailsForManageCampaigns(int CustomerID, string campaignName,string archiveFilter = "active")
        {
            DataTable retTable = new DataTable();
            using (TransactionScope scope = new TransactionScope())
            {
                retTable = ECN_Framework_DataLayer.Communicator.Campaign.GetCampaignDetailsForManageCampaigns(CustomerID,campaignName, archiveFilter);
                scope.Complete();
            }
            return retTable;
        }

        public static void Delete(int campaignID, KMPlatform.Entity.User user)
        {
            //ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.Campaign campaign = GetByCampaignID(campaignID, user, true);
            if (campaign != null)
            {
                //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                //bool updateBlastTable = false;
                //if (campaign.ItemList.Count > 0)
                //{
                //    //get list of CampaignItemBlast from database
                //    List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaign.ItemList[0].CampaignItemID, user, false);
                //    if (dbList.Count > 0)
                //    {
                //        var blastExists = dbList.Where(x => x.BlastID != null);
                //        if (blastExists.Any())
                //            updateBlastTable = true;
                //    }
                //}                

                using (TransactionScope scope = new TransactionScope())
                {
                    //for all of the child table deletes we are overriding so they won't call Blast.CreateBlastsFromCampaignItem() as we call it here
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.Delete(campaignID, user, true);
                    ECN_Framework_DataLayer.Communicator.Campaign.Delete(campaignID, user.CustomerID, user.UserID);

                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(campaign.ItemList[0].CampaignItemID, user);

                    scope.Complete();
                }

            }
        }
    }
}
