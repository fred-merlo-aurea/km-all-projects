using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemBlastRefBlast
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemBlastRefBlast;

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
            return false;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast refBlast, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            if (refBlast.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (refBlast.CampaignItemBlastID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemBlastID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlastLookup = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(refBlast.CampaignItemBlastID.Value, user, false);
                    if (ciBlastLookup == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Exists(ciBlastLookup.CampaignItemID.Value, ciBlastLookup.CampaignItemBlastID, refBlast.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID(ciBlastLookup.CampaignItemID.Value, user, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemBlastRefBlast as there are Sent or Active Blasts"));
                            break;
                        }
                    }
                    
                }

                if (refBlast.RefBlastID == null || (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(refBlast.RefBlastID.Value, refBlast.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "RefBlastID is invalid"));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(refBlast.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (refBlast.CampaignItemBlastRefBlastID <= 0 && (refBlast.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(refBlast.CreatedUserID.Value, refBlast.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast refBlast, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {
            Validate(refBlast, user);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(refBlast,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //get list of CampaignItemBlast from database
            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(refBlast.CampaignItemBlastID.Value, user, false);
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(ciBlast.CampaignItemID.Value, user, false);

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
                refBlast.CampaignItemBlastRefBlastID = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.Save(refBlast);

                //update Blast(s) if necessary
                //if (updateBlastTable)
                //    Blast.CreateBlastsFromCampaignItem(ciBlast.CampaignItemID.Value, user);

                scope.Complete();
            }

            return refBlast.CampaignItemBlastRefBlastID;
        }

        public static bool Exists(int campaignItemBlastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.Exists(campaignItemBlastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int campaignItemBlastID, int campaignItemBlastRefBlastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.Exists(campaignItemBlastID, campaignItemBlastRefBlastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast GetByCampaignItemBlastRefBlastID(int campaignItemBlastRefBlastID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast itemRefBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemRefBlast = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.GetByCampaignItemBlastRefBlastID(campaignItemBlastRefBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemRefBlast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return itemRefBlast;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> GetByCampaignItemBlastID(int campaignItemBlastID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> itemRefBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemRefBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemRefBlastList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return itemRefBlastList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> GetByCampaignItemBlastID_UseAmbientTransaction(int campaignItemBlastID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> itemRefBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();
            using (TransactionScope scope = new TransactionScope())
            {
                itemRefBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemRefBlastList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return itemRefBlastList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> GetByCampaignItemBlastID_NoAccessCheck(int campaignItemBlastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> itemRefBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemRefBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }

            return itemRefBlastList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> GetByCampaignItemBlastID_NoAccessCheck_UseAmbientTransaction(int campaignItemBlastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> itemRefBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();
            using (TransactionScope scope = new TransactionScope())
            {
                itemRefBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }

            return itemRefBlastList;
        }

        public static void Delete(int campaignItemBlastID, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {
            //ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;


            // sunil - 01/15/2016 - not required for indirect deletes from wizard setup - Delete should be checked only for Top level entities (blast, campaign, campaignitem)

            //if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
            //    throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> ciRefBlastList = GetByCampaignItemBlastID(campaignItemBlastID, user);

            if (ciRefBlastList.Count > 0)
            {
                //get list of CampaignItemBlast from database
                ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlastLookup = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(ciRefBlastList[0].CampaignItemBlastID.Value, user, false);
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(ciBlastLookup.CampaignItemID.Value, user, false);

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
                    ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.Delete(campaignItemBlastID, user.CustomerID, user.UserID);

                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(ciBlastLookup.CampaignItemID.Value, user);

                    scope.Complete();
                }

            }
        }

        public static void Delete(int campaignItemBlastID, int campaignItemBlastRefBlastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast ciRefBlast = GetByCampaignItemBlastRefBlastID(campaignItemBlastRefBlastID, user);

            if (ciRefBlast != null)
            {
                //get list of CampaignItemBlast from database
                ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlastLookup = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemBlastID(campaignItemBlastID, user, false);
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> dbList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(ciBlastLookup.CampaignItemID.Value, user, false);

                //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                //bool updateBlastTable = false;
                //var blastExists = dbList.Where(x => x.BlastID != null);
                //if (blastExists.Any())
                //    updateBlastTable = true;

                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.CampaignItemBlastRefBlast.Delete(campaignItemBlastID, campaignItemBlastRefBlastID, user.CustomerID, user.UserID);

                    //update Blast(s) if necessary
                    //if (updateBlastTable)
                    //    Blast.CreateBlastsFromCampaignItem(ciBlastLookup.CampaignItemID.Value, user);

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
