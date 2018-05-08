using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemSuppression
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemSuppression;

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

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            
            if (suppression.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (suppression.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(suppression.CampaignItemID.Value, user, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, suppression.CampaignItemID.Value, suppression.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID(suppression.CampaignItemID.Value, user, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemSuppression as there are Sent or Active Blasts"));
                                break;
                            }
                        }
                        if(suppression.GroupID != null && ciBlastList.Any(bl => bl.GroupID == suppression.GroupID))
                            errorList.Add(new ECNError(Entity, Method, "Suppression Group cannot be the same as the Selected Group"));

                        if (suppression.GroupID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(suppression.GroupID.Value, suppression.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));

                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(suppression.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                            if (suppression.CampaignItemSuppressionID <= 0 && (suppression.CreatedUserID == null || (suppression.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.CreatedUserID.Value, false)))))
                            {
                                if (suppression.CampaignItemSuppressionID <= 0 && (suppression.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(suppression.CreatedUserID.Value, suppression.CustomerID.Value)))
                                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                            }
                            if (suppression.CampaignItemSuppressionID > 0 && (suppression.UpdatedUserID == null || (suppression.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.UpdatedUserID.Value, false)))))
                            {
                                if (suppression.CampaignItemSuppressionID > 0 && (suppression.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(suppression.UpdatedUserID.Value, suppression.CustomerID.Value)))
                                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                            }
                            scope.Complete();
                        }
                    }

                }                
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (suppression.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (suppression.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(suppression.CampaignItemID.Value, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, suppression.CampaignItemID.Value, suppression.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(suppression.CampaignItemID.Value, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemSuppression as there are Sent or Active Blasts"));
                                break;
                            }
                        }
                        if (suppression.GroupID != null && ciBlastList.Any(bl => bl.GroupID == suppression.GroupID))
                            errorList.Add(new ECNError(Entity, Method, "Suppression Group cannot be the same as the Selected Group"));

                        if (suppression.GroupID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(suppression.GroupID.Value, suppression.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));

                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(suppression.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));                            
                            scope.Complete();
                        }
                    }

                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_UseAmbientTransaction(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (suppression.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (suppression.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(suppression.CampaignItemID.Value, user, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists_UseAmbientTransaction(item.CampaignID.Value, suppression.CampaignItemID.Value, suppression.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(suppression.CampaignItemID.Value, user, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent_UseAmbientTransaction(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemSuppression as there are Sent or Active Blasts"));
                                break;
                            }
                        }
                        if (suppression.GroupID != null && ciBlastList.Any(bl => bl.GroupID == suppression.GroupID))
                            errorList.Add(new ECNError(Entity, Method, "Suppression Group cannot be the same as the Selected Group"));

                        if (suppression.GroupID == null || (!ECN_Framework_BusinessLayer.Communicator.Group.Exists_UseAmbientTransaction(suppression.GroupID.Value, suppression.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));

                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(suppression.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                            if (suppression.CreatedUserID == null || (suppression.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.CreatedUserID.Value, false))))
                            {
                                if (suppression.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(suppression.CreatedUserID.Value, suppression.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                            }
                            if (suppression.CampaignItemSuppressionID > 0 && (suppression.UpdatedUserID == null || (suppression.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(suppression.UpdatedUserID.Value, false)))))
                            {
                                if (suppression.CampaignItemSuppressionID > 0 && (suppression.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(suppression.UpdatedUserID.Value, suppression.CustomerID.Value)))
                                    errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                            }
                            scope.Complete();
                        }
                    }

                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression, KMPlatform.Entity.User user)
        {
            Validate(suppression, user);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(suppression,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //get list of CampaignItemBlast from database
            //List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(suppression.CampaignItemID.Value, user, false);

            //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
            //bool updateBlastTable = false;
            //var blastExists = dbList.Where(x => x.BlastID != null);
            //if (blastExists.Any())
            //    updateBlastTable = true;

            using (TransactionScope scope = new TransactionScope())
            {
                suppression.CampaignItemSuppressionID = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Save(suppression);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemSuppressionID(suppression.CampaignItemSuppressionID);
                if (suppression.Filters != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in suppression.Filters)
                    {
                        //check id's
                        if (cibf.CampaignItemSuppresionID == null || cibf.CampaignItemSuppresionID != suppression.CampaignItemSuppressionID)
                        {
                            //id's don't match so it's either new or a copy, either way we need to create new records
                            cibf.CampaignItemBlastFilterID = -1;
                            cibf.CampaignItemBlastID = null;
                            cibf.CampaignItemTestBlastID = null;
                            cibf.CampaignItemSuppresionID = suppression.CampaignItemSuppressionID;
                            cibf.SuppressionGroupID = suppression.GroupID;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }
                    }
                }
                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(suppression.CampaignItemID.Value, user);

                scope.Complete();
            }

            return suppression.CampaignItemSuppressionID;
        }

        public static int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression, KMPlatform.Entity.User user)
        {
            Validate_NoAccessCheck(suppression, user);

           

            //get list of CampaignItemBlast from database
            //List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(suppression.CampaignItemID.Value, user, false);

            //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
            //bool updateBlastTable = false;
            //var blastExists = dbList.Where(x => x.BlastID != null);
            //if (blastExists.Any())
            //    updateBlastTable = true;

            using (TransactionScope scope = new TransactionScope())
            {
                suppression.CampaignItemSuppressionID = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Save(suppression);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemSuppressionID(suppression.CampaignItemSuppressionID);
                if (suppression.Filters != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in suppression.Filters)
                    {
                        //check id's
                        if (cibf.CampaignItemSuppresionID == null || cibf.CampaignItemSuppresionID != suppression.CampaignItemSuppressionID)
                        {
                            //id's don't match so it's either new or a copy, either way we need to create new records
                            cibf.CampaignItemBlastFilterID = -1;
                            cibf.CampaignItemBlastID = null;
                            cibf.CampaignItemTestBlastID = null;
                            cibf.CampaignItemSuppresionID = suppression.CampaignItemSuppressionID;
                            cibf.SuppressionGroupID = suppression.GroupID;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }
                    }
                }
                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(suppression.CampaignItemID.Value, user);

                scope.Complete();
            }

            return suppression.CampaignItemSuppressionID;
        }

        public static int Save_UseAmbientTransaction(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression, KMPlatform.Entity.User user)
        {
            Validate_UseAmbientTransaction(suppression, user);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(suppression, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //get list of CampaignItemBlast from database
            //List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(suppression.CampaignItemID.Value, user, false);

            //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
            //bool updateBlastTable = false;
            //var blastExists = dbList.Where(x => x.BlastID != null);
            //if (blastExists.Any())
            //    updateBlastTable = true;

            using (TransactionScope scope = new TransactionScope())
            {
                suppression.CampaignItemSuppressionID = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Save(suppression);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemSuppressionID(suppression.CampaignItemSuppressionID);
                if (suppression.Filters != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in suppression.Filters)
                    {
                        //check id's
                        if (cibf.CampaignItemSuppresionID == null || cibf.CampaignItemSuppresionID != suppression.CampaignItemSuppressionID)
                        {
                            //id's don't match so it's either new or a copy, either way we need to create new records
                            cibf.CampaignItemBlastFilterID = -1;
                            cibf.CampaignItemBlastID = null;
                            cibf.CampaignItemTestBlastID = null;
                            cibf.CampaignItemSuppresionID = suppression.CampaignItemSuppressionID;
                            cibf.SuppressionGroupID = suppression.GroupID;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }
                    }
                }
                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(suppression.CampaignItemID.Value, user);

                scope.Complete();
            }

            return suppression.CampaignItemSuppressionID;
        }

        public static bool Exists(int campaignItemID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Exists(campaignItemID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int campaignItemID, int campaignItemSuppressionID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Exists(campaignItemID, campaignItemSuppressionID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemSuppression GetByCampaignItemSuppressionID(int campaignItemSuppressionID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItemSuppression itemSuppression = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemSuppression = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.GetByCampaignItemSuppressionID(campaignItemSuppressionID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemSuppression, user))
                throw new ECN_Framework_Common.Objects.SecurityException();
            return itemSuppression;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> GetByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user, bool getChildren = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> itemSuppressionList = new List<ECN_Framework_Entities.Communicator.CampaignItemSuppression>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemSuppressionList = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }
            if(itemSuppressionList.Count > 0 && getChildren)
            {
                foreach(ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in itemSuppressionList)
                {
                    cis.Filters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID(cis.CampaignItemSuppressionID);
                }
            }
            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemSuppressionList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return itemSuppressionList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> GetByCampaignItemID_UseAmbientTransaction(int campaignItemID, KMPlatform.Entity.User user, bool getChildren = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> itemSuppressionList = new List<ECN_Framework_Entities.Communicator.CampaignItemSuppression>();
            using (TransactionScope scope = new TransactionScope())
            {
                itemSuppressionList = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }
            if (itemSuppressionList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in itemSuppressionList)
                {
                    cis.Filters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID_UseAmbientTransaction(cis.CampaignItemSuppressionID);
                }
            }
            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemSuppressionList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return itemSuppressionList;
        }


        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> GetByCampaignItemID_NoAccessCheck(int campaignItemID,  bool getChildren = false)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> itemSuppressionList = new List<ECN_Framework_Entities.Communicator.CampaignItemSuppression>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemSuppressionList = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }
            if (itemSuppressionList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in itemSuppressionList)
                {
                    cis.Filters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID(cis.CampaignItemSuppressionID);
                }
            }

            return itemSuppressionList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(int campaignItemID, bool getChildren = false)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> itemSuppressionList = new List<ECN_Framework_Entities.Communicator.CampaignItemSuppression>();
            using (TransactionScope scope = new TransactionScope())
            {
                itemSuppressionList = ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }
            if (itemSuppressionList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in itemSuppressionList)
                {
                    cis.Filters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID_UseAmbientTransaction(cis.CampaignItemSuppressionID);
                }
            }

            return itemSuppressionList;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.CampaignItemSuppression itemSuppression, KMPlatform.Entity.User user)
        //{
        //    if (itemSuppression != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (itemSuppression.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> itemSuppressionList, KMPlatform.Entity.User user)
        //{
        //    if (itemSuppressionList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from cis in itemSuppressionList
        //                                join c in custList on cis.CustomerID.Value equals c.CustomerID
        //                                select new { cis.CampaignItemSuppressionID };

        //            if (securityCheck.Count() != itemSuppressionList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from cis in itemSuppressionList
        //                                where cis.CustomerID.Value != user.CustomerID
        //                                select new { cis.CampaignItemSuppressionID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Delete(int campaignItemID, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> ciSuppressionList = GetByCampaignItemID(campaignItemID, user);

            if (ciSuppressionList.Count > 0)
            {
                //get list of CampaignItemBlast from database
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = CampaignItemBlast.GetByCampaignItemID(campaignItemID, user, false);

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
                    ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Delete(campaignItemID, user.CustomerID, user.UserID);
                    foreach(ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in ciSuppressionList)
                    {
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemSuppressionID(cis.CampaignItemSuppressionID);
                    }
                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(campaignItemID, user);

                    scope.Complete();
                }

            }
        }

        public static void Delete_NoAccessCheck(int campaignItemID, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {
            
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> ciSuppressionList = GetByCampaignItemID_NoAccessCheck(campaignItemID);

            if (ciSuppressionList.Count > 0)
            {
                //get list of CampaignItemBlast from database
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(campaignItemID, false);

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
                    ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Delete(campaignItemID, user.CustomerID, user.UserID);
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in ciSuppressionList)
                    {
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemSuppressionID(cis.CampaignItemSuppressionID);
                    }
                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(campaignItemID, user);

                    scope.Complete();
                }

            }
        }

        public static void Delete(int campaignItemID, int campaignItemSuppressionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppression = GetByCampaignItemSuppressionID(campaignItemSuppressionID, user);

            if (ciSuppression != null)
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
                    ECN_Framework_DataLayer.Communicator.CampaignItemSuppression.Delete(campaignItemID, campaignItemSuppressionID, user.CustomerID, user.UserID);

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
    }
}
